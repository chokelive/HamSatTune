using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HamSatTune
{
    public partial class frmManualTleManager : Form
    {
        private readonly string filePath;
        private readonly List<ManualTleEntry> entries = new List<ManualTleEntry>();

        public bool TleChanged { get; private set; }

        public frmManualTleManager(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
            lblFile.Text = filePath;
            LoadEntries();
        }

        private void lstTle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSelectedEntry();
        }

        private void bbRefresh_Click(object sender, EventArgs e)
        {
            LoadEntries();
        }

        private void bbUpdate_Click(object sender, EventArgs e)
        {
            if (lstTle.SelectedIndex < 0)
            {
                MessageBox.Show(this, "Select a manual TLE to update.", "Manual TLE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ManualTleEntry updated;
            if (!TryBuildEntryFromFields(out updated))
            {
                return;
            }

            int duplicateIndex = entries.FindIndex(entry => entry.NoradNumber == updated.NoradNumber);
            if (duplicateIndex >= 0 && duplicateIndex != lstTle.SelectedIndex)
            {
                MessageBox.Show(this, "Another manual TLE already uses NORAD " + updated.NoradNumber + ".", "Manual TLE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            entries[lstTle.SelectedIndex] = updated;
            SaveEntries();
            TleChanged = true;
            RefreshList(lstTle.SelectedIndex);
        }

        private void bbRemove_Click(object sender, EventArgs e)
        {
            if (lstTle.SelectedIndex < 0)
            {
                MessageBox.Show(this, "Select a manual TLE to remove.", "Manual TLE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ManualTleEntry entry = entries[lstTle.SelectedIndex];
            DialogResult confirm = MessageBox.Show(
                this,
                "Remove manual TLE for " + entry.Name + "?",
                "Manual TLE",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            int removedIndex = lstTle.SelectedIndex;
            entries.RemoveAt(removedIndex);
            SaveEntries();
            TleChanged = true;
            RefreshList(Math.Min(removedIndex, entries.Count - 1));
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
                string[] lines = File.ReadAllLines(filePath)
                    .Select(line => line.Trim())
                    .Where(line => line.Length > 0)
                    .ToArray();

                for (int i = 0; i + 2 < lines.Length; i += 3)
                {
                    try
                    {
                        Tle tle = new Tle(lines[i], lines[i + 1], lines[i + 2]);
                        entries.Add(new ManualTleEntry(lines[i], lines[i + 1], lines[i + 2], tle.NoradNumber));
                    }
                    catch
                    {
                    }
                }
            }

            RefreshList(entries.Count > 0 ? 0 : -1);
        }

        private void RefreshList(int selectedIndex)
        {
            lstTle.Items.Clear();

            foreach (ManualTleEntry entry in entries)
            {
                lstTle.Items.Add(entry.Name + " (" + entry.NoradNumber + ")");
            }

            if (selectedIndex >= 0 && selectedIndex < lstTle.Items.Count)
            {
                lstTle.SelectedIndex = selectedIndex;
            }
            else
            {
                ClearFields();
            }
        }

        private void ShowSelectedEntry()
        {
            if (lstTle.SelectedIndex < 0 || lstTle.SelectedIndex >= entries.Count)
            {
                ClearFields();
                return;
            }

            ManualTleEntry entry = entries[lstTle.SelectedIndex];
            txtTleData.Text = string.Join(Environment.NewLine, new[] { entry.Name, entry.Line1, entry.Line2 });
        }

        private void ClearFields()
        {
            txtTleData.Text = "";
        }

        private bool TryBuildEntryFromFields(out ManualTleEntry entry)
        {
            entry = null;
            string[] lines = txtTleData.Text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(line => line.Trim())
                .Where(line => line.Length > 0)
                .ToArray();

            if (lines.Length < 3)
            {
                MessageBox.Show(this, "Enter TLE as 3 lines: satellite name, line 1, line 2.", "Manual TLE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTleData.Focus();
                return false;
            }

            string name = lines[0];
            string line1 = lines[1];
            string line2 = lines[2];

            if (!line1.StartsWith("1 ", StringComparison.Ordinal) || !line2.StartsWith("2 ", StringComparison.Ordinal))
            {
                MessageBox.Show(this, "TLE line 1 must start with '1 ' and line 2 must start with '2 '.", "Manual TLE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTleData.Focus();
                return false;
            }

            try
            {
                Tle tle = new Tle(name, line1, line2);
                entry = new ManualTleEntry(name, line1, line2, tle.NoradNumber);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Invalid TLE: " + ex.Message, "Manual TLE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private void SaveEntries()
        {
            List<string> lines = new List<string>();
            foreach (ManualTleEntry entry in entries)
            {
                lines.Add(entry.Name);
                lines.Add(entry.Line1);
                lines.Add(entry.Line2);
            }

            File.WriteAllLines(filePath, lines);
        }

        private class ManualTleEntry
        {
            public ManualTleEntry(string name, string line1, string line2, uint noradNumber)
            {
                Name = name;
                Line1 = line1;
                Line2 = line2;
                NoradNumber = noradNumber;
            }

            public string Name { get; private set; }
            public string Line1 { get; private set; }
            public string Line2 { get; private set; }
            public uint NoradNumber { get; private set; }
        }
    }
}
