using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HamSatTune
{
    public partial class frmQTH : Form
    {
        string prevQTH = "";
        public frmQTH()
        {
            InitializeComponent();
        }

        private void frmQTH_Load(object sender, EventArgs e)
        {
            AppSettingsSection config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location).AppSettings;
            prevQTH = config.Settings["QTH"].Value;
            txtQTH.Text = prevQTH;
        }

        private void bb_saveQTH_Click(object sender, EventArgs e)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["QTH"].Value = txtQTH.Text;
            configuration.Save(ConfigurationSaveMode.Minimal, true);
            ConfigurationManager.RefreshSection("appSettings");

            if (prevQTH != txtQTH.Text)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
