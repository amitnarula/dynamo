using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TPALM
{
    public class CommonUtility
    {
        // This constant is used to determine the keysize of the encryption algorithm
        //TODO : Move to encryption
        private static string phrase = "3hz7FnN8O5";
        private static string initVector = "pemgail9uzpgzl88";
        private static string padString = "KnOlZCEa1oRe2u9OA6smSUe3MctIq9bnSoUYWbYE8hJ63t3SXnUoo7UwPtLTAlb";
        private static string licenseFilePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Data//PASLicense.lic";

        private const int keysize = 256;

        private static bool CheckLicenseValidity(string appName)
        {
            string keyCode = string.Empty;
            if (ValidateLicenseFile(licenseFilePath, appName, out keyCode))
            {
                var validator = new SKGL.Validate();
                validator.Key = keyCode;
                validator.secretPhase = phrase;

                if (validator.IsValid &&
                    !validator.IsExpired &&
                    validator.SetTime >= validator.DaysLeft 
                    &&
                    validator.IsOnRightMachine
                    )
                    return true;
                return false;
            }
            else
                return false;
        }

        private static string GenerateMachineCode()
        { 
            var generator = new SKGL.Generate();
            return EncryptString(Convert.ToString(generator.MachineCode));
        }

        private static string GenerateUID(string appName)
        {
            return HardwareInfo.GenerateUID(appName);
        }

        private static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA512.Create();  //or use SHA1.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private static bool ValidateLicenseFile(string path, string appName, out string keyCode)
        {
            keyCode = string.Empty;
            try
            {
                if (!File.Exists(path))
                    return false;

                DataSet ds = new DataSet();
                ds.ReadXml(path);

                if (ds.Tables != null && ds.Tables[0] != null)
                {
                    DataTable dt = ds.Tables[0];
                    var licenseHash = Convert.ToString(dt.Rows[0]["hash"]);

                    string firstName = Convert.ToString(dt.Rows[0]["firstName"]);
                    string lastName = Convert.ToString(dt.Rows[0]["lastName"]);
                    string licenseKey = Convert.ToString(dt.Rows[0]["licenseKey"]);
                    string uid = Convert.ToString(dt.Rows[0]["uid"]);
                    string mac = Convert.ToString(dt.Rows[0]["macid"]);
                    //DateTime creationDate = Convert.ToDateTime(dt.Rows[0]["date"]);

                    StringBuilder builder = new StringBuilder();
                    builder.Append(mac); //MAC
                    builder.Append(uid); //UID
                    builder.Append(licenseKey); //LicenseKey
                    builder.Append(firstName); //First name
                    builder.Append(lastName); //Last name
                    ///builder.Append(creationDate.ToString("dd/MM/yyyy")); //Creation Date
                    builder.Append(phrase); //Phrase

                    string generatedHash = GetHashString(builder.ToString());

                    if (licenseHash.Equals(generatedHash, StringComparison.InvariantCultureIgnoreCase)
                        && uid.Equals(GetProductUID(appName),StringComparison.InvariantCultureIgnoreCase))
                    {
                        keyCode = licenseKey;
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        //Encrypt
        private static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        //Decrypt
        private static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public static string DecryptString(string encryptedString)
        { 
            return DecryptString(encryptedString, phrase);
        }

        public static string EncryptString(string input)
        {
            return EncryptString(input, phrase);
        }

        public static bool ValidateSoftware(string appName)
        {
            return CheckLicenseValidity(appName);
        }
        
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(padString);

            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string GetMachineCode()
        {
            return GenerateMachineCode();
        }
        public static string GetProductUID(string appName)
        {
            return GenerateUID(appName);
        }
        
    }
}
