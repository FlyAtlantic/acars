using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acars.FlightData
{
    public class FlightPlan
    {
        public int ID
        { get; set; }
        public string AtcCallsign
        { get; set; }
        public Location DepartureAirfield
        { get; set; }
        public Location ArrivalAirfield
        { get; set; }
        public string AlternateICAO
        { get; set; }
        public DateTime DateAssigned
        { get; set; }
        public string Aircraft
        { get; set; }

        public FlightPlan() { }
    }
}
