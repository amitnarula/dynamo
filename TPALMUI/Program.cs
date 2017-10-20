using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TPALM;

namespace TPALMUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!CommonUtility.ValidateSoftware())
                Application.Run(new frmActivate());
            else
                Application.Run(new frmMain());
        }
    }
}
