namespace TPA
{
    partial class FrmActivate
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
            this.btnValidatePaymentCode = new System.Windows.Forms.Button();
            this.btnValidateLicenseFile = new System.Windows.Forms.Button();
            this.lblStep2 = new System.Windows.Forms.Label();
            this.lblStep1 = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnValidatePaymentCode
            // 
            this.btnValidatePaymentCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidatePaymentCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidatePaymentCode.ForeColor = System.Drawing.Color.White;
            this.btnValidatePaymentCode.Location = new System.Drawing.Point(377, 63);
            this.btnValidatePaymentCode.Name = "btnValidatePaymentCode";
            this.btnValidatePaymentCode.Size = new System.Drawing.Size(165, 23);
            this.btnValidatePaymentCode.TabIndex = 11;
            this.btnValidatePaymentCode.Text = "Validate Payment Code";
            this.btnValidatePaymentCode.UseVisualStyleBackColor = true;
            this.btnValidatePaymentCode.Click += new System.EventHandler(this.btnValidatePaymentCode_Click);
            // 
            // btnValidateLicenseFile
            // 
            this.btnValidateLicenseFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidateLicenseFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidateLicenseFile.ForeColor = System.Drawing.Color.White;
            this.btnValidateLicenseFile.Location = new System.Drawing.Point(377, 110);
            this.btnValidateLicenseFile.Name = "btnValidateLicenseFile";
            this.btnValidateLicenseFile.Size = new System.Drawing.Size(165, 23);
            this.btnValidateLicenseFile.TabIndex = 13;
            this.btnValidateLicenseFile.Text = "Validate License File";
            this.btnValidateLicenseFile.UseVisualStyleBackColor = true;
            this.btnValidateLicenseFile.Click += new System.EventHandler(this.btnValidateLicenseFile_Click);
            // 
            // lblStep2
            // 
            this.lblStep2.AutoSize = true;
            this.lblStep2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStep2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(199)))), ((int)(((byte)(234)))));
            this.lblStep2.Location = new System.Drawing.Point(16, 115);
            this.lblStep2.Name = "lblStep2";
            this.lblStep2.Size = new System.Drawing.Size(310, 13);
            this.lblStep2.TabIndex = 10;
            this.lblStep2.Text = "Step 2 : Read your license file to activate test series.";
            // 
            // lblStep1
            // 
            this.lblStep1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStep1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(199)))), ((int)(((byte)(234)))));
            this.lblStep1.Location = new System.Drawing.Point(16, 63);
            this.lblStep1.Name = "lblStep1";
            this.lblStep1.Size = new System.Drawing.Size(357, 33);
            this.lblStep1.TabIndex = 8;
            this.lblStep1.Text = "Step 1 : Validate your payment code and generate licenseValidate your voucher cod" +
    "e and generate license file.";
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(96)))), ((int)(((byte)(156)))));
            this.lblWarning.Location = new System.Drawing.Point(15, 9);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(527, 17);
            this.lblWarning.TabIndex = 7;
            this.lblWarning.Text = "Please follow the below steps to activate Path Abroad Mock Test Series";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(199)))), ((int)(((byte)(234)))));
            this.panel1.Controls.Add(this.lblWarning);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(562, 36);
            this.panel1.TabIndex = 14;
            // 
            // FrmActivate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(96)))), ((int)(((byte)(156)))));
            this.ClientSize = new System.Drawing.Size(562, 165);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnValidatePaymentCode);
            this.Controls.Add(this.btnValidateLicenseFile);
            this.Controls.Add(this.lblStep2);
            this.Controls.Add(this.lblStep1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmActivate";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmActivate_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnValidatePaymentCode;
        private System.Windows.Forms.Button btnValidateLicenseFile;
        private System.Windows.Forms.Label lblStep2;
        private System.Windows.Forms.Label lblStep1;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Panel panel1;
    }
}