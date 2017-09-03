    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSUIPC;
using MySql.Data.MySqlClient;
using static System.Resources.ResXFileRef;

namespace Acars
{
    public enum FlightPhases
    {
        PREFLIGHT = 0,
        TAXIOUT = 1,
        TAKEOFF = 2,
        /// <summary>
        /// Happens first at airborne time
        /// </summary>
        CLIMBING = 3,
        CRUISE = 4,
        DESCENDING = 5,
        APPROACH = 6,
        LANDING = 7,
        TAXIIN = 8
    }

    public partial class Form1 : Form
    {
        #region FSUIPC Offset declarations
        /// <summary>
        /// Offset for Lat features
        /// </summary>
        static private Offset<long> playerLatitude = new Offset<long>(0x0560);
        /// <summary>
        /// Offset for Lon features
        /// </summary>
        static private Offset<long> playerLongitude = new Offset<long>(0x0568);
        /// <summary>
        /// Offset for Heading
        /// </summary>
        static private Offset<Double> compass = new Offset<double>(0x02CC);
        /// <summary>
        /// Offset for Altitude
        /// </summary>
        static private Offset<Double> playerAltitude = new Offset<Double>(0x6020);
        /// <summary>
        /// Airspeed
        /// </summary>
        static private Offset<int> airspeed = new Offset<int>(0x02BC);
        /// <summary>
        /// Squawk code
        /// </summary>
        static private Offset<short> playersquawk = new Offset<short>(0x0354);
        /// <summary>
        /// Gross Weight Pounds
        /// </summary>
        static private Offset<double> playerGW = new Offset<double>(0x30C0);
        /// <summary>
        /// Zero Fuel Weight Pouds
        /// </summary>
        static private Offset<int> playerZFW = new Offset<int>(0x3BFC);
        /// <summary>
        /// Simulator Hour
        /// </summary>
        static private Offset<byte[]> playerSimTime = new Offset<byte[]>(0x023B, 10);
        /// <summary>
        /// On Ground = 1 // Airborne = 0
        /// </summary>
        static private Offset<short> playerAircraftOnGround = new Offset<short>(0x0366, false);
        /// <summary>
        /// Vertical Speed
        /// </summary>
        static private Offset<short> playerVerticalSpeed = new Offset<short>(0x0842);
        /// <summary>
        /// Stall Warning (0=no, 1=stall) com erros quando overspeed
        /// </summary>
        static private Offset<short> playerStall = new Offset<short>(0x036C, false);
        /// <summary>
        /// OverSpeed Warning (0=no, 1=overspeed) com erros quando overspeed
        /// </summary>
        static private Offset<short> playerOverSpeed = new Offset<short>(0x036D, false);
        /// <summary>
        /// 	Slew mode (indicator and control), 0=off, 1=on. (See 05DE also).
        /// </summary>
        static private Offset<short> playerSlew = new Offset<short>(0x05DC, false);
        /// <summary>
        /// 	Parking brake: 0=off, 32767=on
        /// </summary>
        static private Offset<short> playerParkingBrake = new Offset<short>(0x0BC8, false);
        /// <summary>
        /// 		Gear control: 0=Up, 16383=Down
        /// </summary>
        static private Offset<short> playerGear = new Offset<short>(0x0BE8, false);
        /// <summary>
        /// 		Wellcome Message
        /// </summary>
        static private Offset<string> messageWrite = new Offset<string>(0x3380, 128);
        static private Offset<short> messageDuration = new Offset<short>(0x32FA);
        /// <summary>
        /// 		Pause Control
        /// </summary>
        static private Offset<short> playerPauseControl = new Offset<short>(0x0262, false);
        static private Offset<short> playerPauseDisplay = new Offset<short>(0x0264, false);
        /// <summary>
        /// 		Pause Control
        /// </summary>
        static private Offset<short> playerBattery = new Offset<short>(0x3102, false);
        /// <summary>
        ///Landing Lights
        /// </summary>
        static private Offset<short> playerLandingLights = new Offset<short>(0x028C, false);
        /// </summary>
        ///  /// Simulator Hours
        /// </summary>
        static private Offset<byte[]> playerHourSim = new Offset<byte[]>(0x023B, 4);
        /// </summary>
        ///  /// Simulator Minute
        /// </summary>
        static private Offset<byte[]> playerMinuteSim = new Offset<byte[]>(0x023C, 4);
        /// </summary>
        ///  /// Simulator Year
        /// </summary>
        static private Offset<byte[]> playerYearSim = new Offset<byte[]>(0x0240, 4);
        /// </summary>
        ///  /// Day of Year
        /// </summary>
        static private Offset<byte[]> playerDayOfYear = new Offset<byte[]>(0x023E, 4);
        /// </summary>
        ///  /// Simulator Menus
        /// </summary>
        static private Offset<byte[]> playerSimMenus = new Offset<byte[]>(0x32F1, 8);
        #endregion FSUIPC Offset declarations

