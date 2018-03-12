using Acars.FlightData;
using FlightMonitorApi;
using System;

namespace Acars
{
    public partial class DatabaseConnector : IDataConnector
    {
        private const string SELECT_FLIGHT_PLAN = @"
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
    `utilizadores`.`user_email` = @UserEmail;";

        private const string INSERT_PIREP = @"
INSERT INTO `pireps` (
    `date`,
    `flightid`,
    `pilotid`,
    `accepted`
)
SELECT
    @StartTime,
    @FlightId,
    `user_id`,
    '0'
FROM
    `utilizadores`
WHERE
    `user_email` = @UserEmail;";

        private const string TOUCH_PILOT_ASSIGNMENT = @"
UPDATE
    `pilotassignments`
JOIN `utilizadores` ON
    `utilizadores`.`user_id` = `pilotassignments`.`pilot`
SET
    `onflight` = NOW()
WHERE
    `utilizadores`.`user_email` = @UserEmail";

        private const string INSERT_FLIGHT_LOG = @"
INSERT INTO flightLog (
    pirepid,
    time,
    LAT,
    LON,
    ALT,
    HDG,
    GS,
    phase
) VALUES (
    @FlightId,
    @TimeStamp,
    @Latitude,
    @Longitude,
    @Altitude,
    @Heading,
    @GroundSpeed,
    @FlightPhase)";
    }
}
