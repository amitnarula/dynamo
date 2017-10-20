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
using TPA.Entities;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl, ISwitchable
    {
        public Home()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void btnPractice_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Practice());
        }

        private void btnAnswerKey_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Practice(), Mode.ANSWER_KEY);
        }
    }
}
