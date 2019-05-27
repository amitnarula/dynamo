using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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

                int totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted;
                BasicEvaluation(selectedPracticeSetId, out totalSpeaking, out totalSpeakingAttempted, out totalWriting, out totalWritingAttempted, out totalReading, out totalReadingAttempted, out totalListening, out totalListeningAttempted);

                lblWritingResult.Content = string.Format("Writing score : {0} out of {1}", totalWritingAttempted, totalWriting);
                lblReadingResult.Content = string.Format("Reading score : {0} out of {1}", totalReadingAttempted, totalReading);
                lblListeningResult.Content = string.Format("Listening score : {0} out of {1}", totalListeningAttempted, totalListening);
                lblSpeakingResult.Content = string.Format("Speaking score : {0} out of {1}",
                    totalSpeakingAttempted, totalSpeaking); //Speaking evaluation

                // GenerateReport(cmbPracticeSet.SelectedItem.ToString(), totalReadingAttempted, totalWritingAttempted, totalListeningAttempted, totalSpeakingAttempted);
                if (new FileReader().PerformIntegratedEvaluation(selectedPracticeSetId)) // all the questions are evaluated by teacher or automatically then only show the results out of 90
                {
                    IntegratedEvaluation(selectedPracticeSetId, totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void BasicEvaluation(string selectedPracticeSetId, out int totalSpeaking, out int totalSpeakingAttempted, out int totalWriting, out int totalWritingAttempted, out int totalReading, out int totalReadingAttempted, out int totalListening, out int totalListeningAttempted)
        {
            DataSet dsEvalParams = FileReader.ReadFile(FileReader.FileType.EVALUATION_PARAMETER);

            var evalManager = new EvaluationManager();
            //Speaking
            totalSpeaking = evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_ANSWER_SHORT_QUESTION, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId)
            + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_READ, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId)
            + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_LOOK, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId)
            + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LOOK_SPEAK_LISTEN, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId)
            + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_LISTEN, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId);

            //Speaking attempted
            totalSpeakingAttempted = evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SPEAK_ANSWER_SHORT_QUESTION, QuestionType.SPEAKING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SPEAK_READ, QuestionType.SPEAKING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SPEAK_LOOK, QuestionType.SPEAKING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LOOK_SPEAK_LISTEN, QuestionType.SPEAKING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SPEAK_LISTEN, QuestionType.SPEAKING);

            //Writing
            totalWriting = evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SUMMARIZE_TEXT, FileReader.FileType.QUESTION_WRITING, selectedPracticeSetId)
            + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.WRITE_ESSAY, FileReader.FileType.QUESTION_WRITING, selectedPracticeSetId);

            //Writing attempted
            totalWritingAttempted = evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SUMMARIZE_TEXT, QuestionType.WRITING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.WRITE_ESSAY, QuestionType.WRITING);

            //Reading
            totalReading = evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.FILL_IN_BLANK_WITH_OPTIONS, FileReader.FileType.QUESTION_READING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.FILL_IN_BLANKS, FileReader.FileType.QUESTION_READING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.MULTI_CHOICE_MULTIPLE_ANSWER, FileReader.FileType.QUESTION_READING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.MULTI_CHOICE_SINGLE_ANSWER, FileReader.FileType.QUESTION_READING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.REORDER, FileReader.FileType.QUESTION_READING, selectedPracticeSetId);
            //Reading attempted
            totalReadingAttempted = evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.FILL_IN_BLANK_WITH_OPTIONS, QuestionType.READING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.FILL_IN_BLANKS, QuestionType.READING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.MULTI_CHOICE_MULTIPLE_ANSWER, QuestionType.READING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.MULTI_CHOICE_SINGLE_ANSWER, QuestionType.READING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.REORDER, QuestionType.READING);

            //Listening
            totalListening = evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_DICTATE, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_FILL_BLANKS, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_HIGHLIGHT, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_HIGHLIGHT_CORRECT_SUMMARY, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_MULTI_CHOICE, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_MULTI_SELECT, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_SELECT_MISSING_WORD, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
            + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_WRITE, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId);

            //Listening attempted
            totalListeningAttempted = evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_DICTATE, QuestionType.LISTENING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_FILL_BLANKS, QuestionType.LISTENING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_HIGHLIGHT, QuestionType.LISTENING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_HIGHLIGHT_CORRECT_SUMMARY, QuestionType.LISTENING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_MULTI_CHOICE, QuestionType.LISTENING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_MULTI_SELECT, QuestionType.LISTENING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_SELECT_MISSING_WORD, QuestionType.LISTENING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_WRITE, QuestionType.LISTENING);
        }

        private void GenerateReport(string practiceSetName, int readingScore, int writingScore, int listeningScore, int speakingScore)
        {
            TPA.Templates.Common.ReportViewer rptView = new TPA.Templates.Common.ReportViewer();

            //DataTable tableHeader = new DataTable("Header");
            //tableHeader.Rows.Add(new object[] { 1.ToString(), "READING", readingScore.ToString() });
            ////tableHeader.Columns.Add("Sno.", typeof(string));
            ////tableHeader.Columns.Add("Module", typeof(string));
            ////tableHeader.Columns.Add("Score", typeof(string));

            //DataTable tableData = new DataTable("Data");
            //// randomly create some items
            //tableData.Rows.Add(new object[] { 1.ToString(), "READING", readingScore.ToString() });
            //tableData.Rows.Add(new object[] { 2.ToString(), "WRITING", writingScore.ToString() });
            //tableData.Rows.Add(new object[] { 3.ToString(), "LISTENING", listeningScore.ToString() });
            //tableData.Rows.Add(new object[] { 4.ToString(), "SPEAKING", speakingScore.ToString() });

            DataTable tableHeader;
            DataTable tableData;
            object[] obj;
            
            // REPORT 1 DATA
            tableHeader = new DataTable("Header");
            tableData = new DataTable("Data");

            tableHeader.Columns.Add();
            tableHeader.Rows.Add(new object[] { "Sno." });
            tableHeader.Rows.Add(new object[] { "Module" });
            tableHeader.Rows.Add(new object[] { "Type" });
            tableHeader.Rows.Add(new object[] { "Time" });
            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            obj = new object[4];
            
            string targetUserFolder = CommonUtilities.ResolveTargetUserFolder();
            if (!string.IsNullOrEmpty(targetUserFolder))
            {
                targetUserFolder = Path.Combine(baseOutputDirectory, targetUserFolder); //full path
                string screenTimeTrackingFile = Path.Combine(targetUserFolder, "st.json");
                if (!File.Exists(screenTimeTrackingFile))
                {
                    System.Windows.Forms.MessageBox.Show("No screen time data available");
                    return;
                }


                //read existing file
                var screenTimeTrackingInfo = JsonConvert.DeserializeObject<List<ScreenTime>>(File.ReadAllText(screenTimeTrackingFile));
                if (screenTimeTrackingInfo == null)
                {
                    System.Windows.Forms.MessageBox.Show("No screen time data available");
                }

                for (int i = 0; i < screenTimeTrackingInfo.Count; i++)
                {
                    obj[0] = i+1;
                    obj[1] = screenTimeTrackingInfo[i].QuestionType;
                    obj[2] = screenTimeTrackingInfo[i].QuestionTemplate;
                    obj[3] = screenTimeTrackingInfo[i].TimeSpent;
                    tableData.Rows.Add(obj);
                }

            }

            rptView.ReportHeader = tableHeader;
            rptView.ReportData = tableData;
            rptView.ReportTitle = "Practice set 1 , Amit Narula";
            rptView.TemplateType = "TimeAnalysis";
            //obj.GenerateReport();
            rptView.Show();
        }

        private void IntegratedEvaluation(string selectedPracticeSetId, int totalSpeaking, int totalSpeakingAttempted, int totalWriting, int totalWritingAttempted, int totalReading, int totalReadingAttempted, int totalListening, int totalListeningAttempted)
        {
            //INTEGRATED RESULTS OUT OF 90 are only visible when all four modules of the practice set are submitted
            //Check module submission files
            //string[] moduleSubmissionFiles = Directory.GetFiles(baseOutputDirectory, selectedPracticeSetId + "_SUB_*.xml");


            string writingResult, listeningResult, readingResult, speakingResult;
            FetchIntegratedEvaluationResults(selectedPracticeSetId, totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted, out writingResult, out listeningResult, out readingResult, out speakingResult);

            lblWritingResult.Content = writingResult;

            lblListeningResult.Content = listeningResult;


            lblReadingResult.Content = readingResult;


            //SPEAKING
            lblSpeakingResult.Content = speakingResult;

            //}


        }

        public static void FetchIntegratedEvaluationResults(string selectedPracticeSetId, int totalSpeaking, int totalSpeakingAttempted, int totalWriting, int totalWritingAttempted, int totalReading, int totalReadingAttempted, int totalListening, int totalListeningAttempted, out string writingResult, out string listeningResult, out string readingResult, out string speakingResult)
        {
            var evalManager = new EvaluationManager();
            DataSet dsEvalParams = FileReader.ReadFile(FileReader.FileType.EVALUATION_PARAMETER);

            //if (moduleSubmissionFiles.Count() == 4) // for 4 modules WRITING,LISTENING,READING,SPEAKING
            //{
            //WRITING
            int totalWritingIntegrated = totalWriting
           + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.FILL_IN_BLANK_WITH_OPTIONS, FileReader.FileType.QUESTION_READING, selectedPracticeSetId)
           + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.FILL_IN_BLANKS, FileReader.FileType.QUESTION_READING, selectedPracticeSetId)
           + ((evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_WRITE, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
           + (evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_FILL_BLANKS, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId) / 2)
           + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_DICTATE, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)));

            int totalWritingIntegratedAttempted = totalWritingAttempted +
                (
                evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.FILL_IN_BLANK_WITH_OPTIONS, QuestionType.READING)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.FILL_IN_BLANKS, QuestionType.READING)
                + (evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_WRITE, QuestionType.LISTENING)
                + (evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_FILL_BLANKS, QuestionType.LISTENING) / 2)
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_DICTATE, QuestionType.LISTENING)));




            //LISTENING
            int totalListeningIntegrated = totalListening
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_ANSWER_SHORT_QUESTION, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId)
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LOOK_SPEAK_LISTEN, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId, "Content")
            + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_LISTEN, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId, "Content");

            int totalListeningAttemptedIntegrated = totalListeningAttempted
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LOOK_SPEAK_LISTEN, QuestionType.SPEAKING, "Content")
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SPEAK_LISTEN, QuestionType.SPEAKING, "Content");



            //READING
            int totalReadingIntegrated = totalReading
                + (
                Convert.ToInt32(
                90 * (
                (evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_READ, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId, "Content")
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SPEAK_READ, FileReader.FileType.QUESTION_SPEAKING, selectedPracticeSetId, "Oral Fluency")) / (double)totalSpeaking)
                )
                +
                (evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SUMMARIZE_TEXT, FileReader.FileType.QUESTION_WRITING, selectedPracticeSetId, "Content")
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.SUMMARIZE_TEXT, FileReader.FileType.QUESTION_WRITING, selectedPracticeSetId, "Grammar")
                )
                + evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_HIGHLIGHT_CORRECT_SUMMARY, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId)
                + (evalManager.GetTotalPointsByType(dsEvalParams, QuestionTemplates.LISTEN_AND_HIGHLIGHT, FileReader.FileType.QUESTION_LISTENING, selectedPracticeSetId) / 2)
                );

            int totalReadingAttemptedIntegrated = totalReadingAttempted
                +
                (
                Convert.ToInt32(((evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SPEAK_READ, QuestionType.SPEAKING, "Content")
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SPEAK_READ, QuestionType.SPEAKING, "Oral Fluency")) / (double)totalSpeaking) * 90)
                +
                (
                evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SUMMARIZE_TEXT, QuestionType.WRITING, "Content")
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.SUMMARIZE_TEXT, QuestionType.WRITING, "Grammar")
                )
                + evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_HIGHLIGHT, QuestionType.LISTENING)
                + (evalManager.GetAttempatedPointsByQuestionType(selectedPracticeSetId, QuestionTemplates.LISTEN_AND_HIGHLIGHT, QuestionType.LISTENING) / 2)
                );

            writingResult = string.Format("Writing score : {0} out of 90", ((((float)totalWritingIntegratedAttempted) / totalWritingIntegrated) * 90).ToString("0"));
            listeningResult = string.Format("Listening score : {0} out of 90", ((((float)totalListeningAttemptedIntegrated) / totalListeningIntegrated) * 90).ToString("0"));
            readingResult = string.Format("Reading score : {0} out of 90", ((((float)totalReadingAttemptedIntegrated) / totalReadingIntegrated) * 90).ToString("0"));
            speakingResult = string.Format("Speaking score : {0} out of 90",
((((float)totalSpeakingAttempted) / totalSpeaking) * 90).ToString("0"));
            //Speaking evaluation
        }
    }
}
