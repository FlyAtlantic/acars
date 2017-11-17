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
        private PreFlightDataFrm preFlightDataFrm;

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
            preFlightDataFrm = new PreFlightDataFrm();
            preFlightDataFrm.OnStartClicked += (object sender, EventArgs e) => {

                flight.StartFlight();

                TrayIcon.ShowBalloonTip(ToolTipIcon.Info,
                                        String.Format("{0} from {1} to {2}",
                                                      flight.LoadedFlightPlan.AtcCallsign,
                                                      flight.LoadedFlightPlan.DepartureAirfield.Identifier,
                                                      flight.LoadedFlightPlan.ArrivalAirfield.Identifier),
                                        "Start flying!");
                preFlightDataFrm.Hide();
            };

            //
            // Timer
            //
            timer = new Timer();
            timer.Interval = 10000;
            timer.Tick += new EventHandler(GetFlightTimer_Tick);
            timer.Tick += new EventHandler(WaitForSimulatorConnectionTimer_Tick);
            timer.Tick += new EventHandler(WaitForDepartureAirfield);

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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaitEndFlightTimer_Tick(object sender, EventArgs e)
        {
            //Detetar fim do voo
            if (flight.ActualArrivalTime != null && !pilotReportFrm.Visible && flight.LastTelemetry.FlightPhase == FlightPhases.TAXIIN)
            {
                timer.Tick -= WaitEndFlightTimer_Tick;
                TrayIcon.SetStatusText("Waiting for pilot report");
                // validaded company settings

                // enable end flight
                pilotReportFrm.Show(flight);

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

        private void WaitForDepartureAirfield(object sender, EventArgs e)
        {
            try
            {
                if (!FlightDatabase.ValidateLogin(Properties.Settings.Default.Email, Properties.Settings.Default.Password))
                    return;

                if (flight.LastTelemetry == null)
                    return;
                if (flight.LoadedFlightPlan == null)
                    return;
                if (flight.LastTelemetry.Location.GetDistanceTo(flight.LoadedFlightPlan.DepartureAirfield.Position) < 5000)
                {
                    timer.Tick -= new EventHandler(WaitForDepartureAirfield);

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

                if (flight.FlightRunning)
                {
                    TrayIcon.SetStatusText("Flight Running...");                  

                    timer.Tick += WaitEndFlightTimer_Tick;
                }
                t = flight.HandleFlightPhases(t);
                flight.ProcessTelemetry(t);

                //Update flight every 5 minutes
                int reportDelay = 0;

                if (flight.LastTelemetry.Altitude > 6000) {
                    reportDelay = 5;
                    }
                if (flight.LastTelemetry.Altitude < 6000)
                {
                    reportDelay = 1;
                }

                if (flight.PirepID != 0 && flight.TelemetryLog.Count > reportDelay && t.Timestamp.Minute % reportDelay == 0 && flight.TelemetryLog[flight.TelemetryLog.Count-2].Timestamp.Minute != flight.LastTelemetry.Timestamp.Minute)
                    FlightDatabase.UpdateFlight(flight);
                
                if(flight.LastTelemetry.FlightPhase == FlightPhases.LANDING)
                    FlightDatabase.UpdateFlight(flight);

                if (flight.LastTelemetry.FlightPhase == FlightPhases.TAKEOFF)
                    FlightDatabase.UpdateFlight(flight);

                if (flight.LastTelemetry.FlightPhase == FlightPhases.TAXIIN)
                    FlightDatabase.UpdateFlight(flight);
            }
            catch (Exception crap)
            {
                Console.WriteLine("GetFlightTimer_Tick \r\n {0}", App.GetFullMessage(crap));
            }
            finally
            {
                if (flight != null)
                    oldForm.Update(flight);
            }
        }
    }
}
