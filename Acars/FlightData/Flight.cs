using Acars.Events;
using FSUIPC;
using System;
using System.Collections.Generic;

namespace Acars.FlightData
{
    public class Flight
    {
        public static Flight Get()
        {
            return FlightDatabase.GetFlight();
        }

        public Flight(FlightPhases initialPhase = FlightPhases.PREFLIGHT)
        {
            #region Register Events
            activeEvents = new FlightEvent[] {
                new FlightEvent("4D", 5, "Gear down bellow 250 IAS"             , 10, 30, (t) => { return (t.Gear && t.IndicatedAirSpeed > 255); }),
                new FlightEvent("3B", 5, "Landing lights off durring approach"  , 5 , 5 , (t) => { return (t.LandingLights && t.Altitude < 2750); }),
                new FlightEvent("3A", 5, "Landing lights on above 10000 ft"     , 5 , 5 , (t) => { return (t.LandingLights && t.Altitude > 10500); }),
                new FlightEvent("4A", 5, "Landing light on above 250 IAS"       , 5 , 5 , (t) => { return (t.LandingLights && t.IndicatedAirSpeed > 255); }),
                new FlightEvent("7B", 5, "Pitch too high"                       , 10, 30, (t) => { return (t.Pitch > 30); }),
                new FlightEvent("3C", 5, "Speed above 250 IAS bellow 10000 ft"  , 10, 50, (t) => { return (t.IndicatedAirSpeed > 255 && t.Altitude < 9500); }),
                new FlightEvent("3D", 5, "High speed taxi"                      , 5 , 10, (t) => { return (t.GroundSpeed > 30 && t.OnGround); })
            };
            #endregion Register Events

            phase = initialPhase;

            TelemetryLog = new List<Telemetry>();

            ActualArrivalTimeId = -1;
            ActualDepartureTimeId = -1;

            LoadedFlightPlan = new FlightPlan();

            FinalScore = 100;
        }

        public bool verifyAirport()
        {
            //retorna distancia em metros
            bool verifyAirportPlayer = LoadedFlightPlan.DepartureCordinate.GetDistanceTo(new System.Device.Location.GeoCoordinate(LastTelemetry.Latitude, LastTelemetry.Longitude)) > 2000;
            return verifyAirportPlayer;
        }

        #region variables
        // instance
        private FlightPhases phase;

        private FlightEvent[] activeEvents;
        private int ActualDepartureTimeId;
        private int ActualArrivalTimeId;

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
        /// <summary>
        /// Flight Identifier on the database side
        /// </summary>
        public int FlightID
        {
            get;
            internal set;
        }

        public Telemetry ActualDepartureTime
        {
            get { return (ActualDepartureTimeId > -1) ? TelemetryLog[ActualDepartureTimeId] : null; }
        }

        public Telemetry ActualArrivalTime
        {
            get { return (ActualArrivalTimeId > -1) ? TelemetryLog[ActualArrivalTimeId] : null; }
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

        public FlightEvent[] Events
        {
            get;
            private set;
        }

        public int FinalScore
        { get; private set; }

        public int EfficiencyPoints
        { get; private set; }

        public Telemetry LastTelemetry
        {
            get
            {
                return TelemetryLog[TelemetryLog.Count - 1];
            }
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
                        ActualDepartureTimeId = TelemetryLog.Count; // works because we will be inserting the current telemetry data to the TelemetryLog
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
                        ActualArrivalTimeId = TelemetryLog.Count; // again, works because we will be inserting the current telemetry data to the TelemetryLog
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs"></param>
        public void StartFlight(FsuipcWrapper fs)
        {
            FlightDatabase.StartFlight(this);

            // instanciate FS wrapper
            while (fs == null)
                fs = FsuipcWrapper.TryInstantiate();

            //insere e verifica hora zulu no Simulador
            fs.EnvironmentDateTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void AnalyseFlightLog(bool updateScore = false)
        {
            foreach (FlightEvent e in activeEvents)
            {
                EventOccurrence[] r = e.GetOccurrences(TelemetryLog.ToArray(), out int discount);

                FinalScore -= discount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndFlight()
        {
            AnalyseFlightLog(true);

            // calculate flight efficiency
            EfficiencyPoints = Convert.ToInt32(Math.Round(ActualTimeEnRoute.TotalMinutes / 10 * (FinalScore / 1)));

            // do database stuff
            FlightDatabase.EndFlight(this);
        }
    }
}
