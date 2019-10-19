﻿using System;
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
using TPACORE.CoreFramework;
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
            if (!LoginManager.CheckIfAnyUserLoggedIn())
            {
                System.Windows.Forms.MessageBox.Show("You are not logged in, please login to continue.");
                return;
            }

            Switcher.Switch(new Practice(), new ModeSetting()
            {
            });
        }

        private void btnAnswerKey_Click(object sender, RoutedEventArgs e)
        {
            if (!LoginManager.CheckIfAnyUserLoggedIn())
            {
                System.Windows.Forms.MessageBox.Show("You are not logged in, please login to continue.");
                return;
            }
            Switcher.Switch(new Practice(), new ModeSetting()
            {
                QuestionMode = Mode.ANSWER_KEY,
                TestMode = TestMode.Practice,
                SetMode = Mode.ANSWER_KEY
            });
        }

        private void btnMock_Click(object sender, RoutedEventArgs e)
        {
            if (!LoginManager.CheckIfAnyUserLoggedIn())
            {
                System.Windows.Forms.MessageBox.Show("You are not logged in, please login to continue.");
                return;
            }
            Switcher.Switch(new Practice(), new ModeSetting()
            {
                QuestionMode = Mode.QUESTION,
                TestMode = TestMode.Mock,
                SetMode = Mode.QUESTION
            });
        }
        
        private void btnMockAnswerKey_Click(object sender, RoutedEventArgs e)
        {
            if (!LoginManager.CheckIfAnyUserLoggedIn())
            {
                System.Windows.Forms.MessageBox.Show("You are not logged in, please login to continue.");
                return;
            }

            if (LoginManager.CheckIfTeacherLoggedIn() && !LoginManager.CheckIfStudentToEvaluateSet()) {
                EvaluatingStudent studentSetDialog = new EvaluatingStudent();
                if (!studentSetDialog.ShowDialog().Value)
                    return;
            }

            Switcher.Switch(new Practice(), new ModeSetting()
            {
                QuestionMode = Mode.ANSWER_KEY,
                TestMode = TestMode.Mock,
                SetMode = Mode.ANSWER_KEY
            });
        }
    }
}
