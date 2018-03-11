using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace FlightMonitorApi
{
    /// <summary>
    /// Flight Monitor Properties
    /// </summary>
    public static partial class FlightMonitor
    {
        /// <summary>
        /// Delegate that represents a condition to be checked,
        /// if true the snapshot being analyzed will be added to
        /// the Queue.
        /// </summary>
        /// <param name="queued">The last queued snapshot</param>
        /// <param name="contenter">The current snapshot</param>
        /// <returns></returns>
        public delegate bool SnapshotInterest(
            FSUIPCSnapshot queued,
            FSUIPCSnapshot contenter);

        /// <summary>
        /// Snapshot Queue used to to keep track of the data to be 
        /// published
        /// </summary>
        public static ConcurrentQueue<FSUIPCSnapshot> Queue
            = new ConcurrentQueue<FSUIPCSnapshot>();

        /// <summary>
        /// The last queued object for interest comparisons
        /// </summary>
        private static FSUIPCSnapshot lastQueued = null;

        /// <summary>
        /// Gets or sets the list of resgistered interests in the profile
        /// </summary>
        public static List<SnapshotInterest> Interests
            = new List<SnapshotInterest>();

        /// <summary>
        /// Flight monitoring worker
        /// 
        /// Keeps countinously pooling FS data, queueing all resgistered interests
        /// </summary>
        private static Thread monitoringThread
            = new Thread(new ThreadStart(MonitoringWorker));

        /// <summary>
        /// Gets or sets the running state of the sim monitoring thread.
        /// 
        /// Setting to false does not kill the thread, it just signals it to stop
        /// working the next time they check this value (once every worker cycle)
        /// </summary>
        private static bool monitorRunning = false;

        /// <summary>
        /// Gets or sets the running state of the database pusher thread.
        /// 
        /// Setting to false does not kill the thread, it just signals it to stop
        /// working the next time they check this value (once every worker cycle)
        /// 
        /// One last cycle will execute, or any number of cycles required to empty
        /// the snapshot queue.
        /// </summary>
        private static bool databaseRunning = false;
    }
}