        #region Property declaration
        private double landingRate;
        /// <summary>
        /// FSUIPC Wrapper
        /// </summary>
        private FsuipcWrapper fs;
        /// <summary>
        /// Reusable MySQL connection throughout the application
        /// </summary>
        MySqlConnection conn;
        /// <summary>
        /// User entered email
        /// </summary>
        string email;
        /// <summary>
        /// User entered password
        /// </summary>
        string password;
        /// <summary>
        /// Determines if a flight given by the Flight System database has been returned upon login
        /// </summary>
        bool FlightAssignedDone = false;
        /// <summary>
        /// Returns the current fligt phase
        /// </summary>
        FlightPhases flightPhase;
        /// <summary>
        /// Returns last gear lift off time
        /// </summary>
        DateTime departureTime;
        /// <summary>
        /// Returns last gear touch down time
        /// </summary>
        DateTime arrivalTime;
        /// <summary>
        /// Gets computed flight time
        /// 
        /// Both departureTime and arrivalTime must be set, otherwise returns TimeSpan.MinValue
        /// </summary>
        TimeSpan flightTime
        {
            get
            {
                if (departureTime == null || arrivalTime == null)
                    return TimeSpan.MinValue;
                return arrivalTime - departureTime;
            }
        }
        private string ConnectionString
        {
            get
            {
                return String.Format(
                    "server={0};uid={1};pwd={2};database={3};",
                    Properties.Settings.Default.Server,
                    Properties.Settings.Default.Dbuser,
                    Properties.Settings.Default.Dbpass,
                    Properties.Settings.Default.Database);
            }
        }
        #endregion Property declaration

        #region General Helper Function
        /// <summary>
        /// Returns the SHA1 hash string for a given string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string StringToSha1Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
        #endregion General Helper Function

