using Acars.FlightData;

namespace Acars.Events
{
    public class PitchHigh : FlightEvent
    {
        public override string Code
        {
            get { return "7B"; }
        }

        public override int Duration
        {
            get { return 5; }
        }

        public override bool ConditionActive(Telemetry t)
        {
            return (t.Pitch > 30);
        }
    }
}