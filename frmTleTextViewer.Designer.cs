namespace HamSatTune
{
    partial class frmTleTextViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtTle = new System.Windows.Forms.TextBox();
            this.bbRefresh = new System.Windows.Forms.Button();
            this.bbClose = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTle
            // 
            this.txtTle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTle.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTle.Location = new System.Drawing.Point(12, 31);
            this.txtTle.Multiline = true;
            this.txtTle.Name = "txtTle";
            this.txtTle.ReadOnly = true;
            this.txtTle.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTle.Size = new System.Drawing.Size(660, 366);
            this.txtTle.TabIndex = 0;
            this.txtTle.WordWrap = false;
            // 
            // bbRefresh
            // 
            this.bbRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bbRefresh.Location = new System.Drawing.Point(500, 409);
            this.bbRefresh.Name = "bbRefresh";
            this.bbRefresh.Size = new System.Drawing.Size(82, 28);
            this.bbRefresh.TabIndex = 1;
            this.bbRefresh.Text = "Refresh";
            this.bbRefresh.UseVisualStyleBackColor = true;
            this.bbRefresh.Click += new System.EventHandler(this.bbRefresh_Click);
            // 
            // bbClose
            // 
            this.bbClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bbClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bbClose.Location = new System.Drawing.Point(590, 409);
            this.bbClose.Name = "bbClose";
            this.bbClose.Size = new System.Drawing.Size(82, 28);
            this.bbClose.TabIndex = 2;
            this.bbClose.Text = "Close";
            this.bbClose.UseVisualStyleBackColor = true;
            this.bbClose.Click += new System.EventHandler(this.bbClose_Click);
            // 
            // lblFile
            // 
            this.lblFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFile.Location = new System.Drawing.Point(12, 10);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(660, 18);
            this.lblFile.TabIndex = 3;
            this.lblFile.Text = "tle file";
            // 
            // frmTleTextViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bbClose;
            this.ClientSize = new System.Drawing.Size(684, 449);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.bbClose);
            this.Controls.Add(this.bbRefresh);
            this.Controls.Add(this.txtTle);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(540, 320);
            this.Name = "frmTleTextViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TLE Viewer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTle;
        private System.Windows.Forms.Button bbRefresh;
        private System.Windows.Forms.Button bbClose;
        private System.Windows.Forms.Label lblFile;
    }
}
