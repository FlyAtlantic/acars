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
        /// Gross Weight
        /// </summary>
        static private Offset<int> playerGW = new Offset<int>(0x30C0);

        bool FlightAssignedDone = false;
        string email;
        string password;

        private object btnLogin;

        public Form1()
        {
            InitializeComponent();
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
                    Properties.Settings.Default.Email = txtEmail.Text;
                    Properties.Settings.Default.Password = txtPassword.Text;
                    Properties.Settings.Default.Save();
                    email = txtEmail.Text;
                    password = txtPassword.Text;
                }
            }
        }

        private bool DoLogin(string email, string password)
        {
            MySqlConnection conn;

            try
            {
                conn = new MySqlConnection(getConnectionString());
                conn.Open();

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
                            txtFlightInformation.Text = String.Format("Departure: {0} Destination: {1} Departure Time: {2}", result1[0], result1[1], result1[2]);
                            button1.Text = "Start Flight";
                            result1.Close();
                            return true;

                        }
                        if (!FlightAssignedDone)
                        {
                            txtFlightInformation.Text = "No Flight Assigned";
                        }
                        result1.Close();
                        //lblFlightInformation.Text = "No Flight Assigned!";
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
                txtAltitude.Text = String.Format("{0}", (playerAltitude.Value * 3.2808399).ToString("F0"));
                txtHeading.Text = String.Format("{0}", (compass.Value).ToString("F0"));
                txtGroundSpeed.Text = String.Format("{0}", (airspeed.Value / 128).ToString(""));
                txtSquawk.Text = String.Format("{0}", (playersquawk.Value).ToString("X").PadLeft(4, '0'));
                txtGrossWeight.Text = String.Format("{0}", (playerGW.Value * 256).ToString(""));

                MySqlConnection conn = new MySqlConnection(getConnectionString());
                conn.Open();

                // validar email.password
                // preparar a query
                string sqlCommand = "insert into acarslogs values (@altitude)";
                MySqlCommand cmd = new MySqlCommand(sqlCommand, conn);
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
        }

       private string getConnectionString()
        {
            return String.Format("server={0};uid={1};pwd={2};database={3};", Properties.Settings.Default.Server, Properties.Settings.Default.Dbuser, Properties.Settings.Default.Dbpass, Properties.Settings.Default.Database);  
            
        }
    }
}
