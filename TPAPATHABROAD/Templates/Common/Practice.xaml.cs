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
using TPAPathAbroad.FX;
using System.IO;

namespace TPA.Templates.Common
{
    /// <summary>
    /// Interaction logic for Practice.xaml
    /// </summary>
    public partial class Practice : UserControl,ISwitchable
    {
        Mode CurrentMode;
        static string baseOutputDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Data//Temp//";
        
        private SetAttemptTime ResolvePracticeSetAttemptTime(string attemptTimeExpression)
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

        private void LoadPracticeSets()
        {
            DataSet dsPracticeSets = FileReader.ReadFile(FileReader.FileType.PRACTICE_SET);

            List<PracticeSet> lstPracticeSet = new List<PracticeSet>();
            //ObservableCollection<PracticeSet> lstPracticeSet = new ObservableCollection<PracticeSet>();
            foreach (DataRow dRow in dsPracticeSets.Tables["practiceSet"].Rows)
            {
                PracticeSet practiceSet = new PracticeSet();
                practiceSet.Id = Convert.ToString(dRow["id"]);
                practiceSet.Name = Convert.ToString(dRow["name"]);
                practiceSet.Description = Convert.ToString(dRow["description"]);
                practiceSet.SetAttemptTime = ResolvePracticeSetAttemptTime(Convert.ToString(dRow["practiceSetAttemptTime"]));

                

                
                    practiceSet.ReadingEnabled = true;
                    practiceSet.ListeningEnabled = true;
                    practiceSet.WritingEnabled = true;
                    practiceSet.SpeakingEnabled = true;

                    if (CurrentMode == Mode.ANSWER_KEY)
                    {
                        //Removing the UNLOCK files
                        if (File.Exists(baseOutputDirectory + QuestionType.READING.ToString() + practiceSet.Id + "UNLCK.xml"))
                        {
                            practiceSet.ReadingEnabled = true;
                        }
                        else
                            practiceSet.ReadingEnabled = false;

                        if (File.Exists(baseOutputDirectory + QuestionType.LISTENING.ToString() + practiceSet.Id + "UNLCK.xml"))
                            practiceSet.ListeningEnabled = true;
                        else
                            practiceSet.ListeningEnabled = false;

                        if (File.Exists(baseOutputDirectory + QuestionType.WRITING.ToString() + practiceSet.Id + "UNLCK.xml"))
                            practiceSet.WritingEnabled = true;
                        else
                            practiceSet.WritingEnabled = false;

                        if (File.Exists(baseOutputDirectory + QuestionType.SPEAKING.ToString() + practiceSet.Id + "UNLCK.xml"))
                            practiceSet.SpeakingEnabled = true;
                        else
                            practiceSet.SpeakingEnabled = false;
                    }
                
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

            if (CurrentMode == Mode.ANSWER_KEY)
                lblTitlePractice.Content = "Answer Key";
        }

        public Practice()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Practice_Loaded);
        }

        void Practice_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPracticeSets();
        }

        protected void btnItem_Click(object sender, RoutedEventArgs e)
        {
            Button btnSender = (Button)sender;
            var dataItem = ((FrameworkElement)sender).DataContext;
            PracticeSet practiceSet = dataItem as PracticeSet;
            TPA.Entities.Question question = new Entities.Question();
            question.PracticeSetId = practiceSet.Id;
            question.QuestionMode = CurrentMode;
            question.PracticeSetAttemptTime = practiceSet.SetAttemptTime;

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
                    //WinForms.DialogResult result = WinForms.MessageBox.Show("Do you want to restart your test, or continue with the current set?",
                      //    "Restart test?", WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question);

                    WinForms.DialogResult result = MessageDialog.Show("Restart test?",
                        "Do you want to restart your test, or continue with the current set?", "Yes", "No");

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

        private void btnBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Home());
        }
    }
}
