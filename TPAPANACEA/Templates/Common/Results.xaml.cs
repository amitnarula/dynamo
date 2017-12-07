using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TPA.CoreFramework;
using TPA.Entities;
using TPACORE.CoreFramework;

namespace TPAPanacea.Templates.Common
{
    /// <summary>
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class Results : Window
    {
        static string baseOutputDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Data//Temp//";
        
        public Results()
        {
            InitializeComponent();
            DataSet dsPracticeSets = FileReader.ReadFile(FileReader.FileType.PRACTICE_SET);
            for (int count = 0; count < dsPracticeSets.Tables[0].Rows.Count; count++)
            {
                cmbPracticeSet.Items.Add(new ComboBoxItem() {
                    Content ="Practice Set "+(count+1),
                    Tag=Convert.ToString(dsPracticeSets.Tables[0].Rows[count]["Id"])
                });
            }
            cmbPracticeSet.SelectionChanged += CmbPracticeSet_SelectionChanged;

        }

        private void CmbPracticeSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLoadResult_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var evalManager = new EvaluationManager();

                //EvaluationManager.GetAttempatedPointsByQuestionType("abf2b37f406f44d5bc56c90314c1fd7c", QuestionTemplates.MULTI_CHOICE_MULTIPLE_ANSWER, QuestionType.READING);
                
                DataSet dsEvalParams = FileReader.ReadFile(FileReader.FileType.EVALUATION_PARAMETER);
                DataSet dsQuestions = FileReader.ReadFile(FileReader.FileType.QUESTION_WRITING);

                //Writing
                int totalWriting = evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SUMMARIZE_TEXT, FileReader.FileType.QUESTION_WRITING, "abf2b37f406f44d5bc56c90314c1fd7c")
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.WRITE_ESSAY, FileReader.FileType.QUESTION_WRITING, "abf2b37f406f44d5bc56c90314c1fd7c");
                //Writing attempted
                int totalWritingAttempted = evalManager.GetAttempatedPointsByQuestionType("abf2b37f406f44d5bc56c90314c1fd7c", QuestionTemplates.SUMMARIZE_TEXT, QuestionType.WRITING)
                    + evalManager.GetAttempatedPointsByQuestionType("abf2b37f406f44d5bc56c90314c1fd7c", QuestionTemplates.WRITE_ESSAY, QuestionType.WRITING);
                

                lblWritingResult.Content = string.Format("Writing score : {0} out of {1}", totalWritingAttempted, totalWriting);

                //Speaking
                int totalSpeaking = evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_WRITE, FileReader.FileType.QUESTION_SPEAKING, "abf2b37f406f44d5bc56c90314c1fd7c")
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_READ, FileReader.FileType.QUESTION_SPEAKING, "abf2b37f406f44d5bc56c90314c1fd7c")
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_LOOK, FileReader.FileType.QUESTION_SPEAKING, "abf2b37f406f44d5bc56c90314c1fd7c")
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LOOK_SPEAK_LISTEN, FileReader.FileType.QUESTION_SPEAKING, "abf2b37f406f44d5bc56c90314c1fd7c")
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_LISTEN, FileReader.FileType.QUESTION_SPEAKING, "abf2b37f406f44d5bc56c90314c1fd7c");
                //Speaking attempted

                lblSpeakingResult.Content = string.Format("Speaking score : {0} out of {1}", 0, totalSpeaking);

                //Reading

                //Listening
                int totalListening = evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_WRITE, FileReader.FileType.QUESTION_LISTENING, "abf2b37f406f44d5bc56c90314c1fd7c");

                //Getting the attempted score

                //Display module wise
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
