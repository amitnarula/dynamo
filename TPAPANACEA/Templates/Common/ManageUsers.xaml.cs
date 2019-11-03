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
using WinForms = System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace TPAPanacea.Templates.Common
{
    /// <summary>
    /// Interaction logic for ManageUsers.xaml
    /// </summary>
    public partial class ManageUsers : Window
    {
        static string baseOutputDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Data//Temp//";

        enum ScreenMode
        {
            New = 0,
            Edit = 1
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
            dpDOB.SelectedDateFormat = DatePickerFormat.Short;
            btnImage.IsEnabled = false;
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
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()) ||
                string.IsNullOrEmpty(txtPassword.Text.Trim()) ||
                string.IsNullOrEmpty(txtFirstname.Text.Trim()) ||
                string.IsNullOrEmpty(txtLastname.Text.Trim()) ||
                string.IsNullOrEmpty(txtEmail.Text.Trim()) ||
                string.IsNullOrEmpty(txtContact.Text.Trim()) ||
                string.IsNullOrEmpty(dpDOB.Text.Trim()) ||
                !this.IsDOBValid(dpDOB.Text.Trim()))
            {
                System.Windows.Forms.MessageBox.Show("Please enter valid data.");
                return false;

            }
            return true;
        }

        private bool IsDOBValid(string selectedDate)
        {
            DateTime dtOut;

            return DateTime.TryParse(selectedDate, out dtOut);

        }


        private BitmapImage GetBitmapImage(string userId)
        {
            string userimg = System.IO.Path.Combine(baseOutputDirectory, userId, userId + ".jpg");
            userimg = !string.IsNullOrEmpty(userId) && File.Exists(userimg) ? userimg : 
                System.IO.Path.Combine(baseOutputDirectory, "noimage.jpg");

            BitmapImage imgBm = null;

            using (Stream fs = new FileStream(userimg,FileMode.Open))
            {
                // Bitmap bm = new Bitmap(fs);
                // MemoryStream ms = new MemoryStream();
                
                // bm.Save(ms,System.Drawing.Imaging.ImageFormat.Jpeg);
                // fs.Seek(0, SeekOrigin.Begin);
                imgBm = new BitmapImage();
                imgBm.BeginInit();
                imgBm.CacheOption = BitmapCacheOption.OnLoad;
                // ms.Seek(0, SeekOrigin.Begin);
                imgBm.StreamSource = fs;
                imgBm.EndInit();
                fs.Close();

                //bm.Dispose();
                //ms.Close();

            }


            return imgBm;
        }

        private void BindPhoto(string userId)
        {
            
            BitmapImage bm = GetBitmapImage(userId);
            
            imgPhoto.Source = bm;


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
            txtContact.Text = txtEmail.Text = dpDOB.Text = string.Empty;
            dpDOB.SelectedDate = DateTime.Today;
            BindPhoto(username);
            btnImage.IsEnabled = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
                if (this.Mode == ScreenMode.New)
                {
                    string message = UserManager.CreateUser(new User()
                    {
                        ContactNo = txtContact.Text,
                        Email = txtEmail.Text,
                        DOB = dpDOB.SelectedDate.Value.ToString("MM/dd/yyyy"),
                        Firstname = txtFirstname.Text,
                        Lastname = txtLastname.Text,
                        Password = txtPassword.Text,
                        UserId = UserManager.RandomString(5),
                        Username = txtUsername.Text
                    });
                    if (message == "created")
                    {
                        System.Windows.Forms.MessageBox.Show("User created successfully, select user again to add photo.");
                        
                    }
                    else if (message == "limit exceeded")
                    {
                        System.Windows.Forms.MessageBox.Show("Max limit for user is 15");
                    }

                }
                else
                {
                    string message = UserManager.UpdateUser(new User()
                    {
                        ContactNo = txtContact.Text,
                        Email = txtEmail.Text,
                        DOB = dpDOB.SelectedDate.Value.ToString("MM/dd/yyyy"),
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
                DateTime dtDOB;

                var user = (User)item.SelectedItem;
                if (user != null)
                {
                    if (!DateTime.TryParse(user.DOB, out dtDOB))
                    {
                        dtDOB = DateTime.Today;
                    }

                    this.Mode = ScreenMode.Edit;
                    SelectedUser = user;
                    txtFirstname.Text = user.Firstname;
                    txtPassword.Text = user.Password;
                    txtLastname.Text = user.Lastname;
                    txtUsername.Text = user.Username;
                    txtContact.Text = user.ContactNo;
                    txtEmail.Text = user.Email;
                    dpDOB.SelectedDate = dtDOB;
                    BindPhoto(user.UserId);
                    btnImage.IsEnabled = true;
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUser != null)
            {
                var result = System.Windows.Forms.MessageBox.Show("Do you want to delete the user", "Delete user", System.Windows.Forms.MessageBoxButtons.OKCancel);

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    UserManager.DeleteUser(SelectedUser.UserId);
                    SelectedUser = null;
                    Clear(string.Empty);
                    BindUsers();
                }
            }
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            using (WinForms.OpenFileDialog dialog = new WinForms.OpenFileDialog())
            {
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
                if (WinForms.DialogResult.OK == dialog.ShowDialog())
                {
                    if (SelectedUser != null) {
                            string destFileName = System.IO.Path.Combine(baseOutputDirectory, SelectedUser.UserId, SelectedUser.UserId + ".jpg");
                    
                        using (Stream fs = new FileStream(dialog.FileName, FileMode.Open))
                        {
                            BitmapImage imgBm = new BitmapImage();
                            imgBm.BeginInit();
                            imgBm.CacheOption = BitmapCacheOption.OnLoad;
                            fs.Seek(0, SeekOrigin.Begin);
                            imgBm.StreamSource = fs;
                            imgBm.EndInit();
                            using (Stream fsUserImage = File.Create(destFileName)) {
                                fs.Seek(0, SeekOrigin.Begin);
                                fs.CopyTo(fsUserImage);
                                
                            }
                            
                            imgPhoto.Source = imgBm;
                            
                        }
                    }
                        
                    

                    
                }
            }

        }
    }
}
