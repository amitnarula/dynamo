using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using TPA.Entities;
using System.Security.Cryptography;
using CryptoXML;
using TPACORE.CoreFramework;

namespace TPA.CoreFramework
{
    public class AnswerManager
    {
        private const string phrase = "myKey123";
        static string baseOutputDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)+"//Data//Temp//";

        static AnswerManager()
        {
            //baseOutputDirectory = Path.Combine(baseOutputDirectory, CommonUtilities.ResolveTargetUserFolder());
        }

        public static void LogAnswer(QuestionBase questionBase, string answer, string attemptTimeLeft)
        {
            var xmlEncryptor = new XMLEncryptor(phrase, phrase);
            if (questionBase.Mode == Mode.ANSWER_KEY)
                return; //If answerkey mode is running no need to write the answer xml again

            if (string.IsNullOrEmpty(answer)
                || answer.Split(new char[]{'|'}).All(x=>(string.IsNullOrEmpty(x)
                    ||x.Equals("-1"))))
            { 
                //Put unattempted questions here
                string cacheKey = "UN_ATTEMPTED";
                List<string> state = (List<string>)(TPACache.GetItem(cacheKey));
                if (state != null && state.Any())
                {

                    if (!state.Contains((questionBase.QuestionIndex + 1).ToString()))
                    {
                        state.Add((questionBase.QuestionIndex + 1).ToString());
                    }

                }
                else
                {
                    state = new List<string>();
                    state.Add((questionBase.QuestionIndex + 1).ToString());
                }

                TPACache.SetItem(cacheKey, state, new TimeSpan(0, 0, 5, 0, 0)); //Reset the cache
                
            }

            string answerOutputFilename = questionBase.Id + ".xml";
            string targetOutputDirectory = Path.Combine(baseOutputDirectory, CommonUtilities.ResolveTargetUserFolder());
            string answerOutputFilepath = Path.Combine(targetOutputDirectory, answerOutputFilename);

            DataSet dsAnswer = new DataSet("answerDs");
            DataTable dtAnswer = new DataTable("answerDt");
            DataColumn dcAnswer = new DataColumn("answer", typeof(string));
            DataColumn dcAttemptTimeLeft = new DataColumn("attemptTime", typeof(string));

            dtAnswer.Columns.Add(dcAnswer);
            dtAnswer.Columns.Add(dcAttemptTimeLeft);
            DataRow drowAnswer = dtAnswer.NewRow();
            drowAnswer["answer"] = answer == null ? string.Empty : answer;
            drowAnswer["attemptTime"] = attemptTimeLeft;
            dtAnswer.Rows.Add(drowAnswer);
            dsAnswer.Tables.Add(dtAnswer);
            
            //dsAnswer.WriteXml(answerOutputFilepath);
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    dsAnswer.WriteXml(stream, XmlWriteMode.DiffGram);
            //    CreateAnswerFile(stream, answerOutputFilepath);
            //}
            xmlEncryptor.WriteEncryptedXML(dsAnswer, answerOutputFilepath);
            
            if (questionBase.AttemptTimeType == AttemptTimeType.PRACTICE_SET_ITEM)
            {
                //if the question type is LISTEN_AND_WRITE then no need to change overall time of the practice set
                //as those questions have separate attempt and left time which is already loggd
                //and rest of the listening module has overall time which should remain intact 
                if (questionBase.QuestionTemplate != QuestionTemplates.LISTEN_AND_WRITE.ToString())
                {
                    string practiceSetTimeOutputFilename = Path.Combine(Path.Combine(baseOutputDirectory, CommonUtilities.ResolveTargetUserFolder()), questionBase.CurrentQuestionType + questionBase.CurrentPracticeSetId + ".xml");
                    DataSet dsPracticeSetTime = new DataSet("practiceSetTimeDs");
                    DataTable dtPracticeSetTime = new DataTable("practiceSetTimeDt");
                    DataColumn dcPracticeSetTimeLeft = new DataColumn("attemptTimeLeft", typeof(string));

                    dtPracticeSetTime.Columns.Add(dcPracticeSetTimeLeft);
                    DataRow drowPracticeSetTime = dtPracticeSetTime.NewRow();
                    drowPracticeSetTime["attemptTimeLeft"] = attemptTimeLeft;
                    dtPracticeSetTime.Rows.Add(drowPracticeSetTime);
                    dsPracticeSetTime.Tables.Add(dtPracticeSetTime);

                    //dsPracticeSetTime.WriteXml(practiceSetTimeOutputFilename);
                    //using (MemoryStream streamPracticeSetTime = new MemoryStream())
                    //{

                    //    dsPracticeSetTime.WriteXml(streamPracticeSetTime,XmlWriteMode.DiffGram);
                    //    CreateAnswerFile(streamPracticeSetTime, practiceSetTimeOutputFilename);
                    //    streamPracticeSetTime.Position = 0;
                    //}
                    xmlEncryptor.WriteEncryptedXML(dsPracticeSetTime, practiceSetTimeOutputFilename);
                }
            }
            
            EvaluationManager.Evaluate(questionBase, answer); //Evaluation manager logging results
            
        }

