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
        /// Snapshot Queue used to to keep track of the data to be 
        /// published
        /// </summary>
        public ConcurrentQueue<FSUIPCSnapshot> Queue;

        /// <summary>
        /// Queue handle event
        /// </summary>
        private AutoResetEvent QueueHandle;

        /// <summary>
        /// Gets or sets the list of resgistered interests in the profile
        /// </summary>
        public List<FSUIPCInterest> Interests;

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
        private bool monitorRunning;

        /// <summary>
        /// Gets or sets the running state of the database pusher thread.
        /// 
        /// Setting to false does not kill the thread, it just signals it to stop
        /// working the next time they check this value (once every worker cycle)
        /// 
        /// One last cycle will execute, or any number of cycles required to empty
        /// the snapshot queue.
        /// </summary>
        private bool databaseRunning;

        private void InitializeComponent()
        {
            databaseRunning = false;
            monitorRunning = false;
            databaseThread = new Thread(new ThreadStart(DatabaseWorker));
            monitoringThread = new Thread(new ThreadStart(MonitoringWorker));
            Interests = new List<FSUIPCInterest>();
            Queue = new ConcurrentQueue<FSUIPCSnapshot>();
            QueueHandle = new AutoResetEvent(false);
        }
    }
}
