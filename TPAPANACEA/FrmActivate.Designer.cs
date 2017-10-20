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
            this.SuspendLayout();
            // 
            // btnValidatePaymentCode
            // 
            this.btnValidatePaymentCode.Location = new System.Drawing.Point(301, 45);
            this.btnValidatePaymentCode.Name = "btnValidatePaymentCode";
            this.btnValidatePaymentCode.Size = new System.Drawing.Size(163, 23);
            this.btnValidatePaymentCode.TabIndex = 11;
            this.btnValidatePaymentCode.Text = "Validate Payment Code";
            this.btnValidatePaymentCode.UseVisualStyleBackColor = true;
            this.btnValidatePaymentCode.Click += new System.EventHandler(this.btnValidatePaymentCode_Click);
            // 
            // btnValidateLicenseFile
            // 
            this.btnValidateLicenseFile.Location = new System.Drawing.Point(301, 92);
            this.btnValidateLicenseFile.Name = "btnValidateLicenseFile";
            this.btnValidateLicenseFile.Size = new System.Drawing.Size(163, 23);
            this.btnValidateLicenseFile.TabIndex = 13;
            this.btnValidateLicenseFile.Text = "Validate License File";
            this.btnValidateLicenseFile.UseVisualStyleBackColor = true;
            this.btnValidateLicenseFile.Click += new System.EventHandler(this.btnValidateLicenseFile_Click);
            // 
            // lblStep2
            // 
            this.lblStep2.AutoSize = true;
            this.lblStep2.Location = new System.Drawing.Point(13, 97);
            this.lblStep2.Name = "lblStep2";
            this.lblStep2.Size = new System.Drawing.Size(267, 13);
            this.lblStep2.TabIndex = 10;
            this.lblStep2.Text = "Step 2 : Read your license file to activate your software";
            // 
            // lblStep1
            // 
            this.lblStep1.AutoSize = true;
            this.lblStep1.Location = new System.Drawing.Point(13, 50);
            this.lblStep1.Name = "lblStep1";
            this.lblStep1.Size = new System.Drawing.Size(280, 13);
            this.lblStep1.TabIndex = 8;
            this.lblStep1.Text = "Step 1 : Validate your payment code and generate license";
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Location = new System.Drawing.Point(10, 18);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(435, 13);
            this.lblWarning.TabIndex = 7;
            this.lblWarning.Text = "Your product is not activated, Please follow the instructions below to activate y" +
    "our software";
            // 
            // FrmActivate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 140);
            this.Controls.Add(this.btnValidatePaymentCode);
            this.Controls.Add(this.btnValidateLicenseFile);
            this.Controls.Add(this.lblStep2);
            this.Controls.Add(this.lblStep1);
            this.Controls.Add(this.lblWarning);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmActivate";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Activation required";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmActivate_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnValidatePaymentCode;
        private System.Windows.Forms.Button btnValidateLicenseFile;
        private System.Windows.Forms.Label lblStep2;
        private System.Windows.Forms.Label lblStep1;
        private System.Windows.Forms.Label lblWarning;
    }
}