using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acars.FlightData
{
    public class FlightPlan
    {
        public string AtcCallsign
        { get; set; }
        public string DepartureICAO
        { get; set; }
        public string ArrivalICAO
        { get; set; }
        public string AlternateICAO
        { get; set; }
    }
}
