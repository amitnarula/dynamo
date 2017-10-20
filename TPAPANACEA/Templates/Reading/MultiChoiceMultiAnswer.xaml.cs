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
using System.Collections.ObjectModel;

namespace TPA.Templates.Reading
{
    /// <summary>
    /// Interaction logic for MultiChoiceMultiAnswer.xaml
    /// </summary>
    public partial class MultiChoiceMultiAnswer : UserControl,ISwitchable
    {
        MultiChoiceMultiAnswerQuestion question = null;
        private ObservableCollection<Option> Options { get; set; }
        public MultiChoiceMultiAnswer()
        {
            InitializeComponent();
        }

        private void chkOption_Click(object sender, RoutedEventArgs e)
        {

        }

        public void UtilizeState(object state)
        {
            question = (MultiChoiceMultiAnswerQuestion)state;
            lblQuestionTitle.Content = question.Title;
            txtQuestionTitle.Text = question.Title;
            txtBlkInstruction.Text = question.Instruction;
            txtBlockQuestionDescription.Text = question.Description.Trim().Replace("{newline}",Environment.NewLine);
            
            lstBoxOptions.ItemsSource = question.Options;

            if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.QUESTION || question.Mode==Mode.TIME_OUT)
            {
                string[] answerArray = question.CorrectAnswers;

                if (question.Mode == Mode.QUESTION || question.Mode == Mode.TIME_OUT)
                    answerArray = question.UserAnswers;

                if (answerArray.Any())
                {
                    for (int count = 0; count < answerArray.Length; count++)
                    {
                        Option option = question.Options.Find(_ => _.Id == answerArray[count].Trim());
                        if (option != null)
                            option.IsSelected = true;
                    }
                }

                if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
                    lstBoxOptions.IsEnabled = false;
            }

            prevNext.QuestionContext = question;
            prevNext.PrevNextClicked += prevNext_PrevNextClicked;
            prevNext.YourResponseClicked += prevNext_YourResponseClicked;
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.READING;

        }

        void prevNext_YourResponseClicked(object sender, Common.YourResponseEventArgs e)
        {
            string[] answers = e.ShowYourAnswer ? question.UserAnswers : question.CorrectAnswers;
            if (answers.Any())
            {
                question.Options.ForEach((option) => {
                    option.IsSelected = false; //Reset all the selected options
                });

                for (int count = 0; count < answers.Length; count++)
                {
                    Option option = question.Options.Find(_ => _.Id == answers[count].Trim());
                    if (option != null)
                        option.IsSelected = true;
                }
            }
            lstBoxOptions.ItemsSource = question.Options;
            lstBoxOptions.Items.Refresh(); //Refresh the new assignment
        }

        void prevNext_PrevNextClicked(object sender, EventArgs e)
        {
            var userAnswers = question.Options.Where(_ => _.IsSelected).Select(_ => _.Id).ToList();
            string answerString = string.Empty;
            foreach (var answer in userAnswers)
            {
                answerString += answer + "|";
            }
            answerString = answerString.TrimEnd(new char[] { '|' });

            AnswerManager.LogAnswer(question, answerString,prevNext.GetAttemptTimeLeft());
        }

        private void imgQuestion_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(question.Picture))
            {

                // ... Create a new BitmapImage.
                BitmapImage bmpImage = new BitmapImage();
                bmpImage.BeginInit();
                bmpImage.UriSource = new Uri(MediaReader.GetMediaPath(question.Picture));
                bmpImage.EndInit();

                // ... Get Image reference from sender.

                // ... Assign Source.
                imgQuestion.Source = bmpImage;
            }
            else
                imgQuestion.Visibility = Visibility.Hidden;
        }

        private void txtBlockQuestionDescription_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        
    }
}
