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
using TPAPanacea.Templates.Common;
using WinForm = System.Windows.Forms;
using TPACORE.CoreFramework;

namespace TPA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ActivitySwitcher : Window
    {
        public string LoginInfo { get; set; }
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

        public void ShowLoginInfo(string info)
        {
            txtBlockLoginInfo.Visibility = Visibility.Visible;
            txtBlockLoginInfo.Text = "Welcome " + info;

            if (LoginManager.CheckIfTeacherLoggedIn())
            {
                ShowEvaluatorPermittedControls();
            }
        }

        private void ShowEvaluatorPermittedControls()
        {
            btnManageUsers.Visibility = Visibility.Visible;
            btnReports.Visibility = Visibility.Visible;
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var loginResult = TPACache.GetItem(TPACache.LOGIN_KEY);
            var userLoginInfo = TPACache.GetItem(TPACache.STUDENT_LOGIN_INFO) as User;
            if (loginResult != null)
            {
                txtBlockLoginInfo.Visibility = Visibility.Visible;
                //txtBlockLoginInfo.Text = "Welcome, Teacher";
                LoginInfo = "Teacher";

                var result = System.Windows.Forms.MessageBox.Show("Welcome Teacher, you are already logged in. Please press OK to logout", "Logout", System.Windows.Forms.MessageBoxButtons.OKCancel,
                         System.Windows.Forms.MessageBoxIcon.Information, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //perform logout
                    TPACache.RemoveItem(TPACache.LOGIN_KEY);
                    HideEvaluatorPermittedControls();
                }
            }
            else if (userLoginInfo != null) {
                txtBlockLoginInfo.Visibility = Visibility.Visible;
                txtBlockLoginInfo.Text = string.Format("Welcome {0},{1}", userLoginInfo.Firstname, userLoginInfo.Lastname);

                var result = System.Windows.Forms.MessageBox.Show(string.Format("Welcome student {0},{1}, you are already logged in. Please press OK to logout",userLoginInfo.Firstname,userLoginInfo.Lastname), "Logout", System.Windows.Forms.MessageBoxButtons.OKCancel,
                         System.Windows.Forms.MessageBoxIcon.Information, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //perform logout
                    TPACache.RemoveItem(TPACache.STUDENT_LOGIN_INFO);
                    HideEvaluatorPermittedControls();
                }
            }
            else
            {
                //Open login page
                Login lgn = new Login(this);
                lgn.ShowDialog();
            }
        }

        private void HideEvaluatorPermittedControls()
        {
            txtBlockLoginInfo.Visibility = Visibility.Collapsed;
            btnManageUsers.Visibility = Visibility.Collapsed;
            btnReports.Visibility = Visibility.Collapsed;
        }

        private void btnResult_Click(object sender, RoutedEventArgs e)
        {
            new Results().Show();
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            new Reports().ShowDialog();
        }

        private void BtnManageUsers_Click(object sender, RoutedEventArgs e)
        {
            new ManageUsers().ShowDialog();
        }
    }
}
