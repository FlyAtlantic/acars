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
        static private Offset<Double> playerGW = new Offset<Double>(0x30C0);
        /// <summary>
        /// Zero Fuel Weight Pouds
        /// </summary>
        static private Offset<Double> playerZFW = new Offset<Double>(0x3BFC);
        /// <summary>
        /// Simulator Hour
        /// </summary>
        static private Offset<byte[]> playerSimTime = new Offset<byte[]>(0x023B, 10);
        /// <summary>
        /// On Ground = 1 // Airborne = 0
        /// </summary>
        static private new Offset<short> playerAircraftOnGround = new Offset<short>(0x0366, false);
        /// <summary>
        /// Vertical Speed
        /// </summary>
        static private new Offset<int> playerVerticalSpeed = new Offset<int>(0x0842);

        MySqlConnection conn;
        bool FlightAssignedDone = false;
        string email;
        string password;

        FlightPhases flightPhase;

        DateTime departureTime;
        DateTime arrivalTime;
        TimeSpan flightTime
        {
            get
            {
                if (departureTime == null || arrivalTime == null)
                    return TimeSpan.MinValue;
                return arrivalTime - departureTime;
            }
        }

        // FSUIPC information
        bool onGround;

        public Form1()
        {
            InitializeComponent();

            conn = new MySqlConnection(getConnectionString());
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Closing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
            }
        }

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
        private void FlightRun()
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (FlightAssignedDone)
            {
                bool fsuipcOpen = false;
           
                while (!fsuipcOpen)
                    try
                    {
                        FSUIPCConnection.Open();
                        fsuipcOpen = true;
                    }
                    catch (Exception crap) { }

                button1.Enabled = false;
                button1.Text = "Flying...";
                flightacars.Start();
            }
            else
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
                        sqlCommand = "SELECT `departure`, `destination`, `date_Assigned` FROM `pilotassignments` left join flights on pilotassignments.flightid = flights.idf left join utilizadores on pilotassignments.pilot = utilizadores.user_id WHERE utilizadores.user_email=@email";
                        cmd = new MySqlCommand(sqlCommand, conn);
                        cmd.Parameters.AddWithValue("@email", email);
                        MySqlDataReader result1 = cmd.ExecuteReader();

                        while (result1.Read())
                        {
                            txtFlightInformation.Text = String.Format("Departure: {0} Destination: {1} Time Assigned: {2:HHmm}", result1[0], result1[1], result1[2]);
                            button1.Text = "Start Flight";
                            result1.Close();
                            return true;

                        }
                        if (!FlightAssignedDone)
                        {
                            txtFlightInformation.Text = "No Flight Assigned";
                        }
                        result1.Close();
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private void flightacars_Tick(object sender, EventArgs e)
        {
            FSUIPCConnection.Process();
            string result = "";
            try
            {
                // handle flight phase changes
                FlightRun();

                if (departureTime != null)
                    txtDepTime.Text = departureTime.ToString("HH:mm");

                if (arrivalTime != null)
                    txtArrTime.Text = arrivalTime.ToString("HH:mm");

                if (flightTime != TimeSpan.MinValue)
                    txtFlightTime.Text = String.Format("{0}:{1}",
                                                       Math.Truncate(flightTime.TotalHours),
                                                       flightTime.Minutes);

                // process FSUIPC data
                onGround = (playerAircraftOnGround.Value == 0) ? false : true;
                txtAltitude.Text = String.Format("{0} ft", (playerAltitude.Value * 3.2808399).ToString("F0"));
                txtHeading.Text = String.Format("{0} º", (compass.Value).ToString("F0"));
                txtGroundSpeed.Text = String.Format("{0} kt", (airspeed.Value / 128).ToString(""));
                txtVerticalSpeed.Text = String.Format("{0}", (playerVerticalSpeed.Value).ToString("F0"));


                // get current assigned fligth information
                string sqlCommand1 = "SELECT `callsign`, `departure`, `destination`, `alternate` FROM `pilotassignments` left join flights on pilotassignments.flightid = flights.idf left join utilizadores on pilotassignments.pilot = utilizadores.user_id WHERE utilizadores.user_email=@email limit 1";
                MySqlCommand cmd = new MySqlCommand(sqlCommand1, conn);
                cmd.Parameters.AddWithValue("@email", email);
                MySqlDataReader result2 = cmd.ExecuteReader();

                // will only run once, due to query limit
                while (result2.Read())
                {
                    // aircraft wieghts
                    txtGrossWeight.Text = String.Format("{0} kg", (playerGW.Value / 2.2046226218487757).ToString("F0"));
                    txtFuel.Text = String.Format("{0} kg", (playerGW.Value - playerZFW.Value).ToString("F0"));                  

                    // aircraft configuration
                    txtSquawk.Text = String.Format("{0}", (playersquawk.Value).ToString("X").PadLeft(4, '0'));                                       
                    txtCallsign.Text = String.Format("{0}", (result2[0]));
                    txtDeparture.Text = String.Format("{0}", (result2[1]));
                    txtArrival.Text = String.Format("{0}", (result2[2]));
                    txtAlternate.Text = String.Format("{0}", (result2[3]));

                    Console.WriteLine("{0}", playerAircraftOnGround.Value);
                    Console.WriteLine("{0}", playerVerticalSpeed.Value);

                    // get sim time from FSUIPC, no date
                    DateTime fsTime = new DateTime(DateTime.UtcNow.Year, 1, 1, playerSimTime.Value[0], playerSimTime.Value[1], playerSimTime.Value[2]);
                    txtSimHour.Text = fsTime.ToShortTimeString();

                    // only one assigned flight at a time is allowed
                    break;
                }
                // clean up
                result2.Close();



                // validar email.password
                // preparar a query
                string sqlCommand = "insert into acarslogs values (@altitude)";

                // preencher os parametros
                cmd.Parameters.AddWithValue("@altitude", (playerAltitude.Value * 3.2808399).ToString("F2"));
                // executar a query
                object sqlResult = cmd.ExecuteScalar();

                


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

       private string getConnectionString()
        {
            return String.Format("server={0};uid={1};pwd={2};database={3};", Properties.Settings.Default.Server, Properties.Settings.Default.Dbuser, Properties.Settings.Default.Dbpass, Properties.Settings.Default.Database);  
            
        }

        private void chkAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.autologin = chkAutoLogin.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
