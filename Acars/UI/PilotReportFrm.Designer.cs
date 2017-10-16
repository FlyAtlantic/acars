namespace Acars.UI
{
    partial class PilotReportFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPilotMessage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSubmitFlight = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPilotMessage
            // 
            this.txtPilotMessage.Location = new System.Drawing.Point(12, 25);
            this.txtPilotMessage.Multiline = true;
            this.txtPilotMessage.Name = "txtPilotMessage";
            this.txtPilotMessage.Size = new System.Drawing.Size(252, 146);
            this.txtPilotMessage.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Any message relevant to the flight";
            // 
            // btnSubmitFlight
            // 
            this.btnSubmitFlight.Location = new System.Drawing.Point(101, 177);
            this.btnSubmitFlight.Name = "btnSubmitFlight";
            this.btnSubmitFlight.Size = new System.Drawing.Size(75, 23);
            this.btnSubmitFlight.TabIndex = 2;
            this.btnSubmitFlight.Text = "Submit Flight";
            this.btnSubmitFlight.UseVisualStyleBackColor = true;
            this.btnSubmitFlight.Click += new System.EventHandler(this.btnSubmitFlight_Click);
            // 
            // PilotReportFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 207);
            this.Controls.Add(this.btnSubmitFlight);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPilotMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PilotReportFrm";
            this.Text = "Pilot Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPilotMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSubmitFlight;
    }
}