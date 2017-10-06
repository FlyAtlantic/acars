using Acars.FlightData;

namespace Acars.Events
{
    public class LandingLightsOnAbove10000ft : FlightEvent
    {
        public override string Code
        {
            get { return "3A"; }
        }

        public override int Duration
        {
            get { return 5; }
        }

        public override bool ConditionActive(Telemetry t)
        {
            return (t.LandingLights && t.Altitude > 10500);
        }
    }
}