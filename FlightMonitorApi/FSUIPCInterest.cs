using NLog;
using System;

namespace FlightMonitorApi
{
    /// <summary>
    /// Delegate that represents a condition to be checked,
    /// if true the snapshot being analyzed will be added to
    /// the Queue.
    /// </summary>
    /// <returns></returns>
    public delegate bool ScenarioDelegate(
        FSUIPCSnapshot Snapshot,
        ref FSUIPCSnapshot Cached);

    public class FSUIPCInterest
    {
        /// <summary>
        /// A representation of the last snapshot queued for this interest
        /// </summary>
        public FSUIPCSnapshot Cached
        { get; set; }

        /// <summary>
        /// Scenario scpecification
        /// </summary>
        public ScenarioDelegate Scenario
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Snapshot"></param>
        /// <returns></returns>
        public bool IsInteresting(FSUIPCSnapshot Snapshot)
        {
            bool IsIt = false;
            FSUIPCSnapshot _cached = Cached;
            try
            {
                IsIt = Scenario(Snapshot, ref _cached);
            }
            catch (Exception crap)
            {
                // TODO: maybe pass this to the underlying application would
                //       actually be a better approach
                LogManager.GetCurrentClassLogger().Error(
                    crap,
                    "Snapshot scenario validation resulted in an unhandled " +
                    "expection.");
                return false;
            }
            Cached = _cached ?? Cached;
            return IsIt;
        }
    }
}
