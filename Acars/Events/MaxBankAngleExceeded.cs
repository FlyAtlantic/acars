using Acars.FlightData;

namespace Acars.Events
{
    public class MaxBankAngleExceeded : FlightEvent
    {
        public override string Code
        {
            get { return "10A"; }
        }

        public override int Duration
        {
            get { return 5; }
        }

        public override bool ConditionActive(Telemetry t)
        {
            return (t.Bank > 5);
        }
    }
}
