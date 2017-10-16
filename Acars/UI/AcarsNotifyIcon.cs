using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Acars.UI
{
    public class AcarsNotifyIcon
    {
        public NotifyIcon StatusIcon
        { get; private set; }

        public delegate void OpenFlightStatus_ClickEventHandler(object sender, EventArgs e);
        public event OpenFlightStatus_ClickEventHandler OpenFlightStatus_Click;

        public delegate void OpenSettings_ClickEventHandler(object sender, EventArgs e);
        public event OpenSettings_ClickEventHandler OpenSettings_Click;

        public delegate void Close_ClickEventHandler(object sender, EventArgs e);
        public event Close_ClickEventHandler Close_Click;

        private ContextMenuStrip TrayIconContextMenu;
        private ToolStripMenuItem StatusTextMenuItem;
        private ToolStripMenuItem CloseMenuItem;
        private ToolStripMenuItem OpenOldFormMenuItem;
        private ToolStripMenuItem SettingsMenuItem;

        public AcarsNotifyIcon()
        {
            InitializeComponent();
            
            SetStatusText("Waiting for flight");
        }

        public void Dispose()
        {
            StatusIcon.Visible = false;
        }

        public void InitializeComponent()
        {
            // NotifyIcon
            StatusIcon = new NotifyIcon();
            StatusIcon.Visible = true;
            StatusIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            StatusIcon.DoubleClick += StatusIcon_DoubleClick;

            // Context Menu
            TrayIconContextMenu = new ContextMenuStrip();

            // ConextMenus
            StatusTextMenuItem = new ToolStripMenuItem();
            CloseMenuItem = new ToolStripMenuItem();
            OpenOldFormMenuItem = new ToolStripMenuItem();
            SettingsMenuItem = new ToolStripMenuItem();
            TrayIconContextMenu.SuspendLayout();

            // 
            // TrayIconContextMenu
            // 
            this.TrayIconContextMenu.Items.AddRange(new ToolStripItem[] {
                this.StatusTextMenuItem,
                this.OpenOldFormMenuItem,
                this.SettingsMenuItem,
                this.CloseMenuItem
            });
            this.TrayIconContextMenu.Name = "TrayIconContextMenu";
            this.TrayIconContextMenu.Size = new Size(153, 70);
            //
            // StatusTextMenuItem
            //
            this.StatusTextMenuItem.Name = "StatusTextMenuItem";
            this.StatusTextMenuItem.Size = new Size(152, 22);
            this.StatusTextMenuItem.Text = "FlyAtlantic ACARS";
            // 
            // CloseMenuItem
            // 
            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Size = new Size(152, 22);
            this.CloseMenuItem.Text = "Exit";
            this.CloseMenuItem.Click += (s, e) => { Close_Click(this, e); };
            // 
            // OpenOldFormMenuItem
            // 
            this.OpenOldFormMenuItem.Name = "OpenOldFormMenuItem";
            this.OpenOldFormMenuItem.Size = new Size(152, 22);
            this.OpenOldFormMenuItem.Text = "Flight Status";
            this.OpenOldFormMenuItem.Click += (s, e) => { OpenFlightStatus_Click(this, e); };
            // 
            // SettingsMenuItem
            // 
            this.SettingsMenuItem.Name = "SettingsMenuItem";
            this.SettingsMenuItem.Size = new Size(152, 22);
            this.SettingsMenuItem.Text = "Settings";
            this.SettingsMenuItem.Click += (s, e) => { OpenSettings_Click(this, e); };

            TrayIconContextMenu.ResumeLayout(false);
            StatusIcon.ContextMenuStrip = TrayIconContextMenu;
        }

        #region UI Event Handlers

        /// <summary>
        /// Double click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusIcon_DoubleClick(object sender, EventArgs e)
        {

        }

        #endregion UI Event Handlers

        public void ShowBalloonTip(ToolTipIcon BallonTipIcon, string BallonTipText, string BallonTipTitle, int timeout = 10000)
        {
            StatusIcon.BalloonTipIcon = BallonTipIcon;
            StatusIcon.BalloonTipText = BallonTipText;
            StatusIcon.BalloonTipTitle = BallonTipTitle;
            StatusIcon.ShowBalloonTip(timeout);
        }

        public void SetStatusText(string Text)
        {
            StatusIcon.Text = "FlyAltantic ACARS - " + Text;
            StatusTectMenuItem.Text = StatusIcon.Text;
        }
    }
}
