using Acars.FlightData;
using System;
using System.Windows.Forms;

namespace Acars.UI
{
    public partial class PilotReportFrm : Form
    {
        private Flight f;

        public PilotReportFrm()
        {
            InitializeComponent();
            TopLevel = true;
        }

        public void Show(Flight f)
        {
            txtPilotMessage.Text = "";

            this.f = f;

            Show();
        }

        private void btnSubmitFlight_Click(object sender, EventArgs e)
        {
            f.EndFlight();           

            Application.Exit();

            Close();
        }
    }
}
