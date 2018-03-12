using NLog;
using System;
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
            StartMonitoringWorker();
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
            LogManager.GetCurrentClassLogger()
                .Trace("Monitor thread started");
            while (monitorRunning)
            {
                try
                {
                    FSUIPCSnapshot contender = FSUIPCSnapshot.Pool();

                    if (contender != null)
                        foreach (SnapshotInterest s in Interests)
                            if (lastQueued == null || s(lastQueued, contender))
                            {
                                Queue.Enqueue(lastQueued = contender);
                            }

                    Thread.Sleep(100);
                }
                catch(Exception crap)
                {
                    LogManager.GetCurrentClassLogger().Error(crap);
                    throw crap;
                }
            }
            LogManager.GetCurrentClassLogger()
                .Trace("Monitor thread dying on request");
        }

        /// <summary>
        /// Consider async await instead of using a separate thread, this may be
        /// an issue an cause actual sequential behaviour. This is mostly IO bound..
        /// </summary>
        private void DatabaseWorker()
        {
            LogManager.GetCurrentClassLogger()
                .Trace("Database thread started");
            while (databaseRunning)
            {
                try
                {
                    while (!DataConnector.LookupFlightPlan())
                        Thread.Sleep(3000);

                    if (Queue.TryPeek(out FSUIPCSnapshot snapshot))
                    {
                        if (DataConnector.PushOne(snapshot))
                            while (!Queue.TryDequeue(out snapshot)) ;

                        // TODO: do the signaling stuff
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception crap)
                {
                    LogManager.GetCurrentClassLogger().Error(crap);
                    throw crap;
                }
            }
            LogManager.GetCurrentClassLogger()
                .Trace("Database thread dying on request");

            // flush the queue
        }

        /// <summary>
        /// Signals all threads to stop asap
        ///     Note: if any thread is waiting (for a db timeout for example) it
        ///     will still wait for that timeout before dying.
        /// </summary>
        public void SignalStop()
        {
            LogManager.GetCurrentClassLogger()
                .Trace("Sending stop signal to all threads");

            monitorRunning = false;
            databaseRunning = false;
        }
    }
}
