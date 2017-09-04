    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        /// Simulator Rate
        /// </summary>
        static private Offset<int> playerSimRate = new Offset<int>(0x0C1A);
        /// <summary>
        /// QNH Player
        /// </summary>
        static private Offset<short> playerQNH = new Offset<short>(0x0330);
        /// <summary>
        /// /// Number of Engines
        /// </summary>
        static private Offset<int> playerEnginesNumber = new Offset<int>(0x0AEC);
        /// <summary>
        ///  /// /// Engine 1 start
        /// </summary>
        static private Offset<short> playerEngine1start = new Offset<short>(0x0894);
        /// <summary>
        ///  /// /// Engine 2 start
        /// </summary>
        static private Offset<short> playerEngine2start = new Offset<short>(0x092C);
        /// <summary>
        ///  /// /// Engine 3 start
        /// </summary>
        static private Offset<short> playerEngine3start = new Offset<short>(0x09C4);
        /// <summary>
        ///  /// /// Engine 4 start
        /// </summary>
        static private Offset<short> playerEngine4start = new Offset<short>(0x0A5C);
        /// <summary> 
        ///  /// /// Turn Rate (for turn coordinator). 0=level, –512=2min Left, +512=2min Right
        /// </summary>
        static private Offset<int> playerTurnRate = new Offset<int>(0x057C);
        /// <summary>
        #endregion FSUIPC Offset declarations

        #region Property declaration
        private int flightId;
        private int userId;
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
        DateTime arrivalTime = DateTime.MinValue;
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
            conn.Open();

            try
            {
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
                        sqlCommand = "SELECT `flightnumber`, `departure`, `destination`, `alternate`, `date_assigned`, `utilizadores`.`user_id`, `flights`.`idf` FROM `pilotassignments` left join flights on pilotassignments.flightid = flights.idf left join utilizadores on pilotassignments.pilot = utilizadores.user_id WHERE utilizadores.user_email=@email";
                        cmd = new MySqlCommand(sqlCommand, conn);
                        cmd.Parameters.AddWithValue("@email", email);
                        MySqlDataReader result1 = cmd.ExecuteReader();
                        while (result1.Read())
                        {
                            txtCallsign.Text = String.Format("{0}", (result1[0]));
                            txtDeparture.Text = String.Format("{0}", (result1[1]));
                            txtArrival.Text = String.Format("{0}", (result1[2]));
                            txtAlternate.Text = String.Format("{0}", (result1[3]));
                            userId = (int)result1[5];
                            flightId = (int)result1[6];
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
            finally
            {
                conn.Close();
            }
            return false;
        }

        #endregion Specific Helper Functions

        // FSUIPC information
        bool onGround;
        bool Gear;
        bool ParkingBrake;
        bool Slew;
        bool Pause;
        bool OverSpeed;
        bool Stall;
        bool Battery;
        bool LandingLights;
        bool Engine1Start;
        bool Engine2Start;
        bool Engine3Start;
        bool Engine4Start;

        public Form1()
        {
            InitializeComponent();

            conn = new MySqlConnection(ConnectionString);

#if DEBUG
            this.Width = 760;
#else
            this.Width = 387;
#endif
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (FlightAssignedDone && arrivalTime == DateTime.MinValue)
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
            else if (arrivalTime != DateTime.MinValue)
            // End Flight
            {
                // prepare sql commands
                MySqlCommand insertFlight = new MySqlCommand("INSERT INTO `pireps` (`date`, `flighttime`, `flightid`, `pilotid`, `ft/pm`, `sum`, `accepted`, `eps_granted`) VALUES (@date, @flighttime, @flightid, @pilotid, @landingrate, @sum, @accepted, @flighteps);", conn);
                var dateParam = insertFlight.Parameters.Add("@date", MySqlDbType.Date);
                dateParam.Value = DateTime.UtcNow;
                insertFlight.Parameters.AddWithValue("@flighttime", Math.Round(flightTime.TotalMinutes));
                insertFlight.Parameters.AddWithValue("@flightid", flightId);
                insertFlight.Parameters.AddWithValue("@pilotid", userId);
                insertFlight.Parameters.AddWithValue("@landingrate", Math.Round(landingRate));
                insertFlight.Parameters.AddWithValue("@sum", 100);
                insertFlight.Parameters.AddWithValue("@accepted", "1");
                insertFlight.Parameters.AddWithValue("@flighteps", Math.Round(Math.Round(flightTime.TotalMinutes) / 10));

                MySqlCommand updatePilot = new MySqlCommand("UPDATE `utilizadores` SET `eps` = eps + @flighteps WHERE `user_id` = @pilotid;", conn);
                updatePilot.Parameters.AddWithValue("@pilotid", userId);
                updatePilot.Parameters.AddWithValue("@flighteps", Math.Round(Math.Round(flightTime.TotalMinutes) / 10));

                MySqlCommand deleteFlight = new MySqlCommand("DELETE from `pilotassignments` where `pilot` = @pilotid;", conn);
                deleteFlight.Parameters.AddWithValue("@pilotid", userId);

                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    // insert flight
                    int result = 0;
                    while (result == 0)
                        result = insertFlight.ExecuteNonQuery();

                    // update pilot data
                    result = 0;
                    while (result == 0)
                        result = updatePilot.ExecuteNonQuery();

                    // delete flight
                    result = 0;
                    while (result == 0)
                        result = deleteFlight.ExecuteNonQuery();
                }
                catch (Exception crap)
                {
                    transaction.Rollback();
                    MessageBox.Show(crap.Source, crap.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }

                transaction.Commit();
                MessageBox.Show(String.Format("Flight approved, rating 100% {0} EP(s)", Math.Round(Math.Round(flightTime.TotalMinutes) / 10)),
                                "Flight Approved",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                Application.Exit();
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
        private void OnFlight_Tick(object sender, EventArgs e)
        {
            MySqlCommand updatePilotAssignment = new MySqlCommand("UPDATE `pilotassignments` SET `onflight` = @date WHERE `pilot` = @pilotid;", conn);
            updatePilotAssignment.Parameters.AddWithValue("@pilotid", userId);
            updatePilotAssignment.Parameters.AddWithValue("@date", DateTime.UtcNow);

            conn.Open();
            MySqlTransaction transaction = conn.BeginTransaction();
            try
            {
                // update pilotassignment
                int result = 0;
                while (result == 0)
                    result = updatePilotAssignment.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception crap)
            {
                transaction.Rollback();
                MessageBox.Show(crap.Source, crap.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
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
                Pause = (playerPauseDisplay.Value == 0) ? false : true;
                OverSpeed = (playerOverSpeed.Value == 0) ? false : true;
                Stall = (playerStall.Value == 0) ? false : true;
                Battery = (playerBattery.Value == 0) ? false : true;
                LandingLights = (playerLandingLights.Value == 0) ? false : true;
                Engine1Start = (playerEngine1start.Value == 0) ? false : true;
                Engine2Start = (playerEngine2start.Value == 0) ? false : true;
                Engine3Start = (playerEngine3start.Value == 0) ? false : true;
                Engine4Start = (playerEngine4start.Value == 0) ? false : true;



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
                //Simulator Rate Block Action
                if (playerSimRate.Value != 256)
                {
                    playerSimRate.Value = 256;
                    string Message = "Simulator Rate can't be changed!";
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
                 FsLongitude lon = new FsLongitude(playerLongitude.Value);
                FsLatitude lat = new FsLatitude(playerLatitude.Value);
                int turnRate = (((playerTurnRate.Value) / 360) / 65536) * 2;
                Double intAltitude = (playerAltitude.Value * 3.2808399);

                txtLog.Text = String.Format("{0:dd-MM-yyyy HH:mm:ss}\r\n\r\n", DateTime.UtcNow);
                txtLog.Text = txtLog.Text + String.Format("Simulator: {0} \r\n", FSUIPCConnection.FlightSimVersionConnected);
                txtLog.Text = txtLog.Text + String.Format("Simulator Rate: {0} X \r\n\r\n", ((playerSimRate.Value) / 256).ToString("F0"));              
                txtLog.Text = txtLog.Text + String.Format("Latitude: {0} \r\n", lat.DecimalDegrees.ToString().Replace(',', '.'));
                txtLog.Text = txtLog.Text + String.Format("Longitude: {0} \r\n\r\n", lon.DecimalDegrees.ToString().Replace(',', '.'));
                
                txtLog.Text = txtLog.Text + String.Format("Number of Engines: {0} \r\n", (playerEnginesNumber.Value).ToString("F0"));
               
                if (playerEnginesNumber.Value == 1)
                {
                    if (Engine1Start)
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 1 On: {0} \r\n\r\n", playerEngine1start.Value.ToString("F0"));
                    }
                    else
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 1 Off: {0} \r\n\r\n", playerEngine1start.Value.ToString("F0"));
                    }
                }

                if (playerEnginesNumber.Value == 2)
                {
                    if (Engine1Start)
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 1 On: {0} \r\n", playerEngine1start.Value.ToString("F0"));
                    }
                    else
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 1 Off: {0} \r\n", playerEngine1start.Value.ToString("F0"));
                    }
                    if (Engine2Start)
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 2 On: {0} \r\n\r\n", playerEngine2start.Value.ToString("F0"));
                    }
                    else
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 2 Off: {0} \r\n\r\n", playerEngine2start.Value.ToString("F0"));
                    }
                }

                if (playerEnginesNumber.Value == 3)
                {
                    if (Engine1Start)
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 1 On: {0} \r\n", playerEngine1start.Value.ToString("F0"));
                    }
                    else
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 1 Off: {0} \r\n", playerEngine1start.Value.ToString("F0"));
                    }
                    if (Engine2Start)
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 2 On: {0} \r\n", playerEngine2start.Value.ToString("F0"));
                    }
                    else
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 2 Off: {0} \r\n", playerEngine2start.Value.ToString("F0"));
                    }
                    if (Engine3Start)
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 3 On: {0} \r\n\r\n", playerEngine3start.Value.ToString("F0"));
                    }
                    else
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 3 Off: {0} \r\n\r\n", playerEngine3start.Value.ToString("F0"));
                    }
                }

                if (playerEnginesNumber.Value == 4)
                {
                    if (Engine1Start)
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 1 On: {0} \r\n", playerEngine1start.Value.ToString("F0"));
                    }
                    else
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 1 Off: {0} \r\n", playerEngine1start.Value.ToString("F0"));
                    }
                    if (Engine2Start)
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 2 On: {0} \r\n", playerEngine2start.Value.ToString("F0"));
                    }
                    else
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 2 Off: {0} \r\n", playerEngine2start.Value.ToString("F0"));
                    }
                    if (Engine3Start)
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 3 On: {0} \r\n", playerEngine3start.Value.ToString("F0"));
                    }
                    else
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 3 Off: {0} \r\n", playerEngine3start.Value.ToString("F0"));
                    }
                    if (Engine4Start)
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 4 On: {0} \r\n\r\n", playerEngine4start.Value.ToString("F0"));
                    }
                    else
                    {
                        txtLog.Text = txtLog.Text + String.Format("Engine 4 Off: {0} \r\n\r\n", playerEngine4start.Value.ToString("F0"));
                    }
                }

                txtLog.Text = txtLog.Text + String.Format("QNH: {0} mbar \r\n\r\n", ((playerQNH.Value) / 16).ToString("F0"));

                if (playerBattery.Value == 257)
                {
                    txtLog.Text = txtLog.Text + String.Format("Battery On: {0} \r\n", (playerBattery.Value).ToString("F0"));
                }
                else
                {
                    txtLog.Text = txtLog.Text + String.Format("Battery Off: {0} \r\n", (playerBattery.Value).ToString("F0"));
                }                
                if (ParkingBrake)
                {
                    txtLog.Text = txtLog.Text + String.Format("Parking Brakes On: {0} \r\n", (playerParkingBrake.Value).ToString("F0"));
                }
                else
                {
                    txtLog.Text = txtLog.Text + String.Format("Parking Brakes Off: {0} \r\n", (playerParkingBrake.Value).ToString("F0"));
                }
                if (Gear)
                {
                    txtLog.Text = txtLog.Text + String.Format("Gear Down at: {0} ft\r\n\r\n", (playerAltitude.Value * 3.2808399).ToString("F0"));
                }
                else
                {
                    txtLog.Text = txtLog.Text + String.Format("Gear Up at: {0} ft \r\n\r\n", (playerAltitude.Value * 3.2808399).ToString("F0"));
                }

                txtLog.Text = txtLog.Text + String.Format("---PENALIZATIONS---- \r\n");

                if (Slew)
                {
                    txtLog.Text = txtLog.Text + String.Format("EVENT : Slew On: {0} \r\n", (playerSlew.Value).ToString("F0"));
                }
                if (Pause)
                {
                    txtLog.Text = txtLog.Text + String.Format("EVENT : Pause On: {0} \r\n", (playerPauseDisplay.Value).ToString("F0"));
                }
                if (OverSpeed)
                {
                    txtLog.Text = txtLog.Text + String.Format("EVENT : OverSpeed On: {0} \r\n", (playerOverSpeed.Value).ToString("F0"));
                }
                if (Stall)
                {
                    txtLog.Text = txtLog.Text + String.Format("EVENT : Stall On: {0} \r\n", (playerStall.Value).ToString("F0"));
                }
                if (turnRate >= 30)
                {
                    txtLog.Text = txtLog.Text + String.Format("EVENT : Bank Angle: {0} \r\n", (((playerTurnRate.Value) / 360) / 65536) * 2);
                }
                if (intAltitude >= 10000 && LandingLights)
                {
                    txtLog.Text = txtLog.Text + String.Format("EVENT : LandingLights On Above: {0} ft\r\n", (playerAltitude.Value * 3.2808399).ToString("F0"));
                }
                if (intAltitude <= 3000 && !LandingLights)
                {
                    txtLog.Text = txtLog.Text + String.Format("EVENT : LandingLights Off Below: {0} ft\r\n", (playerAltitude.Value * 3.2808399).ToString("F0"));
                }
                txtLog.Text = txtLog.Text + String.Format("---END PENALIZATIONS---- \r\n");
                //Touch Down
                if (flightPhase == FlightPhases.TAXIOUT && landingRate == double.MinValue)
                {
                    // enable end flight
                    button1.Text = "End flight";
                    button1.Enabled = true;

                    landingRate = (playerVerticalSpeed.Value * 3.28084) / -1;
                    txtLandingRate.Text = String.Format("{0} ft/min", landingRate.ToString("F0"));
                    txtLog.Text = txtLog.Text + String.Format("TouchDown: {0} ft/min\r\n", landingRate.ToString("F0"));
                }

                // Compose a string that consists of three lines.
                string lines = txtLog.Text;

                // Write the string to a file.
                string logFilePath = String.Format("{0}log-flight-{1}.txt",
                                                         Application.ExecutablePath.Replace("Acars.exe", ""),
                                                         flightId);
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(logFilePath, true))
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

                Console.WriteLine(crap.Message);
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
