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

        private void btnContinue_Click(object sender, EventArgs e)
        {
            FSUIPCOffsets.zeroFuelWeight.Value = (int)Math.Round(nupZfw.Value * 2.20462262m);
            nupZfw.Value = 0;

            Hide();
        }
    }
}
