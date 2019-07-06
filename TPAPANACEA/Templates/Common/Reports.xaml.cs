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
                ReportName = "Test analysis report",
                TypeValue = "timeanalysis"
            });
            reportTypes.Add(new ReportType()
            {
                ReportName = "Score analysis report",
                TypeValue = "scoreanalysis"
            });
            reportTypes.Add(new ReportType()
            {
                ReportName = "Consolidated report",
                TypeValue = "consolidatedanalysis"
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
            TPACache.SetItem(TPACache.STUDENT_ID_TO_EVALUATE, UserManager.GetUserById(cmbUsers.SelectedValue.ToString()), null);
            if (cmbType.SelectedValue.ToString() == "scoreanalysis")
            {
                GenerateScoreAnalysisReport();
            }
            else if (cmbType.SelectedValue.ToString() == "timeanalysis")
            {
                
                GenerateTimeAnalysisReport();
            }
            else if (cmbType.SelectedValue.ToString() == "consolidatedanalysis")
            {
                GenerateConsolidatedReport();
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

        private void GenerateConsolidatedReport()
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
            tableHeader.Rows.Add(new object[] { "Status" });
            tableHeader.Rows.Add(new object[] { "Speaking" });
            tableHeader.Rows.Add(new object[] { "Writing" });
            tableHeader.Rows.Add(new object[] { "Reading" });
            tableHeader.Rows.Add(new object[] { "Listening" });
            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            obj = new object[6];
            var fileReader = new FileReader();
            for (int count = 0; count < sets.Count; count++)
            {
                obj[0] = count + 1;
                string selectedPracticeSetId = sets[count].Id;
                //TO DO dupolicate impelemntation as that of result for score calculation and to be moved at some common place
                string writingResult, listeningResult, readingResult, speakingResult;
                int totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted;
                Results.BasicEvaluation(selectedPracticeSetId, out totalSpeaking, out totalSpeakingAttempted, out totalWriting, out totalWritingAttempted, out totalReading, out totalReadingAttempted, out totalListening, out totalListeningAttempted);

                if (new FileReader().PerformIntegratedEvaluation(selectedPracticeSetId)) // all the questions are evaluated by teacher or automatically then only show the results out of 90
                {
                    Results.FetchIntegratedEvaluationResults(selectedPracticeSetId, totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted, out writingResult, out listeningResult, out readingResult, out speakingResult);
                    obj[1] = "Evaluated";
                    obj[2] = speakingResult;
                    obj[3] = writingResult;
                    obj[4] = readingResult;
                    obj[5] = listeningResult;
                }
                else
                {
                    string setStatus,speakingStatus,writingStatus,listeningStatus,readingStatus = string.Empty;
                    fileReader.GetPracticeSetStatusForReport(selectedPracticeSetId, out setStatus);
                    fileReader.GetModuleStatusForReport(selectedPracticeSetId, "SPEAKING", out speakingStatus);
                    fileReader.GetModuleStatusForReport(selectedPracticeSetId, "WRITING", out writingStatus);
                    fileReader.GetModuleStatusForReport(selectedPracticeSetId, "READING", out readingStatus);
                    fileReader.GetModuleStatusForReport(selectedPracticeSetId, "LISTENING", out listeningStatus);
                    obj[1] = setStatus;
                    obj[2] = speakingStatus;
                    obj[3] = writingStatus;
                    obj[4] = readingStatus;
                    obj[5] = listeningStatus;

                }

                tableData.Rows.Add(obj);
            }

            rptView.ReportHeader = tableHeader;
            rptView.ReportData = tableData;
            rptView.ReportTitle = string.Format("{0}:{1}", set.Name, usr.Firstname + "," + usr.Lastname);
            rptView.TemplateType = "Timeanalysis";
            //obj.GenerateReport();
            rptView.Show();
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
            tableHeader.Rows.Add(new object[] { "Time taken" });
            tableHeader.Rows.Add(new object[] { "Max points" });
            tableHeader.Rows.Add(new object[] { "Points obtained" });
            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            tableData.Columns.Add();
            obj = new object[6];

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

                if (screenTimeTrackingInfo.Any(x => x.QuestionType == "SPEAKING"))
                {
                    var totalTimespan = new TimeSpan(0,0,0);
                    var totalPoints = 0;
                    var obtainedPoints =0;
                    var info = screenTimeTrackingInfo.Where(x => x.QuestionType == "SPEAKING").ToList();
                    for (int i = 0; i < info.Count; i++)
                    {
                        short outMax = 0;
                        short obt = 0;
                        TimeSpan spent;
                        var result = EvaluationManager.GetResult(new QuestionBase()
                        {
                            Id = info[i].QuestionId,
                            CurrentPracticeSetId = info[i].PracticeSetId,
                            CurrentQuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), info[i].QuestionType, true),
                            QuestionTemplate = info[i].QuestionTemplate
                        });

                        obj[0] = i + 1;
                        obj[1] = "Speaking";
                        obj[2] = CommonUtilities.GetQuestionTemplateFriendlyName(info[i].QuestionTemplate);
                        obj[3] = info[i].TimeSpent;
                        obj[4] = info[i].MaxScore;
                        obj[5] = result != null ? result.Where(x => !string.IsNullOrEmpty(x.ParamScore))
                            .Sum(x => Convert.ToDecimal(x.ParamScore)).ToString() : "Not checked";
                        tableData.Rows.Add(obj);

                        TimeSpan.TryParse(info[i].TimeSpent, out spent);
                        short.TryParse(obj[4].ToString(), out outMax);
                        short.TryParse(obj[5].ToString(), out obt);

                        totalTimespan = totalTimespan.Add(spent);
                        totalPoints += outMax;
                        obtainedPoints += obt;
                    }
                    //total
                    obj[0] = "Total";
                    obj[1] = "Speaking";
                    obj[2] = "Time & Points";
                    obj[3] = totalTimespan.ToString();;
                    obj[4] = totalPoints.ToString();
                    obj[5] = obtainedPoints.ToString();

                    tableData.Rows.Add(obj);
                }
                if (screenTimeTrackingInfo.Any(x => x.QuestionType == "WRITING"))
                {
                    var totalTimespan = new TimeSpan(0, 0, 0);
                    var totalPoints = 0;
                    var obtainedPoints = 0;
                    
                    var info = screenTimeTrackingInfo.Where(x => x.QuestionType == "WRITING").ToList();
                    for (int i = 0; i < info.Count; i++)
                    {
                        short outMax = 0;
                        short obt = 0;
                        TimeSpan spent;
                        
                        var result = EvaluationManager.GetResult(new QuestionBase()
                        {
                            Id = info[i].QuestionId,
                            CurrentPracticeSetId = info[i].PracticeSetId,
                            CurrentQuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), info[i].QuestionType, true),
                            QuestionTemplate = info[i].QuestionTemplate
                        });


                        obj[0] = i + 1;
                        obj[1] = "Writing";
                        obj[2] = CommonUtilities.GetQuestionTemplateFriendlyName(info[i].QuestionTemplate);
                        obj[3] = info[i].TimeSpent;
                        obj[4] = info[i].MaxScore;
                        obj[5] = result != null ? result.Where(x => !string.IsNullOrEmpty(x.ParamScore))
                            .Sum(x => Convert.ToDecimal(x.ParamScore)).ToString() : "Not checked"; 
                        tableData.Rows.Add(obj);

                        TimeSpan.TryParse(info[i].TimeSpent, out spent);
                        short.TryParse(obj[4].ToString(), out outMax);
                        short.TryParse(obj[5].ToString(), out obt);

                        totalTimespan = totalTimespan.Add(spent);
                        totalPoints += outMax;
                        obtainedPoints += obt;
                    }
                    //total
                    obj[0] = "Total";
                    obj[1] = "Speaking";
                    obj[2] = "Time & Points";
                    obj[3] = totalTimespan.ToString(); ;
                    obj[4] = totalPoints.ToString();
                    obj[5] = obtainedPoints.ToString();

                    tableData.Rows.Add(obj);
                }
                if (screenTimeTrackingInfo.Any(x => x.QuestionType == "READING"))
                {
                    var totalTimespan = new TimeSpan(0, 0, 0);
                    var totalPoints = 0;
                    var obtainedPoints = 0;
                    
                    var info = screenTimeTrackingInfo.Where(x => x.QuestionType == "READING").ToList();
                    for (int i = 0; i < info.Count; i++)
                    {
                        short outMax = 0;
                        short obt = 0;
                        TimeSpan spent;
                        
                        var result = EvaluationManager.GetResult(new QuestionBase()
                        {
                            Id = info[i].QuestionId,
                            CurrentPracticeSetId = info[i].PracticeSetId,
                            CurrentQuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), info[i].QuestionType, true),
                            QuestionTemplate = info[i].QuestionTemplate
                        });

                        obj[0] = i + 1;
                        obj[1] = "Reading";
                        obj[2] = CommonUtilities.GetQuestionTemplateFriendlyName(info[i].QuestionTemplate);
                        obj[3] = info[i].TimeSpent;
                        obj[4] = info[i].MaxScore;
                        obj[5] = result != null ? result.Where(x => !string.IsNullOrEmpty(x.ParamScore))
                            .Sum(x => Convert.ToDecimal(x.ParamScore)).ToString() : "Not checked";
                        tableData.Rows.Add(obj);

                        TimeSpan.TryParse(info[i].TimeSpent, out spent);
                        short.TryParse(obj[4].ToString(), out outMax);
                        short.TryParse(obj[5].ToString(), out obt);

                        totalTimespan = totalTimespan.Add(spent);
                        totalPoints += outMax;
                        obtainedPoints += obt;
                    }
                    //total
                    obj[0] = "Total";
                    obj[1] = "Speaking";
                    obj[2] = "Time & Points";
                    obj[3] = totalTimespan.ToString(); ;
                    obj[4] = totalPoints.ToString();
                    obj[5] = obtainedPoints.ToString();

                    tableData.Rows.Add(obj);
                }
               

                if (screenTimeTrackingInfo.Any(x => x.QuestionType == "LISTENING"))
                {
                    var totalTimespan = new TimeSpan(0, 0, 0);
                    var totalPoints = 0;
                    var obtainedPoints = 0;
                    
                    var info = screenTimeTrackingInfo.Where(x => x.QuestionType == "LISTENING").ToList();
                    for (int i = 0; i < info.Count; i++)
                    {
                        short outMax = 0;
                        short obt = 0;
                        TimeSpan spent;
                        
                        var result = EvaluationManager.GetResult(new QuestionBase()
                        {
                            Id = info[i].QuestionId,
                            CurrentPracticeSetId = info[i].PracticeSetId,
                            CurrentQuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), info[i].QuestionType, true),
                            QuestionTemplate = info[i].QuestionTemplate
                        });

                        obj[0] = i + 1;
                        obj[1] = "Listening";
                        obj[2] = CommonUtilities.GetQuestionTemplateFriendlyName(info[i].QuestionTemplate);
                        obj[3] = info[i].TimeSpent;
                        obj[4] = info[i].MaxScore;
                        obj[5] = result != null ? result.Where(x => !string.IsNullOrEmpty(x.ParamScore))
                            .Sum(x => Convert.ToDecimal(x.ParamScore)).ToString() : "Not checked"; ;;
                        tableData.Rows.Add(obj);

                        TimeSpan.TryParse(info[i].TimeSpent, out spent);
                        short.TryParse(obj[4].ToString(), out outMax);
                        short.TryParse(obj[5].ToString(), out obt);

                        totalTimespan = totalTimespan.Add(spent);
                        totalPoints += outMax;
                        obtainedPoints += obt;
                    }
                    //total
                    obj[0] = "Total";
                    obj[1] = "Speaking";
                    obj[2] = "Time & Points";
                    obj[3] = totalTimespan.ToString();
                    obj[4] = totalPoints.ToString();
                    obj[5] = obtainedPoints.ToString();

                    tableData.Rows.Add(obj);
                }

                //for (int i = 0; i < screenTimeTrackingInfo.Count; i++)
                //{
                //    obj[0] = i + 1;
                //    obj[1] = screenTimeTrackingInfo[i].QuestionType;
                //    obj[2] = CommonUtilities.GetQuestionTemplateFriendlyName (screenTimeTrackingInfo[i].QuestionTemplate);
                //    obj[3] = screenTimeTrackingInfo[i].TimeSpent;
                //    tableData.Rows.Add(obj);
                //}

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
            tableHeader.Rows.Add(new object[] { "Module" });
            tableHeader.Rows.Add(new object[] { "Score" });
            
            tableData.Columns.Add();
            tableData.Columns.Add();
            obj = new object[2];

            tableReport.Columns.Add("Module", typeof(string));
            tableReport.Columns.Add("Score", typeof(decimal));

            if (!isIntegratedEvaluation)
            {
                tableData.Rows.Add(new object[] { "Listening", totalListeningAttempted });
                tableData.Rows.Add(new object[] { "Reading", totalReadingAttempted });
                tableData.Rows.Add(new object[] { "Speaking",  totalSpeakingAttempted });
                tableData.Rows.Add(new object[] { "Writing", totalWritingAttempted });
                

                
                tableReport.Rows.Add("Listening", totalListeningAttempted);
                tableReport.Rows.Add("Reading", totalReadingAttempted);
                
                tableReport.Rows.Add("Speaking", totalSpeakingAttempted);
                tableReport.Rows.Add("Writing", totalWritingAttempted);
            }
            else
            {
                
                
                tableData.Rows.Add(new object[] {  "Listening", listeningResult });
                tableData.Rows.Add(new object[] { "Reading", readingResult });
                tableData.Rows.Add(new object[] {  "Speaking",  speakingResult });
                tableData.Rows.Add(new object[] { "Writing", writingResult });

                tableReport.Rows.Add("Listening", Convert.ToDecimal(listeningResult));
                tableReport.Rows.Add("Reading", Convert.ToDecimal(readingResult));
                tableReport.Rows.Add("Speaking", Convert.ToDecimal(speakingResult));
                tableReport.Rows.Add("Writing", Convert.ToDecimal(writingResult));
                
                
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
