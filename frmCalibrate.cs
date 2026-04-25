using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HamSatTune
{
    public partial class frmCalibrate : Form
    {
        Sqf sqf; // Current selectd SQF
        public frmCalibrate()
        {
            InitializeComponent();
        }

        private void frmCalibrate_Load(object sender, EventArgs e)
        {
            sqf = Globals.CurrentSqf;
            lblSatName.Text = "SAT:" + sqf.sateName + " Mode:" + sqf.comment;
            txt_TxOffset.Text = sqf.uplinkOffset.ToString();
            txt_RxOffset.Text = sqf.downlinkOffset.ToString();
        }

        private void bb_txInc1_Click(object sender, EventArgs e)
        {
            sqf.uplinkOffset = sqf.uplinkOffset + 1;
            txt_TxOffset.Text = sqf.uplinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        private void bb_txInc10_Click(object sender, EventArgs e)
        {
            sqf.uplinkOffset = sqf.uplinkOffset + 10;
            txt_TxOffset.Text = sqf.uplinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        private void bb_txInc100_Click(object sender, EventArgs e)
        {
            sqf.uplinkOffset = sqf.uplinkOffset + 100;
            txt_TxOffset.Text = sqf.uplinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        private void bb_txMinus1_Click(object sender, EventArgs e)
        {
            sqf.uplinkOffset = sqf.uplinkOffset - 1;
            txt_TxOffset.Text = sqf.uplinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        private void bb_txMinus10_Click(object sender, EventArgs e)
        {
            sqf.uplinkOffset = sqf.uplinkOffset - 10;
            txt_TxOffset.Text = sqf.uplinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        private void bb_txMinus100_Click(object sender, EventArgs e)
        {
            sqf.uplinkOffset = sqf.uplinkOffset - 100;
            txt_TxOffset.Text = sqf.uplinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        
        private void bb_rxInc1_Click(object sender, EventArgs e)
        {
            sqf.downlinkOffset = sqf.downlinkOffset + 1;
            txt_RxOffset.Text = sqf.downlinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        private void bb_rxInc10_Click(object sender, EventArgs e)
        {
            sqf.downlinkOffset = sqf.downlinkOffset + 10;
            txt_RxOffset.Text = sqf.downlinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        private void bb_rxInc100_Click(object sender, EventArgs e)
        {
            sqf.downlinkOffset = sqf.downlinkOffset + 100;
            txt_RxOffset.Text = sqf.downlinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        private void bb_rxMinus1_Click(object sender, EventArgs e)
        {
            sqf.downlinkOffset = sqf.downlinkOffset - 1;
            txt_RxOffset.Text = sqf.downlinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        private void bb_rxMinus10_Click(object sender, EventArgs e)
        {
            sqf.downlinkOffset = sqf.downlinkOffset - 10;
            txt_RxOffset.Text = sqf.downlinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        private void bb_rxMinus100_Click(object sender, EventArgs e)
        {
            sqf.downlinkOffset = sqf.downlinkOffset - 100;
            txt_RxOffset.Text = sqf.downlinkOffset.ToString();
            Globals.CurrentSqf = sqf;
        }

        private void bb_save_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        // Save only uplinkOffset and downlinkOffset for the current sqf into Doppler.sqf
        public void SaveOffsetsToFile()
        {
            try
            {
                string path = "Doppler.sqf";
                if (!System.IO.File.Exists(path))
                    return;

                var lines = System.IO.File.ReadAllLines(path);
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    var parts = line.Split(',');
                    if (parts.Length < 8)
                        continue;
                    // Match by satellite name
                    if (parts[0] == sqf.sateName && parts[8] == sqf.comment)
                    {
                        // Update only offsets (indexes 6 and 7)
                        parts[6] = sqf.downlinkOffset.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        parts[7] = sqf.uplinkOffset.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        lines[i] = string.Join(",", parts);
                    }
                }

                System.IO.File.WriteAllLines(path, lines);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, "Error saving Doppler.sqf: " + ex.Message);
            }
        }

    }
}
