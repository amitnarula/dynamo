using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TPA
{
	/// <summary>
	/// Interaction logic for Splash.xaml
	/// </summary>
	public partial class SplashPanacia : Window
	{
        int interval = 0;
        #if debug
            interval = 0; 
        else
            interval = 1
        #endif

        int numberOfTicks = 0;
        DispatcherTimer timer;
		public SplashPanacia()
		{
			this.InitializeComponent();

            //Validate Software and licensing part

            //Then move ahead
            txtBlockAppName.Text = Application.Current.FindResource("COMPANY_TITLE").ToString();
            //txtBlockStatus.Text = Application.Current.FindResource("COPYRIGHT").ToString();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, interval);
            timer.Tick+=timer_Tick;
            timer.Start();
			// Insert code required on object creation below this point.
		}

        void timer_Tick(object sender, EventArgs e)
        {
            numberOfTicks++;

            string currentStatus = string.Empty;

            if (numberOfTicks == 1)
            {
                currentStatus = "Loading modules, Please wait.";
            }
            else if (numberOfTicks == 2)
            {
                currentStatus = "Loading questions, Please wait.";
            }
            else if (numberOfTicks == 3)
            {
                currentStatus = "Initializing, Please wait.";
            }
            else if (numberOfTicks == 4)
            {
                currentStatus = "Loading application, Please wait.";
                timer.Stop();
                new ActivitySwitcher().Show();
                Close();
            }

            lblStatus.Content = currentStatus;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
	}
}