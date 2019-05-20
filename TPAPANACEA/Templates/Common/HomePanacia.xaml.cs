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
using TPAPanacea.Templates.Common;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class HomePanacia : UserControl, ISwitchable
    {
        public HomePanacia()
        {
            InitializeComponent();
            this.Loaded += HomePanacia_Loaded;
        }

        private void HomePanacia_Loaded(object sender, RoutedEventArgs e)
        {
            //Login lgn = new Login();
            //lgn.ShowDialog();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void btnPractice_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Practice(), new ModeSetting()
            {
            });
        }

        private void btnAnswerKey_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Practice(), new ModeSetting()
            {
                QuestionMode = Mode.ANSWER_KEY,
                TestMode = TestMode.Practice
            });
        }

        private void btnMock_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Practice(), new ModeSetting()
            {
                QuestionMode = Mode.QUESTION,
                TestMode = TestMode.Mock
            });
        }
        
        private void btnMockAnswerKey_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Practice(), new ModeSetting()
            {
                QuestionMode = Mode.ANSWER_KEY,
                TestMode = TestMode.Mock
            });
        }
    }
}
