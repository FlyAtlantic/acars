using FSUIPC;
using System;

namespace Acars.FlightData
{
    class FSUIPCOffsets
    {
        static public Offset<int> indicatedAirSpeed = new Offset<int>(0x02BC);
        static public Offset<int> pitch = new Offset<int>(0x0578);
        static public Offset<int> bank = new Offset<int>(0x057C);
        static public Offset<short> engine1 = new Offset<short>(0x0894);
        static public Offset<short> engine2 = new Offset<short>(0x092C);
        static public Offset<short> engine3 = new Offset<short>(0x09C4);
        static public Offset<short> engine4 = new Offset<short>(0x0A5C);
        static public Offset<short> parkingBrake = new Offset<short>(0x0BC8, false);
        static public Offset<int> airspeed = new Offset<int>(0x02BC);
        static public Offset<short> onGround = new Offset<short>(0x0366, false);
        static public Offset<short> verticalSpeed = new Offset<short>(0x0842);
        static public Offset<short> throttle = new Offset<short>(0x088C);
        static public Offset<Double> altitude = new Offset<Double>(0x6020);
        static public Offset<short> gear = new Offset<short>(0x0BE8, false);
        static public Offset<short> slew = new Offset<short>(0x05DC, false);
        static public Offset<short> pause = new Offset<short>(0x0264, false);
        static public Offset<short> overSpeed = new Offset<short>(0x036D, false);
        static public Offset<short> stall = new Offset<short>(0x036C, false);
        static public Offset<short> battery = new Offset<short>(0x3102, false);
        static public Offset<short> landingLights = new Offset<short>(0x028C, false);
        static public Offset<double> grossWeight = new Offset<double>(0x30C0);
        static public Offset<int> zeroFuelWeight = new Offset<int>(0x3BFC);
        static public Offset<short> squawk = new Offset<short>(0x0354);
        static public Offset<byte[]> SimTime = new Offset<byte[]>(0x023B, 10);
        static public Offset<int> SimRate = new Offset<int>(0x0C1A);
        static public Offset<short> QNH = new Offset<short>(0x0330);
        static public Offset<int> EnginesNumber = new Offset<int>(0x0AEC);
        static public Offset<Double> compass = new Offset<double>(0x02CC);
        static public Offset<long> Longitude = new Offset<long>(0x0568);
        static public Offset<long> Latitude = new Offset<long>(0x0560);
    }
}
