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
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFlightInformation = new System.Windows.Forms.Label();
            this.txtFlightInformation = new System.Windows.Forms.TextBox();
            this.flightacars = new System.Windows.Forms.Timer(this.components);
            this.lblFlightStatus = new System.Windows.Forms.Label();
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
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(99, 380);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(143, 20);
            this.txtEmail.TabIndex = 0;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(99, 426);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(143, 20);
            this.txtPassword.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 410);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(113, 462);
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
            this.label1.Location = new System.Drawing.Point(154, 364);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Email";
            // 
            // lblFlightInformation
            // 
            this.lblFlightInformation.AutoSize = true;
            this.lblFlightInformation.Location = new System.Drawing.Point(127, 299);
            this.lblFlightInformation.Name = "lblFlightInformation";
            this.lblFlightInformation.Size = new System.Drawing.Size(87, 13);
            this.lblFlightInformation.TabIndex = 6;
            this.lblFlightInformation.Text = "Flight Information";
            // 
            // txtFlightInformation
            // 
            this.txtFlightInformation.Enabled = false;
            this.txtFlightInformation.Location = new System.Drawing.Point(-1, 327);
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
            // lblFlightStatus
            // 
            this.lblFlightStatus.AutoSize = true;
            this.lblFlightStatus.Location = new System.Drawing.Point(138, 9);
            this.lblFlightStatus.Name = "lblFlightStatus";
            this.lblFlightStatus.Size = new System.Drawing.Size(65, 13);
            this.lblFlightStatus.TabIndex = 8;
            this.lblFlightStatus.Text = "Flight Status";
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
            this.lblGroundSpeed.Location = new System.Drawing.Point(140, 26);
            this.lblGroundSpeed.Name = "lblGroundSpeed";
            this.lblGroundSpeed.Size = new System.Drawing.Size(27, 13);
            this.lblGroundSpeed.TabIndex = 14;
            this.lblGroundSpeed.Text = "G/S";
            // 
            // txtGroundSpeed
            // 
            this.txtGroundSpeed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGroundSpeed.Enabled = false;
            this.txtGroundSpeed.Location = new System.Drawing.Point(129, 42);
            this.txtGroundSpeed.Name = "txtGroundSpeed";
            this.txtGroundSpeed.Size = new System.Drawing.Size(49, 13);
            this.txtGroundSpeed.TabIndex = 15;
            this.txtGroundSpeed.Text = "----";
            this.txtGroundSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(205, 26);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "Status";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(184, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(78, 13);
            this.textBox1.TabIndex = 17;
            this.textBox1.Text = "Taking Off";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSquawk
            // 
            this.lblSquawk.AutoSize = true;
            this.lblSquawk.Location = new System.Drawing.Point(263, 26);
            this.lblSquawk.Name = "lblSquawk";
            this.lblSquawk.Size = new System.Drawing.Size(71, 13);
            this.lblSquawk.TabIndex = 18;
            this.lblSquawk.Text = "Squawk Num";
            // 
            // txtSquawk
            // 
            this.txtSquawk.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSquawk.Enabled = false;
            this.txtSquawk.Location = new System.Drawing.Point(273, 42);
            this.txtSquawk.Name = "txtSquawk";
            this.txtSquawk.Size = new System.Drawing.Size(51, 13);
            this.txtSquawk.TabIndex = 19;
            this.txtSquawk.Text = "----";
            this.txtSquawk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCallsign);
            this.groupBox1.Controls.Add(this.txtCallsign);
            this.groupBox1.Controls.Add(this.lblFlightTime);
            this.groupBox1.Controls.Add(this.txtFlightTime);
            this.groupBox1.Controls.Add(this.lblArrTime);
            this.groupBox1.Controls.Add(this.txtArrTime);
            this.groupBox1.Controls.Add(this.lblDepTime);
            this.groupBox1.Controls.Add(this.txtDepTime);
            this.groupBox1.Controls.Add(this.lblSimulatorHour);
            this.groupBox1.Controls.Add(this.txtSimHour);
            this.groupBox1.Controls.Add(this.lblFuelRemaining);
            this.groupBox1.Controls.Add(this.txtFuelRemaining);
            this.groupBox1.Controls.Add(this.lblArrival);
            this.groupBox1.Controls.Add(this.txtAlternate);
            this.groupBox1.Controls.Add(this.lblGrossWeight);
            this.groupBox1.Controls.Add(this.lblAlternate);
            this.groupBox1.Controls.Add(this.txtGrossWeight);
            this.groupBox1.Controls.Add(this.txtArrival);
            this.groupBox1.Controls.Add(this.lblFuel);
            this.groupBox1.Controls.Add(this.txtFuel);
            this.groupBox1.Controls.Add(this.txtDeparture);
            this.groupBox1.Controls.Add(this.lblDeparture);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Controls.Add(this.txtSquawk);
            this.groupBox1.Controls.Add(this.lblAltitude);
            this.groupBox1.Controls.Add(this.lblSquawk);
            this.groupBox1.Controls.Add(this.txtAltitude);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.lblHeading);
            this.groupBox1.Controls.Add(this.txtHeading);
            this.groupBox1.Controls.Add(this.txtGroundSpeed);
            this.groupBox1.Controls.Add(this.lblGroundSpeed);
            this.groupBox1.Location = new System.Drawing.Point(-1, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 207);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // lblCallsign
            // 
            this.lblCallsign.AutoSize = true;
            this.lblCallsign.Location = new System.Drawing.Point(21, 157);
            this.lblCallsign.Name = "lblCallsign";
            this.lblCallsign.Size = new System.Drawing.Size(43, 13);
            this.lblCallsign.TabIndex = 40;
            this.lblCallsign.Text = "Callsign";
            // 
            // txtCallsign
            // 
            this.txtCallsign.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCallsign.Enabled = false;
            this.txtCallsign.Location = new System.Drawing.Point(12, 173);
            this.txtCallsign.Name = "txtCallsign";
            this.txtCallsign.Size = new System.Drawing.Size(55, 13);
            this.txtCallsign.TabIndex = 41;
            this.txtCallsign.Text = "TSZ101";
            this.txtCallsign.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFlightTime
            // 
            this.lblFlightTime.AutoSize = true;
            this.lblFlightTime.Location = new System.Drawing.Point(274, 116);
            this.lblFlightTime.Name = "lblFlightTime";
            this.lblFlightTime.Size = new System.Drawing.Size(58, 13);
            this.lblFlightTime.TabIndex = 38;
            this.lblFlightTime.Text = "Flight Time";
            // 
            // txtFlightTime
            // 
            this.txtFlightTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFlightTime.Enabled = false;
            this.txtFlightTime.Location = new System.Drawing.Point(272, 132);
            this.txtFlightTime.Name = "txtFlightTime";
            this.txtFlightTime.Size = new System.Drawing.Size(55, 13);
            this.txtFlightTime.TabIndex = 39;
            this.txtFlightTime.Text = "01:45";
            this.txtFlightTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblArrTime
            // 
            this.lblArrTime.AutoSize = true;
            this.lblArrTime.Location = new System.Drawing.Point(209, 116);
            this.lblArrTime.Name = "lblArrTime";
            this.lblArrTime.Size = new System.Drawing.Size(46, 13);
            this.lblArrTime.TabIndex = 36;
            this.lblArrTime.Text = "Arr Hour";
            // 
            // txtArrTime
            // 
            this.txtArrTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtArrTime.Enabled = false;
            this.txtArrTime.Location = new System.Drawing.Point(207, 132);
            this.txtArrTime.Name = "txtArrTime";
            this.txtArrTime.Size = new System.Drawing.Size(55, 13);
            this.txtArrTime.TabIndex = 37;
            this.txtArrTime.Text = "01:45";
            this.txtArrTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDepTime
            // 
            this.lblDepTime.AutoSize = true;
            this.lblDepTime.Location = new System.Drawing.Point(148, 116);
            this.lblDepTime.Name = "lblDepTime";
            this.lblDepTime.Size = new System.Drawing.Size(53, 13);
            this.lblDepTime.TabIndex = 34;
            this.lblDepTime.Text = "Dep Hour";
            // 
            // txtDepTime
            // 
            this.txtDepTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDepTime.Enabled = false;
            this.txtDepTime.Location = new System.Drawing.Point(146, 132);
            this.txtDepTime.Name = "txtDepTime";
            this.txtDepTime.Size = new System.Drawing.Size(55, 13);
            this.txtDepTime.TabIndex = 35;
            this.txtDepTime.Text = "01:45";
            this.txtDepTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSimulatorHour
            // 
            this.lblSimulatorHour.AutoSize = true;
            this.lblSimulatorHour.Location = new System.Drawing.Point(76, 116);
            this.lblSimulatorHour.Name = "lblSimulatorHour";
            this.lblSimulatorHour.Size = new System.Drawing.Size(50, 13);
            this.lblSimulatorHour.TabIndex = 32;
            this.lblSimulatorHour.Text = "Sim Time";
            // 
            // txtSimHour
            // 
            this.txtSimHour.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSimHour.Enabled = false;
            this.txtSimHour.Location = new System.Drawing.Point(74, 132);
            this.txtSimHour.Name = "txtSimHour";
            this.txtSimHour.Size = new System.Drawing.Size(55, 13);
            this.txtSimHour.TabIndex = 33;
            this.txtSimHour.Text = "01:45:00";
            this.txtSimHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFuelRemaining
            // 
            this.lblFuelRemaining.AutoSize = true;
            this.lblFuelRemaining.Location = new System.Drawing.Point(25, 116);
            this.lblFuelRemaining.Name = "lblFuelRemaining";
            this.lblFuelRemaining.Size = new System.Drawing.Size(28, 13);
            this.lblFuelRemaining.TabIndex = 30;
            this.lblFuelRemaining.Text = "ETA";
            // 
            // txtFuelRemaining
            // 
            this.txtFuelRemaining.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFuelRemaining.Enabled = false;
            this.txtFuelRemaining.Location = new System.Drawing.Point(12, 132);
            this.txtFuelRemaining.Name = "txtFuelRemaining";
            this.txtFuelRemaining.Size = new System.Drawing.Size(55, 13);
            this.txtFuelRemaining.TabIndex = 31;
            this.txtFuelRemaining.Text = "01:45";
            this.txtFuelRemaining.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblArrival
            // 
            this.lblArrival.AutoSize = true;
            this.lblArrival.Location = new System.Drawing.Point(215, 71);
            this.lblArrival.Name = "lblArrival";
            this.lblArrival.Size = new System.Drawing.Size(36, 13);
            this.lblArrival.TabIndex = 26;
            this.lblArrival.Text = "Arrival";
            // 
            // txtAlternate
            // 
            this.txtAlternate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAlternate.Enabled = false;
            this.txtAlternate.Location = new System.Drawing.Point(273, 87);
            this.txtAlternate.Name = "txtAlternate";
            this.txtAlternate.Size = new System.Drawing.Size(51, 13);
            this.txtAlternate.TabIndex = 29;
            this.txtAlternate.Text = "----";
            this.txtAlternate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblGrossWeight
            // 
            this.lblGrossWeight.AutoSize = true;
            this.lblGrossWeight.Location = new System.Drawing.Point(26, 71);
            this.lblGrossWeight.Name = "lblGrossWeight";
            this.lblGrossWeight.Size = new System.Drawing.Size(26, 13);
            this.lblGrossWeight.TabIndex = 20;
            this.lblGrossWeight.Text = "GW";
            // 
            // lblAlternate
            // 
            this.lblAlternate.AutoSize = true;
            this.lblAlternate.Location = new System.Drawing.Point(275, 71);
            this.lblAlternate.Name = "lblAlternate";
            this.lblAlternate.Size = new System.Drawing.Size(49, 13);
            this.lblAlternate.TabIndex = 28;
            this.lblAlternate.Text = "Alternate";
            // 
            // txtGrossWeight
            // 
            this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGrossWeight.Enabled = false;
            this.txtGrossWeight.Location = new System.Drawing.Point(12, 87);
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
            this.txtArrival.Location = new System.Drawing.Point(212, 87);
            this.txtArrival.Name = "txtArrival";
            this.txtArrival.Size = new System.Drawing.Size(42, 13);
            this.txtArrival.TabIndex = 27;
            this.txtArrival.Text = "----";
            this.txtArrival.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFuel
            // 
            this.lblFuel.AutoSize = true;
            this.lblFuel.Location = new System.Drawing.Point(87, 71);
            this.lblFuel.Name = "lblFuel";
            this.lblFuel.Size = new System.Drawing.Size(28, 13);
            this.lblFuel.TabIndex = 22;
            this.lblFuel.Text = "FOB";
            // 
            // txtFuel
            // 
            this.txtFuel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFuel.Enabled = false;
            this.txtFuel.Location = new System.Drawing.Point(72, 87);
            this.txtFuel.Name = "txtFuel";
            this.txtFuel.Size = new System.Drawing.Size(59, 13);
            this.txtFuel.TabIndex = 23;
            this.txtFuel.Text = "-errors-";
            this.txtFuel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtDeparture
            // 
            this.txtDeparture.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDeparture.Enabled = false;
            this.txtDeparture.Location = new System.Drawing.Point(147, 87);
            this.txtDeparture.Name = "txtDeparture";
            this.txtDeparture.Size = new System.Drawing.Size(49, 13);
            this.txtDeparture.TabIndex = 25;
            this.txtDeparture.Text = "----";
            this.txtDeparture.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDeparture
            // 
            this.lblDeparture.AutoSize = true;
            this.lblDeparture.Location = new System.Drawing.Point(144, 71);
            this.lblDeparture.Name = "lblDeparture";
            this.lblDeparture.Size = new System.Drawing.Size(54, 13);
            this.lblDeparture.TabIndex = 24;
            this.lblDeparture.Text = "Departure";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(16, 262);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(308, 21);
            this.progressBar.TabIndex = 21;
            this.progressBar.Value = 50;
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Location = new System.Drawing.Point(146, 246);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(48, 13);
            this.lblProgressBar.TabIndex = 22;
            this.lblProgressBar.Text = "Progress";
            // 
            // chkAutoLogin
            // 
            this.chkAutoLogin.AutoSize = true;
            this.chkAutoLogin.Location = new System.Drawing.Point(256, 505);
            this.chkAutoLogin.Name = "chkAutoLogin";
            this.chkAutoLogin.Size = new System.Drawing.Size(77, 17);
            this.chkAutoLogin.TabIndex = 23;
            this.chkAutoLogin.Text = "Auto Login";
            this.chkAutoLogin.UseVisualStyleBackColor = true;
            this.chkAutoLogin.CheckedChanged += new System.EventHandler(this.chkAutoLogin_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 534);
            this.Controls.Add(this.chkAutoLogin);
            this.Controls.Add(this.lblProgressBar);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblFlightStatus);
            this.Controls.Add(this.txtFlightInformation);
            this.Controls.Add(this.lblFlightInformation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtEmail);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Label lblFlightStatus;
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
    }
}

