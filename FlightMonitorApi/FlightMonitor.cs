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
            if (running)
                return; 
            running = true;                                                         

            monitoringThread.Start();
        }

        /// <summary>
        /// The flight monitor worker
        /// </summary>
        private static void MonitoringWorker()
        {
            while (running)
            {
                FSUIPCSnapshot contender = FSUIPCSnapshot.Pool();

                foreach (SnapshotInterest s in Interests)
                    if (lastQueued == null || s(lastQueued, contender))
                        Queue.Enqueue(lastQueued = contender);

                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Signals all threads to stop asap
        ///     Note: if any thread is waiting (for a db timeout for example) it
        ///     will still wait for that timeout before dying.
        /// </summary>
        public static void SignalStop()
        {
            running = false;
        }
    }
}
