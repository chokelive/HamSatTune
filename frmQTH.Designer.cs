
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
            this.SuspendLayout();
            // 
            // bb_saveQTH
            // 
            this.bb_saveQTH.Location = new System.Drawing.Point(103, 12);
            this.bb_saveQTH.Name = "bb_saveQTH";
            this.bb_saveQTH.Size = new System.Drawing.Size(50, 23);
            this.bb_saveQTH.TabIndex = 0;
            this.bb_saveQTH.Text = "set";
            this.bb_saveQTH.UseVisualStyleBackColor = true;
            this.bb_saveQTH.Click += new System.EventHandler(this.bb_saveQTH_Click);
            // 
            // txtQTH
            // 
            this.txtQTH.Location = new System.Drawing.Point(13, 14);
            this.txtQTH.Name = "txtQTH";
            this.txtQTH.Size = new System.Drawing.Size(59, 20);
            this.txtQTH.TabIndex = 1;
            // 
            // frmQTH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(165, 51);
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
    }
}