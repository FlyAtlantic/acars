using FSUIPC;
using System;
using System.Linq;
using System.Threading;

namespace FlightMonitorApi
{
    public partial class FSUIPCSnapshot
    {
        /// <summary>
        /// Set when connected to FSUIPC
        /// Gets unset when we can confirm connection was lost
        /// </summary>
        private static bool connected;

        /// <summary>
        /// UTC timestamp of the snapshot, has nothing to do with the actual data
        /// returned, is just a global timestamp for when the data was received
        /// from the underlying Sim connection
        /// </summary>
        public DateTime TimeStamp
        { get; set; }

        public FSUIPCSnapshot()
            : this(false) { }

        public FSUIPCSnapshot(bool InitializeComponent = true)
        {
            TimeStamp = DateTime.UtcNow;

            if (InitializeComponent)
                this.InitializeComponent();
        }

        public void InitializeComponent()
        {
            this.Position = position;
            this.Compass = compass;
            this.Altitude = altitude;
            this.GroundSpeed = groundspeed;
            this.IndicatedAirspeed = indicatedairspeed;
            this.TrueAirpeed = trueairspeed;
            this.OnGround = onground;
            this.QNH = qnh;
        }

        /// <summary>
        /// Returns an FSUIPCSnapshot instance with the current state of the sim
        /// 
        /// 
        /// </summary>
        /// <param name="connectCooldown">Time between reconnect tries to Simulator, in miliseconds.
        /// Defaults to 30000.</param>
        /// <returns></returns>
        public static FSUIPCSnapshot Pool(int connectCooldown = 30000)
        {
            // TODO: do not block the current thread on this
            //       application may receive a OS taskkill command.
            //       Maybe expose connected = true?
            while (!connected)
            {
                try
                {
                    FSUIPCConnection.Open();

                    connected = true;
                }
                catch (FSUIPCException crap)
                {
                    switch (crap.FSUIPCErrorCode)
                    {
                        case FSUIPCError.FSUIPC_ERR_OPEN:
                            connected = true;
                            break;
                        case FSUIPCError.FSUIPC_ERR_NOFS:
                        case FSUIPCError.FSUIPC_ERR_SENDMSG:
                            Thread.Sleep(connectCooldown);
                            break;
                        default:
                            throw new ApplicationException(
                                "Unexpected exception trying FSUIPC.Open(). Check inner exception.",
                                crap);
                    }
                }
            }

            try
            {
                FSUIPCConnection.Process();
            }
            catch (Exception crap)
            {
                // TODO: catch ONLY relevant execeptions
                //       connected = false;
                throw new ApplicationException("Provider.Pool() failed with:", crap);
            }

            if (connected)
            {
                FSUIPCSnapshot data = new FSUIPCSnapshot(true);

                // FSUIPC will return data even if the user is in scenario screen
                // or any other screen really.
                // TODO: actually filter invalid locations
                return data.Position.SequenceEqual(new double[] { 0, 0 }) ? null : data;
            }

            return null;
        }
    }
}
