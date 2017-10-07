using Acars.FlightData;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acars
{
    public class FlyAtlanticHelpers
    {
        public static bool StartFlight(MySqlConnection conn, FsuipcWrapper fs, int userId)
        {
            try
            {
                // instanciate FS wrapper
                while (fs == null)
                    fs = FsuipcWrapper.TryInstantiate();

                MySqlCommand updatePilotAssignment = new MySqlCommand("UPDATE `pilotassignments` SET `onflight` = NOW() WHERE `pilot` = @pilotid;", conn);
                updatePilotAssignment.Parameters.AddWithValue("@pilotid", userId);
                conn.Open();
                updatePilotAssignment.ExecuteNonQuery();
                conn.Close();
                //insere e verifica hora zulu no Simulador
                fs.EnvironmentDateTime = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool EndFlight(MySqlConnection conn, int userId, double flightMinutes, int flightId, double landingRate)
        {
            bool ok = false;
            try
            {
                // prepare sql commands
                MySqlCommand insertFlight = new MySqlCommand("INSERT INTO `pireps` (`date`, `flighttime`, `flightid`, `pilotid`, `ft/pm`, `sum`, `accepted`, `eps_granted`) VALUES (@date, @flighttime, @flightid, @pilotid, @landingrate, @sum, @accepted, @flighteps);", conn);
                var dateParam = insertFlight.Parameters.Add("@date", MySqlDbType.Date);
                dateParam.Value = DateTime.UtcNow;
                insertFlight.Parameters.AddWithValue("@flighttime", Math.Round(flightMinutes));
                insertFlight.Parameters.AddWithValue("@flightid", flightId);
                insertFlight.Parameters.AddWithValue("@pilotid", userId);
                insertFlight.Parameters.AddWithValue("@landingrate", Math.Round(landingRate));
                insertFlight.Parameters.AddWithValue("@sum", 100);
                insertFlight.Parameters.AddWithValue("@accepted", "1");
                insertFlight.Parameters.AddWithValue("@flighteps", Math.Round(Math.Round(flightMinutes) / 10));

                MySqlCommand updatePilot = new MySqlCommand("UPDATE `utilizadores` SET `eps` = eps + @flighteps WHERE `user_id` = @pilotid;", conn);
                updatePilot.Parameters.AddWithValue("@pilotid", userId);
                updatePilot.Parameters.AddWithValue("@flighteps", Math.Round(Math.Round(flightMinutes) / 10));

                MySqlCommand deleteFlight = new MySqlCommand("DELETE from `pilotassignments` where `pilot` = @pilotid;", conn);
                deleteFlight.Parameters.AddWithValue("@pilotid", userId);

                conn.Open();

                // insert flight
                int result = 0;
                while (result == 0)
                    result = insertFlight.ExecuteNonQuery();

                // update pilot data
                result = 0;
                while (result == 0)
                    result = updatePilot.ExecuteNonQuery();

                // delete flight
                result = 0;
                while (result == 0)
                    result = deleteFlight.ExecuteNonQuery();

                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
            }
            finally
            {
                conn.Close();
            }

            return ok;
        }

    }
}
