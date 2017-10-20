namespace TPAMED
{
    partial class frmMediaEncrypter
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
            this.btnEncryptMedia = new System.Windows.Forms.Button();
            this.btnDecrptMedia = new System.Windows.Forms.Button();
            this.btnPlayShow = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnRenameFiles = new System.Windows.Forms.Button();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.ucMediaElement1 = new TPAMED.ucMediaElement();
            this.btnCheckMediaForErrors = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEncryptMedia
            // 
            this.btnEncryptMedia.Location = new System.Drawing.Point(12, 12);
            this.btnEncryptMedia.Name = "btnEncryptMedia";
            this.btnEncryptMedia.Size = new System.Drawing.Size(110, 23);
            this.btnEncryptMedia.TabIndex = 0;
            this.btnEncryptMedia.Text = "Encrypt Media";
            this.btnEncryptMedia.UseVisualStyleBackColor = true;
            this.btnEncryptMedia.Click += new System.EventHandler(this.btnEncryptMedia_Click);
            // 
            // btnDecrptMedia
            // 
            this.btnDecrptMedia.Location = new System.Drawing.Point(128, 12);
            this.btnDecrptMedia.Name = "btnDecrptMedia";
            this.btnDecrptMedia.Size = new System.Drawing.Size(103, 23);
            this.btnDecrptMedia.TabIndex = 1;
            this.btnDecrptMedia.Text = "Decrypt Media";
            this.btnDecrptMedia.UseVisualStyleBackColor = true;
            this.btnDecrptMedia.Click += new System.EventHandler(this.btnDecrptMedia_Click);
            // 
            // btnPlayShow
            // 
            this.btnPlayShow.Location = new System.Drawing.Point(318, 12);
            this.btnPlayShow.Name = "btnPlayShow";
            this.btnPlayShow.Size = new System.Drawing.Size(75, 23);
            this.btnPlayShow.TabIndex = 2;
            this.btnPlayShow.Text = "Testing";
            this.btnPlayShow.UseVisualStyleBackColor = true;
            this.btnPlayShow.Click += new System.EventHandler(this.btnPlayShow_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(13, 40);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(300, 23);
            this.progressBar.TabIndex = 4;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(319, 50);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 5;
            // 
            // btnRenameFiles
            // 
            this.btnRenameFiles.Location = new System.Drawing.Point(237, 12);
            this.btnRenameFiles.Name = "btnRenameFiles";
            this.btnRenameFiles.Size = new System.Drawing.Size(75, 23);
            this.btnRenameFiles.TabIndex = 6;
            this.btnRenameFiles.Text = "Rename";
            this.btnRenameFiles.UseVisualStyleBackColor = true;
            this.btnRenameFiles.Click += new System.EventHandler(this.btnRenameFiles_Click);
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.elementHost1.Location = new System.Drawing.Point(0, 73);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(479, 306);
            this.elementHost1.TabIndex = 3;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.ucMediaElement1;
            // 
            // btnCheckMediaForErrors
            // 
            this.btnCheckMediaForErrors.Location = new System.Drawing.Point(318, 40);
            this.btnCheckMediaForErrors.Name = "btnCheckMediaForErrors";
            this.btnCheckMediaForErrors.Size = new System.Drawing.Size(104, 23);
            this.btnCheckMediaForErrors.TabIndex = 7;
            this.btnCheckMediaForErrors.Text = "Check Errors";
            this.btnCheckMediaForErrors.UseVisualStyleBackColor = true;
            this.btnCheckMediaForErrors.Click += new System.EventHandler(this.btnCheckMediaForErrors_Click);
            // 
            // frmMediaEncrypter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 379);
            this.Controls.Add(this.btnCheckMediaForErrors);
            this.Controls.Add(this.btnRenameFiles);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.btnPlayShow);
            this.Controls.Add(this.btnDecrptMedia);
            this.Controls.Add(this.btnEncryptMedia);
            this.Name = "frmMediaEncrypter";
            this.Text = "Media Encrypter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEncryptMedia;
        private System.Windows.Forms.Button btnDecrptMedia;
        private System.Windows.Forms.Button btnPlayShow;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private ucMediaElement ucMediaElement1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnRenameFiles;
        private System.Windows.Forms.Button btnCheckMediaForErrors;
    }
}

