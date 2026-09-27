using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HamSatTune
{
    public partial class frmLog : Form
    {
        private readonly SatelliteLogStore store;

        public frmLog()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            store = new SatelliteLogStore();
            FillCurrentSatellite();
            RefreshGrid();
            txtCallsign.Focus();
        }

        public void RefreshCurrentContext()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            FillCurrentSatellite();
        }

        public void RefreshCurrentFrequencies()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            Sqf current = Globals.CurrentSqf;
            txtDownlink.Text = FormatKhz(Globals.CalculatedDownlinkHz > 0 ? Globals.CalculatedDownlinkHz : (int)(current.downlinkFreq * 1000));
            txtUplink.Text = FormatKhz(Globals.CalculatedUplinkHz > 0 ? Globals.CalculatedUplinkHz : (int)(current.uplinkFreq * 1000));
        }

        private void FillCurrentSatellite()
        {
            Sqf current = Globals.CurrentSqf;
            txtSatellite.Text = current.sateName ?? "";
            txtMode.Text = current.uplinkMode ?? "";
            cbSubmode.Text = GetDefaultSubmode(current);
            txtGrid.Text = Globals.CurrentGrid ?? "";
            RefreshCurrentFrequencies();
        }

        private void RefreshGrid()
        {
            grid.Rows.Clear();
            foreach (SatelliteLogEntry entry in store.LoadRecent(100))
            {
                grid.Rows.Add(
                    entry.Id,
                    entry.UtcTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    entry.Callsign,
                    entry.RstSent,
                    entry.RstReceived,
                    entry.Grid,
                    entry.Name,
                    store.GetExportSatelliteName(entry),
                    store.GetExportPropMode(),
                    store.GetExportSatMode(entry),
                    store.GetExportMode(entry),
                    store.GetExportSubmode(entry),
                    store.GetExportFreq(entry),
                    store.GetExportBand(entry),
                    store.GetExportFreqRx(entry),
                    store.GetExportBandRx(entry),
                    entry.Comment);
            }
        }

        private void SaveEntry()
        {
            string callsign = txtCallsign.Text.Trim().ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(callsign))
            {
                MessageBox.Show(this, "Enter callsign.", "Satellite Log", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCallsign.Focus();
                return;
            }

            SatelliteLogEntry entry = new SatelliteLogEntry
            {
                UtcTime = DateTime.UtcNow,
                Callsign = callsign,
                RstSent = string.IsNullOrWhiteSpace(txtRstSent.Text) ? "59" : txtRstSent.Text.Trim(),
                RstReceived = string.IsNullOrWhiteSpace(txtRstReceived.Text) ? "59" : txtRstReceived.Text.Trim(),
                Grid = txtGrid.Text.Trim().ToUpperInvariant(),
                Name = txtName.Text.Trim(),
                Satellite = txtSatellite.Text.Trim(),
                Mode = txtMode.Text.Trim(),
                Submode = cbSubmode.Text.Trim().ToUpperInvariant(),
                DownlinkHz = ParseKhzText(txtDownlink.Text),
                UplinkHz = ParseKhzText(txtUplink.Text),
                Comment = txtComment.Text.Trim()
            };

            store.Add(entry);
            RefreshGrid();
            ClearEntry();
        }

        private void ClearEntry()
        {
            txtCallsign.Text = "";
            txtGrid.Text = "";
            txtName.Text = "";
            txtComment.Text = "";
            cbSubmode.Text = "";
            txtRstSent.Text = "59";
            txtRstReceived.Text = "59";
            FillCurrentSatellite();
            txtCallsign.Focus();
        }

        private void bbSave_Click(object sender, EventArgs e)
        {
            SaveEntry();
        }

        private void bbClear_Click(object sender, EventArgs e)
        {
            ClearEntry();
        }

        private void bbDelete_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0)
            {
                return;
            }

            long id = Convert.ToInt64(grid.SelectedRows[0].Cells[0].Value);
            DialogResult confirm = MessageBox.Show(this, "Delete selected QSO?", "Satellite Log", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            store.Delete(id);
            RefreshGrid();
        }

        private void bbExportAdif_Click(object sender, EventArgs e)
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logDirectory);
            string path = Path.Combine(logDirectory, "satellite_log.adi");
            store.ExportAdif(path);
            MessageBox.Show(this, "ADIF exported to " + path, "Satellite Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bbClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveEntry();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                ClearEntry();
                e.SuppressKeyPress = true;
            }
        }

        private void EntryTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            if (sender == txtComment)
            {
                SaveEntry();
                return;
            }

            SelectNextControl((Control)sender, true, true, true, true);
        }

        private string FormatKhz(int hz)
        {
            if (hz <= 0)
            {
                return "";
            }

            return ((double)hz / 1000.0).ToString("0.00");
        }

        private int ParseKhzText(string value)
        {
            double khz;
            if (!double.TryParse(value, out khz))
            {
                return 0;
            }

            return (int)Math.Round(khz * 1000.0);
        }

        private string GetDefaultSubmode(Sqf current)
        {
            if (!string.IsNullOrWhiteSpace(current.comment)
                && current.comment.IndexOf("FT4", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "FT4";
            }

            return "";
        }
    }
}
