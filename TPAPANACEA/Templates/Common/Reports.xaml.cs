using System;
using System.Collections.Generic;
using System.Data;
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
using TPA.Entities;
using TPACORE.CoreFramework;

namespace TPAPanacea.Templates.Common
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        public Reports()
        {
            InitializeComponent();
            this.Loaded += Reports_Loaded;
        }

        private void Reports_Loaded(object sender, RoutedEventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            List<ReportType> reportTypes = new List<ReportType>();
            reportTypes.Add(new ReportType()
            {
                ReportName = "Time analysis",
                TypeValue = "1"
            });
            reportTypes.Add(new ReportType()
            {
                ReportName = "Score analysis",
                TypeValue = "1"
            });

            cmbType.ItemsSource = reportTypes;
            cmbType.DisplayMemberPath = "ReportName";
            cmbType.SelectedValuePath = "TypeValue";

            List<Set> sets = new List<Set>();
            sets.Add(new Set()
            {
                Id = "94fbb82b4fd1485eb61cc3c671da384d",
                Name = "Practice set 1"
            });
            sets.Add(new Set()
            {
                Id = "7fdd81499d6147a1b4a817e56db88024",
                Name = "Practice set 2"
            });
            sets.Add(new Set()
            {
                Id = "88f631b360734b7da3b7c3e6151bd840",
                Name = "Practice set 3"
            });
            sets.Add(new Set()
            {
                Id = "7b04cda952c144788c9d27aa2d5dc710",
                Name = "Practice set 4"
            });
            sets.Add(new Set()
            {
                Id = "d9d885229cf64aceb84b19095b1ffd9c",
                Name = "Practice set 5"
            });
            sets.Add(new Set()
            {
                Id = "94fbb82b4fd1485eb61cc3c671da384d",
                Name = "Mock set 1"
            });
            sets.Add(new Set()
            {
                Id = "7fdd81499d6147a1b4a817e56db88024",
                Name = "Mock set 2"
            });
            sets.Add(new Set()
            {
                Id = "88f631b360734b7da3b7c3e6151bd840",
                Name = "Mock set 3"
            });
            sets.Add(new Set()
            {
                Id = "7b04cda952c144788c9d27aa2d5dc710",
                Name = "Mock set 4"
            });
            sets.Add(new Set()
            {
                Id = "d9d885229cf64aceb84b19095b1ffd9c",
                Name = "Mock set 5"
            });

            cmbSet.ItemsSource = sets;
            cmbSet.DisplayMemberPath = "Name";
            cmbSet.SelectedValuePath = "Id";

            List<User> users = UserManager.GetUsers();
            cmbUsers.ItemsSource = users;
            cmbUsers.SelectedValuePath = "UserId";
            cmbUsers.DisplayMemberPath = "Firstname";


        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (cmbType.SelectedValue==null || cmbSet.SelectedValue == null || cmbUsers.SelectedValue==null)
            {
                System.Windows.Forms.MessageBox.Show("Please select all combinations");
                return;
            }
            //start generating reports
        }

        private void CmbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmbSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    class ReportType
    {
        public string ReportName { get; set; }
        public string TypeValue { get; set; }
    }
}
