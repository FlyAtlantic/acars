using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Device.Location;
using Acars.Events;
using System.Collections.Generic;

namespace Acars.FlightData
{
    public class FlightDatabase
    {
        private static string ConnectionString
        {
            get
            {
                return String.Format(
                    "server={0};uid={1};pwd={2};database={3};Connection Timeout=120;",
                    Properties.Settings.Default.Server,
                    Properties.Settings.Default.Dbuser,
                    Properties.Settings.Default.Dbpass,
                    Properties.Settings.Default.Database);
            }
        }

        /// <summary>
        /// Returns the SHA1 hash string for a given string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string StringToSha1Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ValidateLogin(string email, string password)
        {
            bool validLogin = false;
            string sqlStrValidateLogin = "select count(*) from utilizadores where `user_email`=@email and `user_senha`=@password;";
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            try
            {
                conn.Open();

                MySqlCommand sqlCmd = new MySqlCommand(sqlStrValidateLogin, conn);
                sqlCmd.Parameters.AddWithValue("@email", email);
                sqlCmd.Parameters.AddWithValue("@password", StringToSha1Hash(password));

                validLogin = (Convert.ToInt32(sqlCmd.ExecuteScalar()) == 1);
            }
            catch (Exception crap)
            {
                // log the failure or something, anyway this isn't a good login for sure
                throw new Exception(String.Format("Failed to validate credentials.\r\nSQL Statements: {0}", sqlStrValidateLogin), crap);
            }
            finally
            {
                conn.Close();
            }

            return validLogin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static FlightPlan GetFlightPlan()
        {
            FlightPlan result = new FlightPlan();
            string sqlStrGetFlight = "SELECT `flights`.`flightnumber`, `origin`.`ICAO` as originICAO, `origin`.`LAT` as originLat, `origin`.`LON` originLON, `arrival`.`ICAO` as arrivalICAO, `arrival`.`LAT` as arrivalLat, `arrival`.`LON` as arrivalLON, `flights`.`alternate`, `pilotassignments`.`date_assigned`, `utilizadores`.`user_id`, `flights`.`idf`, `flights`.`flighttime`, `flights`.`aircraft` FROM `pilotassignments` LEFT JOIN `flights` ON `pilotassignments`.`flightid` = `flights`.`idf` LEFT JOIN `airports` origin ON `origin`.`ICAO` = `flights`.`departure` LEFT JOIN `airports` arrival ON `arrival`.`ICAO` = `flights`.`destination` LEFT JOIN `utilizadores` on `pilotassignments`.`pilot` = `utilizadores`.`user_id` WHERE `utilizadores`.`user_email` = @email;";
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            try
            {
                conn.Open();


                // GET FLIGHT DATA
                MySqlCommand sqlCmd = new MySqlCommand(sqlStrGetFlight, conn);
                sqlCmd.Parameters.AddWithValue("@email", Properties.Settings.Default.Email);

                MySqlDataReader sqlCmdRes = sqlCmd.ExecuteReader();
                if (sqlCmdRes.HasRows)
                    while (sqlCmdRes.Read())
                    {
                        result.AtcCallsign = (string)sqlCmdRes[0];
                        result.DepartureAirfield = new Location((string)sqlCmdRes[1],                      // ICAO string
                                                                                 new GeoCoordinate((double)sqlCmdRes[2],    // Latitude
                                                                                                   (double)sqlCmdRes[3]));  // Longitude
                        result.ArrivalAirfield = new Location((string)sqlCmdRes[4],                        // ICAO string
                                                                               new GeoCoordinate((double)sqlCmdRes[5],      // Latitude
                                                                                                 (double)sqlCmdRes[6]));    // Longitude
                        result.AlternateICAO = (string)sqlCmdRes[7];
                        result.ID = (int)sqlCmdRes[10];
                        result.DateAssigned = (DateTime)sqlCmdRes[8];

                        // TODO: Assign performance files from database
                        switch ((string)sqlCmdRes[12])
                        {
                            case "C172":
                                result.Aircraft = new AircraftPerformance(
                                    "C172",
                                    1111,
                                    1111,
                                    14508,
                                    new Dictionary<short, FlapSetting>() {
                                        { 0, new FlapSetting( "0", 128 + 500) },
                                        { 10, new FlapSetting("1", 85) }
                                });
                                break;

                            case "B738":
                                result.Aircraft = new AircraftPerformance(
                                    "B738",
                                    78741,
                                    66361,
                                    41500,
                                    new Dictionary<short, FlapSetting>() {
                                        { 0, new FlapSetting( "0", 400) },
                                        { 1, new FlapSetting("3640", 250) },
                                        { 2, new FlapSetting("7509", 250) },
                                        { 5, new FlapSetting("10239", 250) },
                                        { 10, new FlapSetting("12742", 210) },
                                        { 15, new FlapSetting("13652", 200) },
                                        { 25, new FlapSetting("14335", 190) },
                                        { 30, new FlapSetting("15017", 175) },
                                        { 40, new FlapSetting("16383", 160) }
                                });
                                break;

                            default:
                                result.Aircraft = null;
                                break;
                        }
                    }
                else
                    result = null;
            }
            catch (Exception crap)
            {
                result = null;
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to load flight plan for user {0}.\r\nSQL Statements: {1}", Properties.Settings.Default.Email, sqlStrGetFlight), crap);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flight"></param>
        /// <returns>(int) Inserted pirep ID</returns>
        public static long StartFlight(Flight flight)
        {
            long insertedId = -1;

            string sqlStrInsertPirep = "INSERT INTO `pireps` (`date`, `flightid`, `pilotid`, `accepted`) SELECT @date, @flightid, `user_id`, @accepted FROM `utilizadores` WHERE `user_email` = @email;";

            MySqlConnection conn = new MySqlConnection(ConnectionString);

            try
            {
                conn.Open();

                // INSERT PIREP
                MySqlCommand sqlCmd = new MySqlCommand(sqlStrInsertPirep, conn);
                var dateParam = sqlCmd.Parameters.Add("@date", MySqlDbType.Date);
                dateParam.Value = DateTime.UtcNow;
                sqlCmd.Parameters.AddWithValue("@flightid", flight.LoadedFlightPlan.ID);
                sqlCmd.Parameters.AddWithValue("@accepted", "0");
                sqlCmd.Parameters.AddWithValue("@email", Properties.Settings.Default.Email);

                sqlCmd.ExecuteNonQuery();
                insertedId = sqlCmd.LastInsertedId;
            }
            catch (Exception crap)
            {
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to end the flight plan for user {0}.\r\nSQL Statements: {1}",
                                                  Properties.Settings.Default.Email,
                                                  sqlStrInsertPirep),
                                    crap);
            }
            finally
            {
                conn.Close();
            }

            return insertedId;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void UpdateFlight(Flight flight)
        {
            string sqlStrUpdatePilotAsignments = "UPDATE `pilotassignments` JOIN `utilizadores` ON `utilizadores`.`user_id` = `pilotassignments`.`pilot` SET `onflight` = NOW() WHERE `utilizadores`.`user_email`=@email";
            string sqlStrUpdateFlightLog = "INSERT INTO flightLog (pirepid, time, LAT, LON, ALT, HDG, GS, phase) VALUES (@pirepid, @time, @LAT, @LON, @ALT, @HDG, @GS, @phase)";
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            try
            {
                conn.Open();

                MySqlCommand sqlCmd = new MySqlCommand(sqlStrUpdatePilotAsignments, conn);
                sqlCmd.Parameters.AddWithValue("@email", Properties.Settings.Default.Email);

                sqlCmd.ExecuteNonQuery();

                sqlCmd = new MySqlCommand(sqlStrUpdateFlightLog, conn);
                sqlCmd.Parameters.AddWithValue("@pirepid", flight.PirepID);
                sqlCmd.Parameters.AddWithValue("@time", DateTime.UtcNow);
                sqlCmd.Parameters.AddWithValue("@LAT", flight.LastTelemetry.Latitude);
                sqlCmd.Parameters.AddWithValue("@LON", flight.LastTelemetry.Longitude);
                sqlCmd.Parameters.AddWithValue("@ALT", flight.LastTelemetry.Altitude);
                sqlCmd.Parameters.AddWithValue("@HDG", flight.LastTelemetry.Compass);
                sqlCmd.Parameters.AddWithValue("@GS", flight.LastTelemetry.GroundSpeed);
                sqlCmd.Parameters.AddWithValue("@phase", flight.LastTelemetry.FlightPhase);

                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception crap)
            {
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to start the flight plan for user {0}.\r\nSQL Statements: {1}",
                                                  Properties.Settings.Default.Email,
                                                  sqlStrUpdatePilotAsignments,
                                                  sqlStrUpdateFlightLog),
                                    crap);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flight"></param>
        public static void EndFlight(Flight flight)
        {
            string sqlStrInsertPirep = "UPDATE `pireps` set `date` = @date, `flighttime` = @flighttime, `ft/pm` = @landingrate, `sum` = @sum, `accepted` = @accepted, `eps_granted` = @flighteps WHERE `id` = @pirepid;";
            string sqlStrUpdateUser = "UPDATE `utilizadores` SET `eps` = eps + @flighteps, `location` = @location WHERE `user_email` = @email;";
            string sqlStrDeleteAssignment = "DELETE `pilotassignments` from `pilotassignments` left join `utilizadores` on `pilotassignments`.`pilot` = `utilizadores`.`user_id` where `utilizadores`.`user_email` = @email;";
            string sqlInsertPenalizations = "INSERT INTO penalizations(datepenalization, pirepid, code) VALUES (@DatePenalization, @PirepId, @Code)";
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            try
            {
                conn.Open();

                // INSERT PIREP
                MySqlCommand sqlCmd = new MySqlCommand(sqlStrInsertPirep, conn);
                var dateParam = sqlCmd.Parameters.Add("@date", MySqlDbType.Date);
                dateParam.Value = DateTime.UtcNow;
                sqlCmd.Parameters.AddWithValue("@flighttime", (int)Math.Round(flight.ActualTimeEnRoute.TotalMinutes));
                sqlCmd.Parameters.AddWithValue("@landingrate", (int)Math.Round(flight.ActualArrivalTime.VerticalSpeed));
                sqlCmd.Parameters.AddWithValue("@sum", flight.FinalScore);
                sqlCmd.Parameters.AddWithValue("@accepted", "1");
                sqlCmd.Parameters.AddWithValue("@flighteps", flight.EfficiencyPoints);
                sqlCmd.Parameters.AddWithValue("@pirepid", flight.PirepID);

                sqlCmd.ExecuteNonQuery();

                // UPDATE PILOT DATA
                sqlCmd = new MySqlCommand(sqlStrUpdateUser, conn);
                sqlCmd.Parameters.AddWithValue("@email", Properties.Settings.Default.Email);
                sqlCmd.Parameters.AddWithValue("@flighteps", flight.EfficiencyPoints);
                sqlCmd.Parameters.AddWithValue("@location", flight.LoadedFlightPlan.ArrivalAirfield.Identifier);

                sqlCmd.ExecuteNonQuery();

                // DELETE ASSIGNMENT
                sqlCmd = new MySqlCommand(sqlStrDeleteAssignment, conn);
                sqlCmd.Parameters.AddWithValue("@email", Properties.Settings.Default.Email);

                sqlCmd.ExecuteNonQuery();

                // SEND PENALIZATION INFORMATION
                if (flight.Events != null && flight.Events.Count > 0)
                    foreach (EventOccurrence e in flight.Events)
                    {
                        sqlCmd = new MySqlCommand(sqlInsertPenalizations, conn);
                        sqlCmd.Parameters.AddWithValue("@DatePenalization", flight.TelemetryLog[e.StartId].Timestamp );
                        sqlCmd.Parameters.AddWithValue("@PirepId", flight.PirepID);
                        sqlCmd.Parameters.AddWithValue("@Code", e.Event.Code);
                        sqlCmd.Parameters.AddWithValue("@email", Properties.Settings.Default.Email);

                        sqlCmd.ExecuteNonQuery();
                    }
            }
            catch (Exception crap)
            {
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to end the flight plan for user {0}.\r\nSQL Statements: {1} | {2} | {3}",
                                                  Properties.Settings.Default.Email,
                                                  sqlStrInsertPirep,
                                                  sqlStrUpdateUser,
                                                  sqlStrDeleteAssignment,
                                                  sqlInsertPenalizations),
                                    crap);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
