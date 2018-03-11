using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace FlightMonitorApi
{
    /// <summary>
    /// Flight Monitor Properties
    /// </summary>
    public partial class FlightMonitor
    {
        /// <summary>
        /// Data service provider for flight plans, position reports, and all data
        /// related to the flight not coming from the sim
        /// </summary>
        public IDataConnector DataConnector;

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
        public ConcurrentQueue<FSUIPCSnapshot> Queue;

        /// <summary>
        /// The last queued object for interest comparisons
        /// </summary>
        private FSUIPCSnapshot lastQueued;

        /// <summary>
        /// Gets or sets the list of resgistered interests in the profile
        /// </summary>
        public List<SnapshotInterest> Interests;

        /// <summary>
        /// Flight monitoring worker
        /// 
        /// Keeps countinously pooling FS data, queueing all resgistered interests
        /// </summary>
        private Thread monitoringThread;

        /// <summary>
        /// Data provider worker
        /// </summary>
        private Thread databaseThread;

        /// <summary>
        /// Gets or sets the running state of the sim monitoring thread.
        /// 
        /// Setting to false does not kill the thread, it just signals it to stop
        /// working the next time they check this value (once every worker cycle)
        /// </summary>
        private bool monitorRunning = false;

        /// <summary>
        /// Gets or sets the running state of the database pusher thread.
        /// 
        /// Setting to false does not kill the thread, it just signals it to stop
        /// working the next time they check this value (once every worker cycle)
        /// 
        /// One last cycle will execute, or any number of cycles required to empty
        /// the snapshot queue.
        /// </summary>
        private bool databaseRunning = false;

        private void InitializeComponent()
        {
            databaseRunning = false;
            monitorRunning = false;
            databaseThread = new Thread(new ThreadStart(DatabaseWorker));
            monitoringThread = new Thread(new ThreadStart(MonitoringWorker));
            Interests = new List<SnapshotInterest>();
            lastQueued = null;
            Queue = new ConcurrentQueue<FSUIPCSnapshot>();
        }
    }
}
