using Acars.FlightData;

namespace Acars.Events
{
    public class Speedabove250ktbelow10000ft : FlightEvent
    {
        public override string Code
        {
            get { return "3C"; }
        }

        public override int Duration
        {
            get { return 5; }
        }

        public override bool ConditionActive(Telemetry t)
        {
            return (t.IndicatedAirSpeed > 255 && t.Altitude < 9500);
        }
    }
}