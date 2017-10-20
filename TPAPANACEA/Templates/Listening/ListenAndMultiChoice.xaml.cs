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

namespace TPA.Templates.Listening
{
    /// <summary>
    /// Interaction logic for ListenAndMultiChoice.xaml
    /// </summary>
    public partial class ListenAndMultiChoice : UserControl,ISwitchable
    {
        ListenMultiChoiceQuestion question;
        private ObservableCollection<Option> Options { get; set; }
        
        public ListenAndMultiChoice()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            question = (ListenMultiChoiceQuestion)state;
            txtBlkInstruction.Text = question.Instruction.Trim().Replace("{newline}",string.Empty);
            txtBlkQuestionTitle.Text = question.Title;
            Options = new ObservableCollection<Option>(question.Options);
            

            if (question.Mode == Mode.QUESTION)
            {
                audioPlayer.Delay = question.Delay;
                audioPlayer.Media = question.Media;
            }

            if (question.Mode == Mode.ANSWER_KEY || question.Mode==Mode.QUESTION || question.Mode==Mode.TIME_OUT)
            {
                string[] answerArray = question.CorrectAnswers;

                if (question.Mode == Mode.QUESTION || question.Mode == Mode.TIME_OUT)
                {
                    answerArray = question.UserAnswers;
                }

                if (answerArray.Any())
                {
                    Option optionSelected = Options.ToList().Find(_ => _.Id == answerArray[0].Trim());
                    if (optionSelected != null)
                        optionSelected.IsSelected = true;
                }

                if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
                    lstBoxOptions.IsEnabled = false;
            }

            lstBoxOptions.ItemsSource = Options;
            prevNext.PrevNextClicked += prevNext_PrevNextClicked;
            prevNext.QuestionContext = question;
            prevNext.YourResponseClicked += new Common.PreviousNext.YourResponseEventHandler(prevNext_YourResponseClicked);
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.LISTENING;
        }

        void prevNext_YourResponseClicked(object sender, Common.YourResponseEventArgs e)
        {
            string[] answers = e.ShowYourAnswer ? question.UserAnswers : question.CorrectAnswers;
            ObservableCollection<Option> Answers = null;
            if (answers.Any())
            {
                Answers = new ObservableCollection<Option>(Options);

                Answers.ToList().ForEach((answer) =>
                {
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
          string userAnswer =  question.Options.Where(_ => _.IsSelected).Select(_=>_.Id).SingleOrDefault();
          AnswerManager.LogAnswer(question, userAnswer,prevNext.GetAttemptTimeLeft());
        }

        private void radOption_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radOption = e.Source as RadioButton;
            
            
        }

    }
}
