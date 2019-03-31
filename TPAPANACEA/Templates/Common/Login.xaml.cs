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
using TPACORE.CoreFramework;

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
            string teacherUsername = "evalteacher";
            string teacherPassword = "eval";
            var teacherLoginState = TPACache.GetItem(TPACache.LOGIN_KEY);
            var userLoginState = TPACache.GetItem(TPACache.STUDENT_LOGIN_INFO) as User;

            if (teacherLoginState == null) {
                if (txtUsername.Text.Equals(teacherUsername, StringComparison.InvariantCultureIgnoreCase) && passwordBox.Password.Equals(teacherPassword))
                {
                    System.Windows.Forms.MessageBox.Show("Login Successful, Welcome to evaluation mode.", "Login Successful", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information);
                    TPACache.SetItem(TPACache.LOGIN_KEY, new LoginState() { CurrentStatus = LoginStatus.OK }, null);
                    this.Close();
                    return;
                }
            }

            if (userLoginState == null) { 
                    //try student login
                var user = UserManager.GetUserByCredentials(txtUsername.Text, passwordBox.Password);
                if (user != null)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Login Successful, Welcome student {0},{1}.", user.Firstname, user.Lastname),
                        "Login Successful", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information);
                    //TPACache.SetItem(TPACache.LOGIN_KEY, new LoginState() { CurrentStatus = LoginStatus.OK }, new TimeSpan(0, 1, 0, 0, 0));
                    TPACache.SetItem(TPACache.STUDENT_LOGIN_INFO, user, null);

                    this.Close();
                    return;
                }
            
            }
            System.Windows.Forms.MessageBox.Show("Login Failed.");


        }

    }
}