        public static bool DoAnswersExist(FileReader.FileType fileType, string practiceSetId)
        {
            bool doAnswersExists = false;

            //Fetching all the questions from the given practice set
            DataSet dsQuestions = FileReader.ReadFile(fileType);
            if (dsQuestions.Tables.Count>0)
            {
                DataRow[] questionsDataRows = dsQuestions.Tables["question"].Select("practiceSet='" + practiceSetId + "'");

                //string baseOutDir = Path.Combine(baseOutputDirectory, CommonUtilities.ResolveTargetUserFolder());
                string[] files = Directory.GetFiles(Path.Combine(baseOutputDirectory,CommonUtilities.ResolveTargetUserFolder()));

                foreach (DataRow dRow in questionsDataRows)
                {
                    if (files.Any(x => x.Contains(Convert.ToString(dRow["id"]) + ".xml"))
                        ||files.Any(x => x.Contains(Convert.ToString(dRow["id"]) + ".wav")))
                    {
                        doAnswersExists = true;
                        break;
                    }
                }
            }

            return doAnswersExists;
        }

        public static void DeleteAllUserAnswers(FileReader.FileType fileType,string practiceSetId)
        { 
            DataSet dsQuestions = FileReader.ReadFile(fileType);
            DataRow[] questionsDataRows = dsQuestions.Tables["question"].Select("practiceSet='" + practiceSetId+"'");

            foreach (DataRow questionDataRow in questionsDataRows)
            {
                string fileToDelete = Convert.ToString(questionDataRow["id"])+".xml";
                string targetDirectory = Path.Combine(baseOutputDirectory,CommonUtilities.ResolveTargetUserFolder());

                if (File.Exists(Path.Combine(targetDirectory, fileToDelete)))
                    File.Delete(Path.Combine(targetDirectory, fileToDelete));

                string audioRecordedFileToDelete = Convert.ToString(questionDataRow["id"]) + ".wav";

                if (File.Exists(Path.Combine(targetDirectory, audioRecordedFileToDelete)))
                    File.Delete(Path.Combine(targetDirectory, audioRecordedFileToDelete));
            }

            string itemType = string.Empty;

            string targetOutputDirectory = Path.Combine(baseOutputDirectory, CommonUtilities.ResolveTargetUserFolder());

            if (fileType == FileReader.FileType.QUESTION_READING) //Only reading and listening have overall timer
                itemType = QuestionType.READING.ToString();
            else if (fileType == FileReader.FileType.QUESTION_LISTENING)
                itemType = QuestionType.LISTENING.ToString();

            //Also removing the practice set time left manager file
            if (File.Exists(Path.Combine(targetOutputDirectory ,itemType+ practiceSetId + ".xml")))
            {
                File.Delete(Path.Combine(targetOutputDirectory, itemType + practiceSetId + ".xml"));
            }

            //Removing the UNLOCK files
            if (File.Exists(Path.Combine(targetOutputDirectory, QuestionType.READING.ToString() + practiceSetId + "UNLCK.xml"))
                && fileType == FileReader.FileType.QUESTION_READING)
                File.Delete(Path.Combine(targetOutputDirectory, QuestionType.READING.ToString() + practiceSetId + "UNLCK.xml"));

            if (File.Exists(Path.Combine(targetOutputDirectory, QuestionType.LISTENING.ToString() + practiceSetId + "UNLCK.xml"))
                && fileType == FileReader.FileType.QUESTION_LISTENING)
                File.Delete(Path.Combine(targetOutputDirectory, QuestionType.LISTENING.ToString() + practiceSetId + "UNLCK.xml"));

            if (File.Exists(Path.Combine(targetOutputDirectory, QuestionType.WRITING.ToString() + practiceSetId + "UNLCK.xml"))
                && fileType == FileReader.FileType.QUESTION_WRITING)
                File.Delete(Path.Combine(targetOutputDirectory, QuestionType.WRITING.ToString() + practiceSetId + "UNLCK.xml"));

            if (File.Exists(Path.Combine(targetOutputDirectory, QuestionType.SPEAKING.ToString() + practiceSetId + "UNLCK.xml"))
                && fileType == FileReader.FileType.QUESTION_SPEAKING)
                File.Delete(Path.Combine(targetOutputDirectory, QuestionType.SPEAKING.ToString() + practiceSetId + "UNLCK.xml"));


            //Removing the SUB (submission file
            string[] submissionFiles = new string[] {
            Path.Combine(targetOutputDirectory,practiceSetId+"_SUB_"+QuestionType.READING.ToString()+".xml"),
            Path.Combine(targetOutputDirectory,practiceSetId+"_SUB_"+QuestionType.SPEAKING.ToString()+".xml"),
            Path.Combine(targetOutputDirectory,practiceSetId+"_SUB_"+QuestionType.WRITING.ToString()+".xml"),
            Path.Combine(targetOutputDirectory,practiceSetId+"_SUB_"+QuestionType.LISTENING.ToString()+".xml")
            };

            //Removing evaluation files
            string[] evalFiles = Directory.GetFiles(targetOutputDirectory, "*eval.xml");

            var questionIds = questionsDataRows.Select(x => x["Id"]);

            foreach (var evalFile in evalFiles)
            {
                if (File.Exists(evalFile) && questionIds.Any(x=>evalFile.Contains(x.ToString()))) //only selected practice item (reading/listening etc.) eval files should be removed.
                    File.Delete(evalFile);
            }


            foreach (var item in submissionFiles)
            {
                if (File.Exists(item) && ResolveFileTypeFromFileName(item) == fileType)
                    File.Delete(item);
            }


            //Clearing the cache
            TPACache.RemoveItem(practiceSetId + CommonUtilities.GetQuestionTypeByFileType(fileType).ToString());

        }

