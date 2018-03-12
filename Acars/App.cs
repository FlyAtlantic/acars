using Acars.UI;
using FlightMonitorApi;
using NLog;
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
            flightMonitor.Interests = new List<FlightMonitor.SnapshotInterest>()
            {
                new FlightMonitor.SnapshotInterest(
                    (queued, contender) =>
                    {
                        return (CompassDelta(queued.Compass, contender.Compass)
                            > 10); // heading changes in orders of 10 degrees
                    }),
                new FlightMonitor.SnapshotInterest(
                    (queued, contender) =>
                    {
                        return (Math.Abs(queued.Altitude - contender.Altitude)
                            > 150); // altitude changes in orders of 150 feet
                    }),
                new FlightMonitor.SnapshotInterest(
                    (queued, contender) =>
                    {
                        return (Math.Abs(queued.GroundSpeed - contender.GroundSpeed)
                            > 35); // groundspeed changes in orders of 35 kts
                    }),
                new FlightMonitor.SnapshotInterest(
                    (queued, contender) =>
                    {
                        return ((contender.TimeStamp - queued.TimeStamp)
                            .TotalMinutes > 1); // max 1 mins between queues
                    })
            };
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

            // dispose ui stuff in here
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

        private int CompassDelta(int firstAngle, int secondAngle)
        {
            double difference = secondAngle - firstAngle;
            while (difference < -180)
                difference += 360;
            while (difference > 180)
                difference -= 360;
            return (int)difference;
        }
    }
}
