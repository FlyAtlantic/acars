using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Device.Location;

namespace Acars.FlightData
{
    public class FlightDatabase
    {
        private static string ConnectionString
        {
            get
            {
                return String.Format(
                    "server={0};uid={1};pwd={2};database={3};",
                    Properties.Settings.Default.Properties["Server"],
                    Properties.Settings.Default.Properties["Dbuser"],
                    Properties.Settings.Default.Properties["Dbpass"],
                    Properties.Settings.Default.Properties["Database"]);
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
                validLogin = false;
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
            string sqlStrGetFlight = "SELECT `flights`.`flightnumber`, `origin`.`ICAO` as originICAO, `origin`.`LAT` as originLat, `origin`.`LON` originLON, `arrival`.`ICAO` as arrivalICAO, `arrival`.`LAT` as arrivalLat, `arrival`.`LON` as arrivalLON, `flights`.`alternate`, `pilotassignments`.`date_assigned`, `utilizadores`.`user_id`, `flights`.`idf`, `flights`.`flighttime` FROM `pilotassignments` LEFT JOIN `flights` ON `pilotassignments`.`flightid` = `flights`.`idf` LEFT JOIN `airports` origin ON `origin`.`ICAO` = `flights`.`departure` LEFT JOIN `airports` arrival ON `arrival`.`ICAO` = `flights`.`destination` LEFT JOIN `utilizadores` on `pilotassignments`.`pilot` = `utilizadores`.`user_id` WHERE `utilizadores`.`user_email` = @email;";
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            try
            {
                conn.Open();


                // GET FLIGHT DATA
                MySqlCommand sqlCmd = new MySqlCommand(sqlStrGetFlight, conn);
                sqlCmd.Parameters.AddWithValue("@email", Properties.Settings.Default.Properties["email"]);

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
                    }
                else
                    result = null;
            }
            catch (Exception crap)
            {
                result = null;
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to load flight plan for user {0}.\r\nSQL Statements: {1}", Properties.Settings.Default.Properties["email"], sqlStrGetFlight), crap);
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
        public static void UpdateFlight(Flight flight)
        {
            string sqlStrUpdatePilotAsignments = "UPDATE `pilotassignments` JOIN `utilizadores` ON `utilizadores`.`user_id` = `pilotassignments`.`pilot` SET `onflight` = NOW() WHERE `utilizadores`.`user_email`=@email";
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            try
            {
                conn.Open();

                MySqlCommand sqlCmd = new MySqlCommand(sqlStrUpdatePilotAsignments, conn);
                sqlCmd.Parameters.AddWithValue("@email", Properties.Settings.Default.Properties["email"]);

                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception crap)
            {
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to start the flight plan for user {0}.\r\nSQL Statements: {1}",
                                                  Properties.Settings.Default.Properties["email"],
                                                  sqlStrUpdatePilotAsignments),
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
            string sqlStrInsertPirep = "INSERT INTO `pireps` (`date`, `flighttime`, `flightid`, `pilotid`, `ft/pm`, `sum`, `accepted`, `eps_granted`) SELECT @date, @flighttime, @flightid, `user_id`, @landingrate, @sum, @accepted, @flighteps FROM `utilizadores` WHERE `user_email` = @email;";
            string sqlStrUpdateUser = "UPDATE `utilizadores` SET `eps` = eps + @flighteps WHERE `user_email` = @pilotid;";
            string sqlStrDeleteAssignment = "DELETE from `pilotassignments` left join utilizadores on pilotassignments.pilot = utilizadores.user_id where `user_email` = @pilotid;";
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            try
            {
                conn.Open();

                // INSERT PIREP
                MySqlCommand sqlCmd = new MySqlCommand(sqlStrInsertPirep, conn);
                var dateParam = sqlCmd.Parameters.Add("@date", MySqlDbType.Date);
                dateParam.Value = DateTime.UtcNow;
                sqlCmd.Parameters.AddWithValue("@flighttime", Math.Round(flight.ActualTimeEnRoute.TotalMinutes));
                sqlCmd.Parameters.AddWithValue("@flightid", flight.FlightID);
                sqlCmd.Parameters.AddWithValue("@pilotid", Properties.Settings.Default.Properties["email"]);
                sqlCmd.Parameters.AddWithValue("@landingrate", Math.Round(flight.ActualArrivalTime.VerticalSpeed));
                sqlCmd.Parameters.AddWithValue("@sum", flight.FinalScore);
                sqlCmd.Parameters.AddWithValue("@accepted", "1");
                sqlCmd.Parameters.AddWithValue("@flighteps", flight.EfficiencyPoints);

                sqlCmd.ExecuteNonQuery();

                // UPDATE PILOT DATA
                sqlCmd = new MySqlCommand(sqlStrUpdateUser, conn);
                sqlCmd.Parameters.AddWithValue("@pilotid", Properties.Settings.Default.Properties["email"]);
                sqlCmd.Parameters.AddWithValue("@flighteps", flight.EfficiencyPoints);

                sqlCmd.ExecuteNonQuery();

                // DELETE ASSIGNMENT
                sqlCmd = new MySqlCommand(sqlStrDeleteAssignment, conn);
                sqlCmd.Parameters.AddWithValue("@pilotid", Properties.Settings.Default.Properties["email"]);

                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception crap)
            {
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to end the flight plan for user {0}.\r\nSQL Statements: {1} | {2} | {3}",
                                                  Properties.Settings.Default.Properties["email"],
                                                  sqlStrInsertPirep,
                                                  sqlStrUpdateUser,
                                                  sqlStrDeleteAssignment),
                                    crap);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
