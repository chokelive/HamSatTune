
namespace HamSatTune
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.cbList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_TxFreq = new System.Windows.Forms.Label();
            this.lbl_RxFreq = new System.Windows.Forms.Label();
            this.lbl_el = new System.Windows.Forms.Label();
            this.lbl_az = new System.Windows.Forms.Label();
            this.lbl_downlinkMode = new System.Windows.Forms.Label();
            this.lbl_uplinkMode = new System.Windows.Forms.Label();
            this.txt_TuneRx = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bb_tune = new System.Windows.Forms.Button();
            this.chk_ConnectRig = new System.Windows.Forms.CheckBox();
            this.bb_omirigSetup = new System.Windows.Forms.Button();
            this.bb_freqResetCenter = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bb_qth = new System.Windows.Forms.Button();
            this.lbl_qth = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.bb_tuneTx = new System.Windows.Forms.Button();
            this.chk_Simplex = new System.Windows.Forms.CheckBox();
            this.lbl_rigtype = new System.Windows.Forms.Label();
            this.linkAbout = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // cbList
            // 
            this.cbList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbList.FormattingEnabled = true;
            this.cbList.Location = new System.Drawing.Point(11, 134);
            this.cbList.Margin = new System.Windows.Forms.Padding(2);
            this.cbList.Name = "cbList";
            this.cbList.Size = new System.Drawing.Size(192, 21);
            this.cbList.TabIndex = 0;
            this.cbList.SelectedIndexChanged += new System.EventHandler(this.cbList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 40);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "UpLink";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "DnLink";
            // 
            // lbl_TxFreq
            // 
            this.lbl_TxFreq.AutoSize = true;
            this.lbl_TxFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TxFreq.ForeColor = System.Drawing.Color.Red;
            this.lbl_TxFreq.Location = new System.Drawing.Point(49, 35);
            this.lbl_TxFreq.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_TxFreq.Name = "lbl_TxFreq";
            this.lbl_TxFreq.Size = new System.Drawing.Size(64, 20);
            this.lbl_TxFreq.TabIndex = 3;
            this.lbl_TxFreq.Text = "TxFreq";
            // 
            // lbl_RxFreq
            // 
            this.lbl_RxFreq.AutoSize = true;
            this.lbl_RxFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RxFreq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lbl_RxFreq.Location = new System.Drawing.Point(50, 10);
            this.lbl_RxFreq.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_RxFreq.Name = "lbl_RxFreq";
            this.lbl_RxFreq.Size = new System.Drawing.Size(67, 20);
            this.lbl_RxFreq.TabIndex = 4;
            this.lbl_RxFreq.Text = "RxFreq";
            // 
            // lbl_el
            // 
            this.lbl_el.AutoSize = true;
            this.lbl_el.Location = new System.Drawing.Point(89, 73);
            this.lbl_el.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_el.Name = "lbl_el";
            this.lbl_el.Size = new System.Drawing.Size(20, 13);
            this.lbl_el.TabIndex = 6;
            this.lbl_el.Text = "EL";
            // 
            // lbl_az
            // 
            this.lbl_az.AutoSize = true;
            this.lbl_az.Location = new System.Drawing.Point(29, 73);
            this.lbl_az.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_az.Name = "lbl_az";
            this.lbl_az.Size = new System.Drawing.Size(21, 13);
            this.lbl_az.TabIndex = 5;
            this.lbl_az.Text = "AZ";
            // 
            // lbl_downlinkMode
            // 
            this.lbl_downlinkMode.AutoSize = true;
            this.lbl_downlinkMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbl_downlinkMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_downlinkMode.Location = new System.Drawing.Point(151, 13);
            this.lbl_downlinkMode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_downlinkMode.Name = "lbl_downlinkMode";
            this.lbl_downlinkMode.Size = new System.Drawing.Size(52, 13);
            this.lbl_downlinkMode.TabIndex = 7;
            this.lbl_downlinkMode.Text = "dnMode";
            // 
            // lbl_uplinkMode
            // 
            this.lbl_uplinkMode.AutoSize = true;
            this.lbl_uplinkMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lbl_uplinkMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_uplinkMode.Location = new System.Drawing.Point(151, 40);
            this.lbl_uplinkMode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_uplinkMode.Name = "lbl_uplinkMode";
            this.lbl_uplinkMode.Size = new System.Drawing.Size(52, 13);
            this.lbl_uplinkMode.TabIndex = 8;
            this.lbl_uplinkMode.Text = "upMode";
            // 
            // txt_TuneRx
            // 
            this.txt_TuneRx.Location = new System.Drawing.Point(235, 18);
            this.txt_TuneRx.Margin = new System.Windows.Forms.Padding(2);
            this.txt_TuneRx.Name = "txt_TuneRx";
            this.txt_TuneRx.Size = new System.Drawing.Size(83, 20);
            this.txt_TuneRx.TabIndex = 9;
            this.txt_TuneRx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_TuneRx_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 73);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Az:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(70, 73);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "El:";
            // 
            // bb_tune
            // 
            this.bb_tune.Location = new System.Drawing.Point(322, 18);
            this.bb_tune.Margin = new System.Windows.Forms.Padding(2);
            this.bb_tune.Name = "bb_tune";
            this.bb_tune.Size = new System.Drawing.Size(67, 20);
            this.bb_tune.TabIndex = 13;
            this.bb_tune.Text = "Tune RX";
            this.bb_tune.UseVisualStyleBackColor = true;
            this.bb_tune.Click += new System.EventHandler(this.bb_tune_Click);
            // 
            // chk_ConnectRig
            // 
            this.chk_ConnectRig.AutoSize = true;
            this.chk_ConnectRig.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_ConnectRig.Location = new System.Drawing.Point(235, 71);
            this.chk_ConnectRig.Margin = new System.Windows.Forms.Padding(2);
            this.chk_ConnectRig.Name = "chk_ConnectRig";
            this.chk_ConnectRig.Size = new System.Drawing.Size(120, 17);
            this.chk_ConnectRig.TabIndex = 14;
            this.chk_ConnectRig.Text = "Connect Radio (R1)";
            this.chk_ConnectRig.UseVisualStyleBackColor = true;
            this.chk_ConnectRig.CheckedChanged += new System.EventHandler(this.chk_ConnectRig_CheckedChanged);
            // 
            // bb_omirigSetup
            // 
            this.bb_omirigSetup.Location = new System.Drawing.Point(235, 134);
            this.bb_omirigSetup.Margin = new System.Windows.Forms.Padding(2);
            this.bb_omirigSetup.Name = "bb_omirigSetup";
            this.bb_omirigSetup.Size = new System.Drawing.Size(55, 21);
            this.bb_omirigSetup.TabIndex = 15;
            this.bb_omirigSetup.Text = "OmniRig";
            this.bb_omirigSetup.UseVisualStyleBackColor = true;
            this.bb_omirigSetup.Click += new System.EventHandler(this.bb_omirigSetup_Click);
            // 
            // bb_freqResetCenter
            // 
            this.bb_freqResetCenter.Location = new System.Drawing.Point(135, 69);
            this.bb_freqResetCenter.Margin = new System.Windows.Forms.Padding(2);
            this.bb_freqResetCenter.Name = "bb_freqResetCenter";
            this.bb_freqResetCenter.Size = new System.Drawing.Size(68, 21);
            this.bb_freqResetCenter.TabIndex = 16;
            this.bb_freqResetCenter.Text = "Freq Reset";
            this.bb_freqResetCenter.UseVisualStyleBackColor = true;
            this.bb_freqResetCenter.Click += new System.EventHandler(this.bb_freqResetCenter_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(233, 55);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Auto RX";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(233, 2);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Manual RX";
            // 
            // bb_qth
            // 
            this.bb_qth.Location = new System.Drawing.Point(295, 134);
            this.bb_qth.Name = "bb_qth";
            this.bb_qth.Size = new System.Drawing.Size(48, 21);
            this.bb_qth.TabIndex = 19;
            this.bb_qth.Text = "QTH";
            this.bb_qth.UseVisualStyleBackColor = true;
            this.bb_qth.Click += new System.EventHandler(this.bb_qth_Click);
            // 
            // lbl_qth
            // 
            this.lbl_qth.AutoSize = true;
            this.lbl_qth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_qth.Location = new System.Drawing.Point(40, 91);
            this.lbl_qth.Name = "lbl_qth";
            this.lbl_qth.Size = new System.Drawing.Size(40, 13);
            this.lbl_qth.TabIndex = 20;
            this.lbl_qth.Text = "lblQTH";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "QTH:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Satellite";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(233, 116);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Config";
            // 
            // bb_tuneTx
            // 
            this.bb_tuneTx.Location = new System.Drawing.Point(322, 43);
            this.bb_tuneTx.Name = "bb_tuneTx";
            this.bb_tuneTx.Size = new System.Drawing.Size(67, 20);
            this.bb_tuneTx.TabIndex = 24;
            this.bb_tuneTx.Text = "Tune TX";
            this.bb_tuneTx.UseVisualStyleBackColor = true;
            this.bb_tuneTx.Click += new System.EventHandler(this.bb_tuneTx_Click);
            // 
            // chk_Simplex
            // 
            this.chk_Simplex.AutoSize = true;
            this.chk_Simplex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_Simplex.Location = new System.Drawing.Point(235, 87);
            this.chk_Simplex.Name = "chk_Simplex";
            this.chk_Simplex.Size = new System.Drawing.Size(174, 17);
            this.chk_Simplex.TabIndex = 25;
            this.chk_Simplex.Text = "Simplex (VFO A RX, VFO B TX)";
            this.chk_Simplex.UseVisualStyleBackColor = true;
            this.chk_Simplex.CheckedChanged += new System.EventHandler(this.chk_Simplex_CheckedChanged);
            // 
            // lbl_rigtype
            // 
            this.lbl_rigtype.AutoSize = true;
            this.lbl_rigtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_rigtype.ForeColor = System.Drawing.Color.Maroon;
            this.lbl_rigtype.Location = new System.Drawing.Point(270, 116);
            this.lbl_rigtype.Name = "lbl_rigtype";
            this.lbl_rigtype.Size = new System.Drawing.Size(33, 12);
            this.lbl_rigtype.TabIndex = 26;
            this.lbl_rigtype.Text = "rigtype";
            // 
            // linkAbout
            // 
            this.linkAbout.AutoSize = true;
            this.linkAbout.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkAbout.Location = new System.Drawing.Point(350, 139);
            this.linkAbout.Name = "linkAbout";
            this.linkAbout.Size = new System.Drawing.Size(35, 13);
            this.linkAbout.TabIndex = 27;
            this.linkAbout.TabStop = true;
            this.linkAbout.Text = "About";
            this.linkAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkAbout_LinkClicked);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 162);
            this.Controls.Add(this.linkAbout);
            this.Controls.Add(this.lbl_rigtype);
            this.Controls.Add(this.bb_tuneTx);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbl_qth);
            this.Controls.Add(this.bb_qth);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bb_freqResetCenter);
            this.Controls.Add(this.bb_omirigSetup);
            this.Controls.Add(this.chk_ConnectRig);
            this.Controls.Add(this.bb_tune);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_TuneRx);
            this.Controls.Add(this.lbl_uplinkMode);
            this.Controls.Add(this.lbl_downlinkMode);
            this.Controls.Add(this.lbl_el);
            this.Controls.Add(this.lbl_az);
            this.Controls.Add(this.lbl_RxFreq);
            this.Controls.Add(this.lbl_TxFreq);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbList);
            this.Controls.Add(this.chk_Simplex);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HamSatTune by E29AHU";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_TxFreq;
        private System.Windows.Forms.Label lbl_RxFreq;
        private System.Windows.Forms.Label lbl_el;
        private System.Windows.Forms.Label lbl_az;
        private System.Windows.Forms.Label lbl_downlinkMode;
        private System.Windows.Forms.Label lbl_uplinkMode;
        private System.Windows.Forms.TextBox txt_TuneRx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bb_tune;
        private System.Windows.Forms.CheckBox chk_ConnectRig;
        private System.Windows.Forms.Button bb_omirigSetup;
        private System.Windows.Forms.Button bb_freqResetCenter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button bb_qth;
        private System.Windows.Forms.Label lbl_qth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button bb_tuneTx;
        private System.Windows.Forms.CheckBox chk_Simplex;
        private System.Windows.Forms.Label lbl_rigtype;
        private System.Windows.Forms.LinkLabel linkAbout;
    }
}

