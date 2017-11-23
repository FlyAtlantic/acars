using Acars.FlightData;
using FSUIPC;
using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Acars
{
    public partial class Form1 : Form
    {            
        static private String debugLogText = null;

        public Flight flight;
        private FsuipcWrapper fs;
        private int flightId;
        private int userId;
        public string SpeedV1;
        public string SpeedV2;
        public string SpeedVr;
        public double totaldistance;
        public double distanceremaining;
        public double valuedistance;

        public Form1()
        {
            InitializeComponent();

            #region DEBUG
#if DEBUG
            this.Width = 760;
#else
            this.Width = 387;
#endif
            #endregion DEBUG
        }
        
        public void Update(Flight f)
        {
            SpeedV1 = txtV1.Text;
            SpeedV2 = txtV2.Text;
            SpeedVr = txtVR.Text;
            
            Console.WriteLine(String.Format("{0}", valuedistance.ToString("0")));

            if (f.LastTelemetry != null) {
                txtStatus.Text = f.LastTelemetry.FlightPhase.ToString();
                txtGrossWeight.Text = f.LastTelemetry.GrossWeight.ToString("F0");
                txtZFW.Text = f.LastTelemetry.ZeroFuelWeight.ToString("F0");
                txtFuel.Text = (f.LastTelemetry.GrossWeight - f.LastTelemetry.ZeroFuelWeight).ToString("F0");
                txtSquawk.Text = f.LastTelemetry.Squawk.ToString();
                txtSimHour.Text = f.LastTelemetry.SimTime.ToString("HH:mm");
                txtAltitude.Text = f.LastTelemetry.Altitude.ToString("F0");
                txtHeading.Text = f.LastTelemetry.Compass.ToString("F0");
                txtGroundSpeed.Text = f.LastTelemetry.GroundSpeed.ToString("F0");
                txtVerticalSpeed.Text = f.LastTelemetry.VerticalSpeed.ToString("F0");
                txtGrossWeight.Text = String.Format("{0} kg", f.LastTelemetry.GrossWeight.ToString("F0"));
                txtZFW.Text = String.Format("{0} kg", f.LastTelemetry.ZeroFuelWeight.ToString("F0"));
                txtFuel.Text = String.Format("{0} kg", (f.LastTelemetry.GrossWeight - f.LastTelemetry.ZeroFuelWeight).ToString("F0"));
                txtSquawk.Text = String.Format("{0}", (f.LastTelemetry.Squawk).ToString("X").PadLeft(4, '0'));
                //Log Text
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("{0:dd-MM-yyyy HH:mm:ss}\r\n\r\n", f.LastTelemetry.Timestamp);

                sb.AppendFormat("Simulator: {0} \r\n", FSUIPCConnection.FlightSimVersionConnected);
                sb.AppendFormat("Simulator Rate: {0} X \r\n\r\n", (f.LastTelemetry.SimRate).ToString("F0"));
                sb.AppendFormat("Latitude: {0} \r\n", f.LastTelemetry.Latitude.ToString().Replace(',', '.'));
                sb.AppendFormat("Longitude: {0} \r\n\r\n", f.LastTelemetry.Longitude.ToString().Replace(',', '.'));
                sb.AppendFormat("Gear: {0} \r\n\r\n", f.LastTelemetry.Gear.ToString().Replace(',', '.'));               
                sb.AppendFormat("IAS: {0} \r\n", (f.LastTelemetry.IndicatedAirSpeed).ToString("F0"));
                sb.AppendFormat("QNH: {0} mbar \r\n\r\n", (f.LastTelemetry.QNH).ToString("F0"));
                FSUIPCConnection.Process("AircraftInfo");
                sb.AppendFormat("Aircraft Type: {0} \r\n", FSUIPCOffsets.aircraftType.Value);

                txtLog.Text = sb.ToString();

            }
            if (f.LoadedFlightPlan != null)
            {
                Telemetry t = new Telemetry();
                txtCallsign.Text = f.LoadedFlightPlan.AtcCallsign;
                txtDeparture.Text = f.LoadedFlightPlan.DepartureAirfield.Identifier;
                txtAlternate.Text = f.LoadedFlightPlan.AlternateICAO;
                txtArrival.Text = f.LoadedFlightPlan.ArrivalAirfield.Identifier;
                txtFlightInformation.Text = String.Format("{0} {1} {2} {3:HH:mm}", (f.LoadedFlightPlan.DepartureAirfield.Identifier), (f.LoadedFlightPlan.ArrivalAirfield.Identifier), (f.LoadedFlightPlan.Aircraft), (f.LoadedFlightPlan.DateAssigned));
                if (f.FlightRunning == true)
                    txtAcarsStatus.Text = String.Format("Acars Running");
            }

            // Actual times of Departure, Arrival, Flight Time
            if (f.ActualDepartureTime != null)
                txtDepTime.Text = f.ActualDepartureTime.Timestamp.ToString("HH:mm");
            if (f.ActualArrivalTime != null)
            {
                txtLandingRate.Text = f.ActualArrivalTime.VerticalSpeed.ToString("F0");
            }
            if (f.ActualTimeEnRoute != TimeSpan.MinValue)
                if (f.ActualTimeEnRoute <= TimeSpan.Zero)
                    txtFlightTime.Text = String.Format("00:00");
                else
                    txtFlightTime.Text = String.Format("{0:00}:{1:00}",
                                                       Math.Truncate(f.ActualTimeEnRoute.TotalHours),
                                                       f.ActualTimeEnRoute.Minutes);

        }     

        private void flightacars_Tick(object sender, EventArgs e)
        {
            string result = "";
            try
            {
                // handle flight phase changes
                Telemetry lastTelemetry = flight.HandleFlightPhases();

                txtStatus.Text = lastTelemetry.FlightPhase.ToString();

                // Actual times of Departure, Arrival, Flight Time
                if (flight.ActualDepartureTime != null)
                    txtDepTime.Text = flight.ActualDepartureTime.Timestamp.ToString("HH:mm");
                if (flight.ActualArrivalTime != null)
                {
                    txtArrTime.Text = flight.ActualArrivalTime.Timestamp.ToString("HH:mm");
                    txtLandingRate.Text = flight.ActualArrivalTime.VerticalSpeed.ToString("F0");
                }
                if (flight.ActualTimeEnRoute != TimeSpan.MinValue)
                    if (flight.ActualTimeEnRoute <= TimeSpan.Zero)
                        txtFlightTime.Text = String.Format("00:00");
                    else
                        txtFlightTime.Text = String.Format("{0:00}:{1:00}",
                                                           Math.Truncate(flight.ActualTimeEnRoute.TotalHours),
                                                           flight.ActualTimeEnRoute.Minutes);
                
                           
                
                //verifica hora e corrige hora
                DateTime SimulatorTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, flight.LastTelemetry.SimTime.Hour, flight.LastTelemetry.SimTime.Minute, flight.LastTelemetry.SimTime.Second);
                TimeSpan Time1 = SimulatorTime - DateTime.UtcNow;
                int diffsecpositive = 300;
                int diffsecnegative = -300;

                if (Time1.TotalSeconds >= diffsecpositive || Time1.TotalSeconds <= diffsecnegative)
                {
                    fs.EnvironmentDateTime = DateTime.UtcNow;
                }

            }
            catch (Exception crap)
            {
                result = "";

                Console.WriteLine(crap.Message);
            }
            Console.Write(result);
        }     
    }
}
