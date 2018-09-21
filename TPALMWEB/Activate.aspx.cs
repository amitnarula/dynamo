using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data;

public partial class Activate : System.Web.UI.Page
{
    private const string phrase = "3hz7FnN8O5";
    private const string padString = "KnOlZCEa1oRe2u9OA6smSUe3MctIq9bnSoUYWbYE8hJ63t3SXnUoo7UwPtLTAlb";
    private LicenseManagement licenseManagementObj = null;

    private string GetMachineCodeFromURL()
    {
        return TPALM.CommonUtility.DecryptString(Request.QueryString["mac"].Replace(" ", "+"));
    }

    private string GetApplicationVersionFromURL()
    {
        if (string.IsNullOrEmpty(Request.QueryString["app"]))
            return "v1"; //default version of the application
        else
        {
           var appName = Request.QueryString["app"];
            //decrypt appname
           return TPALM.CommonUtility.DecryptString(appName).Substring(appName.LastIndexOf("V")+1).ToLower();
        }
    }

    private void FetchActivationData()
    {
        try
        {
            //Read the machine code
            if (!string.IsNullOrEmpty(Request.QueryString["mac"])
                && !string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                //Read the machine code & fix for + to empty transformation in URL, back to + from empty space
                string machineCode = GetMachineCodeFromURL();
                string productUID = GetProductUIDFromURL();
                
                //Payment Gateway
                bool isManualPaymentMode = !string.IsNullOrEmpty(Request.QueryString["mode"]) &&
                    Request.QueryString["mode"].Equals("manual", StringComparison.InvariantCultureIgnoreCase);

                if (isManualPaymentMode)
                {
                    //Make the Prepaid panel visible
                    pnlPrePaid.Visible = true;
                }
                else
                {
                    pnlPrePaid.Visible = false;
                }

            }
            else
                Response.Redirect("Error.aspx");
                

        }
        catch (Exception)
        {
            Response.Redirect("Error.aspx");
        }

    }

    private string GetProductUIDFromURL()
    {
        return Request.QueryString["uid"];
    }

    private void ActivateProduct(string productUID,string machineCode, int validityDays, long paymentCodeId, string paymentCode)
    {

        //Create the license key
        var keyGenerator = new SKGL.Generate();
        keyGenerator.secretPhase = phrase;
        string key = keyGenerator.doKey(validityDays, DateTime.Today, Convert.ToInt32(machineCode));

        //Store the key inside the database
        LicenseKey licKey = new LicenseKey();
        licKey.CreatedOn = DateTime.Today;
        licKey.IsExhausted = true;
        licKey.KeyCode = key;
        licKey.ProductUID = productUID;
        licKey.MachineCode = machineCode;
        licKey.PaymentCodeId = paymentCodeId;
        licKey.FirstName = txtFirstname.Text.Trim();
        licKey.Lastname = txtLastname.Text.Trim();
        licKey.Email = txtEmail.Text.Trim();
        
        if (licenseManagementObj.SaveLicenseKey(licKey))
        {
            string licenseFile = string.Empty;
            //Generate and show the license file to the user for download
            //Response.Write(key + " : Your software has been activated, Please use this license key");

            //Generate the license file and allow the user to download it
            GenerateLicenseFile(machineCode, productUID, key, licKey.FirstName, licKey.Lastname, licKey.CreatedOn, out licenseFile);

            //Send information to user and the administrator
            licenseManagementObj.SendInformation(licKey,licenseFile,paymentCode);

            //Download the file
            GenerateInfo(licenseFile);

        }
        else
            Response.Write("Specified key already registered, Please purchase the new one!");

    }

    private void GenerateLicenseFile(string mac,
        string uid, string licenseKey, string firstName, string lastName, DateTime creationDate, out string licenseFile)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(mac); //MAC
        builder.Append(uid); //UID
        builder.Append(licenseKey); //LicenseKey
        builder.Append(firstName); //First name
        builder.Append(lastName); //Last name
        ///builder.Append(creationDate.ToString("dd/MM/yyyy")); //Creation Date
        builder.Append(phrase);

