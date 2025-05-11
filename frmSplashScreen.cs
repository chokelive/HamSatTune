using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HamSatTune
{
    public partial class frmSplashScreen : Form
    {
        //public string txt_status_message = "Ongoing update satellite configulation file from internet...";
        public frmSplashScreen()
        {
            InitializeComponent();
        }

        private void frmSplashScreen_Load(object sender, EventArgs e)
        {
            lblHeader.Text = "HAMSatTune";
            lbl_version.Text = "Version." + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            lbl_statusUpdate.Text = "Ongoing update satellite configulation file from internet...";
        }

    }
}
