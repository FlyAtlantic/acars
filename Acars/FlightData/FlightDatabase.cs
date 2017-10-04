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
            string sqlStr = "SELECT `flightnumber`, `departure`, `destination`, `alternate`, `date_assigned`, `utilizadores`.`user_id`, `flights`.`idf`, `flights`.`flighttime` FROM `pilotassignments` left join flights on pilotassignments.flightid = flights.idf left join utilizadores on pilotassignments.pilot = utilizadores.user_id WHERE utilizadores.user_email=@email";
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            try
            {
                conn.Open();

                MySqlCommand sqlCmd = new MySqlCommand(sqlStr, conn);
                sqlCmd.Parameters.AddWithValue("@email", pilotId);

                MySqlDataReader sqlCmdRes = sqlCmd.ExecuteReader();
                if (sqlCmdRes.HasRows)
                    while (sqlCmdRes.Read())
                    {
                        result.LoadedFlightPlan.AtcCallsign = (string)sqlCmdRes[0];
                        result.LoadedFlightPlan.DepartureICAO = (string)sqlCmdRes[1];
                        result.LoadedFlightPlan.ArrivalICAO = (string)sqlCmdRes[2];
                        result.LoadedFlightPlan.AlternateICAO = (string)sqlCmdRes[3];
                    }
                else
                    result = null;
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
    }
}
