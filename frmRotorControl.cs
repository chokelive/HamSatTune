using System;
using System.Configuration;
using System.IO.Ports;
using System.Windows.Forms;

namespace HamSatTune
{
    public partial class frmRotorControl : Form
    {
        private RotorControlProcess rotor;

        public frmRotorControl()
        {
            InitializeComponent();
            LoadSettings();

            uiTimer.Interval = 1000;
            uiTimer.Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            uiTimer.Stop();
            rotor?.Dispose();
            base.OnFormClosed(e);
        }

        private void frmRotorControl_Load(object sender, EventArgs e)
        {
        }

        private void LoadSettings()
        {
            cbPort.Items.Clear();
            cbPort.Items.AddRange(SerialPort.GetPortNames());

            string portName = GetSetting("RotorPortName", "");
            string baudRate = GetSetting("RotorBaudRate", "9600");
            string tolerance = GetSetting("RotorToleranceThreshold", "2.0");

            if (cbPort.Items.Count == 0)
            {
                cbPort.Items.Add(portName);
            }

            cbPort.SelectedItem = cbPort.Items.Contains(portName) ? portName : cbPort.Items[0];
            cbBaud.SelectedItem = cbBaud.Items.Contains(baudRate) ? baudRate : "9600";

            decimal toleranceValue;
            if (!decimal.TryParse(tolerance, out toleranceValue))
            {
                toleranceValue = 2.0M;
            }

            nudTolerance.Value = Math.Max(nudTolerance.Minimum, Math.Min(nudTolerance.Maximum, toleranceValue));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (rotor != null && rotor.IsConnected)
            {
                rotor.Dispose();
                rotor = null;
                btnConnect.Text = "Connect";
                lblStatus.Text = "Disconnected";
                return;
            }

            SaveSettings();

            int baudRate = int.Parse(cbBaud.Text);
            rotor = new RotorControlProcess(cbPort.Text, baudRate, (double)nudTolerance.Value);
            rotor.RotorUpdated += Rotor_RotorUpdated;

            string error = rotor.Connect();
            if (string.IsNullOrWhiteSpace(error))
            {
                btnConnect.Text = "Disconnect";
                lblStatus.Text = "Connected " + cbPort.Text + " " + cbBaud.Text;
            }
            else
            {
                lblStatus.Text = "Error: " + error;
                rotor.Dispose();
                rotor = null;
            }
        }

        private void uiTimer_Tick(object sender, EventArgs e)
        {
            lblTarget.Text = string.Format("Target AZ {0:0.00}  EL {1:0.00}", Globals.CurrentAz, Globals.CurrentEl);
        }

        private void Rotor_RotorUpdated()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Rotor_RotorUpdated));
                return;
            }

            if (rotor != null)
            {
                lblRotor.Text = string.Format("Rotor AZ {0:0.00}  EL {1:0.00}", rotor.Az, rotor.El);
            }
        }

        private void btnUp_MouseDown(object sender, MouseEventArgs e)
        {
            rotor?.Up();
        }

        private void btnDown_MouseDown(object sender, MouseEventArgs e)
        {
            rotor?.Down();
        }

        private void btnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            rotor?.Left();
        }

        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            rotor?.Right();
        }

        private void moveButton_MouseUp(object sender, MouseEventArgs e)
        {
            rotor?.Stop();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            rotor?.Stop();
        }

        private void SaveSettings()
        {
            SetSetting("RotorPortName", cbPort.Text);
            SetSetting("RotorBaudRate", cbBaud.Text);
            SetSetting("RotorToleranceThreshold", nudTolerance.Value.ToString());
            lblStatus.Text = "Settings saved";
        }

        private string GetSetting(string key, string defaultValue)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            KeyValueConfigurationElement setting = config.AppSettings.Settings[key];
            return setting == null ? defaultValue : setting.Value;
        }

        private void SetSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
