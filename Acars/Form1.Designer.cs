namespace Acars
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFlightInformation = new System.Windows.Forms.Label();
            this.txtFlightInformation = new System.Windows.Forms.TextBox();
            this.flightacars = new System.Windows.Forms.Timer(this.components);
            this.lblAltitude = new System.Windows.Forms.Label();
            this.txtAltitude = new System.Windows.Forms.TextBox();
            this.lblHeading = new System.Windows.Forms.Label();
            this.txtHeading = new System.Windows.Forms.TextBox();
            this.lblGroundSpeed = new System.Windows.Forms.Label();
            this.txtGroundSpeed = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblSquawk = new System.Windows.Forms.Label();
            this.txtSquawk = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLandingRate = new System.Windows.Forms.TextBox();
            this.lblVerticalSpeed = new System.Windows.Forms.Label();
            this.txtVerticalSpeed = new System.Windows.Forms.TextBox();
            this.lblCallsign = new System.Windows.Forms.Label();
            this.txtCallsign = new System.Windows.Forms.TextBox();
            this.lblFlightTime = new System.Windows.Forms.Label();
            this.txtFlightTime = new System.Windows.Forms.TextBox();
            this.lblArrTime = new System.Windows.Forms.Label();
            this.txtArrTime = new System.Windows.Forms.TextBox();
            this.lblDepTime = new System.Windows.Forms.Label();
            this.txtDepTime = new System.Windows.Forms.TextBox();
            this.lblSimulatorHour = new System.Windows.Forms.Label();
            this.txtSimHour = new System.Windows.Forms.TextBox();
            this.lblFuelRemaining = new System.Windows.Forms.Label();
            this.txtFuelRemaining = new System.Windows.Forms.TextBox();
            this.lblArrival = new System.Windows.Forms.Label();
            this.txtAlternate = new System.Windows.Forms.TextBox();
            this.lblGrossWeight = new System.Windows.Forms.Label();
            this.lblAlternate = new System.Windows.Forms.Label();
            this.txtGrossWeight = new System.Windows.Forms.TextBox();
            this.txtArrival = new System.Windows.Forms.TextBox();
            this.lblFuel = new System.Windows.Forms.Label();
            this.txtFuel = new System.Windows.Forms.TextBox();
            this.txtDeparture = new System.Windows.Forms.TextBox();
            this.lblDeparture = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgressBar = new System.Windows.Forms.Label();
            this.chkAutoLogin = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblZFW = new System.Windows.Forms.Label();
            this.txtZFW = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.OnFlight = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(104, 523);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(143, 20);
            this.txtEmail.TabIndex = 0;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(104, 569);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(143, 20);
            this.txtPassword.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(149, 553);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(118, 605);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(159, 507);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Email";
            // 
            // lblFlightInformation
            // 
            this.lblFlightInformation.AutoSize = true;
            this.lblFlightInformation.Location = new System.Drawing.Point(132, 442);
            this.lblFlightInformation.Name = "lblFlightInformation";
            this.lblFlightInformation.Size = new System.Drawing.Size(87, 13);
            this.lblFlightInformation.TabIndex = 6;
            this.lblFlightInformation.Text = "Flight Information";
            // 
            // txtFlightInformation
            // 
            this.txtFlightInformation.Enabled = false;
            this.txtFlightInformation.Location = new System.Drawing.Point(4, 470);
            this.txtFlightInformation.Name = "txtFlightInformation";
            this.txtFlightInformation.Size = new System.Drawing.Size(343, 20);
            this.txtFlightInformation.TabIndex = 7;
            this.txtFlightInformation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // flightacars
            // 
            this.flightacars.Interval = 1000;
            this.flightacars.Tick += new System.EventHandler(this.flightacars_Tick);
            // 
            // lblAltitude
            // 
            this.lblAltitude.AutoSize = true;
            this.lblAltitude.Location = new System.Drawing.Point(18, 26);
            this.lblAltitude.Name = "lblAltitude";
            this.lblAltitude.Size = new System.Drawing.Size(42, 13);
            this.lblAltitude.TabIndex = 10;
            this.lblAltitude.Text = "Altitude";
            // 
            // txtAltitude
            // 
            this.txtAltitude.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAltitude.Enabled = false;
            this.txtAltitude.Location = new System.Drawing.Point(15, 42);
            this.txtAltitude.Name = "txtAltitude";
            this.txtAltitude.Size = new System.Drawing.Size(49, 13);
            this.txtAltitude.TabIndex = 11;
            this.txtAltitude.Text = "----";
            this.txtAltitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Location = new System.Drawing.Point(78, 26);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(47, 13);
            this.lblHeading.TabIndex = 12;
            this.lblHeading.Text = "Heading";
            // 
            // txtHeading
            // 
            this.txtHeading.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHeading.Enabled = false;
            this.txtHeading.Location = new System.Drawing.Point(77, 42);
            this.txtHeading.Name = "txtHeading";
            this.txtHeading.Size = new System.Drawing.Size(49, 13);
            this.txtHeading.TabIndex = 13;
            this.txtHeading.Text = "----";
            this.txtHeading.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblGroundSpeed
            // 
            this.lblGroundSpeed.AutoSize = true;
            this.lblGroundSpeed.Location = new System.Drawing.Point(148, 26);
            this.lblGroundSpeed.Name = "lblGroundSpeed";
            this.lblGroundSpeed.Size = new System.Drawing.Size(27, 13);
            this.lblGroundSpeed.TabIndex = 14;
            this.lblGroundSpeed.Text = "G/S";
            // 
            // txtGroundSpeed
            // 
            this.txtGroundSpeed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGroundSpeed.Enabled = false;
            this.txtGroundSpeed.Location = new System.Drawing.Point(137, 42);
            this.txtGroundSpeed.Name = "txtGroundSpeed";
            this.txtGroundSpeed.Size = new System.Drawing.Size(49, 13);
            this.txtGroundSpeed.TabIndex = 15;
            this.txtGroundSpeed.Text = "----";
            this.txtGroundSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(281, 26);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "Status";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(260, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(78, 13);
            this.textBox1.TabIndex = 17;
            this.textBox1.Text = "Taking Off";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSquawk
            // 
            this.lblSquawk.AutoSize = true;
            this.lblSquawk.Location = new System.Drawing.Point(215, 25);
            this.lblSquawk.Name = "lblSquawk";
            this.lblSquawk.Size = new System.Drawing.Size(46, 13);
            this.lblSquawk.TabIndex = 18;
            this.lblSquawk.Text = "Squawk";
            // 
            // txtSquawk
            // 
            this.txtSquawk.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSquawk.Enabled = false;
            this.txtSquawk.Location = new System.Drawing.Point(213, 41);
            this.txtSquawk.Name = "txtSquawk";
            this.txtSquawk.Size = new System.Drawing.Size(51, 13);
            this.txtSquawk.TabIndex = 19;
            this.txtSquawk.Text = "----";
            this.txtSquawk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtLandingRate);
            this.groupBox1.Controls.Add(this.lblVerticalSpeed);
            this.groupBox1.Controls.Add(this.txtVerticalSpeed);
            this.groupBox1.Controls.Add(this.lblAltitude);
            this.groupBox1.Controls.Add(this.txtAltitude);
            this.groupBox1.Controls.Add(this.lblHeading);
            this.groupBox1.Controls.Add(this.txtHeading);
            this.groupBox1.Controls.Add(this.txtGroundSpeed);
            this.groupBox1.Controls.Add(this.lblGroundSpeed);
            this.groupBox1.Location = new System.Drawing.Point(12, 319);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 67);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flight Status";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(252, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "Touch Down";
            // 
            // txtLandingRate
            // 
            this.txtLandingRate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLandingRate.Enabled = false;
            this.txtLandingRate.Location = new System.Drawing.Point(259, 42);
            this.txtLandingRate.Name = "txtLandingRate";
            this.txtLandingRate.Size = new System.Drawing.Size(55, 13);
            this.txtLandingRate.TabIndex = 41;
            this.txtLandingRate.Text = "----";
            this.txtLandingRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblVerticalSpeed
            // 
            this.lblVerticalSpeed.AutoSize = true;
            this.lblVerticalSpeed.Location = new System.Drawing.Point(209, 26);
            this.lblVerticalSpeed.Name = "lblVerticalSpeed";
            this.lblVerticalSpeed.Size = new System.Drawing.Size(26, 13);
            this.lblVerticalSpeed.TabIndex = 42;
            this.lblVerticalSpeed.Text = "V/S";
            // 
            // txtVerticalSpeed
            // 
            this.txtVerticalSpeed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVerticalSpeed.Enabled = false;
            this.txtVerticalSpeed.Location = new System.Drawing.Point(195, 42);
            this.txtVerticalSpeed.Name = "txtVerticalSpeed";
            this.txtVerticalSpeed.Size = new System.Drawing.Size(55, 13);
            this.txtVerticalSpeed.TabIndex = 43;
            this.txtVerticalSpeed.Text = "----";
            this.txtVerticalSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblCallsign
            // 
            this.lblCallsign.AutoSize = true;
            this.lblCallsign.Location = new System.Drawing.Point(14, 26);
            this.lblCallsign.Name = "lblCallsign";
            this.lblCallsign.Size = new System.Drawing.Size(43, 13);
            this.lblCallsign.TabIndex = 40;
            this.lblCallsign.Text = "Callsign";
            // 
            // txtCallsign
            // 
            this.txtCallsign.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCallsign.Enabled = false;
            this.txtCallsign.Location = new System.Drawing.Point(8, 42);
            this.txtCallsign.Name = "txtCallsign";
            this.txtCallsign.Size = new System.Drawing.Size(55, 13);
            this.txtCallsign.TabIndex = 41;
            this.txtCallsign.Text = "----";
            this.txtCallsign.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFlightTime
            // 
            this.lblFlightTime.AutoSize = true;
            this.lblFlightTime.Location = new System.Drawing.Point(215, 27);
            this.lblFlightTime.Name = "lblFlightTime";
            this.lblFlightTime.Size = new System.Drawing.Size(58, 13);
            this.lblFlightTime.TabIndex = 38;
            this.lblFlightTime.Text = "Flight Time";
            // 
            // txtFlightTime
            // 
            this.txtFlightTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFlightTime.Enabled = false;
            this.txtFlightTime.Location = new System.Drawing.Point(213, 43);
            this.txtFlightTime.Name = "txtFlightTime";
            this.txtFlightTime.Size = new System.Drawing.Size(55, 13);
            this.txtFlightTime.TabIndex = 39;
            this.txtFlightTime.Text = "----";
            this.txtFlightTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblArrTime
            // 
            this.lblArrTime.AutoSize = true;
            this.lblArrTime.Location = new System.Drawing.Point(144, 27);
            this.lblArrTime.Name = "lblArrTime";
            this.lblArrTime.Size = new System.Drawing.Size(46, 13);
            this.lblArrTime.TabIndex = 36;
            this.lblArrTime.Text = "Arr Hour";
            // 
            // txtArrTime
            // 
            this.txtArrTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtArrTime.Enabled = false;
            this.txtArrTime.Location = new System.Drawing.Point(142, 43);
            this.txtArrTime.Name = "txtArrTime";
            this.txtArrTime.Size = new System.Drawing.Size(55, 13);
            this.txtArrTime.TabIndex = 37;
            this.txtArrTime.Text = "----";
            this.txtArrTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDepTime
            // 
            this.lblDepTime.AutoSize = true;
            this.lblDepTime.Location = new System.Drawing.Point(14, 27);
            this.lblDepTime.Name = "lblDepTime";
            this.lblDepTime.Size = new System.Drawing.Size(53, 13);
            this.lblDepTime.TabIndex = 34;
            this.lblDepTime.Text = "Dep Hour";
            // 
            // txtDepTime
            // 
            this.txtDepTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDepTime.Enabled = false;
            this.txtDepTime.Location = new System.Drawing.Point(12, 43);
            this.txtDepTime.Name = "txtDepTime";
            this.txtDepTime.Size = new System.Drawing.Size(55, 13);
            this.txtDepTime.TabIndex = 35;
            this.txtDepTime.Text = "----";
            this.txtDepTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSimulatorHour
            // 
            this.lblSimulatorHour.AutoSize = true;
            this.lblSimulatorHour.Location = new System.Drawing.Point(281, 25);
            this.lblSimulatorHour.Name = "lblSimulatorHour";
            this.lblSimulatorHour.Size = new System.Drawing.Size(50, 13);
            this.lblSimulatorHour.TabIndex = 32;
            this.lblSimulatorHour.Text = "Sim Time";
            // 
            // txtSimHour
            // 
            this.txtSimHour.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSimHour.Enabled = false;
            this.txtSimHour.Location = new System.Drawing.Point(279, 41);
            this.txtSimHour.Name = "txtSimHour";
            this.txtSimHour.Size = new System.Drawing.Size(55, 13);
            this.txtSimHour.TabIndex = 33;
            this.txtSimHour.Text = "----";
            this.txtSimHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFuelRemaining
            // 
            this.lblFuelRemaining.AutoSize = true;
            this.lblFuelRemaining.Location = new System.Drawing.Point(90, 27);
            this.lblFuelRemaining.Name = "lblFuelRemaining";
            this.lblFuelRemaining.Size = new System.Drawing.Size(28, 13);
            this.lblFuelRemaining.TabIndex = 30;
            this.lblFuelRemaining.Text = "ETA";
            // 
            // txtFuelRemaining
            // 
            this.txtFuelRemaining.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFuelRemaining.Enabled = false;
            this.txtFuelRemaining.Location = new System.Drawing.Point(77, 43);
            this.txtFuelRemaining.Name = "txtFuelRemaining";
            this.txtFuelRemaining.Size = new System.Drawing.Size(55, 13);
            this.txtFuelRemaining.TabIndex = 31;
            this.txtFuelRemaining.Text = "01:45";
            this.txtFuelRemaining.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblArrival
            // 
            this.lblArrival.AutoSize = true;
            this.lblArrival.Location = new System.Drawing.Point(148, 26);
            this.lblArrival.Name = "lblArrival";
            this.lblArrival.Size = new System.Drawing.Size(36, 13);
            this.lblArrival.TabIndex = 26;
            this.lblArrival.Text = "Arrival";
            // 
            // txtAlternate
            // 
            this.txtAlternate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAlternate.Enabled = false;
            this.txtAlternate.Location = new System.Drawing.Point(202, 42);
            this.txtAlternate.Name = "txtAlternate";
            this.txtAlternate.Size = new System.Drawing.Size(51, 13);
            this.txtAlternate.TabIndex = 29;
            this.txtAlternate.Text = "----";
            this.txtAlternate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblGrossWeight
            // 
            this.lblGrossWeight.AutoSize = true;
            this.lblGrossWeight.Location = new System.Drawing.Point(21, 25);
            this.lblGrossWeight.Name = "lblGrossWeight";
            this.lblGrossWeight.Size = new System.Drawing.Size(26, 13);
            this.lblGrossWeight.TabIndex = 20;
            this.lblGrossWeight.Text = "GW";
            // 
            // lblAlternate
            // 
            this.lblAlternate.AutoSize = true;
            this.lblAlternate.Location = new System.Drawing.Point(203, 26);
            this.lblAlternate.Name = "lblAlternate";
            this.lblAlternate.Size = new System.Drawing.Size(49, 13);
            this.lblAlternate.TabIndex = 28;
            this.lblAlternate.Text = "Alternate";
            // 
            // txtGrossWeight
            // 
            this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGrossWeight.Enabled = false;
            this.txtGrossWeight.Location = new System.Drawing.Point(7, 41);
            this.txtGrossWeight.Name = "txtGrossWeight";
            this.txtGrossWeight.Size = new System.Drawing.Size(55, 13);
            this.txtGrossWeight.TabIndex = 21;
            this.txtGrossWeight.Text = "----";
            this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtArrival
            // 
            this.txtArrival.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtArrival.Enabled = false;
            this.txtArrival.Location = new System.Drawing.Point(145, 42);
            this.txtArrival.Name = "txtArrival";
            this.txtArrival.Size = new System.Drawing.Size(42, 13);
            this.txtArrival.TabIndex = 27;
            this.txtArrival.Text = "----";
            this.txtArrival.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFuel
            // 
            this.lblFuel.AutoSize = true;
            this.lblFuel.Location = new System.Drawing.Point(156, 25);
            this.lblFuel.Name = "lblFuel";
            this.lblFuel.Size = new System.Drawing.Size(28, 13);
            this.lblFuel.TabIndex = 22;
            this.lblFuel.Text = "FOB";
            // 
            // txtFuel
            // 
            this.txtFuel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFuel.Enabled = false;
            this.txtFuel.Location = new System.Drawing.Point(141, 41);
            this.txtFuel.Name = "txtFuel";
            this.txtFuel.Size = new System.Drawing.Size(59, 13);
            this.txtFuel.TabIndex = 23;
            this.txtFuel.Text = "----";
            this.txtFuel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtDeparture
            // 
            this.txtDeparture.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDeparture.Enabled = false;
            this.txtDeparture.Location = new System.Drawing.Point(81, 42);
            this.txtDeparture.Name = "txtDeparture";
            this.txtDeparture.Size = new System.Drawing.Size(49, 13);
            this.txtDeparture.TabIndex = 25;
            this.txtDeparture.Text = "----";
            this.txtDeparture.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDeparture
            // 
            this.lblDeparture.AutoSize = true;
            this.lblDeparture.Location = new System.Drawing.Point(78, 26);
            this.lblDeparture.Name = "lblDeparture";
            this.lblDeparture.Size = new System.Drawing.Size(54, 13);
            this.lblDeparture.TabIndex = 24;
            this.lblDeparture.Text = "Departure";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(21, 405);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(308, 21);
            this.progressBar.TabIndex = 21;
            this.progressBar.Value = 50;
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Location = new System.Drawing.Point(151, 389);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(48, 13);
            this.lblProgressBar.TabIndex = 22;
            this.lblProgressBar.Text = "Progress";
            // 
            // chkAutoLogin
            // 
            this.chkAutoLogin.AutoSize = true;
            this.chkAutoLogin.Location = new System.Drawing.Point(253, 609);
            this.chkAutoLogin.Name = "chkAutoLogin";
            this.chkAutoLogin.Size = new System.Drawing.Size(77, 17);
            this.chkAutoLogin.TabIndex = 23;
            this.chkAutoLogin.Text = "Auto Login";
            this.chkAutoLogin.UseVisualStyleBackColor = true;
            this.chkAutoLogin.CheckedChanged += new System.EventHandler(this.chkAutoLogin_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtLog);
            this.groupBox2.Location = new System.Drawing.Point(365, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(374, 224);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Flight Log";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(13, 25);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(350, 184);
            this.txtLog.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblCallsign);
            this.groupBox3.Controls.Add(this.txtCallsign);
            this.groupBox3.Controls.Add(this.lblDeparture);
            this.groupBox3.Controls.Add(this.txtDeparture);
            this.groupBox3.Controls.Add(this.lblArrival);
            this.groupBox3.Controls.Add(this.txtArrival);
            this.groupBox3.Controls.Add(this.lblAlternate);
            this.groupBox3.Controls.Add(this.txtAlternate);
            this.groupBox3.Controls.Add(this.lblStatus);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(12, 91);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(347, 70);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Flight Information";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblZFW);
            this.groupBox4.Controls.Add(this.txtZFW);
            this.groupBox4.Controls.Add(this.lblGrossWeight);
            this.groupBox4.Controls.Add(this.txtGrossWeight);
            this.groupBox4.Controls.Add(this.lblFuel);
            this.groupBox4.Controls.Add(this.txtFuel);
            this.groupBox4.Controls.Add(this.lblSquawk);
            this.groupBox4.Controls.Add(this.txtSquawk);
            this.groupBox4.Controls.Add(this.lblSimulatorHour);
            this.groupBox4.Controls.Add(this.txtSimHour);
            this.groupBox4.Location = new System.Drawing.Point(12, 167);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(347, 70);
            this.groupBox4.TabIndex = 42;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Aircraft Information";
            // 
            // lblZFW
            // 
            this.lblZFW.AutoSize = true;
            this.lblZFW.Location = new System.Drawing.Point(89, 25);
            this.lblZFW.Name = "lblZFW";
            this.lblZFW.Size = new System.Drawing.Size(31, 13);
            this.lblZFW.TabIndex = 34;
            this.lblZFW.Text = "ZFW";
            // 
            // txtZFW
            // 
            this.txtZFW.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtZFW.Enabled = false;
            this.txtZFW.Location = new System.Drawing.Point(74, 41);
            this.txtZFW.Name = "txtZFW";
            this.txtZFW.Size = new System.Drawing.Size(59, 13);
            this.txtZFW.TabIndex = 35;
            this.txtZFW.Text = "----";
            this.txtZFW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblDepTime);
            this.groupBox5.Controls.Add(this.txtDepTime);
            this.groupBox5.Controls.Add(this.lblFuelRemaining);
            this.groupBox5.Controls.Add(this.txtFuelRemaining);
            this.groupBox5.Controls.Add(this.lblFlightTime);
            this.groupBox5.Controls.Add(this.txtArrTime);
            this.groupBox5.Controls.Add(this.txtFlightTime);
            this.groupBox5.Controls.Add(this.lblArrTime);
            this.groupBox5.Location = new System.Drawing.Point(12, 243);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(347, 70);
            this.groupBox5.TabIndex = 43;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Schedule Information";
            // 
            // OnFlight
            // 
            this.OnFlight.Interval = 1000;
            this.OnFlight.Tick += new System.EventHandler(this.OnFlight_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Acars.Properties.Resources.banner;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(772, 638);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkAutoLogin);
            this.Controls.Add(this.lblProgressBar);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtFlightInformation);
            this.Controls.Add(this.lblFlightInformation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtEmail);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "FlyAtlantic Acars";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFlightInformation;
        private System.Windows.Forms.TextBox txtFlightInformation;
        private System.Windows.Forms.Timer flightacars;
        private System.Windows.Forms.Label lblAltitude;
        private System.Windows.Forms.TextBox txtAltitude;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.TextBox txtHeading;
        private System.Windows.Forms.Label lblGroundSpeed;
        private System.Windows.Forms.TextBox txtGroundSpeed;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblSquawk;
        private System.Windows.Forms.TextBox txtSquawk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblArrival;
        private System.Windows.Forms.TextBox txtAlternate;
        private System.Windows.Forms.Label lblGrossWeight;
        private System.Windows.Forms.Label lblAlternate;
        private System.Windows.Forms.TextBox txtGrossWeight;
        private System.Windows.Forms.TextBox txtArrival;
        private System.Windows.Forms.Label lblFuel;
        private System.Windows.Forms.TextBox txtFuel;
        private System.Windows.Forms.TextBox txtDeparture;
        private System.Windows.Forms.Label lblDeparture;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblFuelRemaining;
        private System.Windows.Forms.TextBox txtFuelRemaining;
        private System.Windows.Forms.Label lblCallsign;
        private System.Windows.Forms.TextBox txtCallsign;
        private System.Windows.Forms.Label lblFlightTime;
        private System.Windows.Forms.TextBox txtFlightTime;
        private System.Windows.Forms.Label lblArrTime;
        private System.Windows.Forms.TextBox txtArrTime;
        private System.Windows.Forms.Label lblDepTime;
        private System.Windows.Forms.TextBox txtDepTime;
        private System.Windows.Forms.Label lblSimulatorHour;
        private System.Windows.Forms.TextBox txtSimHour;
        private System.Windows.Forms.Label lblProgressBar;
        private System.Windows.Forms.CheckBox chkAutoLogin;
        private System.Windows.Forms.Label lblVerticalSpeed;
        private System.Windows.Forms.TextBox txtVerticalSpeed;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblZFW;
        private System.Windows.Forms.TextBox txtZFW;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLandingRate;
        private System.Windows.Forms.Timer OnFlight;
    }
}

