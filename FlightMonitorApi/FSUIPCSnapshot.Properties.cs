using FSUIPC;
using System;

namespace FlightMonitorApi
{
    public partial class FSUIPCSnapshot
    {
        /// <summary>
        /// Position
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// json output for API
        /// </summary>
        static public Offset<long> _longitude = new Offset<long>(0x0568);
        static public Offset<long> _latitude = new Offset<long>(0x0560);
        private static double[] position
        {
            get
            {
                return new double[] {
                    _latitude.Value * (90.0 / (10001750.0 * 65536.0 * 65536.0)),
                    _longitude.Value * (360.0 / (65536.0 * 65536.0 * 65536.0 * 65536.0))
                };
            }
        }
        public double[] Position
        { get; set; }

        /// <summary>
        /// Compass heading
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<double> _compass = new Offset<double>(0x02CC);
        private static int compass
        {
            get
            {
                return (int)Math.Round(_compass.Value);
            }
        }
        public int Compass
        { get; set; }

        /// <summary>
        /// Altitude
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _altitude = new Offset<int>(0x3324);
        private static int altitude
        {
            get
            {
                return _altitude.Value;
            }
        }
        public int Altitude
        { get; set; }

        /// <summary>
        /// Groundspeed
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _groundspeed = new Offset<int>(0x02B4);
        private static int groundspeed
        {
            get
            {
                return Convert.ToInt32((_groundspeed.Value / 65536) * 1.94384449);
            }
        }
        public int GroundSpeed
        { get; set; }

        /// <summary>
        /// Indicated Airspeed
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _indicatedairspeed = new Offset<int>(0x02BC);
        private static int indicatedairspeed
        {
            get
            {
                return _indicatedairspeed.Value / 128;
            }
        }
        public int IndicatedAirspeed
        { get; set; }

        /// <summary>
        /// True Airspeed
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _trueairspeed = new Offset<int>(0x02B8);
        private static int trueairspeed
        {
            get
            {
                return _trueairspeed.Value / 128;
            }
        }
        public int TrueAirpeed
        { get; set; }

        /// <summary>
        /// On ground value
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<short> _onground = new Offset<short>(0x0366, false);
        private static bool onground
        {
            get
            {
                return (_onground.Value == 1);
            }
        }
        public bool OnGround
        { get; set; }

        /// <summary>
        /// Pressure setting in millibars
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<short> _qnh = new Offset<short>(0x0330);
        private static short qnh
        {
            get
            {
                return Convert.ToInt16(_qnh.Value / 16);
            }
        }
        public short QNH
        { get; set; }
    }
}
