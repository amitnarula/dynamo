using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Reflection;
using TPA.Entities;
using System.Security.Principal;
using System.Security.AccessControl;
using CryptoXML;

namespace TPA.CoreFramework
{
    public class FileReader
    {
        private const string BASE_GUID_KEY = "BASE_GUID";
        private const string phrase = "myKey123";
        static string baseOutputDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Data//Temp//";
        public static DataSet ReadFile(FileType fileType)
        {
            //StringBuilder baseGuid = new StringBuilder(Application.Current.FindResource(BASE_GUID_KEY).ToString());
            StringBuilder baseGuid = new StringBuilder("B4AFE0DCEE464F9282BB52EE0FC03F02");
            string baseGuidPracticeSetFileName = string.Empty;
            switch (fileType)
            {
                case FileType.PRACTICE_SET:
                    baseGuidPracticeSetFileName = baseGuid.Append("_S.xml").ToString();
                    break;
                case FileType.MOCK:
                    baseGuidPracticeSetFileName = baseGuid.Append("_M.xml").ToString();
                    break;
                case FileType.QUESTION_READING:
                    baseGuidPracticeSetFileName = baseGuid.Append("_QR.xml").ToString();
                    break;
                case FileType.QUESTION_WRITING:
                    baseGuidPracticeSetFileName = baseGuid.Append("_QW.xml").ToString();
                    break;
                case FileType.QUESTION_LISTENING:
                    baseGuidPracticeSetFileName = baseGuid.Append("_QL.xml").ToString();
                    break;
                case FileType.QUESTION_SPEAKING:
                    baseGuidPracticeSetFileName = baseGuid.Append("_QS.xml").ToString();
                    break;
                case FileType.QUESTION_TITLES:
                    baseGuidPracticeSetFileName = baseGuid.Append("_QT.xml").ToString();
                    break;
                case FileType.EVALUATION_PARAMETER:
                    baseGuidPracticeSetFileName = baseGuid.Append("_EP.xml").ToString();
                    break;
                default:
                    break;
            }
            

            //string baseGuidPracticeSetFilePath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)
            // + "//Data//" + baseGuidPracticeSetFileName;
            DataSet ds = new DataSet();
            //using (Stream fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TPA.Data." + baseGuidPracticeSetFileName))
            Assembly asm = Assembly.GetEntryAssembly();
            using (Stream fileStream = asm.GetManifestResourceStream(asm.GetName().Name + ".Resources." + baseGuidPracticeSetFileName))
            {
                ds.ReadXml(fileStream);
            }

            return ds;
        }
        public enum FileType
        { 
            PRACTICE_SET,
            QUESTION_READING,
            QUESTION_WRITING,
            QUESTION_LISTENING,
            QUESTION_SPEAKING,
            QUESTION_TITLES,
            EVALUATION_PARAMETER,
            MOCK
        }

        public bool PerformIntegratedEvaluation(string practiceSetId)
        {
            return GetTotalQuesionsByPracticeSet(practiceSetId) <= GetEvaluatedQuestionsByPracticeSet(practiceSetId);
        }

