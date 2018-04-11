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

namespace TPA.Templates.Reading
{
    /// <summary>
    /// Interaction logic for FillInBlanksWithOptions.xaml
    /// </summary>
    public partial class FillInBlanksWithOptions : UserControl,ISwitchable
    {
        FillInTheBlanksWithOptionsQuestion question = null;
        short numberOfComboboxes = 0;
        TextBlock textBlockPadding = null;
        public FillInBlanksWithOptions()
        {
            InitializeComponent();
            textBlockPadding = new TextBlock()
            {
                Background = new SolidColorBrush(Colors.Wheat),
                Name = "TextBlockPadding",
                Height = 20,
                Width = dynamicContent.Width,
                Visibility=Visibility.Hidden
            };
        }

        public void UtilizeState(object state)
        {
            question = (FillInTheBlanksWithOptionsQuestion)state;
            txtBlkInstruction.Text = question.Instruction;

            //string questionDescription = question.Description.Trim().Replace("{newline}", string.Empty); //Adjusting the new line
            string questionDescription = question.Description.Trim(); //Adjusting the new line


            question.Description = questionDescription;

            string[] splitDescription = question.Description.Split(new string[] { "{blank}" }, 
                StringSplitOptions.None);

            string[] answersArray = question.CorrectAnswers;

            //Adding simple textblock to wrap panel to provide padding on the top
            
            //dynamicContent.Children.Add(textBlockPadding); 14 May 2016 inline changes

            for (int count = 0; count < splitDescription.Length; count++)
            {
                Run lblDescription = new Run(splitDescription[count].Trim().Replace("{newline}",Environment.NewLine+Environment.NewLine));
                lblDescription.BaselineAlignment = BaselineAlignment.Center;
                dynamicContent.Inlines.Add(lblDescription);
                
                /*Label lblDescription = new Label();
                lblDescription.Content = splitDescription[count];
                lblDescription.VerticalAlignment = VerticalAlignment.Top;
                //lblDescription.FontWeight = FontWeights.Bold;
                lblDescription.FontSize = 14;
                lblDescription.Padding = new Thickness(2);

                dynamicContent.Children.Add(lblDescription); 14 May 2016 inline changes*/
                

                if (count != splitDescription.Length - 1)
                {
                    ComboBox cmbBoxOptions = new ComboBox();
                    cmbBoxOptions.Height = 25;
                    //cmbBoxOptions.Width = 120;
                    cmbBoxOptions.MinWidth = 120;
                    cmbBoxOptions.VerticalAlignment = VerticalAlignment.Top;
                    cmbBoxOptions.FontSize = 14;
                    //cmbBoxOptions.FontWeight = FontWeights.Bold;
                    cmbBoxOptions.Padding = new Thickness(2);

                    cmbBoxOptions.Margin = new Thickness(2); // 14 May 2016 inline changes

                    List<Option> lstOptions = question.Options[count];
                    foreach (Option option in lstOptions)
                    {
                        cmbBoxOptions.Items.Add(option.OptionText);
                        
                    }
                    if (question.Mode == Mode.ANSWER_KEY || question.Mode==Mode.QUESTION || question.Mode==Mode.TIME_OUT)
                    {
                        if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
                            cmbBoxOptions.IsEnabled = false;

                        if (question.Mode == Mode.QUESTION || question.Mode == Mode.TIME_OUT)
                            answersArray = question.UserAnswers;

                        if (answersArray.Any())
                        {
                            string selectedItem = lstOptions.Where(_ => _.Id == answersArray[count]).Select(_ => _.OptionText).FirstOrDefault();
                            cmbBoxOptions.SelectedValue = selectedItem;
                            
                        }
                    }
                    numberOfComboboxes++;
                    ///dynamicContent.Children.Add(cmbBoxOptions); 14 May 2016 inline changes
                    dynamicContent.Inlines.Add(cmbBoxOptions);
                }

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
            string[] correctAnswers = question.CorrectAnswers;
            if (answers.Any())
            {
                //List<ComboBox> lstComboBoxes = dynamicContent.Children.OfType<ComboBox>().ToList(); 14 May 2016 inline changes
                List<ComboBox> lstComboBoxes = ExtractComboBoxes();
                for (int count = 0; count < numberOfComboboxes; count++)
                {

                    ComboBox cmbBox = lstComboBoxes[count];
                    if (cmbBox != null)
                    {
                        string selectedItem = question.Options[count].Where(_ => _.Id == answers[count])
                            .Select(_ => _.OptionText).FirstOrDefault();
                        cmbBox.SelectedValue = selectedItem;

                        if (answers[count] != correctAnswers[count] && e.ShowYourAnswer)
                            cmbBox.Foreground = Brushes.Red; //wrong options should be in red color
                        else if (answers[count] == correctAnswers[count] && e.ShowYourAnswer)
                            cmbBox.Foreground = Brushes.Green; //right options in green color
                        else
                            cmbBox.Foreground = Brushes.Black; //default in black color

                    }
                    
                }
                dynamicContent.Background = e.ShowYourAnswer ? new SolidColorBrush(Colors.Wheat) : null;
                textBlockPadding.Visibility = e.ShowYourAnswer ? Visibility.Visible : Visibility.Hidden;
            }
        }

        void prevNext_PrevNextClicked(object sender, EventArgs e)
        {
            /*UIElementCollection uiElementCollection = dynamicContent.Children;
            string answer = string.Empty;
            foreach (UIElement elem in uiElementCollection)
            {
                if (elem.GetType() == typeof(ComboBox))
                {
                    ComboBox cmbBx = (ComboBox)elem;
                    answer += cmbBx.SelectedIndex.ToString() + "|";
                }
            }
            answer = answer.TrimEnd(new char[] { '|' });
            AnswerManager.LogAnswer(prevNext.QuestionContext, answer,prevNext.GetAttemptTimeLeft());14 May 2016 inline changes*/

            List<ComboBox> lstComboBoxes = ExtractComboBoxes();
            string answer = string.Empty;
            foreach (ComboBox cmbBx in lstComboBoxes)
            {
                answer += cmbBx.SelectedIndex.ToString() + "|";
            }
            answer = answer.TrimEnd(new char[] { '|' });
            AnswerManager.LogAnswer(prevNext.QuestionContext, answer, prevNext.GetAttemptTimeLeft());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private List<ComboBox> ExtractComboBoxes()
        {
            List<ComboBox> lstComboBoxes = dynamicContent.Inlines
                .Where(x => x.GetType() == typeof(InlineUIContainer))
                .Select(x => (InlineUIContainer)x)
                .Select(x => (ComboBox)x.Child).ToList();
            return lstComboBoxes;
        }
    }
}
