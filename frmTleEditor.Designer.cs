namespace HamSatTune
{
    partial class frmTleEditor
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
            this.txtTleData = new System.Windows.Forms.TextBox();
            this.bbSave = new System.Windows.Forms.Button();
            this.bbCancel = new System.Windows.Forms.Button();
            this.lblTleData = new System.Windows.Forms.Label();
            this.bbViewCurrent = new System.Windows.Forms.Button();
            this.bbViewManual = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtTleData
            // 
            this.txtTleData.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTleData.Location = new System.Drawing.Point(12, 30);
            this.txtTleData.Multiline = true;
            this.txtTleData.Name = "txtTleData";
            this.txtTleData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTleData.Size = new System.Drawing.Size(536, 178);
            this.txtTleData.TabIndex = 0;
            // 
            // bbSave
            // 
            this.bbSave.Location = new System.Drawing.Point(376, 234);
            this.bbSave.Name = "bbSave";
            this.bbSave.Size = new System.Drawing.Size(82, 28);
            this.bbSave.TabIndex = 1;
            this.bbSave.Text = "Save";
            this.bbSave.UseVisualStyleBackColor = true;
            this.bbSave.Click += new System.EventHandler(this.bbSave_Click);
            // 
            // bbCancel
            // 
            this.bbCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bbCancel.Location = new System.Drawing.Point(466, 234);
            this.bbCancel.Name = "bbCancel";
            this.bbCancel.Size = new System.Drawing.Size(82, 28);
            this.bbCancel.TabIndex = 2;
            this.bbCancel.Text = "Cancel";
            this.bbCancel.UseVisualStyleBackColor = true;
            // 
            // lblTleData
            // 
            this.lblTleData.Location = new System.Drawing.Point(12, 12);
            this.lblTleData.Name = "lblTleData";
            this.lblTleData.Size = new System.Drawing.Size(260, 18);
            this.lblTleData.TabIndex = 3;
            this.lblTleData.Text = "TLE data (name, line 1, line 2)";
            // 
            // bbViewCurrent
            // 
            this.bbViewCurrent.Location = new System.Drawing.Point(12, 234);
            this.bbViewCurrent.Name = "bbViewCurrent";
            this.bbViewCurrent.Size = new System.Drawing.Size(90, 24);
            this.bbViewCurrent.TabIndex = 4;
            this.bbViewCurrent.Text = "View Current";
            this.bbViewCurrent.UseVisualStyleBackColor = true;
            this.bbViewCurrent.Click += new System.EventHandler(this.bbViewCurrent_Click);
            // 
            // bbViewManual
            // 
            this.bbViewManual.Location = new System.Drawing.Point(108, 234);
            this.bbViewManual.Name = "bbViewManual";
            this.bbViewManual.Size = new System.Drawing.Size(90, 24);
            this.bbViewManual.TabIndex = 5;
            this.bbViewManual.Text = "View Manual";
            this.bbViewManual.UseVisualStyleBackColor = true;
            this.bbViewManual.Click += new System.EventHandler(this.bbViewManual_Click);
            // 
            // frmTleEditor
            // 
            this.AcceptButton = this.bbSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bbCancel;
            this.ClientSize = new System.Drawing.Size(560, 284);
            this.Controls.Add(this.bbViewManual);
            this.Controls.Add(this.bbViewCurrent);
            this.Controls.Add(this.lblTleData);
            this.Controls.Add(this.bbCancel);
            this.Controls.Add(this.bbSave);
            this.Controls.Add(this.txtTleData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTleEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add TLE";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTleData;
        private System.Windows.Forms.Button bbSave;
        private System.Windows.Forms.Button bbCancel;
        private System.Windows.Forms.Label lblTleData;
        private System.Windows.Forms.Button bbViewCurrent;
        private System.Windows.Forms.Button bbViewManual;
    }
}
