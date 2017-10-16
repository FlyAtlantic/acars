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
        }

        public void Show(Flight f)
        {
            txtPilotMessage.Text = "";

            this.f = f;
        }

        private void btnSubmitFlight_Click(object sender, EventArgs e)
        {
            FlightDatabase.EndFlight(f);

            Hide();
        }
    }
}
