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

namespace TPA.Templates.Listening
{
    /// <summary>
    /// Interaction logic for ListenAndWrite.xaml
    /// </summary>
    public partial class ListenAndWrite : UserControl,ISwitchable
    {
        ListenAndWriteQuestion question;
        int MaxWordCount = 0;
        public ListenAndWrite()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            question = (ListenAndWriteQuestion)state;
            txtBlkInstruction.Text = question.Instruction.Trim().Replace("{newline}",string.Empty);
            MaxWordCount = question.MaxWordCount;
            
            if (question.Mode == Mode.QUESTION)
            {
                audioPlayer.Delay = question.Delay == 0 ? 1 : question.Delay;
                audioPlayer.Media = question.Media;
            }
            if (question.Mode == Mode.ANSWER_KEY || question.Mode==Mode.QUESTION || question.Mode==Mode.TIME_OUT)
            { 
                string[] answerArray = question.CorrectAnswers;

                if (question.Mode == Mode.QUESTION || question.Mode == Mode.TIME_OUT)
                    answerArray = question.UserAnswers;

                if (answerArray.Any())
                {
                    txtArea.Text = answerArray[0];
                }

                if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
                    txtArea.IsEnabled = false;
            }

            prevNext.QuestionContext = question;
            prevNext.PrevNextClicked += prevNext_PrevNextClicked;
            prevNext.YourResponseClicked += new Common.PreviousNext.YourResponseEventHandler(prevNext_YourResponseClicked);
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.LISTENING;

            lblWordCount.Content = "Total Word Count:" + (CommonUtilities.GetWordCount(txtArea.Text) - 1);
        }

        void prevNext_YourResponseClicked(object sender, Common.YourResponseEventArgs e)
        {
            string[] answers = e.ShowYourAnswer ? question.UserAnswers : question.CorrectAnswers;

            if (e.ShowYourAnswer)
            {
                txtArea.Background = new SolidColorBrush(Colors.PaleGreen);
            }
            else
                txtArea.Background = null;

            if (answers.Any())
                txtArea.Text = answers[0];
            else
                txtArea.Text = string.Empty;
        }

        void prevNext_PrevNextClicked(object sender, EventArgs e)
        {
            string answerString = txtArea.Text;
            AnswerManager.LogAnswer(question, answerString,prevNext.GetAttemptTimeLeft());
        }
        private void txtArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            int wordCount = CommonUtilities.GetWordCount(txtArea.Text);
            lblWordCount.Content = "Total Word Count:" + wordCount;

            if (CommonUtilities.GetWordCount(txtArea.Text) >= MaxWordCount && question.Mode == Mode.QUESTION)
            {
                e.Handled = true;
                System.Windows.Forms.MessageBox.Show("Reached the maximum word limit "+MaxWordCount+" for this question!");
            }
        }
    }
}