        string hashing = TPALM.CommonUtility.GetHashString(builder.ToString());

        DataSet dsLicense = new DataSet("License");
        DataTable dtLicense = new DataTable("License");

        DataColumn dcMac = new DataColumn("macid", typeof(string));
        dtLicense.Columns.Add(dcMac);

        DataColumn dcUid = new DataColumn("uid", typeof(string));
        dtLicense.Columns.Add(dcUid);

        DataColumn dcLicenseKey = new DataColumn("licenseKey", typeof(string));
        dtLicense.Columns.Add(dcLicenseKey);

        DataColumn dcFirstName = new DataColumn("firstName", typeof(string));
        dtLicense.Columns.Add(dcFirstName);

        DataColumn dcLastName = new DataColumn("lastName", typeof(string));
        dtLicense.Columns.Add(dcLastName);

        DataColumn dcCreationDate = new DataColumn("date", typeof(string));
        dtLicense.Columns.Add(dcCreationDate);

        DataColumn dcHash = new DataColumn("hash", typeof(string));
        dtLicense.Columns.Add(dcHash);

        DataRow drow = dtLicense.NewRow();
        drow["macid"] = mac;
        drow["uid"] = uid;
        drow["licenseKey"] = licenseKey;
        drow["firstName"] = firstName;
        drow["lastName"] = lastName;
        drow["date"] = creationDate.ToString("dd/MM/yyyy");
        drow["hash"] = hashing;

        dtLicense.Rows.Add(drow);
        dsLicense.Tables.Add(dtLicense);
        
        string fileName = string.Format("License{0}.lic",licenseKey);
        string filePath = Server.MapPath(string.Format("GeneratedLicenses/{0}", fileName));
        using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
        {
            stream.Close();
            dsLicense.WriteXml(filePath);

        }
        licenseFile = filePath;

    }

    private void GenerateInfo(string licenseFile)
    {
        pnlInformation.Visible = true;
        pnlPrePaid.Visible = false;
        litMessage.Text = "Thanks for the registration, your license file has been generated successfully.<br/>"
            + "An email along with license has been sent to your email address provied.Please do check the SPAM folder if you didn't receive "
            + "the email.<br/>In the mean while, please use below link to download your license.";

        btnDownloadFile.CommandArgument = licenseFile;
        
    }

    private void DownloadLicenseFile(string licenseFile)
    {
        //Download License file
        HttpResponse response = HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        response.ContentType = "text/plain";
        response.AddHeader("Content-Disposition", "attachment; filename=License.lic;");
        response.TransmitFile(licenseFile);
        response.Flush();
        response.End();
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        licenseManagementObj = new LicenseManagement();
        btnValidate.Click += new EventHandler(btnValidate_Click);
        btnDownloadFile.Click += new EventHandler(btnDownloadFile_Click);
    }

    void btnDownloadFile_Click(object sender, EventArgs e)
    {
        DownloadLicenseFile(btnDownloadFile.CommandArgument);
    }

    void btnValidate_Click(object sender, EventArgs e)
    {
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FetchActivationData();
            pnlInformation.Visible = false;
        }

    }
    protected void btnValidate_Click1(object sender, EventArgs e)
    {
        int validityDays = 0;
        long paymentCodeId = 0;
        if (licenseManagementObj.ConsumePaymentCode(txtPaymentCode.Text.Trim(), GetApplicationVersionFromURL(), out validityDays, out paymentCodeId))
        {
            if (validityDays > 0 && paymentCodeId > 0)
            {
                lblMessage.Text = "Payment code is validated successfully";
                ActivateProduct(GetProductUIDFromURL(), GetMachineCodeFromURL(), validityDays, paymentCodeId, txtPaymentCode.Text.Trim());
            }
            else
                lblMessage.Text = "The validity days are not valid.";
        }
        else
            lblMessage.Text = "Please enter a valid payment code.";
    }
}