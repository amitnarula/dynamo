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
using TPACORE.CoreFramework;

namespace TPAPanacea.Templates.Common
{
    /// <summary>
    /// Interaction logic for ManageUsers.xaml
    /// </summary>
    public partial class ManageUsers : Window
    {
        enum ScreenMode
        {
            New=0,
            Edit=1
        }

        private User SelectedUser { get; set; }

        private ScreenMode Mode { get; set; }

        public ManageUsers()
        {
            InitializeComponent();
            this.Loaded += ManageUsers_Loaded;
        }

        private void ManageUsers_Loaded(object sender, RoutedEventArgs e)
        {
            BindUsers();
        }

        private void BindUsers()
        {
            lbxUsers.ItemsSource = UserManager.GetUsers();
            lbxUsers.DisplayMemberPath = "Firstname";
            lbxUsers.SelectedValuePath = "UserId";
        }

        private bool IsFormValid()
        {
            if(string.IsNullOrEmpty(txtUsername.Text.Trim())||
                string.IsNullOrEmpty(txtPassword.Text.Trim()) ||
                string.IsNullOrEmpty(txtFirstname.Text.Trim()) ||
                string.IsNullOrEmpty(txtLastname.Text.Trim()))
            {
                System.Windows.Forms.MessageBox.Show("Please enter valid data.");
                return false;
                
            }
            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SelectedUser = null;
            this.Close();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            string username = "student" + (lbxUsers.Items.Count + 1);
            Clear(username);
        }

        private void Clear(string username)
        {
            SelectedUser = null;
            this.Mode = ScreenMode.New;
            txtUsername.Text = username;
            txtPassword.Text = string.Empty;
            txtFirstname.Text = string.Empty;
            txtLastname.Text = string.Empty;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
                if (this.Mode == ScreenMode.New)
                {
                    string message = UserManager.CreateUser(new User()
                    {
                        ContactNo = "9999999990",
                        Email = "user@user.com",
                        Firstname = txtFirstname.Text,
                        Lastname = txtLastname.Text,
                        Password = txtPassword.Text,
                        UserId = UserManager.RandomString(5),
                        Username = txtUsername.Text
                    });
                    if(message == "created")
                    {
                        System.Windows.Forms.MessageBox.Show("User created successfully");

                    }
                    else if(message=="limit exceeded")
                    {
                        System.Windows.Forms.MessageBox.Show("Max limit for user is 15");
                    }
                    
                }
                else
                {
                    string message = UserManager.UpdateUser(new User()
                    {
                        ContactNo = "9999999990",
                        Email = "user@user.com",
                        Firstname = txtFirstname.Text,
                        Lastname = txtLastname.Text,
                        Password = txtPassword.Text,
                        UserId = SelectedUser.UserId,
                        Username = txtUsername.Text
                    });
                    System.Windows.Forms.MessageBox.Show("User updated successfully");
                }
                Clear(string.Empty);
                BindUsers();
            }
        }
        
        private void lbxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListBox)sender;
            if (item != null)
            {
                var user = (User)item.SelectedItem;
                if (user != null)
                {
                    this.Mode = ScreenMode.Edit;
                    SelectedUser = user;
                    txtFirstname.Text = user.Firstname;
                    txtPassword.Text = user.Password;
                    txtLastname.Text = user.Lastname;
                    txtUsername.Text = user.Username;
                }
            }
        }
        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedUser != null)
            {
                var result = System.Windows.Forms.MessageBox.Show("Do you want to delete the user","Delete user",System.Windows.Forms.MessageBoxButtons.OKCancel);

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    UserManager.DeleteUser(SelectedUser.UserId);
                    SelectedUser = null;
                    Clear(string.Empty);
                    BindUsers();
                }
            }
        }
    }
}
