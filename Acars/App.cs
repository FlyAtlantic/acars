using Acars.FlightData;
using Acars.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acars
{
    class App : ApplicationContext
    {
        public static string GetFullMessage(Exception ex)
        {
            return ex.InnerException == null
                 ? ex.Message
                 : ex.Message + " --> " + App.GetFullMessage(ex.InnerException);
        }

        //Component declarations
        private AcarsNotifyIcon TrayIcon;

        private Form1 oldForm;
        private SettingsFrm settingsFrm;
        private PilotReportFrm pilotReportFrm;

        private Timer timer;
        private Timer telemetryTimer;

        private Flight flight;

        public App()
        {
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            InitializeComponent();

            try
            {
                if (!FlightDatabase.ValidateLogin(Properties.Settings.Default.Email, Properties.Settings.Default.Password))
                    settingsFrm.Show();
            }
            catch (Exception crap)
            {
                Console.WriteLine(App.GetFullMessage(crap));
            }

            timer.Start();
        }

        private void InitializeComponent()
        {
            flight = new Flight();

            TrayIcon = new AcarsNotifyIcon();

            oldForm = new Form1();
            settingsFrm = new SettingsFrm();
            pilotReportFrm = new PilotReportFrm();

            //
            // Timer
            //
            timer = new Timer();
            timer.Interval = 10000;
            timer.Tick += new EventHandler(GetFlightTimer_Tick);
            timer.Tick += new EventHandler(WaitForSimulatorConnectionTimer_Tick);

            telemetryTimer = new Timer();
            telemetryTimer.Interval = 1000;
            telemetryTimer.Tick += new EventHandler(ProcessFlightTelemetry);

            TrayIcon.Close_Click += CloseMenuItem_Click;
            TrayIcon.OpenFlightStatus_Click += OpenOldFormMenuItem_Click;
            TrayIcon.OpenSettings_Click += TrayIcon_OpenSettings_Click;
        }

        private void TrayIcon_OpenSettings_Click(object sender, EventArgs e)
        {
            settingsFrm.Show();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            //Cleanup so that the icon will be removed when the application is closed
            TrayIcon.Dispose();
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to close me?",
                    "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void OpenOldFormMenuItem_Click(object sender, EventArgs e)
        {
            if (oldForm == null)
                oldForm = new Form1();

            oldForm.Show();
        }

        /// <summary>
        /// Waits for assigned flight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetFlightTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!FlightDatabase.ValidateLogin(Properties.Settings.Default.Email, Properties.Settings.Default.Password))
                    return;

                // check for assigned flight
                if (flight.GetFlightPlan() != null)
                {
                    timer.Tick -= new EventHandler(GetFlightTimer_Tick);
                    TrayIcon.SetStatusText("Waiting for Simulator!");
                }
            }
            catch (Exception crap)
            {
                Console.WriteLine("GetFlightTimer_Tick \r\n {0}", App.GetFullMessage(crap));
            }
        }

        /// <summary>
        /// Waits for simulator connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaitForSimulatorConnectionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!FlightDatabase.ValidateLogin(Properties.Settings.Default.Email, Properties.Settings.Default.Password))
                    return;

                if (Telemetry.Connect())
                {
                    timer.Tick -= new EventHandler(WaitForSimulatorConnectionTimer_Tick);
                    telemetryTimer.Start();
                }

                // wait for simulator to enable Start Flight Menu Item
            }
            catch (Exception crap)
            {
                Console.WriteLine("GetFlightTimer_Tick \r\n {0}", App.GetFullMessage(crap));
            }
        }

        /// <summary>
        /// Processes flight telemetry data, constantly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessFlightTelemetry(object sender, EventArgs e)
        {
            try
            {
                Telemetry t = Telemetry.GetCurrent();
                if (t == null)
                {
                    telemetryTimer.Stop();
                    timer.Tick -= new EventHandler(WaitForSimulatorConnectionTimer_Tick);

                    TrayIcon.ShowBalloonTip(ToolTipIcon.Warning,
                                            "Connection to simulator lost.",
                                            "Sim Connection Lost");

                    if (flight != null && flight.FlightRunning)
                    {
                        // cancel flight? just wait?
                    }

                    return;
                }
                if (flight.LoadedFlightPlan != null)
                    TrayIcon.SetStatusText("Flight Running...");
                t = flight.HandleFlightPhases(t);
                flight.ProcessTelemetry(t);

                //Update flight every 5 minutes
                if (t.Timestamp.Minute % 5 == 0)
                    FlightDatabase.UpdateFlight(flight);

                //Detetar fim do voo
                if (flight.ActualArrivalTime != null)
                {
                    TrayIcon.SetStatusText("Waiting for pilot report");
                    // validaded company settings

                    // enable end flight
                    pilotReportFrm.Show(flight);
                }

                // UI stuff
                if (flight.LoadedFlightPlan != null && !flight.FlightRunning)
                {
                    flight.StartFlight();

                    TrayIcon.ShowBalloonTip(ToolTipIcon.Info,
                                            String.Format("{0} from {1} to {2}",
                                                          flight.LoadedFlightPlan.AtcCallsign,
                                                          flight.LoadedFlightPlan.DepartureAirfield.Identifier,
                                                          flight.LoadedFlightPlan.ArrivalAirfield.Identifier),
                                            "Start flying!");
                }
            }
            catch (Exception crap)
            {
                Console.WriteLine("GetFlightTimer_Tick \r\n {0}", App.GetFullMessage(crap));
            }
        }
    }
}
