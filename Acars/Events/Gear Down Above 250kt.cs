using Acars.FlightData;

namespace Acars.Events
{
    public class GearDownAbove250kt : FlightEvent
    {
        public override string Code
        {
            get { return "4D"; }
        }

        public override int Duration
        {
            get { return 5; }
        }

        public override bool ConditionActive(Telemetry t)
        {
            return (t.Gear && t.IndicatedAirSpeed > 255);
        }
    }
}
