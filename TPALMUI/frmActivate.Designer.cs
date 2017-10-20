namespace TPALMUI
{
    partial class frmActivate
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
            this.lblWarning = new System.Windows.Forms.Label();
            this.lblStep1 = new System.Windows.Forms.Label();
            this.btnActivate = new System.Windows.Forms.Button();
            this.lblStep2 = new System.Windows.Forms.Label();
            this.btnValidateLicenseFile = new System.Windows.Forms.Button();
            this.btnValidatePaymentCode = new System.Windows.Forms.Button();
            this.lblStep1Option2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Location = new System.Drawing.Point(13, 13);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(435, 13);
            this.lblWarning.TabIndex = 0;
            this.lblWarning.Text = "Your product is not activated, Please follow the instructions below to activate y" +
    "our software";
            // 
            // lblStep1
            // 
            this.lblStep1.AutoSize = true;
            this.lblStep1.Location = new System.Drawing.Point(16, 45);
            this.lblStep1.Name = "lblStep1";
            this.lblStep1.Size = new System.Drawing.Size(258, 13);
            this.lblStep1.TabIndex = 1;
            this.lblStep1.Text = "Step 1 : Activate and generate your license file online";
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(285, 40);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(163, 23);
            this.btnActivate.TabIndex = 2;
            this.btnActivate.Text = "Activate Now";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // lblStep2
            // 
            this.lblStep2.AutoSize = true;
            this.lblStep2.Location = new System.Drawing.Point(16, 128);
            this.lblStep2.Name = "lblStep2";
            this.lblStep2.Size = new System.Drawing.Size(266, 13);
            this.lblStep2.TabIndex = 3;
            this.lblStep2.Text = "Step 2 : Read your license file to validate your software";
            // 
            // btnValidateLicenseFile
            // 
            this.btnValidateLicenseFile.Location = new System.Drawing.Point(285, 123);
            this.btnValidateLicenseFile.Name = "btnValidateLicenseFile";
            this.btnValidateLicenseFile.Size = new System.Drawing.Size(163, 23);
            this.btnValidateLicenseFile.TabIndex = 4;
            this.btnValidateLicenseFile.Text = "Validate License File";
            this.btnValidateLicenseFile.UseVisualStyleBackColor = true;
            this.btnValidateLicenseFile.Click += new System.EventHandler(this.btnValidateLicenseFile_Click);
            // 
            // btnValidatePaymentCode
            // 
            this.btnValidatePaymentCode.Location = new System.Drawing.Point(286, 76);
            this.btnValidatePaymentCode.Name = "btnValidatePaymentCode";
            this.btnValidatePaymentCode.Size = new System.Drawing.Size(163, 23);
            this.btnValidatePaymentCode.TabIndex = 6;
            this.btnValidatePaymentCode.Text = "Validate Payment Code";
            this.btnValidatePaymentCode.UseVisualStyleBackColor = true;
            this.btnValidatePaymentCode.Click += new System.EventHandler(this.btnValidatePaymentCode_Click);
            // 
            // lblStep1Option2
            // 
            this.lblStep1Option2.AutoSize = true;
            this.lblStep1Option2.Location = new System.Drawing.Point(115, 81);
            this.lblStep1Option2.Name = "lblStep1Option2";
            this.lblStep1Option2.Size = new System.Drawing.Size(159, 13);
            this.lblStep1Option2.TabIndex = 5;
            this.lblStep1Option2.Text = "OR validate your payment code ";
            // 
            // frmActivate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 165);
            this.Controls.Add(this.btnValidatePaymentCode);
            this.Controls.Add(this.lblStep1Option2);
            this.Controls.Add(this.btnValidateLicenseFile);
            this.Controls.Add(this.lblStep2);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.lblStep1);
            this.Controls.Add(this.lblWarning);
            this.Name = "frmActivate";
            this.Text = "Activate your product : 2 step actvation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label lblStep1;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.Label lblStep2;
        private System.Windows.Forms.Button btnValidateLicenseFile;
        private System.Windows.Forms.Button btnValidatePaymentCode;
        private System.Windows.Forms.Label lblStep1Option2;
    }
}

