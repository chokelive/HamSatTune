namespace HamSatTune
{
    partial class frmLog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblCall = new System.Windows.Forms.Label();
            this.txtCallsign = new System.Windows.Forms.TextBox();
            this.lblSent = new System.Windows.Forms.Label();
            this.txtRstSent = new System.Windows.Forms.TextBox();
            this.lblRcvd = new System.Windows.Forms.Label();
            this.txtRstReceived = new System.Windows.Forms.TextBox();
            this.lblGrid = new System.Windows.Forms.Label();
            this.txtGrid = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblSat = new System.Windows.Forms.Label();
            this.txtSatellite = new System.Windows.Forms.TextBox();
            this.lblMode = new System.Windows.Forms.Label();
            this.txtMode = new System.Windows.Forms.TextBox();
            this.lblSubmode = new System.Windows.Forms.Label();
            this.cbSubmode = new System.Windows.Forms.ComboBox();
            this.lblDownlink = new System.Windows.Forms.Label();
            this.txtDownlink = new System.Windows.Forms.TextBox();
            this.lblUplink = new System.Windows.Forms.Label();
            this.txtUplink = new System.Windows.Forms.TextBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.bbSave = new System.Windows.Forms.Button();
            this.bbClear = new System.Windows.Forms.Button();
            this.bbDelete = new System.Windows.Forms.Button();
            this.bbExportAdif = new System.Windows.Forms.Button();
            this.bbClose = new System.Windows.Forms.Button();
            this.grid = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUtc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCall = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRstSent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRstReceived = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPropMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSatMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubmode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUplink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDownlink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBandRx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCall
            // 
            this.lblCall.Location = new System.Drawing.Point(12, 14);
            this.lblCall.Name = "lblCall";
            this.lblCall.Size = new System.Drawing.Size(70, 18);
            this.lblCall.TabIndex = 0;
            this.lblCall.Text = "Call";
            // 
            // txtCallsign
            // 
            this.txtCallsign.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCallsign.Location = new System.Drawing.Point(82, 11);
            this.txtCallsign.Name = "txtCallsign";
            this.txtCallsign.Size = new System.Drawing.Size(110, 20);
            this.txtCallsign.TabIndex = 1;
            this.txtCallsign.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EntryTextBox_KeyDown);
            // 
            // lblSent
            // 
            this.lblSent.Location = new System.Drawing.Point(202, 14);
            this.lblSent.Name = "lblSent";
            this.lblSent.Size = new System.Drawing.Size(42, 18);
            this.lblSent.TabIndex = 2;
            this.lblSent.Text = "Sent";
            // 
            // txtRstSent
            // 
            this.txtRstSent.Location = new System.Drawing.Point(244, 11);
            this.txtRstSent.Name = "txtRstSent";
            this.txtRstSent.Size = new System.Drawing.Size(48, 20);
            this.txtRstSent.TabIndex = 3;
            this.txtRstSent.Text = "59";
            this.txtRstSent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EntryTextBox_KeyDown);
            // 
            // lblRcvd
            // 
            this.lblRcvd.Location = new System.Drawing.Point(302, 14);
            this.lblRcvd.Name = "lblRcvd";
            this.lblRcvd.Size = new System.Drawing.Size(42, 18);
            this.lblRcvd.TabIndex = 4;
            this.lblRcvd.Text = "Rcvd";
            // 
            // txtRstReceived
            // 
            this.txtRstReceived.Location = new System.Drawing.Point(344, 11);
            this.txtRstReceived.Name = "txtRstReceived";
            this.txtRstReceived.Size = new System.Drawing.Size(48, 20);
            this.txtRstReceived.TabIndex = 5;
            this.txtRstReceived.Text = "59";
            this.txtRstReceived.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EntryTextBox_KeyDown);
            // 
            // lblGrid
            // 
            this.lblGrid.Location = new System.Drawing.Point(402, 14);
            this.lblGrid.Name = "lblGrid";
            this.lblGrid.Size = new System.Drawing.Size(38, 18);
            this.lblGrid.TabIndex = 6;
            this.lblGrid.Text = "Grid";
            // 
            // txtGrid
            // 
            this.txtGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtGrid.Location = new System.Drawing.Point(440, 11);
            this.txtGrid.Name = "txtGrid";
            this.txtGrid.Size = new System.Drawing.Size(76, 20);
            this.txtGrid.TabIndex = 7;
            this.txtGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EntryTextBox_KeyDown);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(526, 14);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(48, 18);
            this.lblName.TabIndex = 8;
            this.lblName.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(574, 11);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(92, 20);
            this.txtName.TabIndex = 9;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EntryTextBox_KeyDown);
            // 
            // lblSat
            // 
            this.lblSat.Location = new System.Drawing.Point(12, 45);
            this.lblSat.Name = "lblSat";
            this.lblSat.Size = new System.Drawing.Size(70, 18);
            this.lblSat.TabIndex = 10;
            this.lblSat.Text = "Sat";
            // 
            // txtSatellite
            // 
            this.txtSatellite.Location = new System.Drawing.Point(82, 42);
            this.txtSatellite.Name = "txtSatellite";
            this.txtSatellite.ReadOnly = true;
            this.txtSatellite.Size = new System.Drawing.Size(110, 20);
            this.txtSatellite.TabIndex = 11;
            this.txtSatellite.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EntryTextBox_KeyDown);
            // 
            // lblMode
            // 
            this.lblMode.Location = new System.Drawing.Point(202, 45);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(42, 18);
            this.lblMode.TabIndex = 12;
            this.lblMode.Text = "Mode";
            // 
            // txtMode
            // 
            this.txtMode.Location = new System.Drawing.Point(244, 42);
            this.txtMode.Name = "txtMode";
            this.txtMode.ReadOnly = true;
            this.txtMode.Size = new System.Drawing.Size(58, 20);
            this.txtMode.TabIndex = 13;
            this.txtMode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EntryTextBox_KeyDown);
            // 
            // lblSubmode
            // 
            this.lblSubmode.Location = new System.Drawing.Point(310, 45);
            this.lblSubmode.Name = "lblSubmode";
            this.lblSubmode.Size = new System.Drawing.Size(54, 18);
            this.lblSubmode.TabIndex = 14;
            this.lblSubmode.Text = "Submode";
            // 
            // cbSubmode
            // 
            this.cbSubmode.FormattingEnabled = true;
            this.cbSubmode.Items.AddRange(new object[] {
            "",
            "FT4"});
            this.cbSubmode.Location = new System.Drawing.Point(364, 42);
            this.cbSubmode.Name = "cbSubmode";
            this.cbSubmode.Size = new System.Drawing.Size(58, 21);
            this.cbSubmode.TabIndex = 15;
            this.cbSubmode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EntryTextBox_KeyDown);
            // 
            // lblDownlink
            // 
            this.lblDownlink.Location = new System.Drawing.Point(430, 45);
            this.lblDownlink.Name = "lblDownlink";
            this.lblDownlink.Size = new System.Drawing.Size(52, 18);
            this.lblDownlink.TabIndex = 16;
            this.lblDownlink.Text = "DN kHz";
            // 
            // txtDownlink
            // 
            this.txtDownlink.Location = new System.Drawing.Point(482, 42);
            this.txtDownlink.Name = "txtDownlink";
            this.txtDownlink.ReadOnly = true;
            this.txtDownlink.Size = new System.Drawing.Size(90, 20);
            this.txtDownlink.TabIndex = 17;
            this.txtDownlink.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EntryTextBox_KeyDown);
            // 
            // lblUplink
            // 
            this.lblUplink.Location = new System.Drawing.Point(582, 45);
            this.lblUplink.Name = "lblUplink";
            this.lblUplink.Size = new System.Drawing.Size(52, 18);
            this.lblUplink.TabIndex = 18;
            this.lblUplink.Text = "UP kHz";
            // 
            // txtUplink
            // 
            this.txtUplink.Location = new System.Drawing.Point(634, 42);
            this.txtUplink.Name = "txtUplink";
            this.txtUplink.ReadOnly = true;
            this.txtUplink.Size = new System.Drawing.Size(90, 20);
            this.txtUplink.TabIndex = 19;
            this.txtUplink.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EntryTextBox_KeyDown);
            // 
            // lblComment
            // 
            this.lblComment.Location = new System.Drawing.Point(12, 76);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(70, 18);
            this.lblComment.TabIndex = 20;
            this.lblComment.Text = "Comment";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(82, 73);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(584, 20);
            this.txtComment.TabIndex = 21;
            this.txtComment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EntryTextBox_KeyDown);
            // 
            // bbSave
            // 
            this.bbSave.Location = new System.Drawing.Point(12, 106);
            this.bbSave.Name = "bbSave";
            this.bbSave.Size = new System.Drawing.Size(82, 28);
            this.bbSave.TabIndex = 22;
            this.bbSave.Text = "Save";
            this.bbSave.UseVisualStyleBackColor = true;
            this.bbSave.Click += new System.EventHandler(this.bbSave_Click);
            // 
            // bbClear
            // 
            this.bbClear.Location = new System.Drawing.Point(100, 106);
            this.bbClear.Name = "bbClear";
            this.bbClear.Size = new System.Drawing.Size(82, 28);
            this.bbClear.TabIndex = 23;
            this.bbClear.Text = "Clear";
            this.bbClear.UseVisualStyleBackColor = true;
            this.bbClear.Click += new System.EventHandler(this.bbClear_Click);
            // 
            // bbDelete
            // 
            this.bbDelete.Location = new System.Drawing.Point(188, 106);
            this.bbDelete.Name = "bbDelete";
            this.bbDelete.Size = new System.Drawing.Size(82, 28);
            this.bbDelete.TabIndex = 24;
            this.bbDelete.Text = "Delete";
            this.bbDelete.UseVisualStyleBackColor = true;
            this.bbDelete.Click += new System.EventHandler(this.bbDelete_Click);
            // 
            // bbExportAdif
            // 
            this.bbExportAdif.Location = new System.Drawing.Point(276, 106);
            this.bbExportAdif.Name = "bbExportAdif";
            this.bbExportAdif.Size = new System.Drawing.Size(92, 28);
            this.bbExportAdif.TabIndex = 25;
            this.bbExportAdif.Text = "Export ADIF";
            this.bbExportAdif.UseVisualStyleBackColor = true;
            this.bbExportAdif.Click += new System.EventHandler(this.bbExportAdif_Click);
            // 
            // bbClose
            // 
            this.bbClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bbClose.Location = new System.Drawing.Point(684, 106);
            this.bbClose.Name = "bbClose";
            this.bbClose.Size = new System.Drawing.Size(82, 28);
            this.bbClose.TabIndex = 26;
            this.bbClose.Text = "Close";
            this.bbClose.UseVisualStyleBackColor = true;
            this.bbClose.Click += new System.EventHandler(this.bbClose_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colUtc,
            this.colCall,
            this.colRstSent,
            this.colRstReceived,
            this.colGrid,
            this.colName,
            this.colSat,
            this.colPropMode,
            this.colSatMode,
            this.colMode,
            this.colSubmode,
            this.colUplink,
            this.colBand,
            this.colDownlink,
            this.colBandRx,
            this.colComment});
            this.grid.Location = new System.Drawing.Point(12, 146);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(754, 272);
            this.grid.TabIndex = 27;
            // 
            // colId
            // 
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Visible = false;
            // 
            // colUtc
            // 
            this.colUtc.HeaderText = "UTC";
            this.colUtc.Name = "colUtc";
            this.colUtc.ReadOnly = true;
            // 
            // colCall
            // 
            this.colCall.HeaderText = "CALL";
            this.colCall.Name = "colCall";
            this.colCall.ReadOnly = true;
            // 
            // colRstSent
            // 
            this.colRstSent.HeaderText = "RST_SENT";
            this.colRstSent.Name = "colRstSent";
            this.colRstSent.ReadOnly = true;
            // 
            // colRstReceived
            // 
            this.colRstReceived.HeaderText = "RST_RCVD";
            this.colRstReceived.Name = "colRstReceived";
            this.colRstReceived.ReadOnly = true;
            // 
            // colGrid
            // 
            this.colGrid.HeaderText = "MY_GRIDSQUARE";
            this.colGrid.Name = "colGrid";
            this.colGrid.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.HeaderText = "NAME";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colSat
            // 
            this.colSat.HeaderText = "SAT_NAME";
            this.colSat.Name = "colSat";
            this.colSat.ReadOnly = true;
            // 
            // colPropMode
            // 
            this.colPropMode.HeaderText = "PROP_MODE";
            this.colPropMode.Name = "colPropMode";
            this.colPropMode.ReadOnly = true;
            // 
            // colSatMode
            // 
            this.colSatMode.HeaderText = "SAT_MODE";
            this.colSatMode.Name = "colSatMode";
            this.colSatMode.ReadOnly = true;
            // 
            // colMode
            // 
            this.colMode.HeaderText = "MODE";
            this.colMode.Name = "colMode";
            this.colMode.ReadOnly = true;
            // 
            // colSubmode
            // 
            this.colSubmode.HeaderText = "SUBMODE";
            this.colSubmode.Name = "colSubmode";
            this.colSubmode.ReadOnly = true;
            // 
            // colUplink
            // 
            this.colUplink.HeaderText = "FREQ";
            this.colUplink.Name = "colUplink";
            this.colUplink.ReadOnly = true;
            // 
            // colBand
            // 
            this.colBand.HeaderText = "BAND";
            this.colBand.Name = "colBand";
            this.colBand.ReadOnly = true;
            // 
            // colDownlink
            // 
            this.colDownlink.HeaderText = "FREQ_RX";
            this.colDownlink.Name = "colDownlink";
            this.colDownlink.ReadOnly = true;
            // 
            // colBandRx
            // 
            this.colBandRx.HeaderText = "BAND_RX";
            this.colBandRx.Name = "colBandRx";
            this.colBandRx.ReadOnly = true;
            // 
            // colComment
            // 
            this.colComment.HeaderText = "COMMENT";
            this.colComment.Name = "colComment";
            this.colComment.ReadOnly = true;
            // 
            // frmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 430);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.bbClose);
            this.Controls.Add(this.bbExportAdif);
            this.Controls.Add(this.bbDelete);
            this.Controls.Add(this.bbClear);
            this.Controls.Add(this.bbSave);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.txtUplink);
            this.Controls.Add(this.lblUplink);
            this.Controls.Add(this.txtDownlink);
            this.Controls.Add(this.lblDownlink);
            this.Controls.Add(this.txtMode);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.cbSubmode);
            this.Controls.Add(this.lblSubmode);
            this.Controls.Add(this.txtSatellite);
            this.Controls.Add(this.lblSat);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtGrid);
            this.Controls.Add(this.lblGrid);
            this.Controls.Add(this.txtRstReceived);
            this.Controls.Add(this.lblRcvd);
            this.Controls.Add(this.txtRstSent);
            this.Controls.Add(this.lblSent);
            this.Controls.Add(this.txtCallsign);
            this.Controls.Add(this.lblCall);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(700, 360);
            this.Name = "frmLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Satellite Log";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLog_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblCall;
        private System.Windows.Forms.TextBox txtCallsign;
        private System.Windows.Forms.Label lblSent;
        private System.Windows.Forms.TextBox txtRstSent;
        private System.Windows.Forms.Label lblRcvd;
        private System.Windows.Forms.TextBox txtRstReceived;
        private System.Windows.Forms.Label lblGrid;
        private System.Windows.Forms.TextBox txtGrid;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblSat;
        private System.Windows.Forms.TextBox txtSatellite;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.TextBox txtMode;
        private System.Windows.Forms.Label lblSubmode;
        private System.Windows.Forms.ComboBox cbSubmode;
        private System.Windows.Forms.Label lblDownlink;
        private System.Windows.Forms.TextBox txtDownlink;
        private System.Windows.Forms.Label lblUplink;
        private System.Windows.Forms.TextBox txtUplink;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button bbSave;
        private System.Windows.Forms.Button bbClear;
        private System.Windows.Forms.Button bbDelete;
        private System.Windows.Forms.Button bbExportAdif;
        private System.Windows.Forms.Button bbClose;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUtc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCall;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRstSent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRstReceived;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPropMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSatMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubmode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUplink;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBand;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDownlink;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandRx;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
    }
}
