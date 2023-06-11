using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.TLE;
using SGPdotNET.Util;
using System.Configuration;
using System.Reflection;
using System.Net;

namespace HamSatTune
{
    public partial class frmMain : Form
    {
        // Global Variable
        Dictionary<int, Tle> tlelist;
        Tle TleUse;
        uint norad;
        Timer trackingTimer;
        GroundStation groundStation;

        int txFreq;
        
        int startRxFreq;
        int startTxFreq;
        int prevRxFreq;
        int tuneRxFreq;
        int rxDoppler;
        int txDoppler;

        bool SatelliteFrequencyReset = false;
        bool rxFreqChangeFlag = false;

        OmniRig rig;

        struct Sqf
        {
            public string sateName;
            public double downlinkFreq;
            public double uplinkFreq;
            public string downlinkMode;
            public string uplinkMode;
            public string transponderType;
            public double downlinkOffset;
            public double uplinkOffset;
            public string comment;
        }
        Dictionary<int, Sqf> sqflist; //key = satName, value=frequency and mode detail
        Sqf sqf; // Current selectd SQF


        Form _splashScreen;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Form _splashScreen = new frmSplashScreen();
            _splashScreen.Show();

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = this.Text + " v." + version;
            this.KeyPreview = true;

            

            // Read Config
            AppSettingsSection config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location).AppSettings;
            string QTH = config.Settings["QTH"].Value;
            double lat =  M0JIV.MaidenheadLocator.MaidenheadLocatorEngine.GetLatLon(QTH).Lat;
            double lon = M0JIV.MaidenheadLocator.MaidenheadLocatorEngine.GetLatLon(QTH).Lon;

            // Setup Timer
            trackingTimer = new Timer();
            trackingTimer.Interval = 1000;
            trackingTimer.Tick += TrackingTimer_Tick;

            // Setup Omnirig
            rig = new OmniRig();

            // Set up our ground station location
            var location = new GeodeticCoordinate(Angle.FromDegrees(lat), Angle.FromDegrees(lon), 0);
            groundStation = new GroundStation(location);

            loadTle();
            loadSqf();

            _splashScreen.Hide();
            
