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
    /// Interaction logic for EvaluatingStudent.xaml
    /// </summary>
    public partial class EvaluatingStudent : Window
    {
        public EvaluatingStudent()
        {
            InitializeComponent();
            this.Loaded += EvaluatingStudent_Loaded;
        }

        private void EvaluatingStudent_Loaded(object sender, RoutedEventArgs e)
        {
            cmbUsers.ItemsSource = UserManager.GetUsers();
            cmbUsers.SelectedValuePath = "UserId";
            cmbUsers.DisplayMemberPath = "Firstname";
        }

        private void cmbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbUsers.SelectedValue.ToString()))
            {
                this.DialogResult = true;
                User selectedUser = cmbUsers.SelectedItem as User;
                TPACache.SetItem(TPACache.STUDENT_ID_TO_EVALUATE, selectedUser, null);
                System.Windows.Forms.MessageBox.Show("You are now evaluating : " + selectedUser.Firstname + "," + selectedUser.Lastname);
                this.Close();
            }
            
        }
    }
}
