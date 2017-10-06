using Acars.FlightData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acars.UI
{
    public partial class FlightProfileFrm : Form
    {
        public FlightProfileFrm()
        {
            InitializeComponent();
        }

        public void Update(Flight f)
        {
            foreach (Telemetry t in f.TelemetryLog)
            {
                AddTelemetryItem(t);
            }
        }

        private void AddTelemetryItem(Telemetry t)
        {
            ListViewItem item = new ListViewItem();
            item.SubItems.Add(t.FlightPhase.ToString());
            item.SubItems.Add(t.Timestamp.ToShortTimeString());
            lstTelemetry.Items.Add(item);
        }
    }
}
