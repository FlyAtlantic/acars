using Acars.FlightData;
using System;
using System.Windows.Forms;


namespace Acars.UI
{
    public partial class PreFlightDataFrm : Form
    {
        public PreFlightDataFrm()
        {
            InitializeComponent();         
        }

        public event EventHandler<EventArgs> OnStartClicked;

        private void btnContinue_Click(object sender, EventArgs e)
        {
            //Start Flight
            OnStartClicked(this, new EventArgs());
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
