using Acars.FlightData;

namespace Acars.Events
{
    public class Speedabove25ktonTaxi : FlightEvent
    {
        public override string Code
        {
            get { return "3D"; }
        }

        public override int Duration
        {
            get { return 5; }
        }

        public override bool ConditionActive(Telemetry t)
        {
            return (t.GroundSpeed > 30 && t.OnGround);
        }
    }
}
