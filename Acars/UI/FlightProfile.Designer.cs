namespace Acars.UI
{
    partial class FlightProfileFrm
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
            this.lstTelemetry = new System.Windows.Forms.ListView();
            this.FlightPhaseHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TimestampHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lstTelemetry
            // 
            this.lstTelemetry.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FlightPhaseHeader,
            this.TimestampHeader});
            this.lstTelemetry.FullRowSelect = true;
            this.lstTelemetry.Location = new System.Drawing.Point(12, 12);
            this.lstTelemetry.Name = "lstTelemetry";
            this.lstTelemetry.Size = new System.Drawing.Size(636, 259);
            this.lstTelemetry.TabIndex = 0;
            this.lstTelemetry.UseCompatibleStateImageBehavior = false;
            this.lstTelemetry.View = System.Windows.Forms.View.Details;
            // 
            // FlightPhaseHeader
            // 
            this.FlightPhaseHeader.Text = "Phase";
            // 
            // TimestampHeader
            // 
            this.TimestampHeader.Text = "Timestamp";
            this.TimestampHeader.Width = 100;
            // 
            // FlightProfileFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 492);
            this.Controls.Add(this.lstTelemetry);
            this.Name = "FlightProfileFrm";
            this.Text = "FlightProfile";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstTelemetry;
        private System.Windows.Forms.ColumnHeader FlightPhaseHeader;
        private System.Windows.Forms.ColumnHeader TimestampHeader;
    }
}