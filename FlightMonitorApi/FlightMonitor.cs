using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace FlightMonitorApi
{
    public class FlightMonitor
    {
        public delegate bool SnapshotInterest(FSUIPCSnapshot queued, FSUIPCSnapshot contenter);

        public BlockingCollection<FSUIPCSnapshot> Queue;

        private FSUIPCSnapshot lastQueued;

        public List<SnapshotInterest> Interests;

        Thread monitoringThread;

        private bool running = false;

        public FlightMonitor()
        {
            monitoringThread = new Thread(new ThreadStart(MonitoringWorker));
            Queue = new BlockingCollection<FSUIPCSnapshot>();
            Interests = new List<SnapshotInterest>();
            lastQueued = null;
        }

        public void StartWorkers()
        {
            if (running)
                return;
            running = true;

            monitoringThread.Start();
        }

        private void MonitoringWorker()
        {
            while (running)
            {
                FSUIPCSnapshot contender = FSUIPCSnapshot.Pool();

                foreach (SnapshotInterest s in Interests)
                    if (lastQueued == null || s(lastQueued, contender))
                        Queue.Add(lastQueued = contender);

                Thread.Sleep(1);
            }
        }
    }
}
