using System;
using System.IO;
using System.Windows.Forms;

namespace HamSatTune
{
    public partial class frmTleTextViewer : Form
    {
        private readonly string filePath;
        private readonly string missingMessage;

        public frmTleTextViewer(string title, string filePath, string missingMessage)
        {
            InitializeComponent();
            Text = title;
            lblFile.Text = filePath;
            this.filePath = filePath;
            this.missingMessage = missingMessage;
            LoadFile();
        }

        private void bbRefresh_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void bbClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    txtTle.Text = missingMessage;
                    return;
                }

                string text = File.ReadAllText(filePath);
                txtTle.Text = string.IsNullOrWhiteSpace(text) ? missingMessage : NormalizeLineEndings(text);
            }
            catch (Exception ex)
            {
                txtTle.Text = "Cannot read TLE file: " + ex.Message;
            }
        }

        private static string NormalizeLineEndings(string text)
        {
            return text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }
    }
}
