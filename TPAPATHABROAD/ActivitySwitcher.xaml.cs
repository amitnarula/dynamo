using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TPA.CoreFramework;
using TPA.Templates.Common;
using WinForm = System.Windows.Forms;
using TPAPathAbroad.FX;

namespace TPA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ActivitySwitcher : Window
    {
        private void DoAdjustments(UserControl userControl)
        {
            //if (userControl.GetType() == typeof(HomePanacia) ||
              //  userControl.GetType() == typeof(Practice))
            if(userControl.GetType() == typeof(Home) ||
                userControl.GetType() == typeof(Practice))
            {
                content.HorizontalContentAlignment = HorizontalAlignment.Center;
                content.VerticalContentAlignment = VerticalAlignment.Center;
            }
            else
            {
                content.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                content.VerticalContentAlignment = VerticalAlignment.Stretch;
                contentGrid.Background = null;
            
            }

        }

        private void SetApplicationInformation(UserControl userControl)
        {
            if (userControl.GetType() == typeof(Home))
            {
                //txtBlockStatus.Text = string.Empty;
                txtBlockAppTitle.Text = Application.Current.FindResource("COMPANY_TITLE").ToString();

            }
            else
                txtBlockAppTitle.Text = Application.Current.FindResource("PRACTICE_SET_TITLE").ToString();

            txtBlockStatus.Text = Application.Current.FindResource("COPYRIGHT").ToString();
                
        }

        public ActivitySwitcher()
        {
            InitializeComponent();

            Switcher.activitySwitcher = this;
            Switcher.Switch(new Home());
            //Switcher.Switch(new HomePanacia());
        }
        public void Navigate(UserControl nextPage)
        {
            //this.Content = nextPage;
            content.Content = nextPage;

            DoAdjustments(nextPage);

            SetApplicationInformation(nextPage);
            
        }

        public void Navigate(UserControl nextPage, object state)
        {
            //this.Content = nextPage;
            content.Content = nextPage;
            ISwitchable s = nextPage as ISwitchable;

            if (s != null)
                s.UtilizeState(state);
            else
                throw new ArgumentException("NextPage is not ISwitchable! "
                  + nextPage.Name.ToString());

            DoAdjustments(nextPage);

            SetApplicationInformation(nextPage);
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //WinForm.DialogResult result = WinForm.MessageBox.Show("Are you sure you want to exit?", "Are you sure?", 
              //  WinForm.MessageBoxButtons.YesNo, WinForm.MessageBoxIcon.Question);

            WinForm.DialogResult result = MessageDialog.Show("Do you really want to quit?", "", "Yes", "No");

            switch (result)
            {
                case System.Windows.Forms.DialogResult.Yes:
                    this.Close();
                    break;
                default:
                    break;
            }
            
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Home());
            //Switcher.Switch(new HomePanacia());
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.WindowState = WindowState.Maximized;
            var height = System.Windows.SystemParameters.WorkArea.Height;
            var width = System.Windows.SystemParameters.VirtualScreenWidth;

            var newHeight = height;//height - 90;
            var newWidth = width - 200;//width - 90;

            this.Height = newHeight;
            this.Width = newWidth;

            this.Left = (width - newWidth) / 2;
            ///this.Top = (height - newHeight) / 2;
            this.Top = 0;
            //this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

    }
}
