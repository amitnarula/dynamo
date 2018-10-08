﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for LicenseManagerBusinessLogic
/// </summary>
public class LicenseManagement
{
    LicenseManagerEntities dataContext = new LicenseManagerEntities();

    public LicenseManagement()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private LicenseKey GetByMachineCode(string machineCode)
    { 
        return dataContext.LicenseKeys.Where(x=>x.MachineCode.Equals(machineCode,StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
    }

    private LicenseKey GetByLicenseKey(string licenseKey)
    {
        return dataContext.LicenseKeys.Where(x => x.KeyCode.Equals(licenseKey, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
    }
    private LicenseKey GetByProductUID(string productUID)
    {
        return dataContext.LicenseKeys.Where(x => x.ProductUID.Equals(productUID, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
    }

    private bool IsPaymentCodeValid(string code, string appVersion)
    {
        return dataContext.PaymentCodes.SingleOrDefault(x => x.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase)
            && !x.IsExhausted && x.AppVersion.Equals(appVersion, StringComparison.InvariantCultureIgnoreCase)) != null;
    }

    
    public bool SaveLicenseKey(LicenseKey key)
    {
        //if (GetByMachineCode(key.MachineCode) != null)
          //  return false;
        //if (GetByLicenseKey(key.KeyCode) != null)
          //  return false;
        //else if (GetByProductUID(key.ProductUID) != null)
           // return false;
        //else
        //{
            dataContext.LicenseKeys.Add(key);
            return dataContext.SaveChanges() > 0;
        //}
    }

    public bool ConsumePaymentCode(string code, string appVersion, out int validityDays, out long paymentCodeId)
    {
        validityDays = 0;
        paymentCodeId = 0;
        if (IsPaymentCodeValid(code,appVersion))
        {
            PaymentCode paymentCode = dataContext.PaymentCodes.SingleOrDefault(x => x.Code.Equals(code));
            validityDays = paymentCode.ValidityDays;
            paymentCodeId = paymentCode.Id;
            paymentCode.IsExhausted = true;
            dataContext.Entry(paymentCode).State = EntityState.Modified;
            return dataContext.SaveChanges() > 0;
        }
        return false;
    }

    public void SendInformation(LicenseKey licenseKey, string licenseFile,string paymentCode, string licenseContent)
    {
        try
        {
            string subject = "Welcome, your information has been saved. Reference:" + licenseKey.KeyCode;
            StringBuilder builder = new StringBuilder();
            builder.Append("Thanks for the registration. Your new license has been created with following details.");
            builder.Append("Attached is your license file.");
            builder.Append("<br/>");
            builder.Append("Name:" + licenseKey.FirstName);
            builder.Append("<br/>");
            builder.Append("Lastname:" + licenseKey.Lastname);
            builder.Append("<br/>");
            builder.Append("Email:" + licenseKey.Email);
            builder.Append("<br/>");
            builder.Append("Key:" + licenseKey.KeyCode);
            builder.Append("<br/>");
            builder.Append("Product ID:" + licenseKey.ProductUID);
            builder.Append("<br/>");
            builder.Append("Payment Code:" + paymentCode);


            string administratorEmail = ConfigurationManager.AppSettings["adminEmail"];
            string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            int smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
            string smtpUsername = ConfigurationManager.AppSettings["smtpUsername"];
            string smtpPassword = ConfigurationManager.AppSettings["smtpPassword"];

            SmtpClient client = new SmtpClient(smtpServer, smtpPort);
            client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            MailMessage msg = new MailMessage();
            msg.To.Add(administratorEmail);
            msg.To.Add(licenseKey.Email);

            msg.Bcc.Add("amrinder.bhagtana@gmail.com");
            msg.Bcc.Add("amit.narula2008@gmail.com");

            msg.From = new MailAddress(smtpUsername);
            msg.Subject = subject;
            msg.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");

            //Attachment
            /*var attachment = new System.Net.Mail.Attachment(licenseFile);
            msg.Attachments.Add(attachment);*/

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            sw.Write(licenseContent);
            sw.Flush();
            ms.Position = 0;

            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);
            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, ct);
            attachment.ContentDisposition.FileName = string.Format("License{0}.lic", licenseKey.KeyCode);
            msg.Attachments.Add(attachment);

            msg.IsBodyHtml = true;

            msg.Body = builder.ToString();
            client.Send(msg);

            sw.Close();
            sw.Dispose();
            ms.Close();
            ms.Dispose();
        }
        catch (Exception)
        {
            throw;
        }
    }

}