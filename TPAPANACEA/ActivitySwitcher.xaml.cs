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

namespace TPA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ActivitySwitcher : Window
    {
        private void CenterAllignUserControl(UserControl userControl)
        {
            if (userControl.GetType() == typeof(HomePanacia) ||
                userControl.GetType() == typeof(Practice))
            {
                content.HorizontalContentAlignment = HorizontalAlignment.Center;
                content.VerticalContentAlignment = VerticalAlignment.Center;
            }
            else
            {
                content.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                content.VerticalContentAlignment = VerticalAlignment.Stretch;
            
            }

        }

        public ActivitySwitcher()
        {
            InitializeComponent();

            //txtBlockStatus.Text = Application.Current.FindResource("COPYRIGHT").ToString();
            txtBlockStatus.Text = string.Empty;
            txtBlockAppTitle.Text = Application.Current.FindResource("COMPANY_TITLE").ToString();

            Switcher.activitySwitcher = this;
            //Switcher.Switch(new Home());
            Switcher.Switch(new HomePanacia());
        }
        public void Navigate(UserControl nextPage)
        {
            //this.Content = nextPage;
            content.Content = nextPage;
            
            CenterAllignUserControl(nextPage);
            
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

            CenterAllignUserControl(nextPage);
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            WinForm.DialogResult result = WinForm.MessageBox.Show("Are you sure you want to exit?", "Are you sure?", 
                WinForm.MessageBoxButtons.YesNo, WinForm.MessageBoxIcon.Question);

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
            //Switcher.Switch(new Home());
            Switcher.Switch(new HomePanacia());
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    }
}
