using Acars.FlightData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acars
{
    class App : ApplicationContext
    {
        //Component declarations
        private NotifyIcon TrayIcon;
        private ContextMenuStrip TrayIconContextMenu;
        private ToolStripMenuItem CloseMenuItem;
        private ToolStripMenuItem OpenOldFormMenuItem;

        private Form1 oldForm;

        private Timer timer;

        private Flight flight;

        public App()
        {
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            InitializeComponent();
            TrayIcon.Visible = true;

            timer.Start();
        }

        private void InitializeComponent()
        {
            TrayIcon = new NotifyIcon();

            TrayIcon.Text = "Fly Atlantic ACARS";


            //The icon is added to the project resources.
            //Here I assume that the name of the file is 'TrayIcon.ico'
            TrayIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            //Optional - handle doubleclicks on the icon:
            TrayIcon.DoubleClick += TrayIcon_DoubleClick;

            //Optional - Add a context menu to the TrayIcon:
            TrayIconContextMenu = new ContextMenuStrip();
            CloseMenuItem = new ToolStripMenuItem();
            OpenOldFormMenuItem = new ToolStripMenuItem();
            TrayIconContextMenu.SuspendLayout();


            //
            // Timer
            //
            timer = new Timer();
            timer.Interval = 10000;
            timer.Tick += new EventHandler(GetFlightTimer_Tick);
            // 
            // TrayIconContextMenu
            // 
            this.TrayIconContextMenu.Items.AddRange(new ToolStripItem[] {
                this.OpenOldFormMenuItem,
                this.CloseMenuItem
            });
            this.TrayIconContextMenu.Name = "TrayIconContextMenu";
            this.TrayIconContextMenu.Size = new Size(153, 70);
            // 
            // CloseMenuItem
            // 
            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Size = new Size(152, 22);
            this.CloseMenuItem.Text = "Exit";
            this.CloseMenuItem.Click += new EventHandler(this.CloseMenuItem_Click);
            // 
            // OpenOldFormMenuItem
            // 
            this.OpenOldFormMenuItem.Name = "OpenOldFormMenuItem";
            this.OpenOldFormMenuItem.Size = new Size(152, 22);
            this.OpenOldFormMenuItem.Text = "Open Old Form";
            this.OpenOldFormMenuItem.Click += new EventHandler(this.OpenOldFormMenuItem_Click);

            TrayIconContextMenu.ResumeLayout(false);
            TrayIcon.ContextMenuStrip = TrayIconContextMenu;
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            //Cleanup so that the icon will be removed when the application is closed
            TrayIcon.Visible = false;
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            //Here you can do stuff if the tray icon is doubleclicked
            TrayIcon.ShowBalloonTip(10000);
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to close me?",
                    "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void OpenOldFormMenuItem_Click(object sender, EventArgs e)
        {
            if (oldForm == null)
                oldForm = new Form1();

            oldForm.Show();
        }

        private void GetFlightTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                flight = FlightDatabase.GetFlight();
                if (flight != null)
                {
                    timer.Stop();

                    // notify user
                    TrayIcon.BalloonTipIcon = ToolTipIcon.Info;
                    TrayIcon.BalloonTipText = String.Format("{0} from {1} to {2}",
                                                            flight.LoadedFlightPlan.AtcCallsign,
                                                            flight.LoadedFlightPlan.DepartureICAO,
                                                            flight.LoadedFlightPlan.ArrivalICAO);
                    TrayIcon.BalloonTipTitle = "New flight assigned";
                    TrayIcon.ShowBalloonTip(10000);
                }
            }
            catch (Exception crap)
            {

            }
        }
    }
}
