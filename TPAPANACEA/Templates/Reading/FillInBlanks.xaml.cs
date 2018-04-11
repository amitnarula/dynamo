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
using System.Data.Linq;

namespace TPA.Templates.Reading
{
    /// <summary>
    /// Interaction logic for FillInBlanks.xaml
    /// </summary>
    public partial class FillInBlanks : UserControl, ISwitchable
    {
        FillInTheBlanksQuestion question = null;
        public ObservableCollection<Option> Options { get; set; }
        public ObservableCollection<Option> Intermediate { get; set; }
        public FillInBlanks()
        {
            InitializeComponent();
            Options = new ObservableCollection<Option>();
            Intermediate = new ObservableCollection<Option>();
        }

        public void UtilizeState(object state)
        {
            question = (FillInTheBlanksQuestion)state;
            txtBlkInstruction.Text = question.Instruction.Trim().Replace("{newline}",string.Empty);

            foreach (var option in question.Options)
            {
                Options.Add(option);

                //Availalbe options
                var run = new Run(option.OptionText);
                txtAvailalbleOptions.Inlines.Add(run);

                var runBlank = new Run("   ");
                txtAvailalbleOptions.Inlines.Add(runBlank);

            }
            
            //string questionDescription = question.Description.Trim().Replace("{newline}", Environment.NewLine); //Adjusting the new line
            string questionDescription = question.Description.Trim(); //Adjusting the new line


            question.Description = questionDescription;

            string[] splitDescription = question.Description.Split(new string[] { "{blank}" },
                StringSplitOptions.None);

            string[] answersArray = question.CorrectAnswers;

            for (int count = 0; count < splitDescription.Length; count++)
            {
                /*Label lblDescription = new Label();
                lblDescription.Content = splitDescription[count];
                lblDescription.VerticalAlignment = VerticalAlignment.Top;
                lblDescription.HorizontalAlignment = HorizontalAlignment.Left;
                lblDescription.FontSize = 14;
                lblDescription.Padding = new Thickness(2);

                dynamicContent.Children.Add(lblDescription);*/
                
                /*TextBlock txtBlkDescription = new TextBlock();
                txtBlkDescription.TextWrapping = TextWrapping.Wrap;
                txtBlkDescription.Text = splitDescription[count];
                txtBlkDescription.VerticalAlignment = VerticalAlignment.Top;
                txtBlkDescription.HorizontalAlignment = HorizontalAlignment.Left;
                txtBlkDescription.FontSize = 14;

                dynamicContent.Children.Add(txtBlkDescription); Commented on May 14,2016*/

                Run txtBlkDescription = new Run(splitDescription[count].Trim().Replace("{newline}",Environment.NewLine+Environment.NewLine));
                txtBlkDescription.BaselineAlignment = BaselineAlignment.Center;
                dynamicContent.Inlines.Add(txtBlkDescription);

                if (count != splitDescription.Length - 1)
                {
                    ComboBox cmbBoxOptions = new ComboBox();
                    cmbBoxOptions.Height = 25;
                    //cmbBoxOptions.Width = 95;
                    cmbBoxOptions.MinWidth = 95;
                    cmbBoxOptions.VerticalAlignment = VerticalAlignment.Top;
                    cmbBoxOptions.HorizontalAlignment = HorizontalAlignment.Left;
                    cmbBoxOptions.FontSize = 14;
                    cmbBoxOptions.Padding = new Thickness(2);
                    cmbBoxOptions.ItemsSource = Options;
                    cmbBoxOptions.DisplayMemberPath = "OptionText";

                    //Changed to inline added margin
                    cmbBoxOptions.Margin = new Thickness(2);

                    cmbBoxOptions.SelectionChanged += new SelectionChangedEventHandler(cmbBoxOptions_SelectionChanged);

                    if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.QUESTION || question.Mode == Mode.TIME_OUT)
                    {
                        if (question.Mode == Mode.ANSWER_KEY || question.Mode == Mode.TIME_OUT)
                        {
                            cmbBoxOptions.IsEnabled = false;
                            btnReset.IsEnabled = false;
                        }

                        if (question.Mode == Mode.QUESTION || question.Mode == Mode.TIME_OUT)
                            answersArray = question.UserAnswers;

                        if (answersArray.Any())
                        {
                            var selectedItem = Options.Where(_ => _.Id == answersArray[count]).FirstOrDefault();
                            cmbBoxOptions.SelectedItem = selectedItem;
                            cmbBoxOptions.IsEnabled = false;
                        }
                    }
                    //dynamicContent.Children.Add(cmbBoxOptions); Commented on 14 May , 2016
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
                //Populating list of comboboxes from inlines , 14 May 2016
                var inlineUIContainers = dynamicContent.Inlines.Where(x => x.GetType() == typeof(InlineUIContainer));
                List<ComboBox> lstComboBoxes = new List<ComboBox>();

                foreach (InlineUIContainer uiContainer in inlineUIContainers)
                {
                    lstComboBoxes.Add(((ComboBox)uiContainer.Child));
                } //Inline formatting ends here

                for (int count = 0; count < answers.Length; count++)
                {
                    if (!string.IsNullOrEmpty(answers[count]))
                    {
                        //List<ComboBox> lstComboBoxes = dynamicContent.Children.OfType<ComboBox>().ToList(); 14 May 2016
                        var selectedItem = Options.Where(_ => _.Id == answers[count]).FirstOrDefault();

                        if (answers[count] != correctAnswers[count] && e.ShowYourAnswer)
                        {
                            lstComboBoxes[count].Foreground = Brushes.Red; //wrong answers should be in red color
                        }
                        else if (answers[count] == correctAnswers[count] && e.ShowYourAnswer) //right options be in green color
                        {
                            lstComboBoxes[count].Foreground = Brushes.Green;
                        }
                        else
                            lstComboBoxes[count].Foreground = Brushes.Black; //default is black color

                        lstComboBoxes[count].SelectedItem = selectedItem;
                    }

                }                
            }
            dynamicContent.Background = e.ShowYourAnswer ? new SolidColorBrush(Colors.Wheat) : null;
        }