            lbl_RxFreq.Text = "";
            lbl_TxFreq.Text = "";
            lbl_uplinkMode.Text = "";
            lbl_downlinkMode.Text = "";
            lbl_az.Text = "";
            lbl_el.Text = "";
            lbl_qth.Text = QTH;
            chk_Simplex.Enabled = false;
            lbl_rigtype.Text = "";

        }



        private void loadTle()
        {
            // download TLE from network.
            using (var client = new WebClient())
            {
                client.DownloadFile("https://www.amsat.org/tle/current/nasabare.txt", "tles.txt");
            }
            var provider = new LocalTleProvider(true, "tles.txt");

            // download Doppler.sqf from network.
            using (var client = new WebClient())
            {
                client.DownloadFile("https://raw.githubusercontent.com/chokelive/HamSatTune/main/Doppler.sqf", "Doppler.sqf");
            }

            // Get every TLE
            tlelist = provider.GetTles();
        }

        private void loadSqf()
        {
            sqflist = new Dictionary<int, Sqf>();
            int i = 0;
            foreach(string line in File.ReadLines(@"Doppler.sqf"))
            {
                string[] element = line.Split(',');
                Sqf _sqf = new Sqf();
                _sqf.sateName = element[0];
                _sqf.downlinkFreq = Convert.ToDouble(element[1]);
                _sqf.uplinkFreq = Convert.ToDouble(element[2]);
                _sqf.downlinkMode = element[3];
                _sqf.uplinkMode = element[4];
                _sqf.transponderType = element[5];
                _sqf.downlinkOffset = Convert.ToDouble(element[6]);
                _sqf.uplinkOffset = Convert.ToDouble(element[7]);
                _sqf.comment = element[8];

                sqflist.Add(i, _sqf);
                cbList.Items.Add(_sqf.sateName + " " + _sqf.comment);
                i++;
            }
        }

        private void cbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            sqf = sqflist[cbList.SelectedIndex];

            // check norad id
            norad = 0;
            foreach (var tle in tlelist)
            {
                if(tle.Value.Name == sqf.sateName)
                {
                    norad = tle.Value.NoradNumber;
                }
            }

            if (norad != 0)
            {
                TleUse = tlelist[(int)norad];

                // Start Tracking and doppler
                trackingTimer.Start();
            }

            SatelliteFrequencyReset = true;
        }


        // Update Satellite every 1 second
        private void TrackingTimer_Tick(object sender, EventArgs e)
        {
            // Rig Status
            if (chk_ConnectRig.Checked)
            {
                lbl_rigtype.Text = rig.rigType() + " " + rig.rigStatus();
            }
            else
            {
                lbl_rigtype.Text = "No RIG Connected";
            }


            // Doppler Calculation
            if ( SatelliteFrequencyReset==true) // Only 1st time do this scope
            {
                startRxFreq = (int)sqf.downlinkFreq * 1000;
                startTxFreq = (int)sqf.uplinkFreq * 1000;
                prevRxFreq = (int)sqf.downlinkFreq * 1000;
                tuneRxFreq = (int)sqf.downlinkFreq * 1000;

                if (chk_ConnectRig.Checked)
                {
                    rig.setFreq((int)tuneRxFreq);
                    rig.setVFOA();
                    switch (sqf.downlinkMode)
                    {
                        case "CW": rig.setModeCW(); break;
                        case "LSB": rig.setModeLSB(); break;
                        case "USB": rig.setModeUSB(); break;
                        case "FM": rig.setModeFM(); break;
                        case "DATA-USB": rig.setModeUSBData(); break;
                    }

                    if (rig.rigType() != "FT-817")
                    {
                        rig.setVFOB(); 
                        switch (sqf.uplinkMode)
                        {
                            case "CW": rig.setModeCW(); break;
                            case "LSB": rig.setModeLSB(); break;
                            case "USB": rig.setModeUSB(); break;
                            case "FM": rig.setModeFM(); break;
                            case "DATA-USB": rig.setModeUSBData(); break;
                        }
                        rig.setVFOA();
                    }
                }
            }
            // Get rig Rx Frequency
            if (chk_ConnectRig.Checked && SatelliteFrequencyReset!=true )
            {
                if (rig.getTxStatus() != true)
                {

                    if (rig.rigType() == "FT-817")
                    {
                        tuneRxFreq = rig.getFreq();
                    }
                    else
                    {
                        tuneRxFreq = rig.getFreqA();
                    }
                }
            }
            

            // free tune option
            int freetuneOffset = Math.Abs(tuneRxFreq - prevRxFreq);
            if(freetuneOffset>50)  // Ignore some small rig round up frequency
            {
                rxFreqChangeFlag = true;
                Console.WriteLine((tuneRxFreq - prevRxFreq).ToString());
                startRxFreq = tuneRxFreq - rxDoppler;
            }

            Satellite sat = new Satellite(TleUse);
            var observation = groundStation.Observe(sat, DateTime.UtcNow);

            // Display AZ, EL
            lbl_az.Text = observation.Azimuth.Degrees.ToString("#0.#0°");
            lbl_el.Text = observation.Elevation.Degrees.ToString("#0.#0°");

            // Display Frequency
            rxDoppler = (int)observation.GetDopplerShift((double)startRxFreq);
            tuneRxFreq = startRxFreq + rxDoppler;
            lbl_RxFreq.Text = ((double)tuneRxFreq/1000).ToString("#0.#0");
            //prevRxFreq = tuneRxFreq;
            if(chk_ConnectRig.Checked && rxFreqChangeFlag != true)
            {
                if (rig.getTxStatus() != true)
                {
                    if (rig.rigType() == "FT-817")
                    {
                        rig.setFreq(tuneRxFreq);
                        prevRxFreq = rig.getFreq();
                    }
                    else
                    {
                        rig.setFreqA(tuneRxFreq); // add for support another radio.
                        prevRxFreq = rig.getFreqA();
                    }
                                      
                }
            }
            else
            {
                prevRxFreq = tuneRxFreq;
            }


            // Uplink frequency
            txDoppler = -(int)observation.GetDopplerShift(startTxFreq);
            txFreq = startTxFreq + txDoppler;
            // Tunning Adjust follow to Tx
            if (sqf.transponderType == "REV")
            {
                txFreq = txFreq + ((int)sqf.downlinkFreq*1000 - startRxFreq);
            }
            lbl_TxFreq.Text = ((double)txFreq / 1000).ToString("#0.#0");

            // Set TX frequency to IC-705
            if (chk_ConnectRig.Checked)
            {
                if (rig.rigType() != "FT-817")
                {
                    rig.setFreqB(txFreq);
                }
            }

            // Display Mode
            lbl_downlinkMode.Text = sqf.downlinkMode;
            lbl_uplinkMode.Text = sqf.uplinkMode;

            SatelliteFrequencyReset = false;
            rxFreqChangeFlag = false;

        }

        private void bb_tune_Click(object sender, EventArgs e)
        {
            if(Int32.TryParse(txt_TuneRx.Text, out int value))
            tuneRxFreq = (int)Convert.ToDouble(txt_TuneRx.Text)*1000;
        }

        private void txt_TuneRx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bb_tune_Click(this, new EventArgs());
                txt_TuneRx.SelectAll();
            }
        }

        private void chk_ConnectRig_CheckedChanged(object sender, EventArgs e)
        {
            if(chk_ConnectRig.Checked)
            {
                rig.rigConnect();
                rig.setSplit();
                txt_TuneRx.Enabled = false;
                bb_tune.Enabled = false;
                chk_Simplex.Enabled = true;
                chk_Simplex.Checked = true;
                lbl_rigtype.Text = rig.rigType() + " " + rig.rigStatus();
            }
            else
            {
                rig.disConnectRig();
                txt_TuneRx.Enabled = true;
                bb_tune.Enabled = true;
                chk_Simplex.Enabled = false;
                lbl_rigtype.Text = "No Rig Connect";
            }

            SatelliteFrequencyReset = true;
        }

        private void bb_omirigSetup_Click(object sender, EventArgs e)
        {
            rig.OmniRigConfig();
        }

        private void bb_freqResetCenter_Click(object sender, EventArgs e)
        {
            SatelliteFrequencyReset = true;
        }

        private void bb_qth_Click(object sender, EventArgs e)
        {
            using (frmQTH qth = new frmQTH())
            {
                DialogResult dr = qth.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    qth.Dispose();
                    MessageBox.Show(this,"Software will close to reload config");
                    this.Close();
                }
                else
                {
                    qth.Dispose();
                }
            }
        }

        private void chk_Simplex_CheckedChanged(object sender, EventArgs e)
        {
            chk_Simplex.Checked = true;
        }

        private void bb_tuneTx_Click(object sender, EventArgs e)
        {
            if (chk_ConnectRig.Checked)
            {
                rig.toggleVfo();
                rig.setFreq((int)txFreq);
                switch (sqf.uplinkMode)
                {
                    case "CW": rig.setModeCW(); break;
                    case "LSB": rig.setModeLSB(); break;
                    case "USB": rig.setModeUSB(); break;
                    case "FM": rig.setModeFM(); break;
                    case "DATA-USB": rig.setModeUSBData(); break;
                }
                rig.toggleVfo();
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                bb_tuneTx_Click(this, null);
                e.Handled = true;
            }
        }

        private void linkAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/chokelive/HamSatTune/");
        }
    }
}
