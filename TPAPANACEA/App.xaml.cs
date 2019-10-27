using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using TPALM;
using TPACORE.CoreFramework;

namespace TPA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        FrmActivate activateApplicationForm = null;
        public App()
        {
            string appName = "TPAV3"; //PTE Panacea App evaluation name alias is : TPAV2

            // Add the event handler for handling UI thread exceptions to the event.
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            //for DEMO build
            if (!CommonUtility.ValidateSoftware(appName))
            {
                activateApplicationForm = new FrmActivate();
                activateApplicationForm.AppName = appName;
                activateApplicationForm.ShowDialog();
            }
            else
            {
                //this.StartupUri = new Uri("Splash.xaml", UriKind.RelativeOrAbsolute);
                LogManager.WriteLog(LogManager.LogType.INFO, "Application initialized successfully");
                this.StartupUri = new Uri("SplashPanacia.xaml", UriKind.RelativeOrAbsolute);//FISHY
                //System.Windows.Forms.MessageBox.Show("Application is no longer compatible with the current architecture of OS.",
                //  "System Error");
            }

        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("An error was encountered while performing the operation"+
                "\nWe have logged the error for more details and we hope to get it resolved soon.");
            //Log errors
            LogManager.WriteLog(LogManager.LogType.ERROR, e.Exception);
        }

        ~App()
        {
            if (activateApplicationForm != null)
                activateApplicationForm.Dispose();
        }

    }
}
