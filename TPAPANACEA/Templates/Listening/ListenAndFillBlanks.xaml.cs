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

namespace TPA.Templates.Listening
{
    /// <summary>
    /// Interaction logic for ListenAndFillBlanks.xaml
    /// </summary>
    public partial class ListenAndFillBlanks : UserControl,ISwitchable
    {
        ListenAndFillInBlanksQuestion question = null;
        public ListenAndFillBlanks()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            question = (ListenAndFillInBlanksQuestion)state;
            txtBlkInstruction.Text = question.Instruction;
            txtBlkQuestionTitle.Text = question.Title;

            Mode mode = question.Mode;
            string[] answerArray = question.CorrectAnswers;

            if (mode == Mode.QUESTION || mode==Mode.TIME_OUT)
            {
                answerArray = question.UserAnswers;
            }

            //Temporary fix
            string[] splitDescriptionNewLines = question.Description.Replace("{newline}",string.Empty).Trim().Split(new string[] { "{newline}" }, StringSplitOptions.RemoveEmptyEntries);

            for (int count = 0; count < splitDescriptionNewLines.Length; count++)
            {
                Transcript transcript = new Transcript();
                transcript.ID = count;
                transcript.TranscriptAnswserArray = answerArray;
                transcript.Mode = question.Mode;
                transcript.Content = splitDescriptionNewLines[count];
                dynamicContent.Children.Add(transcript);
                //dynamicContent.Children.Add(new StackPanel() { Orientation = Orientation.Horizontal });
            }

            for (int count = 0; count < splitDescriptionNewLines.Length; count++)
            {
                string[] splitDescription = splitDescriptionNewLines[count].Split(new string[] { "{blank}" }, 
                    StringSplitOptions.None);

                for (int innerCount = 0; innerCount < splitDescription.Length; innerCount++)
                {
                    Label lblDescription = new Label();
                    lblDescription.VerticalAlignment = VerticalAlignment.Top;
                    lblDescription.Content = splitDescription[innerCount];

                    //dynamicContent.Children.Add(lblDescription);
                    if (innerCount != splitDescription.Length - 1)
                    {
                        TextBox txtBlank = new TextBox();
                        txtBlank.VerticalAlignment = VerticalAlignment.Center;
                        txtBlank.Height = 20;
                        txtBlank.Width = 70;

                        if (mode == Mode.ANSWER_KEY || mode == Mode.QUESTION || mode==Mode.TIME_OUT)
                        {
                            if (answerArray.Any())
                            {
                                if (!string.IsNullOrEmpty(answerArray[innerCount]))
                                {
                                    txtBlank.Text = answerArray[innerCount].Trim();
                                }
                            }
                            if (mode == Mode.ANSWER_KEY || mode == Mode.TIME_OUT)
                                txtBlank.IsEnabled = false;
                        }

                        //dynamicContent.Children.Add(txtBlank);

                    }

                }
                Rectangle rect = new Rectangle();
                //rect.Margin = new Thickness(0, 0, 0, 0);
                //rect.HorizontalAlignment = HorizontalAlignment.Stretch;
                //rect.FlowDirection = FlowDirection.LeftToRight;
                
                rect.ClipToBounds = true;
                rect.Height = 30;
                //dynamicContent.Children.Add(rect);
                
            }

            if (mode == Mode.QUESTION)
            {
                audioPlayer.Delay = question.Delay;
                audioPlayer.Media = question.Media;
            }

            prevNext.QuestionContext = question;
            prevNext.PrevNextClicked += prevNext_PrevNextClicked;
            prevNext.YourResponseClicked += new PreviousNext.YourResponseEventHandler(prevNext_YourResponseClicked);
            
            breadCrumb.PracticeSetId = question.CurrentPracticeSetId;
            breadCrumb.QuestionTemplate = (QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), question.QuestionTemplate);
            breadCrumb.QuestionType = QuestionType.LISTENING;
        }

        void prevNext_YourResponseClicked(object sender, YourResponseEventArgs e)
        {
            string[] answerArray = e.ShowYourAnswer ? question.UserAnswers : question.CorrectAnswers;

            string[] splitDescriptionNewLines = question.Description.Replace("{newline}",string.Empty).Trim().Split(new string[] { "{newline}" }, StringSplitOptions.RemoveEmptyEntries);

            for (int count = 0; count < splitDescriptionNewLines.Length; count++)
            {
                Transcript transcript = dynamicContent.Children[count] as Transcript;
                transcript.ID = count;
                transcript.TranscriptAnswserArray = answerArray;
                transcript.Mode = question.Mode;
                transcript.Content = splitDescriptionNewLines[count];
                transcript.LoadTranscript();
            }
            dynamicContent.Background = e.ShowYourAnswer ? new SolidColorBrush(Colors.Wheat) : null;
        }

        void prevNext_PrevNextClicked(object sender, EventArgs e)
        {
            
            UIElementCollection uiElementCollection = dynamicContent.Children;
            string answer = string.Empty;
            foreach (UIElement elem in uiElementCollection) //TextBlock
            {
                if (elem is Transcript) //Element be transcript
                {
                    Transcript transcript = elem as Transcript;
                    
                    ///UIElementCollection uiElementCollectionTranscriptWrapPanel = transcript.wrapContent.Children;
                    // 14 May 2016 inline changes
                    var uiElementInlineContainer = transcript.wrapContent.Inlines.Where(x=>x.GetType()==typeof(InlineUIContainer));


                    /*foreach (UIElement wrapContentChild in uiElementCollectionTranscriptWrapPanel)
                    {
                        if (wrapContentChild is TextBox)
                        {
                            TextBox dynamicTextBox = wrapContentChild as TextBox;
                            answer += dynamicTextBox.Text.Trim() + "|";
                        }
                    }*/

                    foreach (InlineUIContainer inlineUIContainer in uiElementInlineContainer)
                    {
                        TextBox dynamicTextBox = inlineUIContainer.Child as TextBox;
                        if (dynamicTextBox != null)
                            answer += dynamicTextBox.Text.Trim() + "|";
                    }

                }

                //if (elem.GetType() == typeof(TextBox)) //Wrong code don't uncomment
                //{
                //    TextBox txtBx = (TextBox)elem;
                //    answer += txtBx.Text.Trim() + "|";
                //}
            }
            answer = answer.Substring(0, answer.Length - 1); // Removing the last unecessary '|'
            AnswerManager.LogAnswer(prevNext.QuestionContext, answer,prevNext.GetAttemptTimeLeft());
        }

        public string[] ProcessCorrectAnswer(string answerString)
        {
            string[] answerArray = answerString.Split(',');
            return answerArray;
        }
    }
}
