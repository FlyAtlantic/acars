using FSUIPC;
using System;

namespace Acars.FlightData
{
    class FSUIPCOffsets
    {
        static public Offset<int> indicatedAirSpeed = new Offset<int>(0x02BC);
        static public Offset<int> pitch = new Offset<int>(0x0578);
        static public Offset<int> bank = new Offset<int>(0x057C);
        static public Offset<byte> engine1 = new Offset<byte>(0x0894);
        static public Offset<byte> engine2 = new Offset<byte>(0x092C);
        static public Offset<byte> engine3 = new Offset<byte>(0x09C4);
        static public Offset<byte> engine4 = new Offset<byte>(0x0A5C);
        static public Offset<byte> parkingBrake = new Offset<byte>(0x0BC8);
        static public Offset<int> parkingBrakeWrite = new Offset<int>(0x0BC8);
        static public Offset<int> groundspeed = new Offset<int>(0x02B4);
        static public Offset<long> machSpeed = new Offset<long>(0x11C6);
        static public Offset<short> onGround = new Offset<short>(0x0366, false);
        static public Offset<short> verticalSpeed = new Offset<short>(0x0842);
        static public Offset<short> throttle = new Offset<short>(0x088C);
        static public Offset<Double> altitude = new Offset<Double>(0x6020);
        static public Offset<short> gear = new Offset<short>(0x0BE8, false);
        static public Offset<int> slew = new Offset<int>(0x05DC);
        static public Offset<byte> pause = new Offset<byte>(0x0264);
        static public Offset<int> pauseWrite = new Offset<int>(0x0262);
        static public Offset<short> overSpeed = new Offset<short>(0x036D, false);
        static public Offset<short> stall = new Offset<short>(0x036C, false);
        static public Offset<short> battery = new Offset<short>(0x3102, false);
        static public Offset<short> landingLights = new Offset<short>(0x028C, false);
        static public Offset<double> grossWeight = new Offset<double>(0x30C0);
        static public Offset<int> zeroFuelWeight = new Offset<int>(0x3BFC);
        static public Offset<short> squawk = new Offset<short>(0x0354);
        static public Offset<byte[]> simTime = new Offset<byte[]>(0x023B, 10);
        static public Offset<int> simRate = new Offset<int>(0x0C1A);
        static public Offset<short> qnh = new Offset<short>(0x0330);
        static public Offset<int> engineCount = new Offset<int>(0x0AEC);
        static public Offset<Double> compass = new Offset<double>(0x02CC);
        static public Offset<long> longitude = new Offset<long>(0x0568);
        static public Offset<long> latitude = new Offset<long>(0x0560);
        static public Offset<string> aircraftType = new Offset<string>("AircraftInfo", 0x3160, 24);
        static public Offset<Double> RadioAltitude = new Offset<Double>(0x31E4);
        static public Offset<string> messageWrite = new Offset<string>(0x3380, 128);
        static public Offset<short> messageDuration = new Offset<short>(0x32FA);
        static public Offset<short> flapsControl = new Offset<short>(0x0BE0);

        static public Offset<byte[]> environmentDateTime = new Offset<byte[]>(0x0238, 10);
        static public Offset<byte[]> environmentDateTimeDayOfYear = new Offset<byte[]>(0x023E, 4);
        static public Offset<byte[]> environmentDateTimeHour = new Offset<byte[]>(0x023B, 4);
        static public Offset<byte[]> environmentDateTimeMinute = new Offset<byte[]>(0x023C, 4);
        static public Offset<byte[]> environmentDateTimeYear = new Offset<byte[]>(0x0240, 4);

        public static bool GetBool(Offset<short> offset)
        {
            return (offset.Value != 0);
        }

        public static int GetInt(Offset<int> offset)
        {
            return offset.Value;
        }
    }
}
