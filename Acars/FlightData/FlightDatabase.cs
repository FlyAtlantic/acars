using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="pilotId"></param>
        /// <returns></returns>
        public static Flight GetFlight(string pilotId)
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
                sqlCmd.Parameters.AddWithValue("@email", pilotId);

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


                // START FLIGHT
                sqlCmd = new MySqlCommand(sqlStrUpdateAssignment, conn);
                sqlCmd.Parameters.AddWithValue("@pilotid", pilotId);

                // TODO: currently handled by the main form, to comissionate when introducting AppContext
                //sqlCmd.ExecuteNonQuery();
            }
            catch (Exception crap)
            {
                result = null;
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to load flight plan for user {0}.\r\nSQL Statement: {1}", pilotId, sqlStr), crap);
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
        /// <param name="pilotId"></param>
        /// <param name="flight"></param>
        public static void EndFlight(string pilotId, Flight flight)
        {
            string sqlStrInsertPirep = "INSERT INTO `pireps` (`date`, `flighttime`, `flightid`, `pilotid`, `ft/pm`, `sum`, `accepted`, `eps_granted`) VALUES (@date, @flighttime, @flightid, @pilotid, @landingrate, @sum, @accepted, @flighteps);";
            string sqlStrUpdateUser = "UPDATE `utilizadores` SET `eps` = eps + @flighteps WHERE `user_id` = @pilotid;";
            string sqlStrDeleteAssignment = "DELETE from `pilotassignments` where `pilot` = @pilotid;";
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
                sqlCmd.Parameters.AddWithValue("@pilotid", pilotId);
                sqlCmd.Parameters.AddWithValue("@landingrate", Math.Round(flight.ActualArrivalTime.VerticalSpeed));
                sqlCmd.Parameters.AddWithValue("@sum", 100);
                sqlCmd.Parameters.AddWithValue("@accepted", "1");
                sqlCmd.Parameters.AddWithValue("@flighteps", Math.Round(Math.Round(flight.ActualTimeEnRoute.TotalMinutes) / 10));

                sqlCmd.ExecuteNonQuery();

                // UPDATE PILOT DATA
                sqlCmd = new MySqlCommand(sqlStrUpdateUser, conn);
                sqlCmd.Parameters.AddWithValue("@pilotid", pilotId);
                sqlCmd.Parameters.AddWithValue("@flighteps", Math.Round(Math.Round(flight.ActualTimeEnRoute.TotalMinutes) / 10));

                sqlCmd.ExecuteNonQuery();

                // DELETE ASSIGNMENT
                sqlCmd = new MySqlCommand(sqlStrDeleteAssignment, conn);
                sqlCmd.Parameters.AddWithValue("@pilotid", pilotId);

                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception crap)
            {
                // pass the exception to the caller with an usefull message
                throw new Exception(String.Format("Failed to end the flight plan for user {0}.\r\nSQL Statement: {1}", pilotId, sqlStr), crap);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
