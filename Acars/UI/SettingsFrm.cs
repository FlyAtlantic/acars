using Acars.FlightData;
using System;
using System.Windows.Forms;

namespace Acars.UI
{
    public partial class SettingsFrm : Form
    {
        public SettingsFrm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void SettingsFrm_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            txtFlyAtlanticEmail.Text = Properties.Settings.Default.Email;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtFlyAtlanticPassword.Text != "")
                if (FlightDatabase.ValidateLogin(txtFlyAtlanticEmail.Text, txtFlyAtlanticPassword.Text))
                {
                    Properties.Settings.Default.Email = txtFlyAtlanticEmail.Text;
                    Properties.Settings.Default.Password = txtFlyAtlanticPassword.Text;
                }
                else
                {
                    MessageBox.Show("Could not validate provided FlyAtlantic credentials.", "Bad login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            Properties.Settings.Default.Save();
        }
    }
}
