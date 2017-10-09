using MySql.Data.MySqlClient;
using System;

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
                    Properties.Settings.Default.Server,
                    Properties.Settings.Default.Dbuser,
                    Properties.Settings.Default.Dbpass,
                    Properties.Settings.Default.Database);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Flight GetFlight()
        {
            Flight result = new Flight();
            string sqlStrGetFlight = "SELECT `flightnumber`, `departure`, `destination`, `alternate`, `date_assigned`, `utilizadores`.`user_id`, `flights`.`idf`, `flights`.`flighttime` FROM `pilotassignments` left join flights on pilotassignments.flightid = flights.idf left join utilizadores on pilotassignments.pilot = utilizadores.user_id WHERE utilizadores.user_email=@email";
            string sqlStrUpdateAssignment = "UPDATE `pilotassignments` SET `onflight` = NOW() WHERE `pilot` = @pilotid;";
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
                        result.LoadedFlightPlan.AtcCallsign = (string)sqlCmdRes[0];
                        result.LoadedFlightPlan.DepartureICAO = (string)sqlCmdRes[1];
                        result.LoadedFlightPlan.ArrivalICAO = (string)sqlCmdRes[2];
                        result.LoadedFlightPlan.AlternateICAO = (string)sqlCmdRes[3];
                        result.FlightID = (int)sqlCmdRes[6];
                    }
                else
                    result = null;
            }
            catch (Exception crap)
            {
                result = null;
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to load flight plan for user {0}.\r\nSQL Statements: {1} | {2}", Properties.Settings.Default.Email, sqlStrGetFlight, sqlStrUpdateAssignment), crap);
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
        public static void StartFlight(Flight flight)
        {
            string sqlStrUpdatePilotAsignments = "UPDATE `pilotassignments` JOIN `utilizadores` ON `utilizadores`.`user_id` = `pilotassignments`.`pilot` SET `onflight` = NOW() WHERE `utilizadores`.`user_email`=@email";
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            try
            {
                conn.Open();

                MySqlCommand sqlCmd = new MySqlCommand(sqlStrUpdatePilotAsignments, conn);
                sqlCmd.Parameters.AddWithValue("@email", Properties.Settings.Default.Email);

                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception crap)
            {
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to start the flight plan for user {0}.\r\nSQL Statements: {1}",
                                                  Properties.Settings.Default.Email,
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
                sqlCmd.Parameters.AddWithValue("@pilotid", Properties.Settings.Default.Email);
                sqlCmd.Parameters.AddWithValue("@landingrate", Math.Round(flight.ActualArrivalTime.VerticalSpeed));
                sqlCmd.Parameters.AddWithValue("@sum", 100);
                sqlCmd.Parameters.AddWithValue("@accepted", "1");
                sqlCmd.Parameters.AddWithValue("@flighteps", Math.Round(Math.Round(flight.ActualTimeEnRoute.TotalMinutes) / 10));

                sqlCmd.ExecuteNonQuery();

                // UPDATE PILOT DATA
                sqlCmd = new MySqlCommand(sqlStrUpdateUser, conn);
                sqlCmd.Parameters.AddWithValue("@pilotid", Properties.Settings.Default.Email);
                sqlCmd.Parameters.AddWithValue("@flighteps", Math.Round(Math.Round(flight.ActualTimeEnRoute.TotalMinutes) / 10));

                sqlCmd.ExecuteNonQuery();

                // DELETE ASSIGNMENT
                sqlCmd = new MySqlCommand(sqlStrDeleteAssignment, conn);
                sqlCmd.Parameters.AddWithValue("@pilotid", Properties.Settings.Default.Email);

                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception crap)
            {
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to end the flight plan for user {0}.\r\nSQL Statements: {1} | {2} | {3}",
                                                  Properties.Settings.Default.Email,
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
