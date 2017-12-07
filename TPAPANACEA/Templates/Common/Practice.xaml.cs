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
using System.Data;
using TPA.CoreFramework;
using TPA.Entities;
using System.Collections.ObjectModel;
using WinForms = System.Windows.Forms;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for Practice.xaml
    /// </summary>
    public partial class Practice : UserControl,ISwitchable
    {
        Mode CurrentMode;
        int PageIndex = 0;

        private PracticeSetAttemptTime ResolvePracticeSetAttemptTime(string attemptTimeExpression)
        {
            PracticeSetAttemptTime attemptTime = null;
            if (!string.IsNullOrEmpty(attemptTimeExpression))
            {
                attemptTime = new PracticeSetAttemptTime();
                string[] attemptTimeItemArray = attemptTimeExpression.Split('-');
                if (attemptTimeItemArray.Any())
                {
                    if (attemptTimeItemArray.Count() == 2)
                    {
                        attemptTime.ItemType = attemptTimeItemArray[0];
                        attemptTime.AttemptTime = attemptTimeItemArray[1];
                    }
                    else if (attemptTimeItemArray.Count() == 1)
                    {
                        attemptTime.AttemptTime = string.Empty;
                    }
                }
                
            //    string[] attemptTimeItems = attemptTimeExpression.Split(',');
            //    if (attemptTimeItems.Any())
            //    {
            //        foreach (var item in attemptTimeItems)
            //        {
            //            string[] attemptTimeItemArray = item.Split('-');
            //            if (attemptTimeItemArray.Any())
            //            {
            //                if (!string.IsNullOrEmpty(attemptTimeItemArray[0]))
            //                {
            //                    string time = string.IsNullOrEmpty(attemptTimeItemArray[1]) ? "CUSTOM" : attemptTimeItemArray[1];
            //                    switch (attemptTimeItemArray[0])
            //                    {
            //                        case "READING":
            //                            attemptTime.ReadingSetTime = time;
            //                            break;
            //                        case "WRITING":
            //                            attemptTime.WritingSetTime = time;
            //                            break;
            //                        case "LISTENING":
            //                            attemptTime.ListeningSetTime = time;
            //                            break;
            //                        case "SPEAKING":
            //                            attemptTime.SpeakingSetTime = time;
            //                            break;
            //                        default:
            //                            break;
            //                    }
            //                }
            //            }
            //        }
            //    }
            }
            return attemptTime;
        }

        enum NextPrevious{
            Default,
            Next,
            Previous
        }

        private void LoadPracticeSets(NextPrevious load,int pageSize=5)
        {
            DataSet dsPracticeSets = FileReader.ReadFile(FileReader.FileType.PRACTICE_SET);

            var allRows = dsPracticeSets.Tables["practiceSet"].Rows;
            int totalPracticeSets = allRows.Count;

            if (load == NextPrevious.Next)
                PageIndex++;
            else if (load == NextPrevious.Previous)
                PageIndex--;
            else if (load == NextPrevious.Default)
                PageIndex = 0;

            int take = pageSize * (PageIndex + 1);
            int skip = pageSize * PageIndex;

            if (PageIndex == 0)
                btnPrevious.IsEnabled = false;
            else
                btnPrevious.IsEnabled = true;
            
            if (take >= totalPracticeSets)
            {
                btnNext.IsEnabled = false;
            }
            else
            {
                btnNext.IsEnabled  = true;
            }

            List<PracticeSet> lstPracticeSet = new List<PracticeSet>();
            var pagedRows = allRows.OfType<DataRow>().Take(take).Skip(skip);

            //ObservableCollection<PracticeSet> lstPracticeSet = new ObservableCollection<PracticeSet>();
            foreach (DataRow dRow in pagedRows)
            {
                PracticeSet practiceSet = new PracticeSet();
                practiceSet.Id = Convert.ToString(dRow["id"]);
                practiceSet.Name = Convert.ToString(dRow["name"]);
                practiceSet.Description = Convert.ToString(dRow["description"]);
                practiceSet.PracticeSetAttemptTime = ResolvePracticeSetAttemptTime(Convert.ToString(dRow["practiceSetAttemptTime"]));
                /*practiceSet.Items = new PracticeSetItem();

                foreach (DataRow dRowItem in dsPracticeSets.Tables["items"].Rows)
                {
                    if (string.Equals(Convert.ToString(dRowItem["practiceSet_id"]), practiceSet.Id, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (Convert.ToBoolean(dRowItem["speaking"]) == true)
                        {
                            practiceSet.Items.Speaking = true;
                        }
                        if (Convert.ToBoolean(dRowItem["writing"]) == true)
                        {
                            practiceSet.Items.Writing = true;
                        }
                        if (Convert.ToBoolean(dRowItem["reading"]) == true)
                        {
                            practiceSet.Items.Reading = true;
                        }
                        if (Convert.ToBoolean(dRowItem["listening"]) == true)
                        {
                            practiceSet.Items.Listening = true;
                        }
                    }
                }*/

                lstPracticeSet.Add(practiceSet);

            }
            practiceSetListBox.ItemsSource = lstPracticeSet;
        }

        public Practice()
        {
            InitializeComponent();
            LoadPracticeSets(NextPrevious.Default);

        }

        protected void btnItem_Click(object sender, RoutedEventArgs e)
        {
            Button btnSender = (Button)sender;
            var dataItem = ((FrameworkElement)sender).DataContext;
            PracticeSet practiceSet = dataItem as PracticeSet;
            TPA.Entities.Question question = new Entities.Question();
            question.PracticeSetId = practiceSet.Id;
            question.QuestionMode = CurrentMode;
            question.PracticeSetAttemptTime = practiceSet.PracticeSetAttemptTime;

            //For DEMO build
            //if (question.PracticeSetId != "abf2b37f406f44d5bc56c90314c1fd7c")
            //{
            //    WinForms.MessageBox.Show("This is a demo application. Please call +91-7087752501 or send email at 'ptepanacea@gmail.com' to purchase the full version.",
            //           "Evaluation only", WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information);
            //    return;
            //}

            switch (btnSender.Name)
            {
                case "btnReading":
                    question.QuestionType = QuestionType.READING;
                    break;
                case "btnListening":
                    question.QuestionType = QuestionType.LISTENING;
                    break;
                case "btnWriting":
                    question.QuestionType = QuestionType.WRITING;
                    break;
                case "btnSpeaking":
                    question.QuestionType = QuestionType.SPEAKING;
                    break;
                default:
                    break;
            }

            if (CurrentMode != Mode.ANSWER_KEY)
            {
                if (AnswerManager.DoAnswersExist(CommonUtilities.GetFileTypeByQuestionType(question.QuestionType), question.PracticeSetId))
                {
                    WinForms.DialogResult result = WinForms.MessageBox.Show("Do you want to restart your test, or continue with the current set?",
                          "Restart test?", WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question);

                    if (result == WinForms.DialogResult.Yes)
                    {
                        AnswerManager.DeleteAllUserAnswers(CommonUtilities.GetFileTypeByQuestionType(question.QuestionType),
                            question.PracticeSetId);
                    }
                }
            }

            Switcher.Switch(new QuestionSwitcher(), question); 
        }
       
        public void UtilizeState(object state)
        {
            CurrentMode = (Mode)state;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            LoadPracticeSets(NextPrevious.Next);
            
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            LoadPracticeSets(NextPrevious.Previous);
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
