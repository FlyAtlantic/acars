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

        /// <summary>
        /// 
        /// </summary>
        private DatabaseConnector databaseConnector;

        /// <summary>
        /// 
        /// </summary>
        private FlightMonitor flightMonitor;

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
            IntegrationTimer.Start();

            ///
            databaseConnector = new DatabaseConnector("prodrigues1990@gmail.com");

            ///
            /// Register on the flight monitor API
            ///
            flightMonitor = new FlightMonitor(databaseConnector);
            flightMonitor.Interests = new List<FlightMonitor.SnapshotInterest>();
            flightMonitor.Queue = new ConcurrentQueue<FSUIPCSnapshot>();
            flightMonitor.StartWorkers();

            /// TODO: do UI stuff
        }

        private void IntegrationTimer_Tick(object sender, EventArgs e)
        {
            if (databaseConnector.ActiveFlightPlan != null)
            {
                TrayIcon.SetStatusText(String.Format("{0}",
                    databaseConnector.ActiveFlightPlan.AtcCallsign));
            }
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            TrayIcon.Dispose();
            flightMonitor.SignalStop();

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
