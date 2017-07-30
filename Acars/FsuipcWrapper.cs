using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSUIPC;

namespace Acars
{
    /// <summary>
    /// 
    /// </summary>
    public class FsuipcWrapper
    {
        #region warpping process and open sequence
        /// <summary>
        /// Time in milliseconds after which an FSUIPC.Process() call is required
        /// </summary>
        public const int processColldownTime = 1000;
        /// <summary>
        /// DateTime last time FSUIPC.Process() was called
        /// </summary>
        private DateTime lastProcessCall;
        /// <summary>
        /// Returns true if FSUIPC.Process() call is required
        /// </summary>
        private bool requiresProcessCall
        {
            get
            {
                // no last call
                if (lastProcessCall == null)
                    return true;

                // last call to long ago
                TimeSpan diff = DateTime.UtcNow - lastProcessCall;
                return (diff.TotalMilliseconds > processColldownTime);
            }
        }

        /// <summary>
        /// Returns FsuipcWrapper if able to FSUIPCConnetion.Open()
        /// otherwise returns null
        /// </summary>
        /// <returns></returns>
        public static FsuipcWrapper TryInstantiate()
        {
            try
            {
                FSUIPCConnection.Open();

                // if above fails we go directly to the catch statement
                // return a new class instance
                return new FsuipcWrapper();
            }
            catch (FSUIPCException ex)
            {
                // FSUIPCException is thrown in the event Open() fails
                //  ! not sure what more triggers that !
                return null;
            }
            catch (Exception ex)
            {
                // no clue what happen, let the application do it's logging
                throw ex;
            }
        }

        /// <summary>
        /// Handles 
        /// </summary>
        /// <returns></returns>
        private void process()
        {
            // for now just pass all expections to the application and call Process();
            try
            {
                if (requiresProcessCall)
                    FSUIPCConnection.Process();
                lastProcessCall = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion warpping process and open sequence

        #region offset declarations

        // CharacterPosition
        private Offset<long> characterLatitude = new Offset<long>(0x0560);
        private Offset<long> characterLongitude = new Offset<long>(0x0568);

        private Offset<long> characterAltitude = new Offset<long>(0x0570);

        // EnvironmentDateTime
        private Offset<byte[]> environmentDateTime = new Offset<byte[]>(0x0238, 10);
        #endregion offset declarations

        #region warp property getters and setters
        public FsLatLonPoint CharacterPosition
        {
            get
            {
                process();
                return new FsLatLonPoint(new FsLatitude(characterLatitude.Value),
                                         new FsLongitude(characterLongitude.Value));
            }
        }

        public long CharacterAltitude
        {
            get
            {
                process();
                return characterAltitude.Value;
            }
        }

        public DateTime EnvironmentDateTime
        {
            get
            {
                process();

                // get year in Simulator
                short year = BitConverter.ToInt16(environmentDateTime.Value, 8);

                // create a time based on Jan 1 of Simulator year
                DateTime result = new DateTime(year, 1, 1, environmentDateTime.Value[0], environmentDateTime.Value[1], environmentDateTime.Value[2]);

                // get day of year from Simulator, and add that to the above time
                short dayOfYear = BitConverter.ToInt16(environmentDateTime.Value, 6);

                // add and return
                return result.Add(new TimeSpan(dayOfYear - 1, 0, 0, 0));
            }
            set
            {
                // convert offseted values to their own byte[] arrays
                byte[] byteArrYear = BitConverter.GetBytes((short)value.Year);
                byte[] byteArrDayOfYear = BitConverter.GetBytes((short)value.DayOfYear - 1);

                // set year
                environmentDateTime.Value[8] = byteArrYear[0];
                environmentDateTime.Value[9] = byteArrYear[1];

                // set day of year
                environmentDateTime.Value[6] = byteArrDayOfYear[0];
                environmentDateTime.Value[7] = byteArrDayOfYear[1];

                // set hour, minute, and second
                environmentDateTime.Value[0] = (byte)value.Hour;
                environmentDateTime.Value[1] = (byte)value.Minute;
                environmentDateTime.Value[2] = (byte)value.Second;

                process();
            }
        }
        #endregion warp property getters and setters
    }
}
