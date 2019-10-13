using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace TPA.CoreFramework
{
    public class MediaReader
    {
        public static string GetMediaPath(string mediaFileName)
        {
            string mediaPath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)
             + "//Data//ENHAVO//" + mediaFileName;

            return ResolveMedia(mediaFileName, mediaPath);

            /*return DataEncryptionManager.CreateTemporaryFile(mediaFileName, string.Empty, mediaFileName);//Data Encryption manager in place*/

        }

        private static string ResolveMedia(string mediaFileName, string mediaPath)
        {
            string decryptedMediaPath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)
             + "//Data//Temp//9b6fca36-b3c9-44a1-b050-228532d08094//ade9eacf-c059-4803-a005-3e38d9292b58//600fe142-3e5d-4890-8eda-ead2e53eeed6//"
             + Path.GetFileNameWithoutExtension(mediaFileName);

            //var result = Assembly.GetExecutingAssembly().GetManifestResourceInfo("TPA.Data." + mediaFileName);

            if (mediaFileName.IndexOf(".tpi") > 0) //Encryption in place
            {
                //Decrypt & Save the file temporary
                return DataEncryptionManager.DecryptFile(mediaPath, decryptedMediaPath + ".jpg");

            }
            else if (mediaFileName.IndexOf(".tpm") > 0)//Encryption in place
            {
                //Decrypt & Save the file temporary
                return DataEncryptionManager.DecryptFile(mediaPath, decryptedMediaPath + ".mp3");

            }
            else
            {
                return mediaPath; //A plain path
            }
        }

        public static string GetResourcePath(string mediaFileName)
        {
            string mediaPath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)
             + "//Resources//" + mediaFileName;

            return mediaPath;
        }

        public static string GetTempMediaPath(string mediaFileName)
        {
            //string mediaPath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)
            // + "//Data//Temp//" + mediaFileName;

            // mediaPath = Path.Combine(mediaPath, CommonUtilities.ResolveTargetUserFolder());

            string mediaPath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)
             + "//Data//Temp//" + CommonUtilities.ResolveTargetUserFolder();

            mediaPath = Path.Combine(mediaPath, mediaFileName);

            //var result = Assembly.GetExecutingAssembly().GetManifestResourceInfo("TPA.Data." + mediaFileName);

            return mediaPath;

        }

        public static string GetMediaPath(string mediaFileName, string appendCustomFolder)
        {
            string mediaPath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)
             + "//Data//ENHAVO//" + appendCustomFolder + "//" + mediaFileName;

            //var result = Assembly.GetExecutingAssembly().GetManifestResourceInfo("TPA.Data." + mediaFileName);

            return ResolveMedia(mediaFileName,mediaPath);

            /*Data Encryption manager in place return DataEncryptionManager.CreateTemporaryFile(mediaFileName, appendCustomFolder, mediaFileName);*/

        }

        public static string GetOutputFileName(string fileName, string fileExtension)
        {
            string outputFileDirectory = Path.Combine(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)
             + "//Data//Temp//", CommonUtilities.ResolveTargetUserFolder());
            string outFilePutPath =  Path.Combine(outputFileDirectory, fileName + "." + fileExtension);

            return outFilePutPath;

        }
    }
}
