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

namespace TPA.Templates.Writing
{
    /// <summary>
    /// Interaction logic for SummarizeText.xaml
    /// </summary>
    public partial class SummarizeText : UserControl,ISwitchable
    {
        SummarizeTextQuestion question;
        int MaxWordCount = 0;
        public SummarizeText()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            question = (SummarizeTextQuestion)state;
            txtBlkText.Text = question.Description.Replace("{newline}",string.Empty);
            txtBlkInstruction.Text = question.Instruction.Replace("{newline}",string.Empty);
            MaxWordCount = question.MaxWordCount;

            if (question.Mode == Mode.ANSWER_KEY || question.Mode==Mode.QUESTION || question.Mode==Mode.TIME_OUT)
            { 
                string[] arrayAnswers= question.CorrectAnswers;

                if (question.Mode == Mode.QUESTION || question.Mode == Mode.TIME_OUT)
                    arrayAnswers = question.UserAnswers;

                if (arrayAnswers.Any())
                    txtSummary.Text = arrayAnswers[0];

                if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
                    txtSummary.IsEnabled = false;
            }

            prevNext.QuestionContext = question;
            prevNext.PrevNextClicked += previousNext_PrevNextClicked;
            prevNext.YourResponseClicked += prevNext_YourResponseClicked;
            prevNext.QuestionTemplateKey = question.QuestionTemplate;
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.WRITING;

            lblWordCount.Content = "Total Word Count:" + (CommonUtilities.GetWordCount(txtSummary.Text)-1);
        }

        void prevNext_YourResponseClicked(object sender, Common.YourResponseEventArgs e)
        {
            string[] answers = e.ShowYourAnswer ? question.UserAnswers : question.CorrectAnswers;

            if (e.ShowYourAnswer)
            {
                txtSummary.Background = new SolidColorBrush(Colors.PaleGreen);
            }
            else
                txtSummary.Background = null;

            if (answers.Any())
                txtSummary.Text = answers[0];
            else
                txtSummary.Text = string.Empty;
        }

        void previousNext_PrevNextClicked(object sender, EventArgs e)
        {
            string answer = txtSummary.Text;
            AnswerManager.LogAnswer(question, answer , prevNext.GetAttemptTimeLeft());
        }

        private void txtSummary_TextChanged(object sender, TextChangedEventArgs e)
        {
            int wordCount = CommonUtilities.GetWordCount(txtSummary.Text);
            lblWordCount.Content = "Total Word Count:"+wordCount;
            if (wordCount >= MaxWordCount && question.Mode == Mode.QUESTION)
            {
                e.Handled = true;
                System.Windows.Forms.MessageBox.Show("Reached the maximum word limit set "+MaxWordCount+" for this question!");
            }
        }
    }
}
