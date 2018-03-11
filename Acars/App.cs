using Acars.UI;
using FlightMonitorApi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Acars
{
    class App : ApplicationContext
    {
        private AcarsNotifyIcon TrayIcon;

        /// <summary>
        /// Takes care of signaling all threads with relevant application state
        /// </summary>
        private Timer IntegrationTimer;

        public App()
        {
            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            TrayIcon = new AcarsNotifyIcon();

            ///
            /// Resgiter basic UI hooks
            ///
            TrayIcon.Close_Click += CloseMenuItem_Click;
            IntegrationTimer = new Timer
            {
                Interval = 1000
            };
            IntegrationTimer.Tick += IntegrationTimer_Tick;


            ///
            /// Register on the flight monitor API
            ///
            FlightMonitor.Interests = new List<FlightMonitor.SnapshotInterest>();
            FlightMonitor.Queue = new ConcurrentQueue<FSUIPCSnapshot>();
            FlightMonitor.StartWorkers();

            /// TODO: do UI stuff
        }

        private void IntegrationTimer_Tick(object sender, EventArgs e)
        {
            //FSUIPCSnapshot snapshot;
            //bool took = FlightMonitor.Queue.TryPeek(out snapshot);
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            TrayIcon.Dispose();
            FlightMonitor.SignalStop();

            // dispose ui stuff and threads in here
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Do you really want to close me?",
                "Are you sure?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation);
            if (result == DialogResult.Yes)
                Application.Exit();
        }
    }
}
