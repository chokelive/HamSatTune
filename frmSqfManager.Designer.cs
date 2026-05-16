namespace HamSatTune
{
    partial class frmSqfManager
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
            this.lstSqf = new System.Windows.Forms.ListBox();
            this.cbSatellite = new System.Windows.Forms.ComboBox();
            this.txtDownlink = new System.Windows.Forms.TextBox();
            this.txtUplink = new System.Windows.Forms.TextBox();
            this.cbDownlinkMode = new System.Windows.Forms.ComboBox();
            this.cbUplinkMode = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.txtDownlinkOffset = new System.Windows.Forms.TextBox();
            this.txtUplinkOffset = new System.Windows.Forms.TextBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.bbNew = new System.Windows.Forms.Button();
            this.bbAdd = new System.Windows.Forms.Button();
            this.bbUpdate = new System.Windows.Forms.Button();
            this.bbRemove = new System.Windows.Forms.Button();
            this.bbRefresh = new System.Windows.Forms.Button();
            this.bbOpenFile = new System.Windows.Forms.Button();
            this.bbClose = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblSatellite = new System.Windows.Forms.Label();
            this.lblDownlink = new System.Windows.Forms.Label();
            this.lblUplink = new System.Windows.Forms.Label();
            this.lblDownlinkMode = new System.Windows.Forms.Label();
            this.lblUplinkMode = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblDownlinkOffset = new System.Windows.Forms.Label();
            this.lblUplinkOffset = new System.Windows.Forms.Label();
            this.lblComment = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstSqf
            // 
            this.lstSqf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstSqf.FormattingEnabled = true;
            this.lstSqf.Location = new System.Drawing.Point(12, 31);
            this.lstSqf.Name = "lstSqf";
            this.lstSqf.Size = new System.Drawing.Size(250, 394);
            this.lstSqf.TabIndex = 0;
            this.lstSqf.SelectedIndexChanged += new System.EventHandler(this.lstSqf_SelectedIndexChanged);
            // 
            // cbSatellite
            // 
            this.cbSatellite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSatellite.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbSatellite.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSatellite.FormattingEnabled = true;
            this.cbSatellite.Location = new System.Drawing.Point(374, 31);
            this.cbSatellite.Name = "cbSatellite";
            this.cbSatellite.Size = new System.Drawing.Size(398, 21);
            this.cbSatellite.TabIndex = 1;
            // 
            // txtDownlink
            // 
            this.txtDownlink.Location = new System.Drawing.Point(374, 63);
            this.txtDownlink.Name = "txtDownlink";
            this.txtDownlink.Size = new System.Drawing.Size(150, 20);
            this.txtDownlink.TabIndex = 2;
            // 
            // txtUplink
            // 
            this.txtUplink.Location = new System.Drawing.Point(622, 63);
            this.txtUplink.Name = "txtUplink";
            this.txtUplink.Size = new System.Drawing.Size(150, 20);
            this.txtUplink.TabIndex = 3;
            // 
            // cbDownlinkMode
            // 
            this.cbDownlinkMode.FormattingEnabled = true;
            this.cbDownlinkMode.Location = new System.Drawing.Point(374, 95);
            this.cbDownlinkMode.Name = "cbDownlinkMode";
            this.cbDownlinkMode.Size = new System.Drawing.Size(150, 21);
            this.cbDownlinkMode.TabIndex = 4;
            // 
            // cbUplinkMode
            // 
            this.cbUplinkMode.FormattingEnabled = true;
            this.cbUplinkMode.Location = new System.Drawing.Point(622, 95);
            this.cbUplinkMode.Name = "cbUplinkMode";
            this.cbUplinkMode.Size = new System.Drawing.Size(150, 21);
            this.cbUplinkMode.TabIndex = 5;
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(374, 127);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(150, 21);
            this.cbType.TabIndex = 6;
            // 
            // txtDownlinkOffset
            // 
            this.txtDownlinkOffset.Location = new System.Drawing.Point(374, 159);
            this.txtDownlinkOffset.Name = "txtDownlinkOffset";
            this.txtDownlinkOffset.Size = new System.Drawing.Size(150, 20);
            this.txtDownlinkOffset.TabIndex = 7;
            // 
            // txtUplinkOffset
            // 
            this.txtUplinkOffset.Location = new System.Drawing.Point(622, 159);
            this.txtUplinkOffset.Name = "txtUplinkOffset";
            this.txtUplinkOffset.Size = new System.Drawing.Size(150, 20);
            this.txtUplinkOffset.TabIndex = 8;
            // 
            // txtComment
            // 
            this.txtComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComment.Location = new System.Drawing.Point(374, 191);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(398, 20);
            this.txtComment.TabIndex = 9;
            // 
            // bbNew
            // 
            this.bbNew.Location = new System.Drawing.Point(374, 230);
            this.bbNew.Name = "bbNew";
            this.bbNew.Size = new System.Drawing.Size(82, 28);
            this.bbNew.TabIndex = 10;
            this.bbNew.Text = "New";
            this.bbNew.UseVisualStyleBackColor = true;
            this.bbNew.Click += new System.EventHandler(this.bbNew_Click);
            // 
            // bbAdd
            // 
            this.bbAdd.Location = new System.Drawing.Point(462, 230);
            this.bbAdd.Name = "bbAdd";
            this.bbAdd.Size = new System.Drawing.Size(82, 28);
            this.bbAdd.TabIndex = 11;
            this.bbAdd.Text = "Add";
            this.bbAdd.UseVisualStyleBackColor = true;
            this.bbAdd.Click += new System.EventHandler(this.bbAdd_Click);
            // 
            // bbUpdate
            // 
            this.bbUpdate.Location = new System.Drawing.Point(550, 230);
            this.bbUpdate.Name = "bbUpdate";
            this.bbUpdate.Size = new System.Drawing.Size(82, 28);
            this.bbUpdate.TabIndex = 12;
            this.bbUpdate.Text = "Update";
            this.bbUpdate.UseVisualStyleBackColor = true;
            this.bbUpdate.Click += new System.EventHandler(this.bbUpdate_Click);
            // 
            // bbRemove
            // 
            this.bbRemove.Location = new System.Drawing.Point(638, 230);
            this.bbRemove.Name = "bbRemove";
            this.bbRemove.Size = new System.Drawing.Size(82, 28);
            this.bbRemove.TabIndex = 13;
            this.bbRemove.Text = "Remove";
            this.bbRemove.UseVisualStyleBackColor = true;
            this.bbRemove.Click += new System.EventHandler(this.bbRemove_Click);
            // 
            // bbRefresh
            // 
            this.bbRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bbRefresh.Location = new System.Drawing.Point(12, 434);
            this.bbRefresh.Name = "bbRefresh";
            this.bbRefresh.Size = new System.Drawing.Size(82, 28);
            this.bbRefresh.TabIndex = 14;
            this.bbRefresh.Text = "Refresh";
            this.bbRefresh.UseVisualStyleBackColor = true;
            this.bbRefresh.Click += new System.EventHandler(this.bbRefresh_Click);
            // 
            // bbOpenFile
            // 
            this.bbOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bbOpenFile.Location = new System.Drawing.Point(100, 434);
            this.bbOpenFile.Name = "bbOpenFile";
            this.bbOpenFile.Size = new System.Drawing.Size(82, 28);
            this.bbOpenFile.TabIndex = 15;
            this.bbOpenFile.Text = "Open File";
            this.bbOpenFile.UseVisualStyleBackColor = true;
            this.bbOpenFile.Click += new System.EventHandler(this.bbOpenFile_Click);
            // 
            // bbClose
            // 
            this.bbClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bbClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bbClose.Location = new System.Drawing.Point(690, 434);
            this.bbClose.Name = "bbClose";
            this.bbClose.Size = new System.Drawing.Size(82, 28);
            this.bbClose.TabIndex = 16;
            this.bbClose.Text = "Close";
            this.bbClose.UseVisualStyleBackColor = true;
            this.bbClose.Click += new System.EventHandler(this.bbClose_Click);
            // 
            // lblFile
            // 
            this.lblFile.Location = new System.Drawing.Point(12, 10);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(760, 18);
            this.lblFile.TabIndex = 17;
            this.lblFile.Text = "Doppler.sqf";
            // 
            // lblSatellite
            // 
            this.lblSatellite.Location = new System.Drawing.Point(268, 34);
            this.lblSatellite.Name = "lblSatellite";
            this.lblSatellite.Size = new System.Drawing.Size(100, 18);
            this.lblSatellite.TabIndex = 18;
            this.lblSatellite.Text = "Satellite";
            // 
            // lblDownlink
            // 
            this.lblDownlink.Location = new System.Drawing.Point(268, 66);
            this.lblDownlink.Name = "lblDownlink";
            this.lblDownlink.Size = new System.Drawing.Size(100, 18);
            this.lblDownlink.TabIndex = 19;
            this.lblDownlink.Text = "Downlink (kHz)";
            // 
            // lblUplink
            // 
            this.lblUplink.Location = new System.Drawing.Point(530, 66);
            this.lblUplink.Name = "lblUplink";
            this.lblUplink.Size = new System.Drawing.Size(86, 18);
            this.lblUplink.TabIndex = 20;
            this.lblUplink.Text = "Uplink (kHz)";
            // 
            // lblDownlinkMode
            // 
            this.lblDownlinkMode.Location = new System.Drawing.Point(268, 98);
            this.lblDownlinkMode.Name = "lblDownlinkMode";
            this.lblDownlinkMode.Size = new System.Drawing.Size(100, 18);
            this.lblDownlinkMode.TabIndex = 21;
            this.lblDownlinkMode.Text = "Downlink Mode";
            // 
            // lblUplinkMode
            // 
            this.lblUplinkMode.Location = new System.Drawing.Point(530, 98);
            this.lblUplinkMode.Name = "lblUplinkMode";
            this.lblUplinkMode.Size = new System.Drawing.Size(86, 18);
            this.lblUplinkMode.TabIndex = 22;
            this.lblUplinkMode.Text = "Uplink Mode";
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(268, 130);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(100, 18);
            this.lblType.TabIndex = 23;
            this.lblType.Text = "Type";
            // 
            // lblDownlinkOffset
            // 
            this.lblDownlinkOffset.Location = new System.Drawing.Point(268, 162);
            this.lblDownlinkOffset.Name = "lblDownlinkOffset";
            this.lblDownlinkOffset.Size = new System.Drawing.Size(100, 18);
            this.lblDownlinkOffset.TabIndex = 24;
            this.lblDownlinkOffset.Text = "DL Offset (kHz)";
            // 
            // lblUplinkOffset
            // 
            this.lblUplinkOffset.Location = new System.Drawing.Point(530, 162);
            this.lblUplinkOffset.Name = "lblUplinkOffset";
            this.lblUplinkOffset.Size = new System.Drawing.Size(86, 18);
            this.lblUplinkOffset.TabIndex = 25;
            this.lblUplinkOffset.Text = "UL Offset (kHz)";
            // 
            // lblComment
            // 
            this.lblComment.Location = new System.Drawing.Point(268, 194);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(100, 18);
            this.lblComment.TabIndex = 26;
            this.lblComment.Text = "Comment";
            // 
            // frmSqfManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bbClose;
            this.ClientSize = new System.Drawing.Size(784, 474);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.lblUplinkOffset);
            this.Controls.Add(this.lblDownlinkOffset);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblUplinkMode);
            this.Controls.Add(this.lblDownlinkMode);
            this.Controls.Add(this.lblUplink);
            this.Controls.Add(this.lblDownlink);
            this.Controls.Add(this.lblSatellite);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.bbClose);
            this.Controls.Add(this.bbOpenFile);
            this.Controls.Add(this.bbRefresh);
            this.Controls.Add(this.bbRemove);
            this.Controls.Add(this.bbUpdate);
            this.Controls.Add(this.bbAdd);
            this.Controls.Add(this.bbNew);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.txtUplinkOffset);
            this.Controls.Add(this.txtDownlinkOffset);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.cbUplinkMode);
            this.Controls.Add(this.cbDownlinkMode);
            this.Controls.Add(this.txtUplink);
            this.Controls.Add(this.txtDownlink);
            this.Controls.Add(this.cbSatellite);
            this.Controls.Add(this.lstSqf);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(720, 380);
            this.Name = "frmSqfManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQF Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstSqf;
        private System.Windows.Forms.ComboBox cbSatellite;
        private System.Windows.Forms.TextBox txtDownlink;
        private System.Windows.Forms.TextBox txtUplink;
        private System.Windows.Forms.ComboBox cbDownlinkMode;
        private System.Windows.Forms.ComboBox cbUplinkMode;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.TextBox txtDownlinkOffset;
        private System.Windows.Forms.TextBox txtUplinkOffset;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button bbNew;
        private System.Windows.Forms.Button bbAdd;
        private System.Windows.Forms.Button bbUpdate;
        private System.Windows.Forms.Button bbRemove;
        private System.Windows.Forms.Button bbRefresh;
        private System.Windows.Forms.Button bbOpenFile;
        private System.Windows.Forms.Button bbClose;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblSatellite;
        private System.Windows.Forms.Label lblDownlink;
        private System.Windows.Forms.Label lblUplink;
        private System.Windows.Forms.Label lblDownlinkMode;
        private System.Windows.Forms.Label lblUplinkMode;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblDownlinkOffset;
        private System.Windows.Forms.Label lblUplinkOffset;
        private System.Windows.Forms.Label lblComment;
    }
}
