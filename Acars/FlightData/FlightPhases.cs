using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acars.FlightData
{
    public enum FlightPhases
    {
        PREFLIGHT = 0,
        TAXIOUT = 1,
        TAKEOFF = 2,
        PUSHBACK = 3,
        CLIMBING = 4,
        CRUISE = 5,
        DESCENDING = 6,
        APPROACH = 7,
        LANDING = 8,
        TAXIIN = 9,
        PARKING = 10
    }
}
