using OmniRig;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.TLE;
using SGPdotNET.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace HamSatTune
{
    public partial class frmMain : Form
    {
        // Global Variable

        int updateInterval = 1000; //ms
        const int Ft4FastTimerInterval = 250; //ms
        const int RotorUpdateInterval = 1000; //ms
        const int MainStatusUpdateInterval = 1000; //ms
        const int VoicePassRefreshIntervalSeconds = 30;
        const int VoiceAnnouncementLeadMinutes = 5;
        const int TleDownloadTimeoutMs = 15000; //ms
        const string TleFileName = "tles.txt";
        const string ManualTleFileName = "manual_tles.txt";

        Dictionary<int, Tle> tlelist;
        Tle TleUse;
        uint norad;
        Timer trackingTimer;
        GroundStation groundStation;

        int txFreq;
        
        int startRxFreq;
        int startRxFreqWithOffset;
        int startTxFreq;
        int startTxFreqWithOffset;
        int prevRxFreq;
        int tuneRxFreq;
        int rxDoppler;
        int txDoppler;

        Double az;
        Double el;
        Double el_last;
        DateTime lastRotorTrackTime = DateTime.MinValue;
        DateTime lastMainStatusUpdateTime = DateTime.MinValue;
        DateTime lastVoicePassRefreshTime = DateTime.MinValue;
        readonly HashSet<string> announcedVoiceEvents = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        List<VoicePass> voicePasses = new List<VoicePass>();
        bool hasObservedElevation;

        bool SatelliteFrequencyReset = false;
        bool SatelliteModeReset = false;
        bool rxFreqChangeFlag = false; 

        OmniRig rig;

        Dictionary<int, Sqf> sqflist; //key = satName, value=frequency and mode detail
        Sqf sqf; // Current selectd SQF
        Sqf sqf_last; // Lasted SQF


        frmSplashScreen _splashScreen;
        HamSatTune.Properties.frmMap mapForm;
        frmNextPass nextPassForm;
        frmRotorControl rotorControlForm;
        frmLog logForm;
        RotorControlProcess mainRotor;
        SpeechSynthesizer voiceSynthesizer;
        bool useThaiVoice;

        public frmMain()
        {
            InitializeComponent();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (voiceSynthesizer != null)
            {
                voiceSynthesizer.SpeakAsyncCancelAll();
                voiceSynthesizer.Dispose();
                voiceSynthesizer = null;
            }

            mainRotor?.Dispose();
            base.OnFormClosed(e);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _splashScreen = new frmSplashScreen();
            _splashScreen.Show();
            _splashScreen.Refresh();

            try
            {
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                this.Text = this.Text + " v." + version;
                this.KeyPreview = true;

                // Read Config
                AppSettingsSection config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location).AppSettings;
                KeyValueConfigurationElement qthSetting = config.Settings["QTH"];
                if (qthSetting == null || string.IsNullOrWhiteSpace(qthSetting.Value))
                {
                    throw new ConfigurationErrorsException("Missing QTH setting in App.config.");
                }

                string QTH = qthSetting.Value.Trim();
                Globals.CurrentGrid = QTH.ToUpperInvariant();
                double lat = M0JIV.MaidenheadLocator.MaidenheadLocatorEngine.GetLatLon(QTH).Lat;
                double lon = M0JIV.MaidenheadLocator.MaidenheadLocatorEngine.GetLatLon(QTH).Lon;

                // Setup Timer
                trackingTimer = new Timer();
                trackingTimer.Interval = updateInterval;
                trackingTimer.Tick += TrackingTimer_Tick;

                if (IsVoiceAnnouncementsEnabled())
                {
                    voiceSynthesizer = new SpeechSynthesizer();
                    useThaiVoice = SelectConfiguredVoice(voiceSynthesizer);
                }

                // Setup Omnirig
                rig = new OmniRig();

                // Set up our ground station location
                var location = new GeodeticCoordinate(Angle.FromDegrees(lat), Angle.FromDegrees(lon), 0);
                groundStation = new GroundStation(location);

                loadTle();
                loadSqf();

                lbl_RxFreq.Text = "";
                lbl_TxFreq.Text = "";
                lbl_uplinkMode.Text = "";
                lbl_downlinkMode.Text = "";
                lbl_az.Text = "";
                lbl_el.Text = "";
                lbl_qth.Text = QTH;
                chk_Simplex.Enabled = false;
                lbl_rigtype.Text = "";
                lbl_rig2type.Text = "";
                lbl_rotortype.Text = "";
            }
            catch (Exception ex)
            {
                if (_splashScreen != null && !_splashScreen.IsDisposed)
                {
                    _splashScreen.Hide();
                }

                MessageBox.Show(this, "Startup failed: " + ex.Message, "HamSatTune", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            finally
            {
                if (_splashScreen != null)
                {
                    _splashScreen.Hide();
                    _splashScreen.Dispose();
                    _splashScreen = null;
                }
            }

        }



        private void loadTle()
        {
            // download TLE from network.  
            try
            {
                using (var client = new TimeoutWebClient(TleDownloadTimeoutMs))
                {
                    string tempFile = "tles_temp.txt";
                    client.DownloadFile("https://www.amsat.org/tle/current/nasabare.txt", tempFile);
                    File.Copy(tempFile, TleFileName, true);
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                }
            }
            catch (Exception)
            {
                UpdateSplashStatus("Cannot update lasted TLE file from Internet...");
                System.Threading.Thread.Sleep(3000);

            }

            if (!File.Exists(TleFileName) && !File.Exists(ManualTleFileName))
            {
                throw new FileNotFoundException("Cannot find local TLE file.", TleFileName);
            }

            loadLocalTle();
        }

        private void loadLocalTle()
        {
            if (!File.Exists(TleFileName) && !File.Exists(ManualTleFileName))
            {
                throw new FileNotFoundException("Cannot find local TLE file.", TleFileName);
            }

            tlelist = new Dictionary<int, Tle>();

            MergeTleFile(TleFileName, tlelist);
            MergeTleFile(ManualTleFileName, tlelist);
        }

        private void MergeTleFile(string path, Dictionary<int, Tle> target)
        {
            if (!File.Exists(path))
            {
                return;
            }

            LocalTleProvider provider = new LocalTleProvider(true, path);
            foreach (KeyValuePair<int, Tle> item in provider.GetTles())
            {
                target[item.Key] = item.Value;
            }
        }

        private void loadSqf()
        {
            sqflist = new Dictionary<int, Sqf>();
            cbList.Items.Clear();

            List<Sqf> sortedSqfList = new List<Sqf>();

            if (!File.Exists("Doppler.sqf"))
            {
                throw new FileNotFoundException("Cannot find Doppler.sqf.", "Doppler.sqf");
            }

            int lineNumber = 0;
            foreach(string line in File.ReadLines(@"Doppler.sqf"))
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] element = line.Split(',');
                if (element.Length < 9)
                {
                    throw new FormatException("Invalid Doppler.sqf format at line " + lineNumber + ".");
                }

                Sqf _sqf = new Sqf();
                _sqf.sateName = element[0].Trim();
                _sqf.downlinkFreq = ParseSqfDouble(element[1], lineNumber, "downlink frequency", false);
                _sqf.uplinkFreq = ParseSqfDouble(element[2], lineNumber, "uplink frequency", true);
                _sqf.downlinkMode = element[3].Trim();
                _sqf.uplinkMode = element[4].Trim();
                _sqf.transponderType = element[5].Trim();
                _sqf.downlinkOffset = ParseSqfDouble(element[6], lineNumber, "downlink offset", true);
                _sqf.uplinkOffset = ParseSqfDouble(element[7], lineNumber, "uplink offset", true);
                _sqf.comment = element[8].Trim();

                sortedSqfList.Add(_sqf);
            }

            if (sortedSqfList.Count == 0)
            {
                throw new InvalidDataException("Doppler.sqf does not contain any satellite entries.");
            }

            sortedSqfList = sortedSqfList
                .OrderBy(item => item.sateName)
                .ThenBy(item => item.comment)
                .ToList();

            for (int i = 0; i < sortedSqfList.Count; i++)
            {
                Sqf _sqf = sortedSqfList[i];
                sqflist.Add(i, _sqf);
                cbList.Items.Add(_sqf.sateName + " " + _sqf.comment);
            }

        }

        private void ReloadSqfPreservingSelection()
        {
            string selectedName = sqf.sateName;
            string selectedComment = sqf.comment;

            loadSqf();

            int selectedIndex = FindSqfIndex(selectedName, selectedComment);
            if (selectedIndex < 0)
            {
                selectedIndex = FindSqfIndex(selectedName, null);
            }

            if (selectedIndex >= 0)
            {
                cbList.SelectedIndex = selectedIndex;
                return;
            }

            Globals.CurrentSqf = new Sqf();
            sqf = Globals.CurrentSqf;
            sqf_last = sqf;
        }

        private int FindSqfIndex(string satelliteName, string comment)
        {
            if (string.IsNullOrWhiteSpace(satelliteName) || sqflist == null)
            {
                return -1;
            }

            foreach (KeyValuePair<int, Sqf> item in sqflist)
            {
                Sqf candidate = item.Value;
                if (!string.Equals(candidate.sateName, satelliteName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (comment == null || string.Equals(candidate.comment, comment, StringComparison.OrdinalIgnoreCase))
                {
                    return item.Key;
                }
            }

            return -1;
        }

        private static double ParseSqfDouble(string value, int lineNumber, string fieldName, bool allowEmpty)
        {
            if (string.IsNullOrWhiteSpace(value) && allowEmpty)
            {
                return 0;
            }

            double result;
            if (!double.TryParse(value.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                throw new FormatException("Invalid " + fieldName + " in Doppler.sqf at line " + lineNumber + ".");
            }

            return result;
        }

        private void UpdateSplashStatus(string message)
        {
            if (_splashScreen == null || _splashScreen.IsDisposed)
            {
                return;
            }

            _splashScreen.lbl_statusUpdate.Text = message;
            _splashScreen.Refresh();
            Application.DoEvents(); // Allow UI to refresh while startup is still synchronous.
        }

        private class TimeoutWebClient : WebClient
        {
            private readonly int timeoutMs;

            public TimeoutWebClient(int timeoutMs)
            {
                this.timeoutMs = timeoutMs;
            }

            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);
                if (request != null)
                {
                    request.Timeout = timeoutMs;
                }

                return request;
            }
        }

        private void cbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.CurrentSqf = sqflist[cbList.SelectedIndex];
            sqf = Globals.CurrentSqf;

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

                hasObservedElevation = false;

                // Start Tracking and doppler
                trackingTimer.Start();
            }

            // For smooth mode change, We will reset frequency when Sattellite and Frequency is difference only.
            if (sqf_last.sateName == sqf.sateName)
            {
                if(sqf_last.downlinkFreq != sqf.downlinkFreq || sqf_last.uplinkFreq != sqf.uplinkFreq)
                {
                    SatelliteFrequencyReset = true;
                }
            }else if(sqf_last.sateName != sqf.sateName)
            {
                SatelliteFrequencyReset = true;
            }

            SatelliteModeReset = true; // only set new mode.

            sqf_last = sqf;
            RefreshOpenLogContext();

        }


        // Update Satellite every 1 second
        private void TrackingTimer_Tick(object sender, EventArgs e)
        {
            sqf = Globals.CurrentSqf; // Get latest SQF in case user change SQF when timer is running.
            bool updateMainStatus = (DateTime.Now - lastMainStatusUpdateTime).TotalMilliseconds >= MainStatusUpdateInterval;

            // Rig1 Status
            if (updateMainStatus)
            {
                if (chk_ConnectRig.Checked)
                {
                    lbl_rigtype.Text = "Rig1: " + rig.rigType() + " " + rig.rigStatus();
                }
                else
                {
                    lbl_rigtype.Text = "No Rig1 Connected";
                }

                // Rig2 Status
                if (chk_ConnectRig2.Checked)
                {
                    lbl_rig2type.Text = "Rig2: " + rig.rig2Type() + " " + rig.rig2Status();
                }
                else
                {
                    lbl_rig2type.Text = "No Rig2 Connected";
                }
            }


            // Doppler Calculation
            if ( SatelliteFrequencyReset==true) // Only 1st time do this scope
            {
                ResetDopplerBase();
            }

            if (SatelliteModeReset == true) // Only 1st time do this scope
            {
                if (chk_ConnectRig.Checked)
                {
                    rig.setFreq((int)tuneRxFreq);
                    rig.setVFOA();
                    switch (sqf.downlinkMode) // for downlink
                    {
                        case "CW": rig.setModeCW_RX(); break;
                        case "LSB": rig.setModeLSB(); break;
                        case "USB": rig.setModeUSB(); break;
                        case "FM": rig.setModeFM(); break;
                        case "USB-D": rig.setModeUSBData(); break;
                        case "USB-L": rig.setModeLSBData(); break;
                    }

                    if (rig.rigType() != "FT-817" || rig.rigType() == "FT-857" || rig.rigType() == "FT-897")  // for downlink
                    {
                        rig.setVFOB();
                        switch (sqf.uplinkMode)
                        {
                            case "CW": rig.setModeCW(); break;
                            case "LSB": rig.setModeLSB(); break;
                            case "USB": rig.setModeUSB(); break;
                            case "FM": rig.setModeFM(); break;
                            case "USB-D": rig.setModeUSBData(); break;
                            case "LSB-D": rig.setModeLSBData(); break;
                        }
                        rig.setVFOA();
                    }

                    if (chk_ConnectRig2.Checked)  // If radio2 Connected
                    {
                        switch (sqf.uplinkMode)
                        {
                            case "CW": rig.setModeCW_Rig2(); break;
                            case "LSB": rig.setModeLSB_Rig2(); break;
                            case "USB": rig.setModeUSB_Rig2(); break;
                            case "FM": rig.setModeFM_Rig2(); break;
                            case "USB-D": rig.setModeUSBData_Rig2(); break;
                            case "LSB-D": rig.setModeLSBData_Rig2(); break;
                        }

                        if (rig.rig2Type() == "FT-817" || rig.rig2Type() == "FT-857" || rig.rig2Type() == "FT-897") // Special only for FT-8x7
                        {
                            switch (sqf.uplinkMode)
                            {
                                case "LSB-D": rig.setModeUSBData_Rig2(); break;
                            }
                        }
                    }

                }
            }



            // Get rig Rx Frequency
            if (chk_ConnectRig.Checked && SatelliteFrequencyReset!=true )
            {
                int rigRxFreq;
                if (TryGetRigRxFrequency(out rigRxFreq))
                {
                    tuneRxFreq = rigRxFreq;
                }
            }
            

            Satellite sat = new Satellite(TleUse);
            DateTime nowUtc = DateTime.UtcNow;
            var observation = groundStation.Observe(sat, nowUtc);

            // Get AZ, EL
            az = observation.Azimuth.Degrees;
            el = observation.Elevation.Degrees;
            Globals.CurrentAz = az;
            Globals.CurrentEl = el;
            UpdateTrackingIntervalForPass();

            UpdateVoiceAnnouncements(nowUtc);

            // reset frequency if Satellite AOS
            bool satelliteAos = hasObservedElevation && el_last < 0 && el > 0;
            if (satelliteAos)
            {
                ResetDopplerBase();
                SatelliteModeReset = true;
            }
            el_last = el;
            hasObservedElevation = true;

            // Display AZ, EL
            if (updateMainStatus)
            {
                lbl_az.Text = az.ToString("#0.#0°");
                lbl_el.Text = el.ToString("#0.#0°");
                lastMainStatusUpdateTime = DateTime.Now;
            }

            // Calculate current downlink Doppler before checking free-tune.
            // Near high elevation the Doppler slope is steep, so using last tick's shift can re-baseline incorrectly.
            startRxFreqWithOffset = startRxFreq + (int)sqf.downlinkOffset; // add offset for user to adjust downlink frequency, avoid some radio cannot set exact frequency issue.
            rxDoppler = (int)observation.GetDopplerShift((double)startRxFreqWithOffset);

            // free tune option
            int freetuneOffset = Math.Abs(tuneRxFreq - prevRxFreq);
            int freetuneThreshold = el > 30 ? 250 : 50; // high elevation Doppler changes quickly, so ignore small CAT/readback drift.
            if(freetuneOffset > freetuneThreshold)
            {
                rxFreqChangeFlag = true;
                Console.WriteLine((tuneRxFreq - prevRxFreq).ToString());
                startRxFreq = tuneRxFreq - rxDoppler - (int)sqf.downlinkOffset;
                startRxFreqWithOffset = startRxFreq + (int)sqf.downlinkOffset;
                rxDoppler = (int)observation.GetDopplerShift((double)startRxFreqWithOffset);
            }

            // Display Frequency
            tuneRxFreq = startRxFreqWithOffset + rxDoppler;
            lbl_RxFreq.Text = ((double)tuneRxFreq/1000).ToString("#0.#0");
            Globals.CalculatedDownlinkHz = tuneRxFreq;
            //prevRxFreq = tuneRxFreq;
            if(chk_ConnectRig.Checked && rxFreqChangeFlag != true)
            {
                if (rig.getTxStatus() != true)
                {
                    if (rig.rigType() == "FT-817" || rig.rigType() == "FT-857" || rig.rigType() == "FT-897" || rig.rigType() == "IC-756 Pro")
                    {
                        rig.setFreq(tuneRxFreq);
                        int rigRxFreq;
                        prevRxFreq = TryGetRigRxFrequency(out rigRxFreq) ? rigRxFreq : tuneRxFreq;
                    }
                    //else if (rig.rigType() == "IC-756 Pro")
                    //{
                    //    rig.setFreq(tuneRxFreq);
                    //    prevRxFreq = rig.getFreq();
                    //}
                    else if (rig.rigType().Contains("IC-9700"))
                    { 
                        rig.setVFOA();
                        rig.setFreqA(tuneRxFreq);
                        int rigRxFreq;
                        prevRxFreq = TryGetRigRxFrequency(out rigRxFreq) ? rigRxFreq : tuneRxFreq;
                    }

                    else
                    {
                        rig.setFreqA(tuneRxFreq); // add for support another radio.
                        int rigRxFreq;
                        prevRxFreq = TryGetRigRxFrequency(out rigRxFreq) ? rigRxFreq : tuneRxFreq;
                    }
                                      
                }
            }
            else
            {
                prevRxFreq = tuneRxFreq;
            }

            // #############################################################################
            // Uplink frequency
            // #############################################################################

            // add offset for user to adjust uplink frequency, avoid some radio cannot set exact frequency issue.
            startTxFreqWithOffset = startTxFreq + (int)sqf.uplinkOffset;

            // Special for FT4, as FT4 have larger Doppler shift, we will adjust Tx frequency follow Rx frequency change to make sure Tx signal always in the middle of Rx signal. 
            if (sqf.comment.Contains("FT4"))
            {
                // Predict middle of FT4 TX slot
                double predictAhead = 3.75;
                var observationMidTx = groundStation.Observe(sat, DateTime.UtcNow.AddSeconds(predictAhead));
                txDoppler = -(int)observationMidTx.GetDopplerShift(startTxFreqWithOffset);
                Console.WriteLine("Predict Doppler Shift for Tx: " + txDoppler.ToString() + " Hz");
            }

            // Normal Doppler
            else 
            {

                txDoppler = -(int)observation.GetDopplerShift(startTxFreqWithOffset);
                Console.WriteLine("Doppler Shift for Tx: " + txDoppler.ToString() + " Hz");
            }

            //txDoppler = -(int)observation.GetDopplerShift(startTxFreqWithOffset);
            txFreq = startTxFreqWithOffset + txDoppler;
            // Tunning Adjust follow to Tx
            if (sqf.transponderType == "REV")
            {
                txFreq = txFreq + ((int)sqf.downlinkFreq*1000 - startRxFreq);
            }
            lbl_TxFreq.Text = ((double)txFreq / 1000).ToString("#0.#0");
            Globals.CalculatedUplinkHz = txFreq;
            RefreshOpenLogFrequencies();

            // Set TX frequency to IC-705 or IC-9700
            if (chk_ConnectRig.Checked)
            {
                //if (rig.rigType() != "FT-817")
                if (rig.rigType().Contains("IC-705"))
                {
                    rig.setFreqB(txFreq);
                }
                else if (rig.rigType().Contains("IC-7100"))
                {
                    rig.setFreqB(txFreq);
                }
                else if (rig.rigType().Contains("IC-9700")) // this model need to control Sub Band
                {
                    rig.setVFOB();
                    rig.setFreq(txFreq);
                    rig.setVFOA();
                }

                // Set TX for Radio 2
                if (chk_ConnectRig2.Checked)
                {
                    rig.setFreq_Rig2(txFreq);
                }
            }

            // Display Mode
            if(sqf.downlinkMode == "CW")
            {
                lbl_downlinkMode.Text = "USB"; // Set USB downlink mode for CW linear transponder
                // and adjust frequency to lower 1 khz to ensure RX signal is covered.
            }
            else
            {
                lbl_downlinkMode.Text = sqf.downlinkMode;
            }
            lbl_uplinkMode.Text = sqf.uplinkMode;

            UpdateRotorTracking();

            Globals.LastTrackingUpdateTime = DateTime.Now;
            Globals.TrackingUpdateNumber++;

            SatelliteFrequencyReset = false;
            SatelliteModeReset = false;
            rxFreqChangeFlag = false;

        }

        private bool TryGetRigRxFrequency(out int frequency)
        {
            frequency = 0;

            if (rig.getTxStatus())
            {
                return false;
            }

            if (rig.rigType().Contains("IC-9700"))
            {
                rig.setVFOA();
            }

            if (rig.rigType() == "FT-817" || rig.rigType() == "FT-857" || rig.rigType() == "FT-897" || rig.rigType() == "IC-756 Pro")
            {
                frequency = rig.getFreq();
            }
            else
            {
                frequency = rig.getFreqA();
            }

            return IsValidFrequency(frequency);
        }

        private bool IsValidFrequency(int frequency)
        {
            return frequency >= 1000000;
        }

        private void UpdateTrackingIntervalForPass()
        {
            int interval = IsFt4Mode() ? Ft4FastTimerInterval : updateInterval;

            if (trackingTimer.Interval != interval)
            {
                trackingTimer.Interval = interval;
            }
        }

        private void UpdateVoiceAnnouncements(DateTime nowUtc)
        {
            if (voiceSynthesizer == null || groundStation == null || sqflist == null || tlelist == null)
            {
                return;
            }

            if ((nowUtc - lastVoicePassRefreshTime).TotalSeconds >= VoicePassRefreshIntervalSeconds)
            {
                RefreshVoicePasses(nowUtc);
            }

            foreach (VoicePass pass in voicePasses)
            {
                TimeSpan untilAos = pass.Aos - nowUtc;
                string warningKey = GetVoiceEventKey(pass, "warning");
                string aosKey = GetVoiceEventKey(pass, "aos");

                if (untilAos > TimeSpan.Zero &&
                    untilAos <= TimeSpan.FromMinutes(VoiceAnnouncementLeadMinutes) &&
                    announcedVoiceEvents.Add(warningKey))
                {
                    AnnounceVoice(BuildAosWarning(pass.Name));
                }

                if (pass.Aos <= nowUtc && nowUtc - pass.Aos <= TimeSpan.FromSeconds(30) &&
                    announcedVoiceEvents.Add(aosKey))
                {
                    AnnounceVoice(BuildAosAnnouncement(pass.Name));
                }
            }
        }

        private void RefreshVoicePasses(DateTime nowUtc)
        {
            List<VoicePass> refreshedPasses = new List<VoicePass>();
            HashSet<string> addedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (Sqf configuredSatellite in sqflist.Values)
            {
                if (string.IsNullOrWhiteSpace(configuredSatellite.sateName) ||
                    !addedNames.Add(configuredSatellite.sateName))
                {
                    continue;
                }

                Tle tle = tlelist.Values.FirstOrDefault(item =>
                    string.Equals(item.Name, configuredSatellite.sateName, StringComparison.OrdinalIgnoreCase));
                if (tle == null)
                {
                    continue;
                }

                try
                {
                    Satellite satellite = new Satellite(tle);
                    List<SatelliteVisibilityPeriod> passes = groundStation.Observe(
                        satellite,
                        nowUtc.AddSeconds(-30),
                        nowUtc.AddHours(24),
                        TimeSpan.FromSeconds(30),
                        Angle.Zero,
                        false,
                        false,
                        0);

                    SatelliteVisibilityPeriod pass = passes.FirstOrDefault(item => item.End > nowUtc);
                    if (pass != null && pass.MaxElevation.Degrees > 0)
                    {
                        refreshedPasses.Add(new VoicePass
                        {
                            Name = configuredSatellite.sateName,
                            Aos = pass.Start,
                            Los = pass.End
                        });
                    }
                }
                catch
                {
                    // An invalid satellite must not stop announcements for the others.
                }
            }

            voicePasses = refreshedPasses;
            lastVoicePassRefreshTime = nowUtc;
        }

        private string GetVoiceEventKey(VoicePass pass, string eventName)
        {
            // Passes are recalculated every 30 seconds.  The calculated AOS can
            // differ by a few seconds between refreshes, even when it is the
            // same physical pass.  Do not use the raw timestamp as the event
            // identity or the same satellite may be announced repeatedly.
            DateTime aosMinute = new DateTime(
                pass.Aos.Year,
                pass.Aos.Month,
                pass.Aos.Day,
                pass.Aos.Hour,
                pass.Aos.Minute,
                0,
                DateTimeKind.Utc);

            return pass.Name + "|" + aosMinute.Ticks + "|" + eventName;
        }

        private void AnnounceVoice(string message)
        {
            if (voiceSynthesizer == null || string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            try
            {
                voiceSynthesizer.SpeakAsync(message);
            }
            catch
            {
                // Voice output must never interrupt satellite tracking.
            }
        }

        private bool SelectConfiguredVoice(SpeechSynthesizer synthesizer)
        {
            string language = GetAppSetting("VoiceLanguage", "th-TH");
            if (!string.Equals(language, "th-TH", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            InstalledVoice thaiVoice = synthesizer.GetInstalledVoices()
                .FirstOrDefault(voice => voice.Enabled &&
                    voice.VoiceInfo.Culture.Name.StartsWith("th-TH", StringComparison.OrdinalIgnoreCase));

            if (thaiVoice == null)
            {
                return false;
            }

            synthesizer.SelectVoice(thaiVoice.VoiceInfo.Name);
            return true;
        }

        private string BuildAosWarning(string satelliteName)
        {
            return useThaiVoice
                ? "อีกห้านาที ดาวเทียม " + satelliteName + " จะเริ่มรับสัญญาณได้"
                : satelliteName + " will reach AOS in five minutes.";
        }

        private string BuildAosAnnouncement(string satelliteName)
        {
            return useThaiVoice
                ? "ดาวเทียม " + satelliteName + " เริ่มรับสัญญาณได้แล้ว"
                : satelliteName + " AOS now.";
        }

        private bool IsVoiceAnnouncementsEnabled()
        {
            bool enabled;
            return bool.TryParse(GetAppSetting("VoiceAnnouncements", "true"), out enabled) && enabled;
        }

        private class VoicePass
        {
            public string Name { get; set; }
            public DateTime Aos { get; set; }
            public DateTime Los { get; set; }
        }

        private bool IsFt4Mode()
        {
            return !string.IsNullOrEmpty(sqf.comment) && sqf.comment.Contains("FT4");
        }

        private void ResetDopplerBase()
        {
            startRxFreq = (int)sqf.downlinkFreq * 1000;
            startTxFreq = (int)sqf.uplinkFreq * 1000;
            prevRxFreq = (int)sqf.downlinkFreq * 1000;
            tuneRxFreq = (int)sqf.downlinkFreq * 1000;

            if (sqf.downlinkMode == "CW")
            {
                startRxFreq = startRxFreq - 1000; // adjust frequency lower 1 kHz so RX signal is covered.
            }
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

                if (!rig.rigType().Contains("IC-9700"))  // Ignore split setting for IC-9700 and this model has satellite mode.
                {
                    rig.setSplit();
                }
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
                lbl_rigtype.Text = "Rig1 Not Connect";
            }

            SatelliteFrequencyReset = true;
        }


        private void chk_ConnectRigTX_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_ConnectRig2.Checked)
            {
                rig.rig2Connect();
                txt_TuneRx.Enabled = false;
                bb_tune.Enabled = false;
                chk_Simplex.Enabled = false;
                chk_Simplex.Checked = false;
                lbl_rig2type.Text = rig.rig2Type() + " " + rig.rig2Status();
            }
            else
            {
                rig.disConnectRig2();
                txt_TuneRx.Enabled = true;
                bb_tune.Enabled = true;
                chk_Simplex.Enabled = false;
                lbl_rig2type.Text = "Rig2 Not Connect";
            }
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


        private void linkAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/chokelive/HamSatTune/");
        }

        private void bb_rigcal_Click(object sender, EventArgs e)
        {
            using (frmCalibrate rigcal = new frmCalibrate())
            {
                DialogResult dr = rigcal.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    // Save updated offsets back to Doppler.sqf
                    rigcal.SaveOffsetsToFile();
                    ReloadSqfPreservingSelection(); // reload sqf to make sure get latest sqf if user update sqf file when software is running.
                    rigcal.Dispose();
                }
            }
        }


        // #############################################################################
        // Open Doppler.sqf in editor when user click the button, so user can edit Doppler.sqf to add new satellite or adjust frequency and mode for existing satellite.
        private void bb_sqf_Click(object sender, EventArgs e)
        {
            using (frmSqfManager sqfManager = new frmSqfManager("Doppler.sqf", GetTleSatelliteNames()))
            {
                sqfManager.ShowDialog(this);
                if (sqfManager.SqfChanged)
                {
                    ReloadSqfPreservingSelection();
                }
            }
        }

        private IEnumerable<string> GetTleSatelliteNames()
        {
            if (tlelist == null)
            {
                return Enumerable.Empty<string>();
            }

            return tlelist.Values
                .Where(tle => tle != null && !string.IsNullOrWhiteSpace(tle.Name))
                .Select(tle => tle.Name.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private void bb_tle_Click(object sender, EventArgs e)
        {
            using (frmTleEditor tleEditor = new frmTleEditor())
            {
                DialogResult dialogResult = tleEditor.ShowDialog(this);
                if (tleEditor.ManualTleChanged)
                {
                    loadLocalTle();
                }

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    if (!SaveManualTle(tleEditor.SatelliteName, tleEditor.TleLine1, tleEditor.TleLine2, tleEditor.ParsedTle))
                    {
                        return;
                    }

                    loadLocalTle();
                    MessageBox.Show(this, "TLE saved and reloaded. Add matching SQF frequency data if this satellite is not in the list.", "HamSatTune", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Cannot save TLE: " + ex.Message, "HamSatTune", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bb_map_Click(object sender, EventArgs e)
        {
            if (mapForm == null || mapForm.IsDisposed)
            {
                mapForm = new HamSatTune.Properties.frmMap();
            }

            mapForm.Show();
            mapForm.BringToFront();
        }

        private void bb_pass_Click(object sender, EventArgs e)
        {
            if (nextPassForm == null || nextPassForm.IsDisposed)
            {
                nextPassForm = new frmNextPass();
            }

            nextPassForm.Show();
            nextPassForm.BringToFront();
        }

        private void bb_rotor_Click(object sender, EventArgs e)
        {
            if (rotorControlForm == null || rotorControlForm.IsDisposed)
            {
                rotorControlForm = new frmRotorControl();
            }

            rotorControlForm.Show();
            rotorControlForm.BringToFront();
        }

        private void bb_log_Click(object sender, EventArgs e)
        {
            if (logForm == null || logForm.IsDisposed)
            {
                logForm = new frmLog();
            }
            else
            {
                logForm.RefreshCurrentContext();
            }

            logForm.Show();
            logForm.BringToFront();
        }

        private void RefreshOpenLogContext()
        {
            if (logForm == null || logForm.IsDisposed)
            {
                return;
            }

            logForm.RefreshCurrentContext();
        }

        private void RefreshOpenLogFrequencies()
        {
            if (logForm == null || logForm.IsDisposed)
            {
                return;
            }

            logForm.RefreshCurrentFrequencies();
        }

        private void chk_AutoTrackRotor_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_AutoTrackRotor.Checked)
            {
                ConnectMainRotor();
            }
            else
            {
                DisconnectMainRotor();
            }
        }

        private void ConnectMainRotor()
        {
            DisconnectMainRotor();

            string portName = GetAppSetting("RotorPortName", "COM1");
            int baudRate;
            if (!int.TryParse(GetAppSetting("RotorBaudRate", "9600"), out baudRate))
            {
                baudRate = 9600;
            }

            double tolerance;
            if (!double.TryParse(GetAppSetting("RotorToleranceThreshold", "2.0"), out tolerance))
            {
                tolerance = 2.0;
            }

            mainRotor = new RotorControlProcess(portName, baudRate, tolerance);
            mainRotor.RotorUpdated += MainRotor_RotorUpdated;

            string error = mainRotor.Connect();
            if (string.IsNullOrWhiteSpace(error))
            {
                lbl_rotortype.Text = "Rotor: connected " + portName;
                lastRotorTrackTime = DateTime.MinValue;
            }
            else
            {
                mainRotor.Dispose();
                mainRotor = null;
                chk_AutoTrackRotor.Checked = false;
                lbl_rotortype.Text = "Rotor: " + error;
            }
        }

        private void DisconnectMainRotor()
        {
            if (mainRotor != null)
            {
                mainRotor.Dispose();
                mainRotor = null;
            }

            lbl_rotortype.Text = "Rotor: disconnected";
        }

        private void UpdateRotorTracking() // Set rotor position every 1 second.
        {
            if (!chk_AutoTrackRotor.Checked || mainRotor == null || !mainRotor.IsConnected)
            {
                return;
            }

            if ((DateTime.Now - lastRotorTrackTime).TotalMilliseconds < RotorUpdateInterval)
            {
                return;
            }

            lastRotorTrackTime = DateTime.Now;
            mainRotor.SetPosition(az, el);
        }

        private void MainRotor_RotorUpdated()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(MainRotor_RotorUpdated));
                return;
            }

            if (mainRotor != null)
            {
                lbl_rotortype.Text = string.Format("Rotor: AZ {0:0.00} EL {1:0.00}", mainRotor.Az, mainRotor.El);
            }
        }

        private string GetAppSetting(string key, string defaultValue)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            KeyValueConfigurationElement setting = config.AppSettings.Settings[key];
            return setting == null ? defaultValue : setting.Value;
        }

        private bool SaveManualTle(string name, string line1, string line2, Tle parsedTle)
        {
            string path = ManualTleFileName;
            List<string> lines = File.Exists(path)
                ? File.ReadAllLines(path).ToList()
                : new List<string>();

            int existingIndex = FindTleEntryIndex(lines, parsedTle.NoradNumber);
            if (existingIndex >= 0)
            {
                DialogResult replace = MessageBox.Show(
                    this,
                    "TLE for NORAD " + parsedTle.NoradNumber + " already exists. Replace it?",
                    "HamSatTune",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (replace != DialogResult.Yes)
                {
                    return false;
                }

                int removeCount = GetTleEntryLineCount(lines, existingIndex);
                lines.RemoveRange(existingIndex, removeCount);
                InsertTleEntry(lines, existingIndex, name, line1, line2);
            }
            else
            {
                if (tlelist != null && tlelist.ContainsKey((int)parsedTle.NoradNumber))
                {
                    DialogResult replaceDownloaded = MessageBox.Show(
                        this,
                        "Downloaded TLE for NORAD " + parsedTle.NoradNumber + " already exists. Save a manual TLE that overrides it?",
                        "HamSatTune",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (replaceDownloaded != DialogResult.Yes)
                    {
                        return false;
                    }
                }

                if (lines.Count > 0 && !string.IsNullOrWhiteSpace(lines[lines.Count - 1]))
                {
                    lines.Add("");
                }

                InsertTleEntry(lines, lines.Count, name, line1, line2);
            }

            File.WriteAllLines(path, lines);
            return true;
        }

        private int FindTleEntryIndex(List<string> lines, uint noradNumber)
        {
            string noradText = noradNumber.ToString(CultureInfo.InvariantCulture).PadLeft(5, ' ');

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                if (!line.StartsWith("1 ", StringComparison.Ordinal) || line.Length < 7)
                {
                    continue;
                }

                if (line.Substring(2, 5) == noradText)
                {
                    return i > 0 && !lines[i - 1].StartsWith("2 ", StringComparison.Ordinal) ? i - 1 : i;
                }
            }

            return -1;
        }

        private int GetTleEntryLineCount(List<string> lines, int entryIndex)
        {
            if (entryIndex < 0 || entryIndex >= lines.Count)
            {
                return 0;
            }

            if (lines[entryIndex].StartsWith("1 ", StringComparison.Ordinal))
            {
                return entryIndex + 1 < lines.Count && lines[entryIndex + 1].StartsWith("2 ", StringComparison.Ordinal) ? 2 : 1;
            }

            if (entryIndex + 2 < lines.Count
                && lines[entryIndex + 1].StartsWith("1 ", StringComparison.Ordinal)
                && lines[entryIndex + 2].StartsWith("2 ", StringComparison.Ordinal))
            {
                return 3;
            }

            return 1;
        }

        private void InsertTleEntry(List<string> lines, int index, string name, string line1, string line2)
        {
            lines.Insert(index, line2);
            lines.Insert(index, line1);
            lines.Insert(index, name);
        }

        // Open Doppler.sqf in the user's default editor (Notepad) for manual editing
        private void OpenDopplerInEditor()
        {
            string path = "Doppler.sqf";
            try
            {
                if (!File.Exists(path))
                {
                    // Create an empty file so editor can open it
                    File.WriteAllText(path, "");
                }

                Process.Start("notepad.exe", path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Cannot open Doppler.sqf: " + ex.Message);
            }
        }
    }
}
