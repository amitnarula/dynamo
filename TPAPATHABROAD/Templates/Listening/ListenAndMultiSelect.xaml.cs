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
    /// Interaction logic for ListenAndMultiSelect.xaml
    /// </summary>
    public partial class ListenAndMultiSelect : UserControl,ISwitchable
    {
        ListenMultiSelectQuestion question;
        private ObservableCollection<Option> Options { get; set; }
        public ListenAndMultiSelect()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            question = (ListenMultiSelectQuestion)state;
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
                    answerArray = question.UserAnswers;
                if (answerArray.Any())
                {
                    for (int count = 0; count < answerArray.Length; count++)
                    {
                        Option optionSelected = Options.ToList().Find(_ => _.Id == answerArray[count].Trim());
                        if (optionSelected != null)
                            optionSelected.IsSelected = true;
                    }
                }

                if (question.Mode == Mode.ANSWER_KEY || question.Mode==Mode.TIME_OUT)
                    lstBoxOptions.IsEnabled = false;
            }

            lstBoxOptions.ItemsSource = Options;
            prevNext.QuestionContext = question;
            prevNext.PrevNextClicked += prevNext_PrevNextClicked;
            prevNext.YourResponseClicked += new Common.PreviousNext.YourResponseEventHandler(prevNext_YourResponseClicked);
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.LISTENING;
        }

        void prevNext_YourResponseClicked(object sender, Common.YourResponseEventArgs e)
        {
            string[] answers = e.ShowYourAnswer ? question.UserAnswers : question.CorrectAnswers;
            if (answers.Any())
            {
                question.Options.ForEach((option) =>
                {
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
    }
}
