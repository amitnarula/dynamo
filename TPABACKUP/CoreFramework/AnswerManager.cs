using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using TPA.Entities;

namespace TPA.CoreFramework
{
    public class AnswerManager
    {
        static string baseOutputDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)+"//Data//";
        public static void LogAnswer(QuestionBase questionBase, string answer, string attemptTimeLeft)
        {
            if (questionBase.Mode == Mode.ANSWER_KEY)
                return; //If answerkey mode is running no need to write the answer xml again
            string answerOutputFilename = questionBase.Id + ".xml";
            string answerOutputFilepath = baseOutputDirectory + answerOutputFilename;

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
            dsAnswer.WriteXml(answerOutputFilepath);

            if (questionBase.AttemptTimeType == AttemptTimeType.PRACTICE_SET_ITEM)
            {
                string practiceSetTimeOutputFilename = baseOutputDirectory+questionBase.CurrentQuestionType + questionBase.CurrentPracticeSetId + ".xml";
                DataSet dsPracticeSetTime = new DataSet("practiceSetTimeDs");
                DataTable dtPracticeSetTime = new DataTable("practiceSetTimeDt");
                DataColumn dcPracticeSetTimeLeft = new DataColumn("attemptTimeLeft", typeof(string));

                dtPracticeSetTime.Columns.Add(dcPracticeSetTimeLeft);
                DataRow drowPracticeSetTime = dtPracticeSetTime.NewRow();
                drowPracticeSetTime["attemptTimeLeft"] = attemptTimeLeft;
                dtPracticeSetTime.Rows.Add(drowPracticeSetTime);
                dsPracticeSetTime.Tables.Add(dtPracticeSetTime);
                dsPracticeSetTime.WriteXml(practiceSetTimeOutputFilename);

            }
            
        }

        public static bool DoAnswersExist(FileReader.FileType fileType, string practiceSetId)
        {
            bool doAnswersExists = false;

            //Fetching all the questions from the given practice set
            DataSet dsQuestions = FileReader.ReadFile(fileType);
            if (dsQuestions.Tables.Count>0)
            {
                DataRow[] questionsDataRows = dsQuestions.Tables["question"].Select("practiceSet='" + practiceSetId + "'");

                string[] files = Directory.GetFiles(baseOutputDirectory);

                foreach (DataRow dRow in questionsDataRows)
                {
                    if (files.Any(_ => _.Contains(Convert.ToString(dRow["id"]) + ".xml")))
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

                File.Delete(baseOutputDirectory + fileToDelete);
            }

            string itemType = string.Empty;

            if (fileType == FileReader.FileType.QUESTION_READING) //Only reading and listening have overall timer
                itemType = QuestionType.READING.ToString();
            else if (fileType == FileReader.FileType.QUESTION_LISTENING)
                itemType = QuestionType.LISTENING.ToString();

            //Also removing the practice set time left manager file
            if (File.Exists(baseOutputDirectory +itemType+ practiceSetId + ".xml"))
            {
                File.Delete(baseOutputDirectory +itemType+ practiceSetId + ".xml");
            }

            //Clearing the cache
            TPACache.RemoveItem(practiceSetId + CommonUtilities.GetQuestionTypeByFileType(fileType).ToString());

        }

        public static Answer ReadAnswer(string practiceSetId, string questionId)
        {
            Answer userAnswer = null;
            string answerFileName = questionId + ".xml";
            string answerFilepath = baseOutputDirectory + answerFileName;

            string[] userAnswers = new string[] { };
            string attemptTime = "00:00:00";
            
            if (File.Exists(answerFilepath))
            {
                userAnswer = new Answer();
                DataSet dsAnswer = new DataSet();
                dsAnswer.ReadXml(answerFilepath);

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
    }

    public class Answer
    {
        public string[] Answers { get; set; }
        public string AttemptTime { get; set; }
    }
}