        private int GetTotalQuesionsByPracticeSet(string practiceSetId)
        {

            try
            {
                DataSet dsQuestions = new DataSet();

                var questionsCount = FileReader.ReadFile(FileType.QUESTION_LISTENING).Tables["question"].Select("practiceSet='" + practiceSetId + "'").Count()
                    + FileReader.ReadFile(FileType.QUESTION_READING).Tables["question"].Select("practiceSet='" + practiceSetId + "'").Count()
                    + FileReader.ReadFile(FileType.QUESTION_SPEAKING).Tables["question"].Select("practiceSet='" + practiceSetId + "'").Count()
                    + FileReader.ReadFile(FileType.QUESTION_WRITING).Tables["question"].Select("practiceSet='" + practiceSetId + "'").Count();

                return questionsCount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private int GetEvaluatedQuestionsByPracticeSet(string practiceSetId)
        {
            string[] evalFiles = Directory.GetFiles(Path.Combine(baseOutputDirectory,CommonUtilities.ResolveTargetFolder()), "*eval.xml");

            return evalFiles.Count(x => x.Contains(practiceSetId));
        }

        public string GetPracticeSetStatusForReport(string practiceSetId, out string status) {
            status = "Not attempted";

            if (GetEvaluatedQuestionsByPracticeSet(practiceSetId) > 0 && GetEvaluatedQuestionsByPracticeSet(practiceSetId) < GetTotalQuesionsByPracticeSet(practiceSetId))
            {
                status = "Partially attempted";
            }
            else if (GetEvaluatedQuestionsByPracticeSet(practiceSetId) >= GetTotalQuesionsByPracticeSet(practiceSetId)) {
                status = "Attempted";
            }
            
            return status;
        }

        public string GetModuleStatusForReport(string practiceSetId,string module, out string status) {
            status = "Not attempted";

            if (IsModuleCompletelyAttempted(practiceSetId, module)) {
                status = "Attempted";
            }

            if (IsEvaluationStartedForModule(practiceSetId, module)) {
                status = "Evaluating";
            }

            return status;
        }

        private bool IsEvaluationStartedForModule(string practiceSetId, string module) {
            string[] evalFiles = Directory.GetFiles(Path.Combine(baseOutputDirectory, CommonUtilities.ResolveTargetFolder()), "*eval.xml");

            return evalFiles.Any(x => x.Contains(practiceSetId) && x.Contains(module));
        }

        private bool IsModuleCompletelyAttempted(string practiceSetId, string module) {
            string[] submissionFiles = Directory.GetFiles(Path.Combine(baseOutputDirectory, CommonUtilities.ResolveTargetFolder()));

            return submissionFiles.Any(x => x.Contains(practiceSetId) && x.Contains("SUB"));
        }

        public static void ProvideWriteAccessToFolder(string folder, bool hideFolder)
        {
            SecurityIdentifier snewId = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            DirectoryInfo di = new DirectoryInfo(folder);
            DirectorySecurity ds = di.GetAccessControl();
            // add a new file access rule w/ write/modify for all users to the directory security object

            ds.AddAccessRule(new FileSystemAccessRule(snewId,
                                                  FileSystemRights.Write | FileSystemRights.Modify,
                                                  InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,   // all sub-dirs to inherit
                                                  PropagationFlags.None,
                                                  AccessControlType.Allow));
            if (hideFolder)
                di.Attributes = FileAttributes.Hidden | FileAttributes.Directory;
            // Turn write and modify on
            // Apply the directory security to the directory
            di.SetAccessControl(ds);
        }

        public static SetAttemptTime ResolvePracticeSetAttemptTimeLeft(string practiceSetId,string itemType,int totalNumberOfQuestions)
        {
            if (itemType == "READING" || itemType == "LISTENING") //Only reading & listening items have overall timer
            {

                SetAttemptTime attemptTimeObj = new SetAttemptTime();
                attemptTimeObj.ItemType = itemType;

                ///TimeSpan tsAttemptTimeLeft = TimeSpan.FromSeconds(7); //For debug purposes
                TimeSpan tsAttemptTimeLeft = TimeSpan.FromMinutes(2).Add(TimeSpan.FromMinutes(totalNumberOfQuestions * 2));
                //Total time = Number of questions X 2 + 2 minutes as specified

                if (itemType == "LISTENING")
                { 
                    //Calculating the total time as per expression for listening
                    DataSet dsPracticeSet = FileReader.ReadFile(FileType.PRACTICE_SET);
                    DataTable dtPracticeSet = null;

                    if (dsPracticeSet.Tables[0] != null)
                    {
                        dtPracticeSet = dsPracticeSet.Tables[0];

                        DataRow dRowPracticeSet = dtPracticeSet.Select("id='" + practiceSetId+"'").FirstOrDefault();

                        if (dRowPracticeSet != null)
                        {
                            string practiceSetExpression = Convert.ToString(dRowPracticeSet["practiceSetAttemptTime"]);
                            if (practiceSetExpression.IndexOf('-')>0)
                            {
                                string practiceSetListeningTime = practiceSetExpression.Split('-')[1];
                                tsAttemptTimeLeft = TimeSpan.Parse(practiceSetListeningTime);
                            }

                        }
                    }

                }

                string file = Path.Combine(Path.Combine(baseOutputDirectory,CommonUtilities.ResolveTargetUserFolder()) , itemType + practiceSetId + ".xml");
                if (File.Exists(file))
                {
                    var xmlEncryptor = new XMLEncryptor(phrase,phrase);
                    DataSet ds = xmlEncryptor.ReadEncryptedXML(file);

                    if (ds.Tables != null && ds.Tables[0] != null)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[0]["attemptTimeLeft"]) == "TIME_OUT")
                        {
                            attemptTimeObj.AttemptTime = Mode.TIME_OUT.ToString();
                            return attemptTimeObj;
                        }
                    }
                    //Try parse the time, because it may have TIME_OUT as a value
                    tsAttemptTimeLeft = TimeSpan.Parse(Convert.ToString(ds.Tables[0].Rows[0]["attemptTimeLeft"]));

                }

                attemptTimeObj.AttemptTime = tsAttemptTimeLeft.ToString();

                return attemptTimeObj;
            }
            else
                return null;

        }
    }
}