        #region Specific Helper Functions
        /// <summary>
        /// Prepares all variables for a fresh flight
        /// </summary>
        /// <returns></returns>
        private void FlightStart()
        {
            flightPhase = FlightPhases.PREFLIGHT;
            onGround = true;
        }
        /// <summary>
        /// Handle flight phases
        /// </summary>
        private void HandleFlightPhases()
        {
            switch (flightPhase)
            {
                case FlightPhases.PREFLIGHT:
                    // check for airborne
                    if (!onGround)
                    {
                        flightPhase = FlightPhases.CLIMBING;
                        departureTime = DateTime.UtcNow;
                    }
                    break;
                case FlightPhases.CLIMBING:
                    if (onGround)
                    {
                        flightPhase = FlightPhases.TAXIOUT;
                        arrivalTime = DateTime.UtcNow;
                    }
                    break;
            }

        }
        /// <summary>
        /// Handle login and assigned flight confirmation
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool DoLogin(string email, string password)
        {
            try
            {
                if (conn == null || conn.State != ConnectionState.Open)
                {
                    conn = new MySqlConnection(ConnectionString);
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Conn State was not Open", ex);
                    }
                }

                // validar email.password
                // preparar a query
                string sqlCommand = "select count(*) from utilizadores where `user_email`=@email and `user_senha`=@password;";
                MySqlCommand cmd = new MySqlCommand(sqlCommand, conn);
                // preencher os parametros
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", StringToSha1Hash(password));
                // executar a query
                object result = cmd.ExecuteScalar();
                // validar que retornou 1
                if (result != null)
                {
                    int r = Convert.ToInt32(result);

                    if (r == 0)
                    {
                        txtFlightInformation.Text = "Bad Login!";

                    }
                    else
                    {
                        // get current assigned fligth information
                        sqlCommand = "SELECT `flightnumber`, `departure`, `destination`, `alternate`, `date_assigned` FROM `pilotassignments` left join flights on pilotassignments.flightid = flights.idf left join utilizadores on pilotassignments.pilot = utilizadores.user_id WHERE utilizadores.user_email=@email";
                        cmd = new MySqlCommand(sqlCommand, conn);
                        cmd.Parameters.AddWithValue("@email", email);
                        MySqlDataReader result1 = cmd.ExecuteReader();
                        while (result1.Read())
                        {
                            txtCallsign.Text = String.Format("{0}", (result1[0]));
                            txtDeparture.Text = String.Format("{0}", (result1[1]));
                            txtArrival.Text = String.Format("{0}", (result1[2]));
                            txtAlternate.Text = String.Format("{0}", (result1[3]));
                            txtFlightInformation.Text = String.Format("{0} {1} {2} {3:HH:mm}", (result1[0]), (result1[1]), (result1[2]), (result1[4]));

                            button1.Text = "Start Flight";
                            return true;

                        }
                        if (!FlightAssignedDone)
                        {
                            txtFlightInformation.Text = "No Flight Assigned";
                        }
                        conn.Close();
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        #endregion Specific Helper Functions

        // FSUIPC information
        bool onGround;
        bool Gear;
        bool ParkingBrake;
        bool Slew;
        bool OverSpeed;
        bool Stall;
        bool Battery;
        bool LandingLights;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (FlightAssignedDone)
                // Start Flight
            {
                // instanciate FS wrapper
                while (fs == null)
                    fs = FsuipcWrapper.TryInstantiate();            

                string Message = "Welcome to FlyAtlantic Acars";
                messageWrite.Value = Message;
                messageDuration.Value = 10;
                FSUIPCConnection.Process();

                button1.Enabled = false;
                button1.Text = "Flying...";

                landingRate = double.MinValue;

                flightacars.Start();
            }
            else
                // Login
            {
                

                FlightAssignedDone = DoLogin(txtEmail.Text, txtPassword.Text);
                if (FlightAssignedDone)
                {
                    // prepare current flight
                    FlightStart();

                    // save validated credentials
                    Properties.Settings.Default.Email = txtEmail.Text;
                    Properties.Settings.Default.Password = txtPassword.Text;
                    Properties.Settings.Default.Save();
                    email = txtEmail.Text;
                    password = txtPassword.Text;
                }
            }
        }

        private void flightacars_Tick(object sender, EventArgs e)
        {
            FSUIPCConnection.Process();
            string result = "";
            try
            {
                // handle flight phase changes
                HandleFlightPhases();

                if (departureTime != null)
                    txtDepTime.Text = departureTime.ToString("HH:mm");

                if (arrivalTime != null)
                    txtArrTime.Text = arrivalTime.ToString("HH:mm");

                if (flightTime != TimeSpan.MinValue)
                    txtFlightTime.Text = String.Format("{0:00}:{1:00}",
                                                       Math.Truncate(flightTime.TotalHours),
                                                       flightTime.Minutes);
                else { };

                // process FSUIPC data
                onGround = (playerAircraftOnGround.Value == 0) ? false : true;
                Gear = (playerGear.Value == 0) ? false : true;
                ParkingBrake = (playerParkingBrake.Value == 0) ? false : true;
                Slew = (playerSlew.Value == 0) ? false : true;
                OverSpeed = (playerOverSpeed.Value == 0) ? false : true;
                Stall = (playerStall.Value == 0) ? false : true;
                Battery = (playerBattery.Value == 0) ? false : true;
                LandingLights = (playerLandingLights.Value == 0) ? false : true;

                txtAltitude.Text = String.Format("{0} ft", (playerAltitude.Value * 3.2808399).ToString("F0"));
                txtHeading.Text = String.Format("{0} º", (compass.Value).ToString("F0"));
                txtGroundSpeed.Text = String.Format("{0} kt", (airspeed.Value / 128).ToString(""));
                txtVerticalSpeed.Text = String.Format("{0} ft/m", ((playerVerticalSpeed.Value * 3.28084) / -1).ToString("F0"));

                ///FSUIPC Permanent Actions
                //Slew Mode Bloked
                if (playerSlew.Value != 0)
                {
                    playerSlew.Value = 0;
                    string Message = "You can't use Slew Mode!";
                    messageWrite.Value = Message;
                    messageDuration.Value = 5;
                    FSUIPCConnection.Process();
                }
                //Unpaused Action
                if (playerPauseDisplay.Value != 0)
                {
                    playerPauseControl.Value = 0;
                    string Message = "Simulator can't be paused!";
                    messageWrite.Value = Message;
                    messageDuration.Value = 5;
                    FSUIPCConnection.Process();
                }

                // aircraft wieghts
                txtGrossWeight.Text = String.Format("{0} kg", ((playerGW.Value) * 0.45359237).ToString("F0"));
                txtZFW.Text = String.Format("{0} kg", ((playerZFW.Value / 256) * 0.45359237).ToString("F0"));                    
                txtFuel.Text = String.Format("{0} kg", ((playerGW.Value / 2.2046226218487757)-((playerZFW.Value / 256) * 0.45359237)).ToString("F0"));

                //insere e verifica hora zulu no Simulador
                fs.EnvironmentDateTime = DateTime.UtcNow;

                //Log Text
                txtLog.Text = String.Format("{0:dd-MM-yyyy HH:mm:ss}\r\n", DateTime.UtcNow);
                txtLog.Text = txtLog.Text + String.Format("Simulator: {0} \r\n", FSUIPCConnection.FlightSimVersionConnected);
                if (Gear) {
                    txtLog.Text = txtLog.Text + String.Format("Gear Down at: {0} ft\r\n", (playerAltitude.Value * 3.2808399).ToString("F0"));
                }
                else
                {
                    txtLog.Text = txtLog.Text + String.Format("Gear Up at: {0} ft\r\n", (playerAltitude.Value * 3.2808399).ToString("F0"));
                }

                if (ParkingBrake)
                {
                    txtLog.Text = txtLog.Text + String.Format("Parking Brakes On: {0} \r\n", (playerParkingBrake.Value).ToString("F0"));
                }
                else
                {
                    txtLog.Text = txtLog.Text + String.Format("Parking Brakes Off: {0} \r\n", (playerParkingBrake.Value).ToString("F0"));
                }

                if (Slew)
                {
                    txtLog.Text = txtLog.Text + String.Format("Slew On: {0} \r\n", (playerSlew.Value).ToString("F0"));
                }
                else
                {
                    txtLog.Text = txtLog.Text + String.Format("Slew Off: {0} \r\n", (playerSlew.Value).ToString("F0"));
                }

                if (OverSpeed)
                {
                    txtLog.Text = txtLog.Text + String.Format("OverSpeed On: {0} \r\n", (playerOverSpeed.Value).ToString("F0"));
                }
                else
                {
                    txtLog.Text = txtLog.Text + String.Format("OverSpeed Off: {0} \r\n", (playerOverSpeed.Value).ToString("F0"));
                }


                if (Stall)
                {
                    txtLog.Text = txtLog.Text + String.Format("Stall On: {0} \r\n", (playerStall.Value).ToString("F0"));
                }
                else
                {
                    txtLog.Text = txtLog.Text + String.Format("Stall Off: {0} \r\n", (playerStall.Value).ToString("F0"));
                }

                if (playerBattery.Value == 257)
                {
                    txtLog.Text = txtLog.Text + String.Format("Battery On: {0} \r\n", (playerBattery.Value).ToString("F0"));
                }
                else
                {
                    txtLog.Text = txtLog.Text + String.Format("Battery Off: {0} \r\n", (playerBattery.Value).ToString("F0"));
                }

                if (LandingLights)
                {
                    txtLog.Text = txtLog.Text + String.Format("LandingLights On at: {0} ft\r\n", (playerAltitude.Value * 3.2808399).ToString("F0"));
                }
                else
                {
                    txtLog.Text = txtLog.Text + String.Format("LandingLights Off at: {0} ft\r\n", (playerAltitude.Value * 3.2808399).ToString("F0"));
                }

                //Touch Down
                if (onGround && landingRate == double.MinValue)
                {
                    landingRate = (playerVerticalSpeed.Value * 3.28084) / -1;
                    txtLandingRate2.Text = String.Format("{0} ft/min", landingRate.ToString("F0"));
                    txtLog.Text = txtLog.Text + String.Format("TouchDown: {0} ft/min\r\n", landingRate.ToString("F0"));
                } else if (!onGround)
                {
                    landingRate = double.MinValue;
                    txtLandingRate2.Text = "";
                }

                // Compose a string that consists of three lines.
                string lines = txtLog.Text;

                // Write the string to a file.
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Log.txt", true))
                {
                    file.WriteLine(txtLog.Text);
                }


                txtSquawk.Text = String.Format("{0}", (playersquawk.Value).ToString("X").PadLeft(4, '0'));

                // get sim time from FSUIPC, no date
                DateTime fsTime = new DateTime(DateTime.UtcNow.Year, 1, 1, playerSimTime.Value[0], playerSimTime.Value[1], playerSimTime.Value[2]);
                txtSimHour.Text = fsTime.ToShortTimeString();
                result = "[{";

                // Latitude and Longitude 
                // Shows using the FsLongitude and FsLatitude classes to easily work with Lat/Lon
                // Create new instances of FsLongitude and FsLatitude using the raw 8-Byte data from the FSUIPC Offsets
                FsLongitude lon = new FsLongitude(playerLongitude.Value);
                FsLatitude lat = new FsLatitude(playerLatitude.Value);
                // Use the ToString() method to output in human readable form:
                // (note that many other properties are avilable to get the Lat/Lon in different numerical formats)
                result += String.Format("\"latitude\":\"{0}\",\"longitude\":\"{1}\"", lat.DecimalDegrees.ToString().Replace(',', '.'), lon.DecimalDegrees.ToString().Replace(',', '.'));
                result += String.Format(",\"altitude\":\"{0}\"", (playerAltitude.Value * 3.2808399).ToString("F2"));
                result += String.Format(",\"heading\":\"{0}\"", compass.Value.ToString("F2"));

                result += "}]";
            }
            catch (Exception crap)
            {
                result = "";
            }
            Console.Write(result);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtEmail.Text = Properties.Settings.Default.Email;
            txtPassword.Text = Properties.Settings.Default.Password;

            chkAutoLogin.Checked = Properties.Settings.Default.autologin;
            // call function that handles login button clicks
            btnLogin_Click(this, e);
        }

        private void chkAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.autologin = chkAutoLogin.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
