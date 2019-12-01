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
                ReportName = "Detailed analysis report",
                TypeValue = "detailedanalysis"
            });
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
                ReportName = "Consolidated analysis report",
                TypeValue = "consolidatedanalysis"
            });

            cmbType.ItemsSource = reportTypes;
            cmbType.DisplayMemberPath = "ReportName";
            cmbType.SelectedValuePath = "TypeValue";

            sets = new List<Set>();
            //sets.Add(new Set()
            //{
            //    Id = "94fbb82b4fd1485eb61cc3c671da384d",
            //    Name = "Practice set 1"
            //});
            //sets.Add(new Set()
            //{
            //    Id = "7fdd81499d6147a1b4a817e56db88024",
            //    Name = "Practice set 2"
            //});
            //sets.Add(new Set()
            //{
            //    Id = "88f631b360734b7da3b7c3e6151bd840",
            //    Name = "Practice set 3"
            //});
            //sets.Add(new Set()
            //{
            //    Id = "7b04cda952c144788c9d27aa2d5dc710",
            //    Name = "Practice set 4"
            //});
            //sets.Add(new Set()
            //{
            //    Id = "d9d885229cf64aceb84b19095b1ffd9c",
            //    Name = "Practice set 5"
            //});
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
            sets.Add(new Set()
            {
                Id = "5dcbc550333c488996e65d41dad0bfe7",
                Name = "Mock set 6"
            });
            sets.Add(new Set()
            {
                Id = "0b7002c915a441eebdb4448e4388ba5b",
                Name = "Mock set 7"
            });
            sets.Add(new Set()
            {
                Id = "05424cf5c42745de8f6fdef719e5351b",
                Name = "Mock set 8"
            });
            sets.Add(new Set()
            {
                Id = "db28642c43f44f048e0e1be5cf8984f6",
                Name = "Mock set 9"
            });
            sets.Add(new Set()
            {
                Id = "4e1a53fbe93d491289754abda2ea6984",
                Name = "Mock set 10"
            });


            cmbSet.ItemsSource = sets;
            cmbSet.DisplayMemberPath = "Name";
            cmbSet.SelectedValuePath = "Id";

            users = UserManager.GetUsers();
            cmbUsers.ItemsSource = users;
            cmbUsers.SelectedValuePath = "UserId";
            cmbUsers.DisplayMemberPath = "Firstname";


            //cmbSet.SelectedIndex = 0;
            //cmbUsers.SelectedIndex = 0;
            //cmbType.SelectedIndex = 0;
            //BtnGenerate_Click(null, null);


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
            else if (cmbType.SelectedValue.ToString() == "detailedanalysis")
            {
                GenerateDetailedReport();
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

        private void GenerateNewReport() {
        
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
                List<ParameterizedScore> parameterizedScore = new List<ParameterizedScore>();
                List<ParameterizedScore> parameterizedMaxScore = new List<ParameterizedScore>();

                obj[0] = count + 1;
                string selectedPracticeSetId = sets[count].Id;
                //TO DO dupolicate impelemntation as that of result for score calculation and to be moved at some common place
                string writingResult, listeningResult, readingResult, speakingResult;
                int totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted;
                Results.BasicEvaluation(selectedPracticeSetId, out totalSpeaking, out totalSpeakingAttempted, out totalWriting, out totalWritingAttempted, out totalReading, out totalReadingAttempted, out totalListening, out totalListeningAttempted, ref parameterizedScore,ref parameterizedMaxScore);

                if (new FileReader().PerformIntegratedEvaluation(selectedPracticeSetId)) // all the questions are evaluated by teacher or automatically then only show the results out of 90
                {
                   var resultDic = Results.FetchIntegratedEvaluationResults(selectedPracticeSetId, totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted, out writingResult, out listeningResult, out readingResult, out speakingResult);
                    obj[1] = "Evaluated";
                    obj[2] = Convert.ToInt16(resultDic["Speaking"])+"/90";
                    obj[3] = Convert.ToInt16(resultDic["Writing"]) + "/90";
                    obj[4] = Convert.ToInt16(resultDic["Reading"]) + "/90";
                    obj[5] = Convert.ToInt16(resultDic["Listening"]) + "/90";
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
            rptView.ReportTitle = string.Format("{0}", usr.Firstname + "," + usr.Lastname);

            
            rptView.TemplateType = "ConsolidatedAnalysis";
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
                    obj[1] = "Writing";
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
                    obj[1] = "Reading";
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
                    obj[1] = "Listening";
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
            List<ParameterizedScore> parameterizedScore = new List<ParameterizedScore>();
            List<ParameterizedScore> parameterizedMaxScore = new List<ParameterizedScore>();
            int totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted;
            Results.BasicEvaluation(selectedSetId, out totalSpeaking, out totalSpeakingAttempted, out totalWriting, out totalWritingAttempted, out totalReading, out totalReadingAttempted, out totalListening, out totalListeningAttempted, ref parameterizedScore, ref parameterizedMaxScore);
            bool isIntegratedEvaluation = false;
            Dictionary<string, double> resultDic = null;

            if (new FileReader().PerformIntegratedEvaluation(selectedSetId)) // all the questions are evaluated by teacher or automatically then only show the results out of 90
            {
                isIntegratedEvaluation = true;

                resultDic = Results.FetchIntegratedEvaluationResults(selectedSetId, totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted, out writingResult, out listeningResult, out readingResult, out speakingResult);
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


                tableData.Rows.Add(new object[] { "Listening", Convert.ToInt16(resultDic["Listening"])<10?10: Convert.ToInt16(resultDic["Listening"])});
                tableData.Rows.Add(new object[] { "Reading", Convert.ToInt16( resultDic["Reading"])<10?10:Convert.ToInt16( resultDic["Reading"]) });
                tableData.Rows.Add(new object[] { "Speaking",  Convert.ToInt16(resultDic["Speaking"])<10?10: Convert.ToInt16(resultDic["Speaking"])});
                tableData.Rows.Add(new object[] { "Writing", Convert.ToInt16(resultDic["Writing"]) < 10 ? 10 : Convert.ToInt16(resultDic["Writing"]) });

                tableReport.Rows.Add("Listening", resultDic["Listening"] < 10 ? 10 : resultDic["Listening"]);
                tableReport.Rows.Add("Reading", resultDic["Reading"]<10?10:resultDic["Reading"]);
                tableReport.Rows.Add("Speaking", resultDic["Speaking"]<10?10:resultDic["Speaking"]);
                tableReport.Rows.Add("Writing", resultDic["Writing"]<10?10:resultDic["Writing"]);
                
                
            }


            rptView.ReportHeader = tableHeader;
            rptView.ReportData = tableData;
            rptView.ReportGraph = tableReport;
            rptView.ReportTitle = string.Format("{0}:{1}", set.Name, usr.Firstname + "," + usr.Lastname);
            rptView.TemplateType = "Scoreanalysis";
            //obj.GenerateReport();
            rptView.Show();
        }

        private void GenerateDetailedReport()
        {
            string selectedSetId = cmbSet.SelectedValue.ToString();
            string writingResult = null;
            string listeningResult = null;
            string readingResult = null;
            string speakingResult = null;

            decimal grammar = 0, pronunciation = 0, vocabulary = 0, oralFluency = 0, spelling = 0, writtenDiscourse = 0;
            decimal grammarTotal = 0, pronunciationTotal = 0, vocabularyTotal = 0, oralFluencyTotal = 0, spellingTotal = 0, writtenDiscourseTotal = 0;

            List<ParameterizedScore> parameterizedScore = new List<ParameterizedScore>();
            List<ParameterizedScore> parameterizedMaxScore = new List<ParameterizedScore>();

            int totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted;
            Results.BasicEvaluation(selectedSetId, out totalSpeaking, out totalSpeakingAttempted, out totalWriting, out totalWritingAttempted, out totalReading, out totalReadingAttempted, out totalListening, out totalListeningAttempted, ref parameterizedScore, ref parameterizedMaxScore);
            bool isIntegratedEvaluation = false;
            Dictionary<string, double> resultDic = null;

            if (new FileReader().PerformIntegratedEvaluation(selectedSetId)) // all the questions are evaluated by teacher or automatically then only show the results out of 90
            {
                isIntegratedEvaluation = true;

                resultDic = Results.FetchIntegratedEvaluationResults(selectedSetId, totalSpeaking, totalSpeakingAttempted, totalWriting, totalWritingAttempted, totalReading, totalReadingAttempted, totalListening, totalListeningAttempted, out writingResult, out listeningResult, out readingResult, out speakingResult);
            }
            else {
                System.Windows.Forms.MessageBox.Show("No screen time data available");
                return;
            }

            TPA.Templates.Common.ReportViewer rptView = new TPA.Templates.Common.ReportViewer();

            var usr = users.First(x => x.UserId == cmbUsers.SelectedValue.ToString());
            var set = sets.First(x => x.Id == cmbSet.SelectedValue.ToString());

            DataTable tableHeader;
            DataTable tableData;
            DataTable tableReport;
            DataTable tableReport2;
            object[] obj;

            // REPORT 1 DATA
            tableHeader = new DataTable("Header");
            tableData = new DataTable("Data");
            tableReport = new DataTable("Score");
            tableReport2 = new DataTable("Skill");

            tableHeader.Columns.Add();
            tableHeader.Rows.Add(new object[] { "Module" });
            tableHeader.Rows.Add(new object[] { "Score" });

            tableData.Columns.Add();
            tableData.Columns.Add();
            obj = new object[2];

            tableReport.Columns.Add("Module", typeof(string));
            tableReport.Columns.Add("Score", typeof(decimal));
            tableReport2.Columns.Add("Skill", typeof(string));
            tableReport2.Columns.Add("Score", typeof(decimal));


            if (!isIntegratedEvaluation)
            {
                tableData.Rows.Add(new object[] { "Listening", totalListeningAttempted });
                tableData.Rows.Add(new object[] { "Reading", totalReadingAttempted });
                tableData.Rows.Add(new object[] { "Speaking", totalSpeakingAttempted });
                tableData.Rows.Add(new object[] { "Writing", totalWritingAttempted });

                
                tableReport.Rows.Add("Writing", totalWritingAttempted);
                tableReport.Rows.Add("Speaking", totalSpeakingAttempted);
                tableReport.Rows.Add("Reading", totalReadingAttempted);
                tableReport.Rows.Add("Listening", totalListeningAttempted);
                
                tableReport2.Rows.Add("Written Discourse", writtenDiscourse);
                tableReport2.Rows.Add("Vocabulary", vocabulary);
                tableReport2.Rows.Add("Spelling", spelling);
                tableReport2.Rows.Add("Pronunciation", pronunciation);
                tableReport2.Rows.Add("Oral Fluency", oralFluency);
                tableReport2.Rows.Add("Grammar", grammar);


            }
            else
            {


                tableData.Rows.Add(new object[] { "Listening", listeningResult });
                tableData.Rows.Add(new object[] { "Reading", readingResult });
                tableData.Rows.Add(new object[] { "Speaking", speakingResult });
                tableData.Rows.Add(new object[] { "Writing", writingResult });


                tableReport.Rows.Add("Writing", resultDic["Writing"] < 10 ? 10 : resultDic["Writing"]);
                tableReport.Rows.Add("Speaking", resultDic["Speaking"] < 10 ? 10 : resultDic["Speaking"]);
                tableReport.Rows.Add("Reading", resultDic["Reading"]<10?10:resultDic["Reading"]);
                tableReport.Rows.Add("Listening", resultDic["Listening"]<10?10:resultDic["Listening"]);

                var resParameterizedScore = parameterizedScore.GroupBy(x => x.ParamName).Select(g => new {
                   Parameter = g.Key,
                   Score = g.Sum(x=>x.ParamScore)
                });


                var resParameterizedMaxScore = parameterizedMaxScore.GroupBy(x => x.ParamName).Select(g => new
                {
                    Parameter = g.Key,
                    Score = g.Sum(x => x.ParamScore)
                });

                writtenDiscourse =resParameterizedScore.Where(x=>x.Parameter=="Structure"||x.Parameter=="Linguistic range").Sum(x=>x.Score);
                writtenDiscourseTotal = resParameterizedMaxScore.Where(x => x.Parameter == "Structure" || x.Parameter == "Linguistic range").Sum(x => x.Score);
                
                vocabulary = resParameterizedScore.Where(x => x.Parameter == "Vocabulary" || x.Parameter == "Correct or Incorrect").Sum(x => x.Score);//resParameterizedScore.SingleOrDefault(x=>x.Parameter=="Vocabulary").Score;
                vocabularyTotal = resParameterizedMaxScore.Where(x => x.Parameter == "Vocabulary" || x.Parameter == "Correct or Incorrect").Sum(x => x.Score);//resParameterizedScore.SingleOrDefault(x=>x.Parameter=="Vocabulary").Score;
                

                spelling=resParameterizedScore.SingleOrDefault(x => x.Parameter == "Spelling").Score;
                spellingTotal = resParameterizedMaxScore.SingleOrDefault(x => x.Parameter == "Spelling").Score;
                

                pronunciation =resParameterizedScore.SingleOrDefault(x => x.Parameter == "Pronunciation").Score;
                pronunciationTotal = resParameterizedMaxScore.SingleOrDefault(x => x.Parameter == "Pronunciation").Score;


                oralFluency =resParameterizedScore.SingleOrDefault(x => x.Parameter == "Oral Fluency").Score;
                oralFluencyTotal = resParameterizedMaxScore.SingleOrDefault(x => x.Parameter == "Oral Fluency").Score;


                grammar=resParameterizedScore.SingleOrDefault(x => x.Parameter == "Grammar").Score;
                grammarTotal = resParameterizedMaxScore.SingleOrDefault(x => x.Parameter == "Grammar").Score;

                tableReport2.Rows.Add("Written Discourse", Convert.ToInt16((writtenDiscourse / writtenDiscourseTotal) * 90) < 10 ? 10 : Convert.ToInt16((writtenDiscourse / writtenDiscourseTotal) * 90));
                tableReport2.Rows.Add("Vocabulary", Convert.ToInt16((vocabulary/vocabularyTotal)*90)<10?10:Convert.ToInt16((vocabulary/vocabularyTotal)*90));
                tableReport2.Rows.Add("Spelling", Convert.ToInt16((spelling/spellingTotal)*90)<10?10:Convert.ToInt16((spelling/spellingTotal)*90));
                //tableReport2.Rows.Add("Pronunciation", Convert.ToInt16(((pronunciation/pronunciationTotal)*Convert.ToInt16((90/1.25))))); //changed as per request
                tableReport2.Rows.Add("Pronunciation", Convert.ToInt16((pronunciation / pronunciationTotal) * 90)<10?10:Convert.ToInt16((pronunciation / pronunciationTotal) * 90));
                tableReport2.Rows.Add("Oral Fluency", Convert.ToInt16((oralFluency/oralFluencyTotal)*90)<10?10:Convert.ToInt16((oralFluency/oralFluencyTotal)*90));
                tableReport2.Rows.Add("Grammar", Convert.ToInt16((grammar / grammarTotal) * 90) < 10 ? 10 : Convert.ToInt16((grammar / grammarTotal) * 90));


            }

            var userBasicData = UserManager.GetUserById(cmbUsers.SelectedValue.ToString());
            var ownerInfo = UserManager.GetUserById("institute");

            rptView.ReportHeader = tableHeader;
            rptView.ReportData = tableData;
            rptView.ReportGraph = tableReport;
            rptView.ReportGraph2 = tableReport2;
            rptView.ReportTitle = string.Format("{0}:{1}", set.Name, usr.Firstname + "," + usr.Lastname);

            rptView.ReportDocumentValues = new Dictionary<string, object>();
            
            rptView.ReportDocumentValues.Add("Fullname", userBasicData.Firstname+","+userBasicData.Lastname);
            rptView.ReportDocumentValues.Add("DOB", userBasicData.DOB);
            rptView.ReportDocumentValues.Add("Contact", userBasicData.ContactNo);
            rptView.ReportDocumentValues.Add("Email", userBasicData.Email);
            rptView.ReportDocumentValues.Add("Country", "India");
            rptView.ReportDocumentValues.Add("RegistrationID", string.Format("000NUYR{0}",userBasicData.UserId));
            rptView.ReportDocumentValues.Add("TestTakerID", string.Format("000NUYR{0}", userBasicData.UserId));
            rptView.ReportDocumentValues.Add("ReportCode", string.Format("{1} #A87UYEIU{0}", userBasicData.UserId, set.Name));
            rptView.ReportDocumentValues.Add("Photo", GetPhotoPath(userBasicData.UserId));
            
            rptView.ReportDocumentValues.Add("Comment", GenerateComment(80));
            rptView.ReportDocumentValues.Add("ExamCenter", ownerInfo==null?"Panacea":ownerInfo.Firstname);

            rptView.ReportDocumentValues.Add("Listening", Convert.ToInt16(resultDic["Listening"])<10?10:Convert.ToInt16(resultDic["Listening"]));
            rptView.ReportDocumentValues.Add("Speaking", Convert.ToInt16(resultDic["Speaking"])<10?10:Convert.ToInt16(resultDic["Speaking"]));
            rptView.ReportDocumentValues.Add("Writing", Convert.ToInt16(resultDic["Writing"])<10?10:Convert.ToInt16(resultDic["Writing"]));
            rptView.ReportDocumentValues.Add("Reading", Convert.ToInt16(resultDic["Reading"])<10?10:Convert.ToInt16(resultDic["Reading"]));

            var overallScore = GetOverallScore(Convert.ToInt16(resultDic["Listening"]),
                Convert.ToInt16(resultDic["Speaking"]),
                Convert.ToInt16(resultDic["Writing"]),
                Convert.ToInt16(resultDic["Reading"]));

            rptView.ReportDocumentValues.Add("OverallScore", overallScore);

            rptView.ReportDocumentValues.Add("OverallBands", string.Format("Overall {0} Bands", GetOverallBands(Convert.ToInt16(overallScore))));

            rptView.ReportDocumentValues.Add("Grammar", Convert.ToInt16((grammar / grammarTotal) * 90) < 10 ? 10 : Convert.ToInt16((grammar / grammarTotal) * 90));
            rptView.ReportDocumentValues.Add("Vocabulary", Convert.ToInt16((vocabulary / vocabularyTotal) * 90)<10?10:Convert.ToInt16((vocabulary / vocabularyTotal) * 90));
            rptView.ReportDocumentValues.Add("WrittenDiscourse", Convert.ToInt16((writtenDiscourse / writtenDiscourseTotal) * 90)<10?10:Convert.ToInt16((writtenDiscourse / writtenDiscourseTotal) * 90));
            rptView.ReportDocumentValues.Add("Spelling", Convert.ToInt16((spelling / spellingTotal) * 90)<10?10:Convert.ToInt16((spelling / spellingTotal) * 90));
            rptView.ReportDocumentValues.Add("OralFluency", Convert.ToInt16((oralFluency / oralFluencyTotal) * 90)<10?10:Convert.ToInt16((oralFluency / oralFluencyTotal) * 90));
            //rptView.ReportDocumentValues.Add("Pronunciation", Convert.ToInt16(((pronunciation / pronunciationTotal) * Convert.ToInt16((90 / 1.25))))<10?10:Convert.ToInt16(((pronunciation / pronunciationTotal) * Convert.ToInt16((90 / 1.25))))); //changed as per request
            rptView.ReportDocumentValues.Add("Pronunciation", Convert.ToInt16((pronunciation / pronunciationTotal) * 90) < 10 ? 10 : Convert.ToInt16((pronunciation / pronunciationTotal) * 90));


            rptView.TemplateType = "detailedanalysis";
            //obj.GenerateReport();
            rptView.Show();
        }

        private string GetOverallScore(int listening,int speaking,int writing,int reading) {
            return ((listening + speaking + writing + reading) / 4).ToString();
        }

        class ScoreRange {
            short min;
            short max;

            public ScoreRange(short min,short max)
            {
                this.Min = min;
                this.Max = max;
            }

            public short Min { get; private set; }
            public short Max { get; private set; }
        }

        private string GetOverallBands(int overallScore) {
            var dic = new Dictionary<string, ScoreRange>();
            dic.Add("4.5", new ScoreRange(30,36));
            dic.Add("5", new ScoreRange(36,43));
            dic.Add("5.5", new ScoreRange(43,50));
            dic.Add("6", new ScoreRange(50,58));
            dic.Add("6.5", new ScoreRange(58,65));
            dic.Add("7", new ScoreRange(65,73));
            dic.Add("7.5", new ScoreRange(73, 79));
            dic.Add("8", new ScoreRange(79,83));
            dic.Add("8.5", new ScoreRange(79,83));
            dic.Add("9", new ScoreRange(86,90));

            var overallBands = dic.SingleOrDefault(x => overallScore >= x.Value.Min && overallScore <= x.Value.Max);

            return overallBands.Key != null ? overallBands.Key : "4";


        }

        private string GetPhotoPath(string userId) {
            string file = Path.Combine(baseOutputDirectory,userId,userId+".jpg");

            return File.Exists(file) ? file : Path.Combine(baseOutputDirectory, "noimage.jpg");
        }

        private string GenerateComment(int score) {

            if (score > 50 && score < 60) {
                return "50 to 60 Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            }
            else if (score > 60 && score < 70) {
                return "60 to 70 Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            }
            else if (score > 70 && score < 80) {
                return "70 to 80 Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            }
            return "Some default text Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
        }

    }

    class ReportType
    {
        public string ReportName { get; set; }
        public string TypeValue { get; set; }
    }
}
