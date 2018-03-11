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
        private static Offset<long> _longitude = new Offset<long>(0x0568);
        private static Offset<long> _latitude = new Offset<long>(0x0560);
        private static double[] position
        {
            get
            {
                return new double[] {
                    _latitude.Value * (90.0 / (10001750.0 * 65536.0 * 65536.0)),
                    _longitude.Value *
                    (360.0 / (65536.0 * 65536.0 * 65536.0 * 65536.0))
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

        /// <summary>
        /// pitch setting in degrees
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _pitch = new Offset<int>(0x0578);
        private static int pitch
        {
            get
            {
                return Convert.ToInt32((((_pitch.Value / 360) / 65536) *2) / -1);
            }
        }
        public int Pitch
        { get; set; }

        /// <summary>
        /// bank setting in degrees
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _bank = new Offset<int>(0x057C);
        private static int bank
        {
            get
            {
                return Convert.ToInt32(((_bank.Value / 360) / 65536) * 2);
            }
        }
        public int Bank
        { get; set; }

        /// <summary>
        /// engine1 setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<byte> _engine1 = new Offset<byte>(0x0894);
        private static byte engine1
        {
            get
            {
                return Convert.ToByte((_engine1.Value == 0) ? false : true);
            }
        }
        public byte Engine1
        { get; set; }

        /// <summary>
        /// engine2 setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<byte> _engine2 = new Offset<byte>(0x092C);
        private static byte engine2
        {
            get
            {
                return Convert.ToByte((_engine2.Value == 0) ? false : true);
            }
        }
        public byte Engine2
        { get; set; }

        /// <summary>
        /// engine3 setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<byte> _engine3 = new Offset<byte>(0x09C4);
        private static byte engine3
        {
            get
            {
                return Convert.ToByte((_engine3.Value == 0) ? false : true);
            }
        }
        public byte Engine3
        { get; set; }

        /// <summary>
        /// engine4 setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<byte> _engine4 = new Offset<byte>(0x0A5C);
        private static byte engine4
        {
            get
            {
                return Convert.ToByte((_engine4.Value == 0) ? false : true);
            }
        }
        public byte Engine4
        { get; set; }

        /// <summary>
        /// parkingbrakes setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<byte> _parkingbrakes = new Offset<byte>(0x0BC8);
        private static byte parkingbrakes
        {
            get
            {
                return Convert.ToByte((_parkingbrakes.Value == 0) ? false : true);
            }
        }
        public byte Parkingbrakes
        { get; set; }

        /// <summary>
        /// parkingbrakes write setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _parkingbrakeswrite = new Offset<int>(0x0BC8);
        private static int parkingbrakeswrite
        {
            get
            {
                return Convert.ToInt32(_parkingbrakeswrite.Value);
            }
        }
        public int ParkingbrakesWrite
        { get; set; }

        /// <summary>
        /// verticalSpeed setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<short> _verticalSpeed = new Offset<short>(0x0842);
        private static short verticalSpeed
        {
            get
            {
                return Convert.ToInt16((_verticalSpeed.Value * 3.28084) / -1);
            }
        }
        public short VerticalSpeed
        { get; set; }

        /// <summary>
        /// throttle setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<short> _throttle = new Offset<short>(0x088C);
        private static short throttle
        {
            get
            {
                return Convert.ToInt16(_throttle.Value);
            }
        }
        public short Throttle
        { get; set; }

        /// <summary>
        /// gear setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<short> _gear = new Offset<short>(0x0BE8);
        private static short gear
        {
            get
            {
                return Convert.ToInt16(_gear.Value);
            }
        }
        public short Gear
        { get; set; }

        /// <summary>
        /// slew setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _slew = new Offset<int>(0x05DC);
        private static int slew
        {
            get
            {
                return Convert.ToInt32(_slew.Value);
            }
        }
        public int Slew
        { get; set; }

        /// <summary>
        /// pause setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<byte> _pause = new Offset<byte>(0x0264);
        private static byte pause
        {
            get
            {
                return Convert.ToByte((_pause.Value == 0) ? false : true);
            }
        }
        public byte Pause
        { get; set; }

        /// <summary>
        /// pauseWrite setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _pauseWrite = new Offset<int>(0x0262);
        private static int pauseWrite
        {
            get
            {
                return Convert.ToInt32(_pauseWrite.Value);
            }
        }
        public int PauseWrite
        { get; set; }

        /// <summary>
        /// LandingLights setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<short> _landingLights = new Offset<short>(0x028C);
        private static short landingLights
        {
            get
            {
                return Convert.ToInt16((_landingLights.Value == 0) ? false : true);
            }
        }
        public short LandingLights
        { get; set; }

        /// <summary>
        /// Squawk setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<short> _squawk = new Offset<short>(0x028C);
        private static short squawk
        {
            get
            {
                return Convert.ToInt16(_squawk.Value);
            }
        }
        public short Squawk
        { get; set; }

        /// <summary>
        /// SimRate setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _simRate = new Offset<int>(0x0C1A);
        private static int simRate
        {
            get
            {
                return Convert.ToInt32(_simRate.Value / 256);
            }
        }
        public int SimRate
        { get; set; }

        /// <summary>
        /// engineCount setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _engineCount = new Offset<int>(0x0AEC);
        private static int engineCount
        {
            get
            {
                return Convert.ToInt32(_engineCount.Value);
            }
        }
        public int EngineCount
        { get; set; }

        /// <summary>
        /// radioAltitude setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<Double> _radioAltitude = new Offset<Double>(0x31E4);
        private static Double radioAltitude
        {
            get
            {
                return Convert.ToDouble(_radioAltitude.Value / 65536);
            }
        }
        public Double RadioAltitude
        { get; set; }

        /// <summary>
        /// zeroFuelWeight setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<double> _zeroFuelWeight = new Offset<double>(0x30C0);
        private static double zeroFuelWeight
        {
            get
            {
                return Convert.ToDouble((_zeroFuelWeight.Value / 256) * 0.45359237);
            }
        }
        public double ZeroFuelWeight
        { get; set; }

        /// <summary>
        /// grossWeight setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<int> _grossWeight = new Offset<int>(0x30C0);
        private static int grossWeight
        {
            get
            {
                return Convert.ToInt32(_grossWeight.Value * 0.45359237);
            }
        }
        public int GrossWeight
        { get; set; }


        /// <summary>
        /// messageWrite setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<string> _messageWrite = new Offset<string>(0x3380, 128);
        private static string messageWrite
        {
            get
            {
                return Convert.ToString(_messageWrite.Value);
            }
        }
        public string MessageWrite
        { get; set; }

        /// <summary>
        /// messageDuration setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<short> _messageDuration = new Offset<short>(0x32FA);
        private static short messageDuration
        {
            get
            {
                return Convert.ToInt16(_messageDuration.Value);
            }
        }
        public short MessageDuration
        { get; set; }

        /// <summary>
        /// flaps setting
        /// 
        /// Offsets
        /// static proterty for correctly processing Offset data
        /// instance property
        /// </summary>
        private static Offset<short> _flaps = new Offset<short>(0x0BE0);
        private static short flaps
        {
            get
            {
                return Convert.ToInt16(_flaps.Value);
            }
        }
        public short Flaps
        { get; set; }
    }
}
