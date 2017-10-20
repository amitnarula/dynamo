using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using TPALM;
using TPACORE.CoreFramework;
using TPAPathAbroad.FX;

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
            string appName = "PAS"; //PTE Path abroad App name alias is : PAS

            // Add the event handler for handling UI thread exceptions to the event.
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            
            //Uri uri = new Uri("PresentationFramework.aero;component\\themes/aero.normalcolor.xaml"
              //  , UriKind.Relative);
            //aero2 is required
            //Uri uri = new Uri("PresentationFramework.Zune;V3.0.0.0;31bf3856ad364e35;component/themes/Zune.NormalColor.xaml", UriKind.Relative);

            //Resources.MergedDictionaries.Add(Application.LoadComponent(uri) as ResourceDictionary);

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
                this.StartupUri = new Uri("Splash.xaml", UriKind.RelativeOrAbsolute); //FISHY
                //System.Windows.Forms.MessageBox.Show("Application is no longer compatible with the current architecture of OS.",
                //  "System Error");
            }
            
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("An error was encountered while performing the operation"+
              //  "\nWe have logged the error for more details and we hope to get it resolved soon.");
            MessageDialog.Show("Error logged", "An error was encountered while performing the operation" +
                "\nWe have logged the error for more details and we hope to get it resolved soon.", "Ok", "Cancel");
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
