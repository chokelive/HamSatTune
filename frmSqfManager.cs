using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HamSatTune
{
    public partial class frmSqfManager : Form
    {
        private readonly string filePath;
        private readonly List<string> tleSatelliteNames;
        private readonly List<SqfEntry> entries = new List<SqfEntry>();
        private static readonly string[] ModeOptions = { "CW", "CWN", "FM", "FMD", "FMN", "LSB", "USB", "LSBD", "USBD", "PKT" };
        private static readonly string[] TransponderTypeOptions = { "NOR", "REV" };

        public bool SqfChanged { get; private set; }

        public frmSqfManager(string filePath, IEnumerable<string> tleSatelliteNames)
        {
            InitializeComponent();
            this.filePath = filePath;
            this.tleSatelliteNames = (tleSatelliteNames ?? Enumerable.Empty<string>())
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                .ToList();
            lblFile.Text = filePath;
            LoadModeOptions();
            LoadTransponderTypeOptions();
            LoadTleSatelliteNames();
            LoadEntries();
        }

        private void lstSqf_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSelectedEntry();
        }

        private void bbAdd_Click(object sender, EventArgs e)
        {
            SqfEntry entry;
            if (!TryBuildEntryFromFields(out entry))
            {
                return;
            }

            entries.Add(entry);
            SaveEntries();
            SqfChanged = true;
            RefreshList(entries.Count - 1);
        }

        private void bbUpdate_Click(object sender, EventArgs e)
        {
            if (lstSqf.SelectedIndex < 0)
            {
                MessageBox.Show(this, "Select an SQF entry to update.", "SQF Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqfEntry entry;
            if (!TryBuildEntryFromFields(out entry))
            {
                return;
            }

            entries[lstSqf.SelectedIndex] = entry;
            SaveEntries();
            SqfChanged = true;
            RefreshList(lstSqf.SelectedIndex);
        }

        private void bbRemove_Click(object sender, EventArgs e)
        {
            if (lstSqf.SelectedIndex < 0)
            {
                MessageBox.Show(this, "Select an SQF entry to remove.", "SQF Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqfEntry entry = entries[lstSqf.SelectedIndex];
            DialogResult confirm = MessageBox.Show(
                this,
                "Remove SQF entry for " + entry.SatelliteName + " " + entry.Comment + "?",
                "SQF Manager",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            int removedIndex = lstSqf.SelectedIndex;
            entries.RemoveAt(removedIndex);
            SaveEntries();
            SqfChanged = true;
            RefreshList(Math.Min(removedIndex, entries.Count - 1));
        }

        private void bbNew_Click(object sender, EventArgs e)
        {
            lstSqf.ClearSelected();
            ClearFields();
            cbSatellite.Focus();
        }

        private void bbRefresh_Click(object sender, EventArgs e)
        {
            LoadEntries();
        }

        private void bbOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "");
                }

                Process.Start("notepad.exe", filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Cannot open Doppler.sqf: " + ex.Message, "SQF Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadEntries()
        {
            entries.Clear();

            if (File.Exists(filePath))
            {
                int lineNumber = 0;
                foreach (string line in File.ReadLines(filePath))
                {
                    lineNumber++;
                    SqfEntry entry;
                    if (TryParseEntry(line, lineNumber, out entry))
                    {
                        entries.Add(entry);
                    }
                }
            }

            entries.Sort((left, right) =>
            {
                int nameCompare = string.Compare(left.SatelliteName, right.SatelliteName, StringComparison.OrdinalIgnoreCase);
                return nameCompare != 0 ? nameCompare : string.Compare(left.Comment, right.Comment, StringComparison.OrdinalIgnoreCase);
            });

            RefreshList(entries.Count > 0 ? 0 : -1);
        }

        private void RefreshList(int selectedIndex)
        {
            lstSqf.Items.Clear();

            foreach (SqfEntry entry in entries)
            {
                lstSqf.Items.Add(entry.SatelliteName + " " + entry.Comment);
            }

            if (selectedIndex >= 0 && selectedIndex < lstSqf.Items.Count)
            {
                lstSqf.SelectedIndex = selectedIndex;
            }
            else
            {
                ClearFields();
            }
        }

        private void ShowSelectedEntry()
        {
            if (lstSqf.SelectedIndex < 0 || lstSqf.SelectedIndex >= entries.Count)
            {
                ClearFields();
                return;
            }

            SqfEntry entry = entries[lstSqf.SelectedIndex];
            cbSatellite.Text = entry.SatelliteName;
            txtDownlink.Text = entry.DownlinkFreq;
            txtUplink.Text = entry.UplinkFreq;
            cbDownlinkMode.Text = entry.DownlinkMode;
            cbUplinkMode.Text = entry.UplinkMode;
            cbType.Text = entry.TransponderType;
            txtDownlinkOffset.Text = entry.DownlinkOffset;
            txtUplinkOffset.Text = entry.UplinkOffset;
            txtComment.Text = entry.Comment;
        }

        private void ClearFields()
        {
            cbSatellite.Text = "";
            txtDownlink.Text = "";
            txtUplink.Text = "";
            cbDownlinkMode.Text = "";
            cbUplinkMode.Text = "";
            cbType.Text = "";
            txtDownlinkOffset.Text = "";
            txtUplinkOffset.Text = "";
            txtComment.Text = "";
        }

        private void LoadTleSatelliteNames()
        {
            cbSatellite.Items.Clear();
            foreach (string name in tleSatelliteNames)
            {
                cbSatellite.Items.Add(name);
            }
        }

        private void LoadModeOptions()
        {
            cbDownlinkMode.Items.Clear();
            cbUplinkMode.Items.Clear();
            cbDownlinkMode.Items.AddRange(ModeOptions);
            cbUplinkMode.Items.AddRange(ModeOptions);
        }

        private void LoadTransponderTypeOptions()
        {
            cbType.Items.Clear();
            cbType.Items.AddRange(TransponderTypeOptions);
        }

        private bool TryParseEntry(string line, int lineNumber, out SqfEntry entry)
        {
            entry = null;
            if (string.IsNullOrWhiteSpace(line))
            {
                return false;
            }

            string[] parts = line.Split(',');
            if (parts.Length < 9)
            {
                return false;
            }

            double downlink;
            if (!TryParseNumber(parts[1], false, out downlink))
            {
                return false;
            }

            double unused;
            if (!TryParseNumber(parts[2], true, out unused) ||
                !TryParseNumber(parts[6], true, out unused) ||
                !TryParseNumber(parts[7], true, out unused))
            {
                return false;
            }

            entry = new SqfEntry(
                parts[0].Trim(),
                NormalizeNumberText(parts[1]),
                NormalizeOptionalNumberText(parts[2]),
                parts[3].Trim(),
                parts[4].Trim(),
                parts[5].Trim(),
                NormalizeOptionalNumberText(parts[6]),
                NormalizeOptionalNumberText(parts[7]),
                parts[8].Trim());
            return true;
        }

        private bool TryBuildEntryFromFields(out SqfEntry entry)
        {
            entry = null;

            string satelliteName = cbSatellite.Text.Trim();
            if (string.IsNullOrWhiteSpace(satelliteName))
            {
                MessageBox.Show(this, "Enter satellite name.", "SQF Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbSatellite.Focus();
                return false;
            }

            double downlink;
            if (!TryParseNumber(txtDownlink.Text, false, out downlink))
            {
                MessageBox.Show(this, "Enter a valid downlink frequency.", "SQF Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDownlink.Focus();
                return false;
            }

            double unused;
            if (!TryParseNumber(txtUplink.Text, true, out unused))
            {
                MessageBox.Show(this, "Enter a valid uplink frequency or leave it blank for RX-only.", "SQF Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUplink.Focus();
                return false;
            }

            if (!TryParseNumber(txtDownlinkOffset.Text, true, out unused))
            {
                MessageBox.Show(this, "Enter a valid downlink offset or leave it blank.", "SQF Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDownlinkOffset.Focus();
                return false;
            }

            if (!TryParseNumber(txtUplinkOffset.Text, true, out unused))
            {
                MessageBox.Show(this, "Enter a valid uplink offset or leave it blank.", "SQF Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUplinkOffset.Focus();
                return false;
            }

            entry = new SqfEntry(
                satelliteName,
                NormalizeNumberText(txtDownlink.Text),
                NormalizeOptionalNumberText(txtUplink.Text),
                cbDownlinkMode.Text.Trim(),
                cbUplinkMode.Text.Trim(),
                cbType.Text.Trim(),
                NormalizeOptionalNumberText(txtDownlinkOffset.Text),
                NormalizeOptionalNumberText(txtUplinkOffset.Text),
                txtComment.Text.Trim());
            return true;
        }

        private static bool TryParseNumber(string value, bool allowEmpty, out double result)
        {
            if (string.IsNullOrWhiteSpace(value) && allowEmpty)
            {
                result = 0;
                return true;
            }

            return double.TryParse(value.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        }

        private static string NormalizeNumberText(string value)
        {
            double number;
            return double.TryParse(value.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out number)
                ? number.ToString(CultureInfo.InvariantCulture)
                : value.Trim();
        }

        private static string NormalizeOptionalNumberText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "" : NormalizeNumberText(value);
        }

        private void SaveEntries()
        {
            List<string> lines = entries.Select(entry => entry.ToFileLine()).ToList();
            File.WriteAllLines(filePath, lines);
        }

        private class SqfEntry
        {
            public SqfEntry(
                string satelliteName,
                string downlinkFreq,
                string uplinkFreq,
                string downlinkMode,
                string uplinkMode,
                string transponderType,
                string downlinkOffset,
                string uplinkOffset,
                string comment)
            {
                SatelliteName = satelliteName;
                DownlinkFreq = downlinkFreq;
                UplinkFreq = uplinkFreq;
                DownlinkMode = downlinkMode;
                UplinkMode = uplinkMode;
                TransponderType = transponderType;
                DownlinkOffset = downlinkOffset;
                UplinkOffset = uplinkOffset;
                Comment = comment;
            }

            public string SatelliteName { get; private set; }
            public string DownlinkFreq { get; private set; }
            public string UplinkFreq { get; private set; }
            public string DownlinkMode { get; private set; }
            public string UplinkMode { get; private set; }
            public string TransponderType { get; private set; }
            public string DownlinkOffset { get; private set; }
            public string UplinkOffset { get; private set; }
            public string Comment { get; private set; }

            public string ToFileLine()
            {
                return string.Join(",", new[]
                {
                    SatelliteName,
                    DownlinkFreq,
                    UplinkFreq,
                    DownlinkMode,
                    UplinkMode,
                    TransponderType,
                    DownlinkOffset,
                    UplinkOffset,
                    Comment
                });
            }
        }
    }
}
