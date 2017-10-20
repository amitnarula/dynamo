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
using System.Windows.Forms;
using TPAPathAbroad.FX;

namespace TPAPathAbroad.Templates.Common
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window,IDisposable
    {
        Window parentWindow = null;
        DialogResult result;
        MessageDialog m=null;
        public Dialog()
        {
            InitializeComponent();
            parentWindow = System.Windows.Application.Current.MainWindow;
            if (parentWindow != null)
            {
                parentWindow.Background = new SolidColorBrush(Colors.White);
                parentWindow.Background.Opacity = 0.9;
            }

        }

        void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            parentWindow.Background = new SolidColorBrush(Colors.White);
            
            this.Close();
        }

        public Dialog(MessageDialog mDialog)
        {
            m = mDialog;
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            parentWindow.Background = new SolidColorBrush(Colors.White);
            Close();
        }

        public void Dispose()
        {
            
        }
    }
}
