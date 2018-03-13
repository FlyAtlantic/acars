using System;

namespace Acars.FlightData
{
    public class FlightPlan
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Id
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AtcCallsign
        { get; set; }
        
        /// <summary>
        /// Departure airfield ICAO
        /// </summary>
        public string DepartureIcao
        { get; set; }

        /// <summary>
        /// Departure airfield latitude
        /// </summary>
        public double DepartureLat
        { get; set; }

        /// <summary>
        /// Departure airfield longitude
        /// </summary>
        public double DepartureLng
        { get; set; }

        /// <summary>
        /// Destination airfield ICAO
        /// </summary>
        public string DestinationIcao
        { get; set; }

        /// <summary>
        /// Destination airfield latitude
        /// </summary>
        public double DestinationLat
        { get; set; }

        /// <summary>
        /// Destination airfield longitude
        /// </summary>
        public double DestinationLng
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AlternateIcao
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateAssigned
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CidVatsim
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? AssignId
        { get; set; }

        public FlightPlan() { }
    }
}
