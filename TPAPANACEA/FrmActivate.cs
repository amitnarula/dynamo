using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using TPALM;
using System.IO;

namespace TPA
{
    public partial class FrmActivate : Form
    {
        //private const string ACTIVATION_BASE_URL = "http://localhost:55555/TPALMWEB/activate.aspx";
        private const string ACTIVATION_BASE_URL = "http://ptepanacea.in/activate.aspx";
        private string licenseFilePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Data//PASLicense.lic";
        public string AppName { get; set; }

        private string GetActivationUrl(string machineCode,string productUID, bool prePaidActivation)
        {
            StringBuilder url = new StringBuilder(ACTIVATION_BASE_URL);
            url.AppendFormat("?mac={0}&uid={1}&app={2}", machineCode, productUID, CommonUtility.EncryptString(AppName));
            if (prePaidActivation)
                url.AppendFormat("&mode=manual");

            
            
            return Uri.EscapeUriString(url.ToString());
            
        }

        private bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Please make sure you are connected to internet");
                return false;
            }
        }

        public FrmActivate()
        {
            InitializeComponent();
        }
        
        private void btnValidatePaymentCode_Click(object sender, EventArgs e)
        {
            //Redirect to the url , payment, download license file
            //if (CheckForInternetConnection())
            //{
                Process.Start(GetActivationUrl(CommonUtility.GetMachineCode(), CommonUtility.GetProductUID(AppName), true));
            //}
        }

        private void btnValidateLicenseFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "License File|*.lic";
                    openFileDialog.Title = "Validate license file";
                    openFileDialog.ShowDialog();

                    // If the file name is not an empty string open it for saving.
                    if (!string.IsNullOrEmpty(openFileDialog.FileName))
                    {
                        // Saves the Image via a FileStream created by the OpenFile method.
                        using (System.IO.FileStream fs =
                           (System.IO.FileStream)openFileDialog.OpenFile())
                        {

                            using (FileStream fsLicense = File.Create(licenseFilePath))
                            {
                                fs.CopyTo(fsLicense);
                            }

                        }
                    }
                }

                if (CommonUtility.ValidateSoftware(AppName))
                {
                    MessageBox.Show("Your product has been activated successfully, Please restart your application again.");

                    var dialogOwnerInfo = new FrmOwner();

                    while(dialogOwnerInfo.ShowDialog() != DialogResult.OK){
                        continue;
                    }
                    this.Close();
                    this.Dispose();

                }
                else
                    MessageBox.Show("Product activation failed.");
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't save the license file or the location is not accessible.");
            }
            
        }

        private void FrmActivate_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

    }
}
