using SGPdotNET.TLE;
using System;
using System.Linq;
using System.Windows.Forms;

namespace HamSatTune
{
    public partial class frmTleEditor : Form
    {
        private const string CurrentTleFileName = "tles.txt";
        private const string ManualTleFileName = "manual_tles.txt";

        public string SatelliteName { get; private set; }
        public string TleLine1 { get; private set; }
        public string TleLine2 { get; private set; }
        public Tle ParsedTle { get; private set; }
        public bool ManualTleChanged { get; private set; }

        public frmTleEditor()
        {
            InitializeComponent();
        }

        private void bbViewCurrent_Click(object sender, EventArgs e)
        {
            using (frmTleTextViewer viewer = new frmTleTextViewer("Current TLE", CurrentTleFileName, "No current TLE file found."))
            {
                viewer.ShowDialog(this);
            }
        }

        private void bbViewManual_Click(object sender, EventArgs e)
        {
            using (frmManualTleManager manager = new frmManualTleManager(ManualTleFileName))
            {
                manager.ShowDialog(this);
                if (manager.TleChanged)
                {
                    ManualTleChanged = true;
                }
            }
        }

        private void bbSave_Click(object sender, EventArgs e)
        {
            string name;
            string line1;
            string line2;
            if (!TryReadTleData(out name, out line1, out line2))
            {
                return;
            }

            try
            {
                ParsedTle = new Tle(name, line1, line2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Invalid TLE: " + ex.Message, "Add TLE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SatelliteName = name;
            TleLine1 = line1;
            TleLine2 = line2;
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool TryReadTleData(out string name, out string line1, out string line2)
        {
            name = "";
            line1 = "";
            line2 = "";

            string[] lines = txtTleData.Text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(line => line.Trim())
                .Where(line => line.Length > 0)
                .ToArray();

            if (lines.Length < 3)
            {
                MessageBox.Show(this, "Enter TLE as 3 lines: satellite name, line 1, line 2.", "Add TLE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTleData.Focus();
                return false;
            }

            name = lines[0];
            line1 = lines[1];
            line2 = lines[2];

            if (!line1.StartsWith("1 ", StringComparison.Ordinal) || !line2.StartsWith("2 ", StringComparison.Ordinal))
            {
                MessageBox.Show(this, "TLE line 1 must start with '1 ' and line 2 must start with '2 '.", "Add TLE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTleData.Focus();
                return false;
            }

            return true;
        }
    }
}
