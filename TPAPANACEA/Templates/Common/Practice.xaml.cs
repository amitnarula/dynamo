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
        TestMode CurrentTestMode;
        int PageIndex = 0;

        private SetAttemptTime ResolveSetAttemptTime(string attemptTimeExpression)
        {
            SetAttemptTime attemptTime = null;
            if (!string.IsNullOrEmpty(attemptTimeExpression))
            {
                attemptTime = new SetAttemptTime();
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

        private void LoadSets(NextPrevious load,int pageSize=10)
        {
            DataSet dsSets;
            string tableKey = string.Empty;
            string attemptTimeColumnKey = string.Empty;
            if (CurrentTestMode == TestMode.Mock)
            {
                dsSets = FileReader.ReadFile(FileReader.FileType.MOCK);
                tableKey = "mockSet";
                attemptTimeColumnKey = "mockSetAttemptTime";
            }
            else
            {
                dsSets = FileReader.ReadFile(FileReader.FileType.PRACTICE_SET);
                tableKey = "practiceSet";
                attemptTimeColumnKey = "practiceSetAttemptTime";
            }
            
            var allRows = dsSets.Tables[tableKey].Rows;
            int totalSets = allRows.Count;

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
            
            if (take >= totalSets)
            {
                btnNext.IsEnabled = false;
            }
            else
            {
                btnNext.IsEnabled  = true;
            }

            List<Set> lstSet = new List<Set>();
            var pagedRows = allRows.OfType<DataRow>().Take(take).Skip(skip);

            //ObservableCollection<PracticeSet> lstPracticeSet = new ObservableCollection<PracticeSet>();
            foreach (DataRow dRow in pagedRows)
            {
                Set set;
                if (this.CurrentTestMode == TestMode.Mock)
                    set = new MockSet();
                else set = new PracticeSet();
                
                set.Id = Convert.ToString(dRow["id"]);
                set.Name = Convert.ToString(dRow["name"]);
                set.Description = Convert.ToString(dRow["description"]);
                set.SetAttemptTime = ResolveSetAttemptTime(Convert.ToString(dRow[attemptTimeColumnKey]));
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

                lstSet.Add(set);

            }
            if (CurrentTestMode == TestMode.Mock)
                mockSetListBox.ItemsSource = lstSet;
            else
                practiceSetListBox.ItemsSource = lstSet;
        }

        public Practice()
        {
            InitializeComponent();
            mockSetListBox.Visibility = Visibility.Collapsed;
        }

        protected void btnItem_Click(object sender, RoutedEventArgs e)
        {
            Button btnSender = (Button)sender;
            var dataItem = ((FrameworkElement)sender).DataContext;
            Set set = dataItem as Set;
            TPA.Entities.Question question = new Entities.Question();
            question.PracticeSetId = set.Id;
            question.QuestionMode = CurrentMode;
            question.PracticeSetAttemptTime = set.SetAttemptTime;

            if (CurrentTestMode == TestMode.Mock)
            {
                //setting intial test of reading and mode to the question as test mode = mock
                question.QuestionType = QuestionType.SPEAKING;
                question.TestMode = TestMode.Mock;

                //check if user saved something as save and exit

                //check save and exit state
                var testModeStateForMockTest = TPACache.GetItem(
                    "MOCK" + set.Id) as CurrentState;
                if (testModeStateForMockTest != null)
                {
                    //we need to check the question type because now Mock mode runs all modules
                    //we need to check like, which question type and which question index was saved by user before save and exit
                    //index will be checked and set at question switcher --> process question
                    //which question type and which number/index?
                    question.QuestionType = testModeStateForMockTest.QuestionType; 
                }
            }
            
            //For DEMO build
            //if (question.PracticeSetId != "abf2b37f406f44d5bc56c90314c1fd7c")
            //{
            //    WinForms.MessageBox.Show("This is a demo application. Please call +91-7087752501 or send email at 'ptepanacea@gmail.com' to purchase the full version.",
            //           "Evaluation only", WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information);
            //    return;
            //}
            else
            {
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
            var modeSetting = (ModeSetting)state;
            CurrentMode = modeSetting.QuestionMode;
            CurrentTestMode = modeSetting.TestMode;

            if (CurrentTestMode == TestMode.Practice)
            {
                lblTitle.Content = "Practice sets";
            }
            else if (CurrentTestMode == TestMode.Mock)
            {
                lblTitle.Content = "Mock sets";
                mockSetListBox.Visibility = Visibility.Visible;
                practiceSetListBox.Visibility = Visibility.Collapsed;

            }
            LoadSets(NextPrevious.Default);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            LoadSets(NextPrevious.Next);
            
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            LoadSets(NextPrevious.Previous);
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        
    }
}
