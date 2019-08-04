using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class ListenAndHighlight : UserControl,ISwitchable
    {
        ListenAndHighlightQuestion question;
        public ListenAndHighlight()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            question = (ListenAndHighlightQuestion)state;
            txtBlkInstruction.Text = question.Instruction;
            string description = question.Description.Replace("{newline}", string.Empty);
            string[] descriptionArray = description.Split(new char[]{' '});//Splitting with space
            foreach (var item in descriptionArray)
            {
                txtBlockQuestionDescription.Inlines.Add(new Run(item));
                txtBlockQuestionDescription.Inlines.Add(new Run(" "));
            }
            
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
                    foreach (var item in answerArray)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            if (question.Mode == Mode.ANSWER_KEY)
                                HighlightWord(item);
                            else
                                txtBlockQuestionDescription.Inlines.ElementAt(Convert.ToInt32(item)).Background = new SolidColorBrush(Colors.Yellow);

                        }
                            
                    }
                }

                if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
                    txtBlockQuestionDescription.IsEnabled = false;
            }

            prevNext.QuestionContext = question;
            prevNext.PrevNextClicked += prevNext_PrevNextClicked;
            prevNext.YourResponseClicked += new Common.PreviousNext.YourResponseEventHandler(prevNext_YourResponseClicked);
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.LISTENING;

            
        }

        private void HighlightWord(string item)
        {
            var matchedInlines = txtBlockQuestionDescription
                   .Inlines
                   .Where(x => ((string)x.GetValue(Run.TextProperty)).Trim(new char[] { ' ', ',', '.' })
                   .Equals(item, StringComparison.InvariantCultureIgnoreCase));

            Inline inline = null;

            if (matchedInlines.Count() > 1)
            {
                inline = matchedInlines.LastOrDefault();
            }
            else if (matchedInlines.Count() == 1)
            {
                inline = matchedInlines.FirstOrDefault();
            }

            //Inline inline = txtBlockQuestionDescription
            //       .Inlines
            //       .FirstOrDefault(x => ((string)x.GetValue(Run.TextProperty)).Trim(new char[] { ' ', ',', '.' })
            //       .Equals(item, StringComparison.InvariantCultureIgnoreCase));
            
            if (inline != null)
                inline.Background = new SolidColorBrush(Colors.Yellow);
        }


        void prevNext_YourResponseClicked(object sender, Common.YourResponseEventArgs e)
        {
            string[] answerArray = e.ShowYourAnswer ? question.UserAnswers : question.CorrectAnswers;
            txtBlockQuestionDescription.Inlines.ToList().ForEach((inline) =>
            {
                inline.Background = null;
            });
            
            if (answerArray.Any())
            {
                foreach (var item in answerArray)
                {
                    if (e.ShowYourAnswer)
                        txtBlockQuestionDescription.Inlines.ElementAt(Convert.ToInt32(item)).Background = new SolidColorBrush(Colors.Yellow);
                    else
                        HighlightWord(item);
                }
            }
        }

        void prevNext_PrevNextClicked(object sender, EventArgs e)
        {
            string answerString=string.Empty;
            for (int count = 0; count < txtBlockQuestionDescription.Inlines.Count; count++)
            {
                var run = txtBlockQuestionDescription.Inlines.ElementAt(count) as Run;
                if (string.IsNullOrWhiteSpace(run.Text))
                    continue;

                if (run.Background != null)
                    //answerString += run.Text + "|";
                    answerString += count + "|";
            }
            answerString = answerString.TrimEnd(new char[] { '|' });
            AnswerManager.LogAnswer(question, answerString,prevNext.GetAttemptTimeLeft());
        }

        private void txtBlockQuestionDescription_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var run = e.OriginalSource as Run;
            if (run != null)
            {
                run.Background = run.Background == null && !string.IsNullOrWhiteSpace(run.Text) ?
                    new SolidColorBrush(Colors.Yellow) : null;
            }
        }
        
    }
}
