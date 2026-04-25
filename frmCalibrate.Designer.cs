namespace HamSatTune
{
    partial class frmCalibrate
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
            this.bb_txInc100 = new System.Windows.Forms.Button();
            this.bb_txInc10 = new System.Windows.Forms.Button();
            this.bb_txInc1 = new System.Windows.Forms.Button();
            this.bb_rxInc1 = new System.Windows.Forms.Button();
            this.bb_rxInc10 = new System.Windows.Forms.Button();
            this.bb_rxInc100 = new System.Windows.Forms.Button();
            this.bb_txMinus1 = new System.Windows.Forms.Button();
            this.bb_txMinus10 = new System.Windows.Forms.Button();
            this.bb_txMinus100 = new System.Windows.Forms.Button();
            this.bb_rxMinus1 = new System.Windows.Forms.Button();
            this.bb_rxMinus10 = new System.Windows.Forms.Button();
            this.bb_rxMinus100 = new System.Windows.Forms.Button();
            this.txt_RxOffset = new System.Windows.Forms.TextBox();
            this.txt_TxOffset = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bb_save = new System.Windows.Forms.Button();
            this.lblSatName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bb_txInc100
            // 
            this.bb_txInc100.Location = new System.Drawing.Point(181, 144);
            this.bb_txInc100.Name = "bb_txInc100";
            this.bb_txInc100.Size = new System.Drawing.Size(35, 23);
            this.bb_txInc100.TabIndex = 0;
            this.bb_txInc100.Text = ">>>";
            this.bb_txInc100.UseVisualStyleBackColor = true;
            this.bb_txInc100.Click += new System.EventHandler(this.bb_txInc100_Click);
            // 
            // bb_txInc10
            // 
            this.bb_txInc10.Location = new System.Drawing.Point(181, 115);
            this.bb_txInc10.Name = "bb_txInc10";
            this.bb_txInc10.Size = new System.Drawing.Size(35, 23);
            this.bb_txInc10.TabIndex = 1;
            this.bb_txInc10.Text = ">>";
            this.bb_txInc10.UseVisualStyleBackColor = true;
            this.bb_txInc10.Click += new System.EventHandler(this.bb_txInc10_Click);
            // 
            // bb_txInc1
            // 
            this.bb_txInc1.Location = new System.Drawing.Point(181, 86);
            this.bb_txInc1.Name = "bb_txInc1";
            this.bb_txInc1.Size = new System.Drawing.Size(35, 23);
            this.bb_txInc1.TabIndex = 2;
            this.bb_txInc1.Text = ">";
            this.bb_txInc1.UseVisualStyleBackColor = true;
            this.bb_txInc1.Click += new System.EventHandler(this.bb_txInc1_Click);
            // 
            // bb_rxInc1
            // 
            this.bb_rxInc1.Location = new System.Drawing.Point(55, 86);
            this.bb_rxInc1.Name = "bb_rxInc1";
            this.bb_rxInc1.Size = new System.Drawing.Size(35, 23);
            this.bb_rxInc1.TabIndex = 5;
            this.bb_rxInc1.Text = ">";
            this.bb_rxInc1.UseVisualStyleBackColor = true;
            this.bb_rxInc1.Click += new System.EventHandler(this.bb_rxInc1_Click);
            // 
            // bb_rxInc10
            // 
            this.bb_rxInc10.Location = new System.Drawing.Point(55, 115);
            this.bb_rxInc10.Name = "bb_rxInc10";
            this.bb_rxInc10.Size = new System.Drawing.Size(35, 23);
            this.bb_rxInc10.TabIndex = 4;
            this.bb_rxInc10.Text = ">>";
            this.bb_rxInc10.UseVisualStyleBackColor = true;
            this.bb_rxInc10.Click += new System.EventHandler(this.bb_rxInc10_Click);
            // 
            // bb_rxInc100
            // 
            this.bb_rxInc100.Location = new System.Drawing.Point(55, 144);
            this.bb_rxInc100.Name = "bb_rxInc100";
            this.bb_rxInc100.Size = new System.Drawing.Size(35, 23);
            this.bb_rxInc100.TabIndex = 3;
            this.bb_rxInc100.Text = ">>>";
            this.bb_rxInc100.UseVisualStyleBackColor = true;
            this.bb_rxInc100.Click += new System.EventHandler(this.bb_rxInc100_Click);
            // 
            // bb_txMinus1
            // 
            this.bb_txMinus1.Location = new System.Drawing.Point(134, 86);
            this.bb_txMinus1.Name = "bb_txMinus1";
            this.bb_txMinus1.Size = new System.Drawing.Size(35, 23);
            this.bb_txMinus1.TabIndex = 8;
            this.bb_txMinus1.Text = "<";
            this.bb_txMinus1.UseVisualStyleBackColor = true;
            this.bb_txMinus1.Click += new System.EventHandler(this.bb_txMinus1_Click);
            // 
            // bb_txMinus10
            // 
            this.bb_txMinus10.Location = new System.Drawing.Point(134, 115);
            this.bb_txMinus10.Name = "bb_txMinus10";
            this.bb_txMinus10.Size = new System.Drawing.Size(35, 23);
            this.bb_txMinus10.TabIndex = 7;
            this.bb_txMinus10.Text = "<<";
            this.bb_txMinus10.UseVisualStyleBackColor = true;
            this.bb_txMinus10.Click += new System.EventHandler(this.bb_txMinus10_Click);
            // 
            // bb_txMinus100
            // 
            this.bb_txMinus100.Location = new System.Drawing.Point(134, 144);
            this.bb_txMinus100.Name = "bb_txMinus100";
            this.bb_txMinus100.Size = new System.Drawing.Size(35, 23);
            this.bb_txMinus100.TabIndex = 6;
            this.bb_txMinus100.Text = "<<<";
            this.bb_txMinus100.UseVisualStyleBackColor = true;
            this.bb_txMinus100.Click += new System.EventHandler(this.bb_txMinus100_Click);
            // 
            // bb_rxMinus1
            // 
            this.bb_rxMinus1.Location = new System.Drawing.Point(7, 86);
            this.bb_rxMinus1.Name = "bb_rxMinus1";
            this.bb_rxMinus1.Size = new System.Drawing.Size(35, 23);
            this.bb_rxMinus1.TabIndex = 11;
            this.bb_rxMinus1.Text = "<";
            this.bb_rxMinus1.UseVisualStyleBackColor = true;
            this.bb_rxMinus1.Click += new System.EventHandler(this.bb_rxMinus1_Click);
            // 
            // bb_rxMinus10
            // 
            this.bb_rxMinus10.Location = new System.Drawing.Point(7, 115);
            this.bb_rxMinus10.Name = "bb_rxMinus10";
            this.bb_rxMinus10.Size = new System.Drawing.Size(35, 23);
            this.bb_rxMinus10.TabIndex = 10;
            this.bb_rxMinus10.Text = "<<";
            this.bb_rxMinus10.UseVisualStyleBackColor = true;
            this.bb_rxMinus10.Click += new System.EventHandler(this.bb_rxMinus10_Click);
            // 
            // bb_rxMinus100
            // 
            this.bb_rxMinus100.Location = new System.Drawing.Point(7, 144);
            this.bb_rxMinus100.Name = "bb_rxMinus100";
            this.bb_rxMinus100.Size = new System.Drawing.Size(35, 23);
            this.bb_rxMinus100.TabIndex = 9;
            this.bb_rxMinus100.Text = "<<<";
            this.bb_rxMinus100.UseVisualStyleBackColor = true;
            this.bb_rxMinus100.Click += new System.EventHandler(this.bb_rxMinus100_Click);
            // 
            // txt_RxOffset
            // 
            this.txt_RxOffset.Location = new System.Drawing.Point(7, 50);
            this.txt_RxOffset.Name = "txt_RxOffset";
            this.txt_RxOffset.Size = new System.Drawing.Size(83, 20);
            this.txt_RxOffset.TabIndex = 12;
            // 
            // txt_TxOffset
            // 
            this.txt_TxOffset.Location = new System.Drawing.Point(133, 50);
            this.txt_TxOffset.Name = "txt_TxOffset";
            this.txt_TxOffset.Size = new System.Drawing.Size(83, 20);
            this.txt_TxOffset.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "RX (kHz)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(131, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "TX (kHz)";
            // 
            // bb_save
            // 
            this.bb_save.Location = new System.Drawing.Point(84, 186);
            this.bb_save.Name = "bb_save";
            this.bb_save.Size = new System.Drawing.Size(48, 20);
            this.bb_save.TabIndex = 17;
            this.bb_save.Text = "Save";
            this.bb_save.UseVisualStyleBackColor = true;
            this.bb_save.Click += new System.EventHandler(this.bb_save_Click);
            // 
            // lblSatName
            // 
            this.lblSatName.AutoSize = true;
            this.lblSatName.Location = new System.Drawing.Point(7, 9);
            this.lblSatName.Name = "lblSatName";
            this.lblSatName.Size = new System.Drawing.Size(35, 13);
            this.lblSatName.TabIndex = 18;
            this.lblSatName.Text = "label3";
            // 
            // frmCalibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 212);
            this.Controls.Add(this.lblSatName);
            this.Controls.Add(this.bb_save);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_TxOffset);
            this.Controls.Add(this.txt_RxOffset);
            this.Controls.Add(this.bb_rxMinus1);
            this.Controls.Add(this.bb_rxMinus10);
            this.Controls.Add(this.bb_rxMinus100);
            this.Controls.Add(this.bb_txMinus1);
            this.Controls.Add(this.bb_txMinus10);
            this.Controls.Add(this.bb_txMinus100);
            this.Controls.Add(this.bb_rxInc1);
            this.Controls.Add(this.bb_rxInc10);
            this.Controls.Add(this.bb_rxInc100);
            this.Controls.Add(this.bb_txInc1);
            this.Controls.Add(this.bb_txInc10);
            this.Controls.Add(this.bb_txInc100);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmCalibrate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Radio Offset Calibration";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmCalibrate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bb_txInc100;
        private System.Windows.Forms.Button bb_txInc10;
        private System.Windows.Forms.Button bb_txInc1;
        private System.Windows.Forms.Button bb_rxInc1;
        private System.Windows.Forms.Button bb_rxInc10;
        private System.Windows.Forms.Button bb_rxInc100;
        private System.Windows.Forms.Button bb_txMinus1;
        private System.Windows.Forms.Button bb_txMinus10;
        private System.Windows.Forms.Button bb_txMinus100;
        private System.Windows.Forms.Button bb_rxMinus1;
        private System.Windows.Forms.Button bb_rxMinus10;
        private System.Windows.Forms.Button bb_rxMinus100;
        private System.Windows.Forms.TextBox txt_RxOffset;
        private System.Windows.Forms.TextBox txt_TxOffset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bb_save;
        private System.Windows.Forms.Label lblSatName;
    }
}