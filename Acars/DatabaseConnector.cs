using Acars.FlightData;
using Dapper;
using FlightMonitorApi;
using MySql.Data.MySqlClient;
using System;

namespace Acars
{
    public class DatabaseConnector : IDataConnector
    {
        public DatabaseConnector(string UserEmail)
        {
            this.UserEmail = UserEmail;
            this.ActiveFlightPlan = null;
        }

        private static string ConnectionString =>
            String.Format(
                @"server={0};uid={1};pwd={2};database={3};Connection Timeout=60;",
                Properties.Settings.Default.Server,
                Properties.Settings.Default.Dbuser,
                Properties.Settings.Default.Dbpass,
                Properties.Settings.Default.Database);

        private string UserEmail
        { get; set; }

        public FlightPlan ActiveFlightPlan
        { get; private set; }
        
        public bool BeforeStart()
        {
            if (ActiveFlightPlan != null)
                return true;

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                FlightPlan p;
                try
                {
                    conn.Open();
                    p = conn.QueryFirstOrDefault<FlightPlan>(@"
SELECT
    `flights`.`idf`                     AS `Id`,
    `flights`.`flightnumber`            AS `AtcCallsign`,
    `origin`.`ICAO`                     AS `DepartureIcao`,
    `origin`.`LAT`                      AS `DepartureLat`,
    `origin`.`LON`                      AS `DepartureLng`,
    `arrival`.`ICAO`                    AS `ArrivalIcao`,
    `arrival`.`LAT`                     AS `ArrivalLat`,
    `arrival`.`LON`                     AS `ArrivalLng`,
    `flights`.`alternate`               AS `AlternateIcao`,
    `pilotassignments`.`date_assigned`  AS `DateAssigned`,
    `utilizadores`.`idvatsim`           AS `CidVatsim`,
    `pilotassignments`.`id`             AS `AssignId`
FROM `pilotassignments`
LEFT JOIN `flights` ON
    `pilotassignments`.`flightid` = `flights`.`idf`
LEFT JOIN `airports` origin ON
    `origin`.`ICAO` = `flights`.`departure`
LEFT JOIN `airports` arrival ON
    `arrival`.`ICAO` = `flights`.`destination`
LEFT JOIN `utilizadores` ON
    `pilotassignments`.`pilot` = `utilizadores`.`user_id`
WHERE
    `utilizadores`.`user_email` = @UserEmail;",
    new { UserEmail });
                }
                catch (InvalidOperationException)
                {
                    p = null;
                }

                if (p != null)
                    ActiveFlightPlan = p;
            }

            return (ActiveFlightPlan != null);
        }
    }
}
