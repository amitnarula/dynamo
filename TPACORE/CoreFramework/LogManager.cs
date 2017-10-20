using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TPACORE.CoreFramework
{
    public class LogManager
    {
        private static string LogFileDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Logs//";
        public enum LogType
        {
            INFO,
            ERROR,
            WARNING,
            DEBUG
        }

        public static void WriteLog(LogType logType, Exception e)
        {
            string file = string.Format("appLog-{0:yyyy-MM-dd_hh-mm-ss-tt}.log", DateTime.Now);
            string logFile = Path.Combine(LogFileDirectory, file);

            if (!File.Exists(logFile))
            {
                if (e != null)
                {
                    File.WriteAllText(logFile, logType.ToString() + ":>" +
                        e.ToString());
                }
            }
        }

        public static void WriteLog(LogType logType, string message)
        {
            //string fileName = Path.Combine(LogFileDirectory, "appStart.log");
            //File.CreateText(fileName).Write(logType.ToString() + ":>" +message);
            
        }
    }
}
