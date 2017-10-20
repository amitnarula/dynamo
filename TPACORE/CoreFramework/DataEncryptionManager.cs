using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace TPA.CoreFramework
{
    public class DataEncryptionManager
    {
        private static Stream GetResourceStream(string filename, string subFolder)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string resname = string.Empty;
            if (string.IsNullOrEmpty(subFolder))
                resname = asm.GetName().Name + ".Data." + filename;
            else
                resname = asm.GetName().Name + ".Data." + subFolder + "." + filename;
            return asm.GetManifestResourceStream(resname);

        }

        private static string GetTempFilePath(string fileName)
        {
            string tmpFile = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\" + fileName;
            return tmpFile;
        }

        public static string CreateTemporaryFile(string inputFileName, string subFolder, string outputFileName)
        {
            using (Stream stream = GetResourceStream(inputFileName, subFolder))
            {
                if (stream != null)
                {
                    string tmpDirectory = "Tmp";

                    var di = Directory.CreateDirectory(tmpDirectory);
                    FileReader.ProvideWriteAccessToFolder(tmpDirectory,true);
                    
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    string outputFile = System.IO.Path.Combine(tmpDirectory, outputFileName);

                    outputFileName = outputFile + System.IO.Path.GetExtension(inputFileName);

                    using (FileStream fs = File.Create(outputFileName))
                    {
                        // Initialize the bytes array with the stream length and then fill it with data
                        byte[] bytesInStream = new byte[stream.Length];
                        stream.Read(bytesInStream, 0, bytesInStream.Length);
                        // Use write method to write to the file specified above
                        fs.Write(bytesInStream, 0, bytesInStream.Length);
                    }
                }
            }

            return GetTempFilePath(outputFileName);
        }
        public static void RemoveTemp()
        {
            string tmpDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)+"\\Tmp";
            if (Directory.Exists(tmpDirectory))
                Directory.Delete(tmpDirectory, true);
        }

        public static string DecryptFile(string fullFilePath,string outputFileName)
        {
            if (!File.Exists(outputFileName))
            {
                string base64 = File.ReadAllText(fullFilePath);
                byte[] data = Convert.FromBase64String(base64);
                File.WriteAllBytes(outputFileName, data);
                
            }
            return outputFileName;
        }
    }
}
