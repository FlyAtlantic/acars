using Acars.FlightData;

namespace Acars.Events
{
    public class LandingLightsOnAbove250kt : FlightEvent
    {
        public override string Code
        {
            get { return "4A"; }
        }

        public override int Duration
        {
            get { return 5; }
        }

        public override bool ConditionActive(Telemetry t)
        {
            return (t.LandingLights && t.IndicatedAirSpeed > 255);
        }
    }
}
