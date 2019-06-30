using Newtonsoft.Json;
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
using Path = System.IO.Path;

namespace TPAPanacea.Templates.Common
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        static string baseOutputDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Data//Temp//";
        List<ReportType> reportTypes = null;
        List<Set> sets = null;
        List<User> users = null;
        public Reports()
        {
            InitializeComponent();
            this.Loaded += Reports_Loaded;
        }

        private void Reports_Loaded(object sender, RoutedEventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            reportTypes = new List<ReportType>();
            reportTypes.Add(new ReportType()
            {
                ReportName = "Time analysis",
                TypeValue = "timeanalysis"
            });
            reportTypes.Add(new ReportType()
            {
                ReportName = "Score analysis",
                TypeValue = "scoreanalysis"
            });

            cmbType.ItemsSource = reportTypes;
            cmbType.DisplayMemberPath = "ReportName";
            cmbType.SelectedValuePath = "TypeValue";

            sets = new List<Set>();
            sets.Add(new Set()
            {
                Id = "94fbb82b4fd1485eb61cc3c671da384d",
                Name = "Practice set 1"
            });
            sets.Add(new Set()
            {
                Id = "7fdd81499d6147a1b4a817e56db88024",
                Name = "Practice set 2"
            });
            sets.Add(new Set()
            {
                Id = "88f631b360734b7da3b7c3e6151bd840",
                Name = "Practice set 3"
            });
            sets.Add(new Set()
            {
                Id = "7b04cda952c144788c9d27aa2d5dc710",
                Name = "Practice set 4"
            });
            sets.Add(new Set()
            {
                Id = "d9d885229cf64aceb84b19095b1ffd9c",
                Name = "Practice set 5"
            });
            sets.Add(new Set()
            {
                Id = "94fbb82b4fd1485eb61cc3c671da384d",
                Name = "Mock set 1"
            });
            sets.Add(new Set()
            {
                Id = "7fdd81499d6147a1b4a817e56db88024",
                Name = "Mock set 2"
            });
            sets.Add(new Set()
            {
                Id = "88f631b360734b7da3b7c3e6151bd840",
                Name = "Mock set 3"
            });
            sets.Add(new Set()
            {
                Id = "7b04cda952c144788c9d27aa2d5dc710",
                Name = "Mock set 4"
            });
            sets.Add(new Set()
            {
                Id = "d9d885229cf64aceb84b19095b1ffd9c",
                Name = "Mock set 5"
            });

            cmbSet.ItemsSource = sets;
            cmbSet.DisplayMemberPath = "Name";
            cmbSet.SelectedValuePath = "Id";

            users = UserManager.GetUsers();
            cmbUsers.ItemsSource = users;
            cmbUsers.SelectedValuePath = "UserId";
            cmbUsers.DisplayMemberPath = "Firstname";


        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (cmbType.SelectedValue==null || cmbSet.SelectedValue == null || cmbUsers.SelectedValue==null)
            {
                System.Windows.Forms.MessageBox.Show("Please select all combinations");
                return;
            }

            if (cmbType.SelectedValue.ToString() == "scoreanalysis")
            {
                TPACache.SetItem(TPACache.STUDENT_ID_TO_EVALUATE, UserManager.GetUserById(cmbUsers.SelectedValue.ToString()), null);
                GenerateScoreAnalysisReport();
            }
            else if (cmbType.SelectedValue.ToString() == "timeanalysis")
            {
                GenerateTimeAnalysisReport();
            }
        }

        private void CmbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmbSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GenerateTimeAnalysisReport()
        {
            TPA.Templates.Common.ReportViewer rptView = new TPA.Templates.Common.ReportViewer();

            var usr = users.First(x => x.UserId == cmbUsers.SelectedValue.ToString());
            var set = sets.First(x => x.Id == cmbSet.SelectedValue.ToString());

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

            string targetUserFolder = cmbUsers.SelectedValue.ToString();
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
                
                var screenTimeTrackingInfo = JsonConvert.DeserializeObject<List<ScreenTime>>(File.ReadAllText(screenTimeTrackingFile)).Where(x=>x.PracticeSetId == set.Id).ToList();
                if (screenTimeTrackingInfo == null)
                {
                    System.Windows.Forms.MessageBox.Show("No screen time data available");
                    return;
                }

                if (!screenTimeTrackingInfo.Any())
                {
                    System.Windows.Forms.MessageBox.Show("No screen time data available");
                    return;

                }

                for (int i = 0; i < screenTimeTrackingInfo.Count; i++)
                {
                    obj[0] = i + 1;
                    obj[1] = screenTimeTrackingInfo[i].QuestionType;
                    obj[2] = CommonUtilities.GetQuestionTemplateFriendlyName (screenTimeTrackingInfo[i].QuestionTemplate);
                    obj[3] = screenTimeTrackingInfo[i].TimeSpent;
                    tableData.Rows.Add(obj);
                }

            }

            

            rptView.ReportHeader = tableHeader;
            rptView.ReportData = tableData;
            rptView.ReportTitle = string.Format("{0}:{1}",set.Name,usr.Firstname+","+usr.Lastname);
            rptView.TemplateType = "Timeanalysis";
            //obj.GenerateReport();
            rptView.Show();
        }

        private void GenerateScoreAnalysisReport()
        {
            string selectedSetId = cmbSet.SelectedValue.ToString();
            string writingResult = null;
            string listeningResult = null;
            string readingResult = null;
            string speakingResult = null;
            int totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted;
            Results.BasicEvaluation(selectedSetId, out totalSpeaking, out totalSpeakingAttempted, out totalWriting, out totalWritingAttempted, out totalReading, out totalReadingAttempted, out totalListening, out totalListeningAttempted);
            bool isIntegratedEvaluation = false;

            if (new FileReader().PerformIntegratedEvaluation(selectedSetId)) // all the questions are evaluated by teacher or automatically then only show the results out of 90
            {
                isIntegratedEvaluation = true;
                
                Results.FetchIntegratedEvaluationResults(selectedSetId, totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted, out writingResult, out listeningResult, out readingResult, out speakingResult);
            }

            TPA.Templates.Common.ReportViewer rptView = new TPA.Templates.Common.ReportViewer();

            var usr = users.First(x => x.UserId == cmbUsers.SelectedValue.ToString());
            var set = sets.First(x => x.Id == cmbSet.SelectedValue.ToString());

            DataTable tableHeader;
            DataTable tableData;
            DataTable tableReport;
            object[] obj;

            // REPORT 1 DATA
            tableHeader = new DataTable("Header");
            tableData = new DataTable("Data");
            tableReport = new DataTable("Score");

            tableHeader.Columns.Add();
            tableHeader.Rows.Add(new object[] { "Sno." });
            tableHeader.Rows.Add(new object[] { "Module" });
            tableHeader.Rows.Add(new object[] { "Score" });
            tableHeader.Rows.Add(new object[] { "Max. Score" });

            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            obj = new object[4];

            tableReport.Columns.Add("Module", typeof(string));
            tableReport.Columns.Add("Score", typeof(decimal));

            if (!isIntegratedEvaluation)
            {
                tableData.Rows.Add(new object[] { 1.ToString(), "Reading", totalReadingAttempted, totalReading });
                tableData.Rows.Add(new object[] { 2.ToString(), "Writing", totalWritingAttempted, totalWriting });
                tableData.Rows.Add(new object[] { 3.ToString(), "Listening", totalListeningAttempted, totalListening });
                tableData.Rows.Add(new object[] { 4.ToString(), "Speaking",  totalSpeakingAttempted,totalSpeaking });

                tableReport.Rows.Add("Reading", totalReadingAttempted);
                tableReport.Rows.Add("Writing", totalWritingAttempted);
                tableReport.Rows.Add("Listening", totalListeningAttempted);
                tableReport.Rows.Add("Speaking", totalSpeakingAttempted);
            }
            else
            {
                tableData.Rows.Add(new object[] { 1.ToString(), "Reading", readingResult,90 });
                tableData.Rows.Add(new object[] { 2.ToString(), "Writing", writingResult,90 });
                tableData.Rows.Add(new object[] { 3.ToString(), "Listening", listeningResult,90 });
                tableData.Rows.Add(new object[] { 4.ToString(), "Speaking",  speakingResult,90 });

                tableReport.Rows.Add("Reading", Convert.ToDecimal(readingResult));
                tableReport.Rows.Add("Writing", Convert.ToDecimal(writingResult));
                tableReport.Rows.Add("Listening", Convert.ToDecimal(listeningResult));
                tableReport.Rows.Add("Speaking", Convert.ToDecimal(speakingResult));
            }


            rptView.ReportHeader = tableHeader;
            rptView.ReportData = tableData;
            rptView.ReportGraph = tableReport;
            rptView.ReportTitle = string.Format("{0}:{1}", set.Name, usr.Firstname + "," + usr.Lastname);
            rptView.TemplateType = "Scoreanalysis";
            //obj.GenerateReport();
            rptView.Show();
        }

    }

    class ReportType
    {
        public string ReportName { get; set; }
        public string TypeValue { get; set; }
    }
}
