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
             + "//Data//" + mediaFileName;

            var result = Assembly.GetExecutingAssembly().GetManifestResourceInfo("TPA.Data." + mediaFileName);
            
            return mediaPath;

            /*return DataEncryptionManager.CreateTemporaryFile(mediaFileName, string.Empty, mediaFileName);Data Encryption manager in place*/

        }

        public static string GetMediaPath(string mediaFileName, string appendCustomFolder)
        {
            string mediaPath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)
             + "//Data//" + appendCustomFolder + "//" + mediaFileName;

            var result = Assembly.GetExecutingAssembly().GetManifestResourceInfo("TPA.Data." + mediaFileName);

            return mediaPath;

            /*Data Encryption manager in place return DataEncryptionManager.CreateTemporaryFile(mediaFileName, appendCustomFolder, mediaFileName);*/

        }

        public static string GetOutputFileName(string fileName, string fileExtension)
        {
            string outFilePutPath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)
             + "//Data//" + fileName + "." + fileExtension;

            return outFilePutPath;

        }
    }
}