        private static FileReader.FileType ResolveFileTypeFromFileName(string fileName)
        {
            if (fileName.ToLower().Contains("reading"))
                return FileReader.FileType.QUESTION_READING;

            else if (fileName.ToLower().Contains("writing"))
                return FileReader.FileType.QUESTION_WRITING;

            else if (fileName.ToLower().Contains("speaking"))
                return FileReader.FileType.QUESTION_SPEAKING;

            else if (fileName.ToLower().Contains("listening"))
                return FileReader.FileType.QUESTION_LISTENING;
            else
                throw new ArgumentException("filename");
        }

        public static Answer ReadAnswer(string practiceSetId, string questionId)
        {
            var xmlEncryptor = new XMLEncryptor(phrase, phrase);
            Answer userAnswer = null;
            string answerFileName = questionId + ".xml";
            string targetDirectory = Path.Combine(baseOutputDirectory,CommonUtilities.ResolveTargetUserFolder());
            string answerFilepath = Path.Combine(targetDirectory, answerFileName);

            string[] userAnswers = new string[] { };
            string attemptTime = "00:00:00";
            
            if (File.Exists(answerFilepath))
            {
                userAnswer = new Answer();
                DataSet dsAnswer = new DataSet();

                //dsAnswer.ReadXml(answerFilepath);
                dsAnswer = xmlEncryptor.ReadEncryptedXML(answerFilepath);

                DataTable dtAnswer = dsAnswer.Tables["answerDt"];

                if (dtAnswer != null)
                {
                    if(!dtAnswer.Columns.Contains("answer"))
                        userAnswers=string.Empty.Split(new char[] { '|' });
                    else
                        userAnswers = Convert.ToString(dtAnswer.Rows[0]["answer"]).Split(new char[] { '|' });

                    attemptTime = Convert.ToString(dtAnswer.Rows[0]["attemptTime"]);
                }

                userAnswer.Answers = userAnswers;
                userAnswer.AttemptTime = attemptTime;
            }
            
            return userAnswer;
        }

        


        ///<summary>
        /// Steve Lydford - 12/05/2008.
        ///
        /// Encrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        private static void CreateAnswerFile(Stream inputStream, string outputFile)
        {

            try
            {
                string password = phrase; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                //FileStream fsIn = new FileStream(inputFile, FileMode.Open);
                
                int data;
                while ((data = inputStream.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                inputStream.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {
                throw;
            }
        }
        ///<summary>
        /// Steve Lydford - 12/05/2008.
        ///
        /// Decrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        /*public void DecryptFile(string inputFile, string outputFile)
        {

            {
                string password = @"myKey123"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
        }*/

        private static DataSet ReadAnswerFile(string inputFile)
        {
            string password = phrase; // Your Key Here

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateDecryptor(key, key),
                CryptoStreamMode.Read);


            int data;
            DataSet ds = null;
            using (Stream stream = new MemoryStream())
            {
                while ((data = cs.ReadByte()) != -1)
                {
                    stream.WriteByte((byte)data);
                }
                ds = new DataSet();
                ds.ReadXml(stream);
            }

            cs.Close();
            fsCrypt.Close();

            return ds;
        }
    }

    public class Answer
    {
        public string[] Answers { get; set; }
        public string AttemptTime { get; set; }
    }
}
