using System.Threading;

namespace FlightMonitorApi
{
    public static partial class FlightMonitor
    {
        /// <summary>
        /// Starts monitoring and publishing threads
        /// 
        /// Use SignalStop() to request them stop asap
        ///     Note: if any thread is waiting (for a db timeout for example) it
        ///     will still wait for that timeout before dying.
        /// </summary>
        public static void StartWorkers()
        {
            if (monitorRunning)
                return; 
            monitorRunning = true;
        }

        public static void StartMonitoringWorker()
        {
            monitorRunning = true;
            if (monitoringThread.IsAlive)
                return;
            monitoringThread.Start();
        }
        /// <summary>
        /// The flight monitor worker
        /// </summary>
        private static void MonitoringWorker()
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

        private static void DatabaseWorker()
        {
            while (databaseRunning)
            {
                if (data)
            }

            // flush the queue
        }

        /// <summary>
        /// Signals all threads to stop asap
        ///     Note: if any thread is waiting (for a db timeout for example) it
        ///     will still wait for that timeout before dying.
        /// </summary>
        public static void SignalStop()
        {
            monitorRunning = false;
        }
    }
}
