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
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmbPracticeSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                var selectedPracticeSetId = ((ComboBoxItem)(cmbPracticeSet.SelectedItem)).Tag.ToString();
                var evalManager = new EvaluationManager();

                //EvaluationManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.MULTI_CHOICE_MULTIPLE_ANSWER, QuestionType.READING);

                DataSet dsEvalParams = FileReader.ReadFile(FileReader.FileType.EVALUATION_PARAMETER);

                //Speaking
                int totalSpeaking = evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_ANSWER_SHORT_QUESTION, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_READ, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_LOOK, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LOOK_SPEAK_LISTEN, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_LISTEN, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId);
                //Speaking attempted
                int totalSpeakingAttempted = evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SPEAK_ANSWER_SHORT_QUESTION, QuestionType.SPEAKING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SPEAK_READ, QuestionType.SPEAKING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SPEAK_LOOK, QuestionType.SPEAKING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LOOK_SPEAK_LISTEN, QuestionType.SPEAKING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SPEAK_LISTEN, QuestionType.SPEAKING);


                lblSpeakingResult.Content = string.Format("Speaking score : {0} out of {1} {2}/90"
                    , totalSpeakingAttempted, 
                    totalSpeaking,
                    (((float)totalSpeakingAttempted) / totalSpeaking) * 90);


                //Reading
                int totalReading = evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.FILL_IN_BLANK_WITH_OPTIONS, FileReader.FileType.QUESTION_READING, selectedPracticeSetId)
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.FILL_IN_BLANKS, FileReader.FileType.QUESTION_READING, selectedPracticeSetId)
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.MULTI_CHOICE_MULTIPLE_ANSWER, FileReader.FileType.QUESTION_READING, selectedPracticeSetId)
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.MULTI_CHOICE_SINGLE_ANSWER, FileReader.FileType.QUESTION_READING, selectedPracticeSetId)
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.REORDER, FileReader.FileType.QUESTION_READING, selectedPracticeSetId);
                //Reading attempted
                int totalReadingAttempted = evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.FILL_IN_BLANK_WITH_OPTIONS, QuestionType.READING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.FILL_IN_BLANKS, QuestionType.READING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.MULTI_CHOICE_MULTIPLE_ANSWER, QuestionType.READING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.MULTI_CHOICE_SINGLE_ANSWER, QuestionType.READING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.REORDER, QuestionType.READING);

                lblReadingResult.Content = string.Format("Reading score : {0} out of {1}", totalReadingAttempted, totalReading);

                int totalListening = evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_DICTATE, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_FILL_BLANKS, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_HIGHLIGHT, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_HIGHLIGHT_CORRECT_SUMMARY, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_MULTI_CHOICE, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_MULTI_SELECT, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_SELECT_MISSING_WORD, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_WRITE, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId);


                int totalListeningAttempted = evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_DICTATE, QuestionType.LISTENING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_FILL_BLANKS, QuestionType.LISTENING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_HIGHLIGHT, QuestionType.LISTENING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_HIGHLIGHT_CORRECT_SUMMARY, QuestionType.LISTENING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_MULTI_CHOICE, QuestionType.LISTENING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_MULTI_SELECT, QuestionType.LISTENING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_SELECT_MISSING_WORD, QuestionType.LISTENING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_WRITE, QuestionType.LISTENING);
                lblListeningResult.Content = string.Format("Listening score : {0} out of {1}", totalListeningAttempted, totalListening);

                //Writing
                int totalWriting = evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SUMMARIZE_TEXT, FileReader.FileType.QUESTION_WRITING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.WRITE_ESSAY, FileReader.FileType.QUESTION_WRITING, selectedPracticeSetId);
                //Writing attempted
                int totalWritingAttempted = evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SUMMARIZE_TEXT, QuestionType.WRITING)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.WRITE_ESSAY, QuestionType.WRITING);

                lblWritingResult.Content = string.Format("Writing score : {0} out of {1}", totalWritingAttempted, totalWriting);

                int totalWritingIntegrated = totalWriting
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.FILL_IN_BLANK_WITH_OPTIONS, FileReader.FileType.QUESTION_READING, selectedPracticeSetId)
                    + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_WRITE, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                    + (evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_FILL_BLANKS, QuestionType.LISTENING) / 2)
                    + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_DICTATE, QuestionType.LISTENING);


                int totalSpeakingIntegrated = totalSpeaking;
                int totalListeningIntegrated = 0;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