        void cmbBoxOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*foreach (UIElement item in dynamicContent.Children)
            {
                if (item.GetType() == typeof(ComboBox))
                {
                    ComboBox cmbBox = item as ComboBox;
                    ComboBox senderCmbBox = sender as ComboBox;
                    Option selectedItem = senderCmbBox.SelectedItem as Option;
                    if (selectedItem != null)
                    {
                        IEnumerable<Option> availableOptions = senderCmbBox.ItemsSource as IEnumerable<Option>;
                        Intermediate = new ObservableCollection<Option>(availableOptions);

                        if (senderCmbBox != cmbBox && cmbBox.IsEnabled)
                        {
                            Intermediate.Remove(selectedItem);
                            cmbBox.ItemsSource = Intermediate;
                        }
                        else
                            senderCmbBox.IsEnabled = false;
                    }
                }
            }* 14 May 2016 Commented since some formatting issues were introduced with inlines*/

            List<ComboBox> lstComboBoxes = ExtractComboBoxes();

            foreach (ComboBox cmbBox in lstComboBoxes)
            {
                ComboBox senderCmbBox = sender as ComboBox;
                Option selectedItem = senderCmbBox.SelectedItem as Option;
                if (selectedItem != null)
                {
                    IEnumerable<Option> availableOptions = senderCmbBox.ItemsSource as IEnumerable<Option>;
                    Intermediate = new ObservableCollection<Option>(availableOptions);

                    if (senderCmbBox != cmbBox && cmbBox.IsEnabled)
                    {
                        Intermediate.Remove(selectedItem);
                        cmbBox.ItemsSource = Intermediate;
                    }
                    else
                        senderCmbBox.IsEnabled = false;
                }
            }

        }

        private List<ComboBox> ExtractComboBoxes()
        {
            List<ComboBox> lstComboBoxes = dynamicContent.Inlines
                .Where(x => x.GetType() == typeof(InlineUIContainer))
                .Select(x => (InlineUIContainer)x)
                .Select(x => (ComboBox)x.Child).ToList();
            return lstComboBoxes;
        }
        void prevNext_PrevNextClicked(object sender, EventArgs e)
        {
            /* UIElementCollection uiElementCollection = dynamicContent.Children;
             string answer = string.Empty;
             foreach (UIElement elem in uiElementCollection)
             {
                 if (elem.GetType() == typeof(ComboBox))
                 {
                     ComboBox cmbBx = (ComboBox)elem;
                     Option selectedItem = cmbBx.SelectedItem as Option;
                     string selectedItemId = selectedItem != null ? selectedItem.Id : "-1";
                     answer += selectedItemId + "|";
                 }
             }
             answer = answer.TrimEnd(new char[] { '|' });
             AnswerManager.LogAnswer(prevNext.QuestionContext, answer, prevNext.GetAttemptTimeLeft());14 May 2016*/


            List<ComboBox> lstComboBoxes = ExtractComboBoxes();
            string answer = string.Empty;
            foreach (ComboBox cmbBx in lstComboBoxes)
            {

                Option selectedItem = cmbBx.SelectedItem as Option;
                string selectedItemId = selectedItem != null ? selectedItem.Id : "-1";
                answer += selectedItemId + "|";

            }
            answer = answer.TrimEnd(new char[] { '|' });
            AnswerManager.LogAnswer(prevNext.QuestionContext, answer, prevNext.GetAttemptTimeLeft());

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            List<ComboBox> lstComboBoxes = ExtractComboBoxes();

            foreach (ComboBox cmbBox in lstComboBoxes)
            {
                cmbBox.IsEnabled = true;
                cmbBox.ItemsSource = Options;
                cmbBox.SelectedIndex = -1;
            }

            /*foreach (UIElement item in dynamicContent.Children)
            {
                if (item.GetType() == typeof(ComboBox))
                {
                    ComboBox cmbBox = item as ComboBox;
                    cmbBox.IsEnabled = true;
                    cmbBox.ItemsSource = Options;
                    cmbBox.SelectedIndex = -1;
                }
            }14 May 2016*/
        }
    }
}
