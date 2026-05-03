
namespace HamSatTune
{
    partial class frmQTH
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
            this.bb_saveQTH = new System.Windows.Forms.Button();
            this.txtQTH = new System.Windows.Forms.TextBox();
            this.txtCallsign = new System.Windows.Forms.TextBox();
            this.labelQTH = new System.Windows.Forms.Label();
            this.labelCallsign = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bb_saveQTH
            // 
            this.bb_saveQTH.Location = new System.Drawing.Point(99, 70);
            this.bb_saveQTH.Name = "bb_saveQTH";
            this.bb_saveQTH.Size = new System.Drawing.Size(75, 23);
            this.bb_saveQTH.TabIndex = 0;
            this.bb_saveQTH.Text = "Save";
            this.bb_saveQTH.UseVisualStyleBackColor = true;
            this.bb_saveQTH.Click += new System.EventHandler(this.bb_saveQTH_Click);
            // 
            // txtQTH
            // 
            this.txtQTH.Location = new System.Drawing.Point(74, 12);
            this.txtQTH.Name = "txtQTH";
            this.txtQTH.Size = new System.Drawing.Size(100, 20);
            this.txtQTH.TabIndex = 1;
            // 
            // txtCallsign
            // 
            this.txtCallsign.Location = new System.Drawing.Point(74, 40);
            this.txtCallsign.Name = "txtCallsign";
            this.txtCallsign.Size = new System.Drawing.Size(100, 20);
            this.txtCallsign.TabIndex = 2;
            // 
            // labelQTH
            // 
            this.labelQTH.AutoSize = true;
            this.labelQTH.Location = new System.Drawing.Point(12, 15);
            this.labelQTH.Name = "labelQTH";
            this.labelQTH.Size = new System.Drawing.Size(30, 13);
            this.labelQTH.TabIndex = 3;
            this.labelQTH.Text = "QTH";
            // 
            // labelCallsign
            // 
            this.labelCallsign.AutoSize = true;
            this.labelCallsign.Location = new System.Drawing.Point(12, 43);
            this.labelCallsign.Name = "labelCallsign";
            this.labelCallsign.Size = new System.Drawing.Size(43, 13);
            this.labelCallsign.TabIndex = 4;
            this.labelCallsign.Text = "Callsign";
            // 
            // frmQTH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 105);
            this.Controls.Add(this.labelCallsign);
            this.Controls.Add(this.labelQTH);
            this.Controls.Add(this.txtCallsign);
            this.Controls.Add(this.txtQTH);
            this.Controls.Add(this.bb_saveQTH);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmQTH";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup QTH";
            this.Load += new System.EventHandler(this.frmQTH_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bb_saveQTH;
        private System.Windows.Forms.TextBox txtQTH;
        private System.Windows.Forms.TextBox txtCallsign;
        private System.Windows.Forms.Label labelQTH;
        private System.Windows.Forms.Label labelCallsign;
    }
}
