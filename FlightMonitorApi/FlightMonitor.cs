using System.Threading;

namespace FlightMonitorApi
{
    public partial class FlightMonitor
    {
        public FlightMonitor(IDataConnector DataConnector)
        {
            this.DataConnector = DataConnector;
            InitializeComponent();
        }

        /// <summary>
        /// Starts monitoring and publishing threads
        /// 
        /// Use SignalStop() to request them stop asap
        ///     Note: if any thread is waiting (for a db timeout for example) it
        ///     will still wait for that timeout before dying.
        /// </summary>
        public void StartWorkers()
        {
            StartDatabaseWorker();
        }

        private void StartMonitoringWorker()
        {
            monitorRunning = true;
            if (monitoringThread.IsAlive)
                return;
            monitoringThread.Start();
        }

        private void StartDatabaseWorker()
        {
            databaseRunning = true;
            if (databaseThread.IsAlive)
                return;
            databaseThread.Start();
        }

        /// <summary>
        /// The flight monitor worker
        /// </summary>
        private void MonitoringWorker()
        {
            while (monitorRunning)
            {
                FSUIPCSnapshot contender = FSUIPCSnapshot.Pool();

                foreach (SnapshotInterest s in Interests)
                    if (lastQueued == null || s(lastQueued, contender))
                        Queue.Enqueue(lastQueued = contender);

                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Consider async await instead of using a separate thread, this may be
        /// an issue an cause actual sequential behaviour. This is mostly IO bound..
        /// </summary>
        private void DatabaseWorker()
        {
            while (databaseRunning)
            {
                while (!DataConnector.BeforeStart())
                    Thread.Sleep(30000);


            }

            // flush the queue
        }

        /// <summary>
        /// Signals all threads to stop asap
        ///     Note: if any thread is waiting (for a db timeout for example) it
        ///     will still wait for that timeout before dying.
        /// </summary>
        public void SignalStop()
        {
            monitorRunning = false;
        }
    }
}
