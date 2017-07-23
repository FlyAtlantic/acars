using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSUIPC;

namespace Acars
{
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

        #region warp property getters and setters

        #endregion warp property getters and setters
    }
}
