using System.Collections.Concurrent;
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
        public static ConcurrentBag<SnapshotInterest> Interests
            = new ConcurrentBag<SnapshotInterest>();

        /// <summary>
        /// Flight monitoring worker
        /// 
        /// Keeps countinously pooling FS data, queueing all resgistered interests
        /// </summary>
        private static Thread monitoringThread
            = new Thread(new ThreadStart(MonitoringWorker));

        /// <summary>
        /// Gets or sets the running state of all threads.
        /// 
        /// Setting to false does kill the threads, it just signals them to stop
        /// working the next time they check this value (once every worker cycle)
        /// </summary>
        private static bool running = false;
    }
}
