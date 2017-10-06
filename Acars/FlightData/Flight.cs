using FSUIPC;
using System;
using System.Collections.Generic;

namespace Acars.FlightData
{
    public class Flight
    {
        public Flight(FlightPhases initialPhase = FlightPhases.PREFLIGHT)
        {
            phase = initialPhase;

            TelemetryLog = new List<Telemetry>();

            ActualArrivalTime = null;
            ActualDepartureTime = null;

            LoadedFlightPlan = new FlightPlan();
        }

        #region variables
        // instance
        private FlightPhases phase;

        // statics
        static private Offset<short> engine1 = new Offset<short>(0x0894);
        static private Offset<short> parkingBrake = new Offset<short>(0x0BC8, false);
        static private Offset<int> airspeed = new Offset<int>(0x02BC);
        static private Offset<short> onGround = new Offset<short>(0x0366, false);
        static private Offset<short> verticalSpeed = new Offset<short>(0x0842);
        static private Offset<short> throttle = new Offset<short>(0x088C);
        static private Offset<Double> altitude = new Offset<Double>(0x6020);
        #endregion variables

        #region Properties
        public int FlightID
        {
            get;
            internal set;
        }
        public Telemetry ActualDepartureTime
        {
            get;
            private set;
        }

        public Telemetry ActualArrivalTime
        {
            get;
            private set;
        }

        public TimeSpan ActualTimeEnRoute
        {
            get
            {
                if (ActualDepartureTime == null || ActualArrivalTime == null)
                    return TimeSpan.MinValue;
                return ActualArrivalTime.Timestamp - ActualDepartureTime.Timestamp;
            }
        }

        public List<Telemetry> TelemetryLog
        {
            get;
            private set;
        }

        public FlightPlan LoadedFlightPlan
        {
            get; private set;
        }
        #endregion Properties

        /// <summary>
        /// Handle flight phases
        /// </summary>
        public Telemetry HandleFlightPhases()
        {
            // calculate all telemetry data we need
            Telemetry currentTelemetry = Telemetry.GetCurrent();

            // handle switching phase
            switch (phase)
            {
                case FlightPhases.PREFLIGHT:
                    if (currentTelemetry.Engine1 && !currentTelemetry.ParkingBrake)
                        phase = FlightPhases.PUSHBACK;
                    break;
                case FlightPhases.PUSHBACK:
                    if (currentTelemetry.Engine1 && !currentTelemetry.ParkingBrake && currentTelemetry.IndicatedAirSpeed >= 10)
                        phase = FlightPhases.TAXIOUT;
                    break;
                case FlightPhases.TAXIOUT:
                    if (currentTelemetry.Engine1 && currentTelemetry.IndicatedAirSpeed >= 30)
                    {
                        ActualDepartureTime = currentTelemetry;
                        phase = FlightPhases.TAKEOFF;
                    }
                    break;
                case FlightPhases.TAKEOFF:
                    if (!currentTelemetry.OnGround)
                        phase = FlightPhases.CLIMBING;
                    break;
                case FlightPhases.CLIMBING:
                    if (currentTelemetry.VerticalSpeed <= 100 && currentTelemetry.VerticalSpeed >= -100 && !currentTelemetry.OnGround)
                        phase = FlightPhases.CRUISE;
                    else if (currentTelemetry.VerticalSpeed <= 100 && !currentTelemetry.OnGround)
                        phase = FlightPhases.DESCENDING;
                    break;
                case FlightPhases.CRUISE:
                    if (currentTelemetry.VerticalSpeed <= 100 && !currentTelemetry.OnGround)
                        phase = FlightPhases.DESCENDING;
                    else if (currentTelemetry.VerticalSpeed >= 100 && !currentTelemetry.OnGround)
                        phase = FlightPhases.CLIMBING;
                    break;
                case FlightPhases.DESCENDING:
                    if (!currentTelemetry.OnGround && currentTelemetry.IndicatedAirSpeed <= 200 && currentTelemetry.Altitude <= 6000)
                        phase = FlightPhases.APPROACH;
                    else if (currentTelemetry.VerticalSpeed >= 100 && !currentTelemetry.OnGround)
                        phase = FlightPhases.CLIMBING;
                    break;
                case FlightPhases.APPROACH:
                    if (currentTelemetry.OnGround)
                    {
                        ActualArrivalTime = currentTelemetry;
                        phase = FlightPhases.LANDING;
                    }
                    else if (currentTelemetry.VerticalSpeed >= 100)
                        phase = FlightPhases.CLIMBING;
                    break;
                case FlightPhases.LANDING:
                    if (currentTelemetry.IndicatedAirSpeed <= 40 && currentTelemetry.OnGround)
                        phase = FlightPhases.TAXIIN;
                    break;
            }

            currentTelemetry.FlightPhase = phase;
            TelemetryLog.Add(currentTelemetry);
            return currentTelemetry;
        }
    }
}
