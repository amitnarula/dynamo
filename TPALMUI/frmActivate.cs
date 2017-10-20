using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TPALM;
using System.Net;
using System.Diagnostics;
using System.Web;


namespace TPALMUI
{
    public partial class frmActivate : Form
    {
        private const string ACTIVATION_BASE_URL = "http://localhost:55555/TPALMWEB/activate.aspx";

        private string GetActivationUrl(string machineCode,string productUID, bool prePaidActivation)
        {
            StringBuilder url = new StringBuilder(ACTIVATION_BASE_URL);
            url.AppendFormat("?mac={0}&uid={1}", machineCode, productUID);
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

        public frmActivate()
        {
            InitializeComponent();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            //Redirect to the url , payment, download license file
            if (CheckForInternetConnection())
            {
                Process.Start(GetActivationUrl(CommonUtility.GetMachineCode(),CommonUtility.GetProductUID(),false));
            }
        }

        private void btnValidateLicenseFile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("License file is valid?", CommonUtility.ValidateSoftware().ToString());
            
        }

        private void btnValidatePaymentCode_Click(object sender, EventArgs e)
        {
            //Redirect to the url , payment, download license file
            if (CheckForInternetConnection())
            {
                Process.Start(GetActivationUrl(CommonUtility.GetMachineCode(), CommonUtility.GetProductUID(), true));
            }
        }
    }
}
