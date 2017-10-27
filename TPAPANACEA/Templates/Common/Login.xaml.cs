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
using System.Windows.Shapes;
using TPA.CoreFramework;

namespace TPAPanacea.Templates.Common
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        
        public Login()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //Perform login and save the login info
            string username = "evalteacher";
            string password = "eval";
            var loginState = TPACache.GetItem(TPACache.LOGIN_KEY);
            
            if (loginState == null)
            {
                if (txtUsername.Text.Equals(username, StringComparison.InvariantCultureIgnoreCase) && passwordBox.Password.Equals(password))
                {
                    System.Windows.Forms.MessageBox.Show("Login Successful, Welcome to evaluation mode.","Login Successful",System.Windows.Forms.MessageBoxButtons.OKCancel,System.Windows.Forms.MessageBoxIcon.Information);
                    TPACache.SetItem(TPACache.LOGIN_KEY, new LoginState() { CurrentStatus = LoginStatus.OK }, new TimeSpan(0, 1, 0, 0, 0));
                    this.Close();
                }
                else
                    System.Windows.Forms.MessageBox.Show("Login Failed.");
            }

        }

    }
}
