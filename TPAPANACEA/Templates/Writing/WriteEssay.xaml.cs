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
using TPA.Templates.Common;

namespace TPA.Templates.Writing
{
    /// <summary>
    /// Interaction logic for WriteEssay.xaml
    /// </summary>
    public partial class WriteEssay : UserControl,ISwitchable
    {
        WriteEssayQuestion question;
        int MaxWordCount = 0;
        public WriteEssay()
        {
            InitializeComponent();
            this.Loaded += WriteEssay_Loaded;
        }
        
        private void WriteEssay_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtEssay);
        }

        public void UtilizeState(object state)
        {
            question = (WriteEssayQuestion)state;
            txtBlkText.Text = question.Description.Trim().Replace("{newline}", string.Empty);
            txtBlkInstruction.Text = question.Instruction.Replace("{newline}", string.Empty);
            MaxWordCount = question.MaxWordCount;

            if (question.Mode == Mode.ANSWER_KEY || question.Mode==Mode.QUESTION || question.Mode==Mode.TIME_OUT)
            {
                string[] arrayAnswers = question.CorrectAnswers;

                if (question.Mode == Mode.QUESTION || question.Mode == Mode.TIME_OUT)
                    arrayAnswers = question.UserAnswers;

                if (arrayAnswers.Any())
                    txtEssay.Text = arrayAnswers[0];

                if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
                    txtEssay.IsEnabled = false;
            }

            prevNext.QuestionContext = question;
            prevNext.PrevNextClicked += previousNext_PrevNextClicked;
            prevNext.YourResponseClicked+=prevNext_YourResponseClicked;
            prevNext.QuestionTemplateKey = question.QuestionTemplate;
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.WRITING;

            lblWordCount.Content = "Total Word Count:" + (CommonUtilities.GetWordCount(txtEssay.Text) - 1);
        }

        void prevNext_YourResponseClicked(object sender, YourResponseEventArgs e)
        {
            string[] answers = e.ShowYourAnswer ? question.UserAnswers : question.CorrectAnswers;

            if (e.ShowYourAnswer)
            {
                txtEssay.Background = new SolidColorBrush(Colors.PaleGreen);
            }
            else
                txtEssay.Background = null;

            if (answers.Any())
                txtEssay.Text = answers[0];
            else
                txtEssay.Text = string.Empty;
        }

        void previousNext_PrevNextClicked(object sender, EventArgs e)
        {
            string answer = txtEssay.Text;
            AnswerManager.LogAnswer(question, answer, prevNext.GetAttemptTimeLeft());
        }

        private void txtEssay_TextChanged(object sender, TextChangedEventArgs e)
        {
            int wordCount = CommonUtilities.GetWordCount(txtEssay.Text);
            lblWordCount.Content = "Total Word Count:" + wordCount;
            if (wordCount >= MaxWordCount && question.Mode == Mode.QUESTION)
            {
                e.Handled = true;
                System.Windows.Forms.MessageBox.Show("Reached the maximum word limit "+MaxWordCount+" for this question!");
            }
        }
    }
}
 