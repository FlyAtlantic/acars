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
                        foreach (FSUIPCInterest i in Interests)
                            if (i.IsInteresting(contender))
                            {
                                Queue.Enqueue(contender);
                                LogManager.GetCurrentClassLogger()
                                    .Trace(String.Format(
                                        "alt:{0} hdg:{2} gs:{1} lat:{3} lng:{4}",
                                        contender.Altitude,
                                        contender.Compass,
                                        contender.GroundSpeed,
                                        contender.Position[0],
                                        contender.Position[1]
                                        ));
                                QueueHandle.Set();
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

            // wait for a flight plan
            while (databaseRunning)
                if (DataConnector.LookupFlightPlan())
                    break;
                else
                    Thread.Sleep(3000);

            // keep pushing stuff to the server
            while (databaseRunning)
            {
                try
                {
                    FSUIPCSnapshot newSnapshot = null;
                    if (Queue.TryDequeue(out newSnapshot))
                    {
                        if (!DataConnector.PushOne(newSnapshot)) // TODO: async
                            // failed to send to server
                            // send back to the queue
                            Queue.Enqueue(newSnapshot);
                    }
                    else
                        QueueHandle.WaitOne();
                }
                catch (Exception crap)
                {
                    LogManager.GetCurrentClassLogger().Error(crap);
                    throw crap;
                }
            }
            LogManager.GetCurrentClassLogger()
                .Trace("Database thread dying on request");

            // TODO: flush the queue
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
