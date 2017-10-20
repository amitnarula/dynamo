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
using System.Collections.ObjectModel;

namespace TPA.Templates.Reading
{
    /// <summary>
    /// Interaction logic for MultiChoiceSingleAnswer.xaml
    /// </summary>
    public partial class MultiChoiceSingleAnswer : UserControl,ISwitchable
    {
        MultiChoiceSingleAnswerQuestion question = null;
        private ObservableCollection<Option> Options { get; set; }
        public MultiChoiceSingleAnswer()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            question = (MultiChoiceSingleAnswerQuestion)state;
            lblQuestionTitle.Content = question.Title;
            txtQuestionTitle.Text = question.Title;
            txtBlockQuestionDescription.Text = question.Description.Trim().Replace("{newline}",Environment.NewLine);
            txtBlkInstruction.Text = question.Instruction;
            Options = new ObservableCollection<Option>(question.Options);
            if (question.Mode == Mode.ANSWER_KEY  || question.Mode==Mode.QUESTION || question.Mode==Mode.TIME_OUT)
            {
                string[] answerArray = question.CorrectAnswers;

                if (question.Mode == Mode.QUESTION || question.Mode == Mode.TIME_OUT)
                    answerArray = question.UserAnswers;

                if (answerArray.Any())
                {
                    Option option = Options.ToList().Find(_ => _.Id == answerArray[0].Trim());
                    if (option != null)
                    {
                        option.IsSelected = true;
                    }
                }

                if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
                    lstBoxOptions.IsEnabled = false;
            }
            lstBoxOptions.ItemsSource = Options;
            prevNext.QuestionContext = question;
            prevNext.PrevNextClicked += prevNext_PrevNextClicked;
            prevNext.YourResponseClicked += prevNext_YourResponseClicked;
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.READING;

        }

        void prevNext_YourResponseClicked(object sender, YourResponseEventArgs e)
        {
            string[] answers = e.ShowYourAnswer ? question.UserAnswers : question.CorrectAnswers;
            ObservableCollection<Option> Answers=null;
            if (answers.Any())
            {
                Answers = new ObservableCollection<Option>(Options);

                Answers.ToList().ForEach((answer) => {
                    answer.IsSelected = false;
                });

                var ans = Answers.Where(x => x.Id == answers[0]).SingleOrDefault();
                if (ans != null)
                {
                    ans.IsSelected = true;
                }
            }
            lstBoxOptions.ItemsSource = Answers;
        }

        void prevNext_PrevNextClicked(object sender, EventArgs e)
        {
            string userAnswer = question.Options.Where(_ => _.IsSelected).Select(_ => _.Id).SingleOrDefault();
            AnswerManager.LogAnswer(question, userAnswer,prevNext.GetAttemptTimeLeft());
        }

        private void radOption_Click(object sender, RoutedEventArgs e)
        {
           
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
    }
}
