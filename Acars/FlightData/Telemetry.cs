using FSUIPC;
using System;

namespace Acars.FlightData
{
    public class Telemetry
    {
        /// <summary>
        /// UTC time of collection
        /// </summary>
        public DateTime Timestamp;
        /// <summary>
        /// Kts
        /// </summary>
        public int IndicatedAirSpeed;
        /// <summary>
        /// Angle
        /// </summary>
        public int Pitch;
        /// <summary>
        /// Angle
        /// </summary>
        public int Bank;
        /// <summary>
        /// Engine 1 running state
        /// </summary>
        public bool Engine1;
        /// <summary>
        /// Parking Brake state
        /// </summary>
        public bool ParkingBrake;
        /// <summary>
        /// Returns true if aircraft is on ground, false otherwise
        /// </summary>
        public bool OnGround;
        /// <summary>
        /// Vertical Speed in ft/min
        /// </summary>
        public double VerticalSpeed;
        /// <summary>
        /// Throttle position ?%
        /// </summary>
        public short Throttle;
        /// <summary>
        /// Altitude? MSL AGL ... ?, in ft most likely
        /// </summary>
        public double Altitude;

        public static Telemetry GetCurrent()
        {
            Telemetry result = new Telemetry();

            // snapshot data
            FSUIPCConnection.Process();
            result.Timestamp = DateTime.UtcNow;

            // capture values
            result.IndicatedAirSpeed = (indicatedAirSpeed.Value / 128);
            result.Pitch = ((((pitch.Value) / 360) / 65536) * 2) / -1;
            result.Bank = (((bank.Value) / 360) / 65536) * 2;
            result.Engine1 = (engine1.Value == 0) ? false : true;
            result.ParkingBrake = (parkingBrake.Value == 0) ? false : true;
            result.OnGround = (onGround.Value == 0) ? false : true;
            result.VerticalSpeed = (verticalSpeed.Value * 3.28084) / -1;
            result.Throttle = throttle.Value;
            result.Altitude = (altitude.Value * 3.2808399);

            return result;
        }

        private static Offset<int> indicatedAirSpeed = new Offset<int>(0x02BC);
        static private Offset<int> pitch = new Offset<int>(0x0578);
        static private Offset<int> bank = new Offset<int>(0x057C);
        static private Offset<short> engine1 = new Offset<short>(0x0894);
        static private Offset<short> parkingBrake = new Offset<short>(0x0BC8, false);
        static private Offset<int> airspeed = new Offset<int>(0x02BC);
        static private Offset<short> onGround = new Offset<short>(0x0366, false);
        static private Offset<short> verticalSpeed = new Offset<short>(0x0842);
        static private Offset<short> throttle = new Offset<short>(0x088C);
        static private Offset<Double> altitude = new Offset<Double>(0x6020);
    }
}
