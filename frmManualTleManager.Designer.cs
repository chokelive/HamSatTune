namespace HamSatTune
{
    partial class frmManualTleManager
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
            this.lstTle = new System.Windows.Forms.ListBox();
            this.txtTleData = new System.Windows.Forms.TextBox();
            this.bbUpdate = new System.Windows.Forms.Button();
            this.bbRemove = new System.Windows.Forms.Button();
            this.bbRefresh = new System.Windows.Forms.Button();
            this.bbClose = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblTleData = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstTle
            // 
            this.lstTle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstTle.FormattingEnabled = true;
            this.lstTle.Location = new System.Drawing.Point(12, 31);
            this.lstTle.Name = "lstTle";
            this.lstTle.Size = new System.Drawing.Size(238, 329);
            this.lstTle.TabIndex = 0;
            this.lstTle.SelectedIndexChanged += new System.EventHandler(this.lstTle_SelectedIndexChanged);
            // 
            // txtTleData
            // 
            this.txtTleData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTleData.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTleData.Location = new System.Drawing.Point(316, 31);
            this.txtTleData.Multiline = true;
            this.txtTleData.Name = "txtTleData";
            this.txtTleData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTleData.Size = new System.Drawing.Size(456, 104);
            this.txtTleData.TabIndex = 1;
            // 
            // bbUpdate
            // 
            this.bbUpdate.Location = new System.Drawing.Point(316, 158);
            this.bbUpdate.Name = "bbUpdate";
            this.bbUpdate.Size = new System.Drawing.Size(82, 28);
            this.bbUpdate.TabIndex = 2;
            this.bbUpdate.Text = "Update";
            this.bbUpdate.UseVisualStyleBackColor = true;
            this.bbUpdate.Click += new System.EventHandler(this.bbUpdate_Click);
            // 
            // bbRemove
            // 
            this.bbRemove.Location = new System.Drawing.Point(404, 158);
            this.bbRemove.Name = "bbRemove";
            this.bbRemove.Size = new System.Drawing.Size(82, 28);
            this.bbRemove.TabIndex = 3;
            this.bbRemove.Text = "Remove";
            this.bbRemove.UseVisualStyleBackColor = true;
            this.bbRemove.Click += new System.EventHandler(this.bbRemove_Click);
            // 
            // bbRefresh
            // 
            this.bbRefresh.Location = new System.Drawing.Point(12, 370);
            this.bbRefresh.Name = "bbRefresh";
            this.bbRefresh.Size = new System.Drawing.Size(82, 28);
            this.bbRefresh.TabIndex = 4;
            this.bbRefresh.Text = "Refresh";
            this.bbRefresh.UseVisualStyleBackColor = true;
            this.bbRefresh.Click += new System.EventHandler(this.bbRefresh_Click);
            // 
            // bbClose
            // 
            this.bbClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bbClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bbClose.Location = new System.Drawing.Point(690, 370);
            this.bbClose.Name = "bbClose";
            this.bbClose.Size = new System.Drawing.Size(82, 28);
            this.bbClose.TabIndex = 5;
            this.bbClose.Text = "Close";
            this.bbClose.UseVisualStyleBackColor = true;
            this.bbClose.Click += new System.EventHandler(this.bbClose_Click);
            // 
            // lblFile
            // 
            this.lblFile.Location = new System.Drawing.Point(12, 10);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(760, 18);
            this.lblFile.TabIndex = 8;
            this.lblFile.Text = "manual_tles.txt";
            // 
            // lblTleData
            // 
            this.lblTleData.Location = new System.Drawing.Point(256, 34);
            this.lblTleData.Name = "lblTleData";
            this.lblTleData.Size = new System.Drawing.Size(54, 54);
            this.lblTleData.TabIndex = 9;
            this.lblTleData.Text = "TLE data";
            // 
            // frmManualTleManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bbClose;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.lblTleData);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.bbClose);
            this.Controls.Add(this.bbRefresh);
            this.Controls.Add(this.bbRemove);
            this.Controls.Add(this.bbUpdate);
            this.Controls.Add(this.txtTleData);
            this.Controls.Add(this.lstTle);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(680, 320);
            this.Name = "frmManualTleManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manual TLE Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstTle;
        private System.Windows.Forms.TextBox txtTleData;
        private System.Windows.Forms.Button bbUpdate;
        private System.Windows.Forms.Button bbRemove;
        private System.Windows.Forms.Button bbRefresh;
        private System.Windows.Forms.Button bbClose;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblTleData;
    }
}
