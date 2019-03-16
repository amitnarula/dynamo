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
    public partial class HomePanacia : UserControl, ISwitchable
    {
        public HomePanacia()
        {
            InitializeComponent();
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
                TestMode = TestMode.Mock
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
    }
}
