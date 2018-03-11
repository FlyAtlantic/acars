namespace Acars.FlightData
{
    public class Flight
    {



        ///
        /// Below is the various conditions we must add as interests to the flight
        /// monitor API
        ///
        ///
        //public Flight(FlightPhases initialPhase = FlightPhases.PREFLIGHT)
        //{
        //    #region Register Events
        //    activeEvents = new FlightEvent[] {
        //        new FlightEvent("2A", 5, "Bank Angle Exceeded"                       , 30, 30, (t) => { return (t.Bank > 30); }),
        //        new FlightEvent("3A", 5, "Landing lights on above 10000 ft"     , 5 , 5 , (t) => { return (t.LandingLights && (t.Altitude > 10500)); }),
        //        new FlightEvent("3B", 5, "Landing lights off durring approach"  , 5 , 5 , (t) => { return (!t.LandingLights && (t.RadioAltitude < 2750 || t.Altitude < 2500) && phase == FlightPhases.APPROACH); }),
        //        new FlightEvent("3C", 5, "Speed above 250 IAS bellow 10000 ft"  , 10, 10, (t) => { return ((t.IndicatedAirSpeed > 255) && (t.Altitude < 9500)); }),
        //        new FlightEvent("3D", 5, "High Speed On Taxi Out"                      , 5 , 5, (t) => { return ((t.GroundSpeed > 30 && t.OnGround) && phase == FlightPhases.TAXIOUT); }),
        //        new FlightEvent("3E", 5, "High Speed On Taxi In"                      , 5 , 5, (t) => { return ((t.GroundSpeed > 30 && t.OnGround) && phase == FlightPhases.TAXIIN); }),
        //        new FlightEvent("4A", 5, "Landing light on above 250 IAS"       , 5 , 5 , (t) => { return (t.LandingLights && t.IndicatedAirSpeed > 255); }),
        //        new FlightEvent("4B", 5, "Landing lights Off On TakeOff"  , 5 , 5 , (t) => { return (!t.LandingLights && phase == FlightPhases.TAKEOFF); }),
        //        new FlightEvent("4C", 5, "Pitch High Below 1500ft on Departure(Radio)"             , 10, 10, (t) => { return ((t.RadioAltitude < 1500) && t.Pitch > 20); }),
        //        new FlightEvent("4D", 5, "Gear down above 250 IAS"             , 10, 30, (t) => { return (t.Gear && t.IndicatedAirSpeed > 255); }),
        //        new FlightEvent("6A", 1, "Maximum TakeOff Weight Excceded"             , 30, 30, (t) => { return (phase == FlightPhases.TAKEOFF && (t.GrossWeight > LoadedFlightPlan.Aircraft.MTW)); }),
        //        new FlightEvent("6B", 1, "Maximum Landing Weight Excceded"             , 30, 30, (t) => { return (phase == FlightPhases.LANDING && (t.GrossWeight > LoadedFlightPlan.Aircraft.MLW)); }),
        //        new FlightEvent("6C", 1, "Maximum Service Ceiling Excceded"             , 30, 30, (t) => { return (!t.OnGround && (t.Altitude > LoadedFlightPlan.Aircraft.Celling)); }),
        //        //new FlightEvent("6D", 1, "Maximum Speed Flap Excceded"             , 30, 30, (t) => {
        //        //    var flapSetting = LoadedFlightPlan.Aircraft.FlapSettings.OrderBy(x => x.Key).Where(x => x.Key < t.Flaps).FirstOrDefault().Value;
        //        //    return (!t.OnGround && (t.IndicatedAirSpeed > flapSetting.IASLimit));
        //        //}),

        //        new FlightEvent("6D", 1, "Maximum Speed Flap Excceded"                       , 30, 30, (t) => { return LoadedFlightPlan.Aircraft.FlapSettings.ContainsKey(t.Flaps) && (LoadedFlightPlan.Aircraft.FlapSettings[t.Flaps].IASLimit < t.IndicatedAirSpeed); }),
        //        new FlightEvent("7B", 5, "Pitch too high"                       , 30, 30, (t) => { return (t.Pitch > 30); })
        //    };
        //    #endregion Register Events

        //    phase = initialPhase;
        //    FlightRunning = false;

        //    TelemetryLog = new List<Telemetry>();

        //    ActualArrivalTimeId = -1;
        //    ActualDepartureTimeId = -1;

        //    LoadedFlightPlan = null;

        //    FinalScore = 100;

        //    Events = new List<EventOccurrence>();
        //}
        ///// <summary>
        ///// Handle flight phases
        ///// </summary>
        //public Telemetry HandleFlightPhases(Telemetry currentTelemetry = null)
        //{
        //    if(currentTelemetry == null)
        //        currentTelemetry = Telemetry.GetCurrent();

        //    // handle switching phase
        //    switch (phase)
        //    {
        //        case FlightPhases.PREFLIGHT:
        //            if (currentTelemetry.Engine1 && !currentTelemetry.ParkingBrake)
        //                phase = FlightPhases.PUSHBACK;
        //            break;
        //        case FlightPhases.PUSHBACK:
        //            if (currentTelemetry.Engine1 && !currentTelemetry.ParkingBrake && currentTelemetry.IndicatedAirSpeed >= 10)
        //                phase = FlightPhases.TAXIOUT;
        //            break;
        //        case FlightPhases.TAXIOUT:
        //            if (currentTelemetry.Engine1 && currentTelemetry.IndicatedAirSpeed >= 30)
        //            {
        //                ActualDepartureTimeId = TelemetryLog.Count; // works because we will be inserting the current telemetry data to the TelemetryLog
        //                phase = FlightPhases.TAKEOFF;
        //            }
        //            break;
        //        case FlightPhases.TAKEOFF:
        //            if (!currentTelemetry.OnGround)
        //                phase = FlightPhases.CLIMBING;
        //            break;
        //        case FlightPhases.CLIMBING:
        //            if (currentTelemetry.VerticalSpeed <= 100 && currentTelemetry.VerticalSpeed >= -100 && !currentTelemetry.OnGround)
        //                phase = FlightPhases.CRUISE;
        //            else if (currentTelemetry.VerticalSpeed <= -100 && !currentTelemetry.OnGround && LastTelemetry.Location.GetDistanceTo(LoadedFlightPlan.ArrivalAirfield.Position) > 18000)
        //                phase = FlightPhases.DESCENDING;
        //            break;
        //        case FlightPhases.CRUISE:
        //            if (currentTelemetry.VerticalSpeed <= -100 && !currentTelemetry.OnGround && LastTelemetry.Location.GetDistanceTo(LoadedFlightPlan.ArrivalAirfield.Position) > 18000)
        //                phase = FlightPhases.DESCENDING;
        //            else if (currentTelemetry.VerticalSpeed >= 100 && !currentTelemetry.OnGround)
        //                phase = FlightPhases.CLIMBING;
        //            else if (!currentTelemetry.OnGround && currentTelemetry.IndicatedAirSpeed <= 200 && LastTelemetry.Location.GetDistanceTo(LoadedFlightPlan.ArrivalAirfield.Position) < 18000)
        //                phase = FlightPhases.APPROACH;
        //            break;
        //        case FlightPhases.DESCENDING:
        //            if (!currentTelemetry.OnGround && currentTelemetry.IndicatedAirSpeed <= 200 && LastTelemetry.Location.GetDistanceTo(LoadedFlightPlan.ArrivalAirfield.Position) < 18000)
        //                phase = FlightPhases.APPROACH;
        //            else if (currentTelemetry.VerticalSpeed >= 100 && !currentTelemetry.OnGround)
        //                phase = FlightPhases.CLIMBING;
        //            else if (currentTelemetry.VerticalSpeed <= 100 && currentTelemetry.VerticalSpeed >= -100 && !currentTelemetry.OnGround)
        //                phase = FlightPhases.CRUISE;
        //            break;
        //        case FlightPhases.APPROACH:
        //            if (currentTelemetry.OnGround)
        //            {
        //                ActualArrivalTimeId = TelemetryLog.Count; // again, works because we will be inserting the current telemetry data to the TelemetryLog
        //                phase = FlightPhases.LANDING;
        //            }
        //            else if (currentTelemetry.VerticalSpeed >= 100)
        //                phase = FlightPhases.CLIMBING;
        //            break;
        //        case FlightPhases.LANDING:
        //            if (currentTelemetry.IndicatedAirSpeed <= 40 && currentTelemetry.OnGround)
        //                phase = FlightPhases.TAXIIN;
        //            break;
        //        case FlightPhases.TAXIIN:
        //            if (!currentTelemetry.Engine1 && !currentTelemetry.Engine2 && !currentTelemetry.Engine3 && !currentTelemetry.Engine4 && currentTelemetry.ParkingBrake)
        //                phase = FlightPhases.PARKING;
        //            break;
        //    }
        //    currentTelemetry.FlightPhase = phase;
        //    return currentTelemetry;
        //}

        //private bool IsUpdateRequired()
        //{
        //    if (LastUpdate == null)
        //        return true;

        //    // check for minimum time requirement for update (15min, 10min for 'safety reasons')
        //    TimeSpan timeDiff = LastTelemetry.Timestamp - LastUpdate.Timestamp;
        //    if (timeDiff.TotalMinutes >= 10)
        //        return true;

        //    // check flight phase change (TODO: discontinue)
        //    if (LastTelemetry.FlightPhase != LastUpdate.FlightPhase)
        //        return true;

        //    // check if altitude changed more than 50ft
        //    double altDiff = LastTelemetry.Altitude - LastUpdate.Altitude;
        //    if (Math.Abs(altDiff) >= 50.0)
        //        return true;

        //    // check speed changed more than 5 knots
        //    int spdDiff = (int)LastTelemetry.GroundSpeed - (int)LastUpdate.GroundSpeed;
        //    if (Math.Abs(spdDiff) >= 5)
        //        return true;

        //    // Heading changed more than 5 degrees (TODO: probably using trigonometry would be a wise idea)
        //    double hdgDiff = LastTelemetry.Compass - LastUpdate.Compass;
        //    if (Math.Abs(hdgDiff) >= 5.0)
        //        return true;

        //    //
        //    // TODOs
        //    //

        //    // Event triggered

        //    return false;
        //}
    }
}
