using CryptoXML;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using TPA.CoreFramework;
using TPA.Entities;
using TPACORE.Entities;

namespace TPACORE.CoreFramework
{
    public class EvaluationManager
    {
        private const string phrase = "myKey123";
        static string baseOutputDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Data//Temp//";

        public static void Evaluate(QuestionBase questionContext, List<EvaluationResult> result)
        {
            var xmlEncryptor = new XMLEncryptor(phrase, phrase);
            string evalOutputFilename = questionContext.Id + "_" + questionContext.CurrentPracticeSetId + "_" + questionContext.CurrentQuestionType.ToString() + "_" + questionContext.QuestionTemplate + "_eval.xml";
            string evalOutputFilepath = baseOutputDirectory + evalOutputFilename;

            DataSet dsEval = new DataSet("evalDs");
            DataTable dtEval = new DataTable("evalDt");
            DataColumn dcEval = new DataColumn("evalDc", typeof(string));

            string resultString = string.Empty;
            foreach (var res in result)
            {
                resultString += res.ParamName + "=" + res.ParamScore + ";";
            }

            dtEval.Columns.Add(dcEval);
            DataRow drowEval = dtEval.NewRow();
            drowEval["evalDc"] = resultString;

            dtEval.Rows.Add(drowEval);
            dsEval.Tables.Add(dtEval);
            
            xmlEncryptor.WriteEncryptedXML(dsEval, evalOutputFilepath);

        }

        public static void Evaluate(QuestionBase questionContext, string userAnswer)
        {
            if (questionContext.CurrentQuestionType == QuestionType.READING || questionContext.CurrentQuestionType == QuestionType.LISTENING)
            {
                var xmlEncryptor = new XMLEncryptor(phrase, phrase);
                string evalOutputFilename = questionContext.Id + "_" + questionContext.CurrentPracticeSetId + "_" + questionContext.CurrentQuestionType.ToString() + "_" + questionContext.QuestionTemplate + "_eval.xml";
                string evalOutputFilepath = baseOutputDirectory + evalOutputFilename;

                DataSet dsEval = new DataSet("evalDs");
                DataTable dtEval = new DataTable("evalDt");
                DataColumn dcEval = new DataColumn("evalDc", typeof(decimal));
                decimal result = 0;
                


                switch ((QuestionTemplates)Enum.Parse(typeof(QuestionTemplates), questionContext.QuestionTemplate, true))
                {
                    case QuestionTemplates.MULTI_CHOICE_SINGLE_ANSWER:
                    case QuestionTemplates.LISTEN_MULTI_CHOICE:
                        
                            if (questionContext.CorrectAnswers[0] == userAnswer)
                                result = 1;
                            else
                                result = 0;
                        break;
                    case QuestionTemplates.MULTI_CHOICE_MULTIPLE_ANSWER :
                    case QuestionTemplates.FILL_IN_BLANK_WITH_OPTIONS:
                    case QuestionTemplates.FILL_IN_BLANKS:
                    case QuestionTemplates.LISTEN_MULTI_SELECT:
                    case QuestionTemplates.LISTEN_HIGHLIGHT_CORRECT_SUMMARY:
                    case QuestionTemplates.LISTEN_SELECT_MISSING_WORD:
                    case QuestionTemplates.LISTEN_AND_FILL_BLANKS:
                        {
                            int correct = 0;
                            for (int count = 0; count < questionContext.CorrectAnswers.Length; count++)
                            {
                                if (!string.IsNullOrEmpty(userAnswer))
                                {
                                    if (questionContext.CorrectAnswers[count] == userAnswer.Split('|')[count])
                                    {
                                        correct++;
                                    }
                                }
                            }
                            result = correct;
                        }
                        break;
                    case QuestionTemplates.REORDER://logic required
                        {
                            int correct = 0;

                            List<string> correctPair = new List<string>();
                            List<string> userAnswerPair = new List<string>();

                            for (int count = 0; count < questionContext.CorrectAnswers.Length; count++)
                            {
                                int currentIndex = count;
                                int nextIndex = count == questionContext.CorrectAnswers.Length - 1 ? 0 : count + 1;

                                correctPair.Add(questionContext.CorrectAnswers[currentIndex] + "," 
                                    + questionContext.CorrectAnswers[nextIndex]);

                                var userAnswerSplitArr = userAnswer.Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);
                                userAnswerPair.Add(userAnswerSplitArr[currentIndex] + "," + userAnswerSplitArr[nextIndex]);
                            }

                            foreach (var item in correctPair)
                            {
                                if(userAnswerPair.Contains(item))
                                {
                                    correct++;
                                }
                            }
                            result = correct;
                        }
                        break;
                    case QuestionTemplates.LISTEN_AND_HIGHLIGHT:
                        {
                            int correct = 0;
                            for (int count = 0; count < questionContext.CorrectAnswers.Length; count++)
                            {
                                if(questionContext.CorrectAnswers[count] == userAnswer.Split('|')[count])
                                {
                                    correct++;
                                }
                            }
                            result = correct;

                        }
                        break;
                    case QuestionTemplates.LISTEN_AND_DICTATE://logic required
                        {
                            var splitChars = new char[]{ ' '};
                            var correctWords = questionContext.CorrectAnswers[0].Split(splitChars,
                                StringSplitOptions.RemoveEmptyEntries);
                            var userAnswerWords = userAnswer.Split(splitChars,
                                StringSplitOptions.RemoveEmptyEntries); //split by spaces
                            int correct =0;
                            foreach (var userWord in userAnswerWords)
                            {
                                if (correctWords.Contains(userWord))
                                    correct++;
                            }

                            result = correct;
                        }
                        break;
                    default:
                        break;
                }
                dtEval.Columns.Add(dcEval);
                DataRow drowEval = dtEval.NewRow();
                drowEval["evalDc"] = result;

                dtEval.Rows.Add(drowEval);
                dsEval.Tables.Add(dtEval);

                xmlEncryptor.WriteEncryptedXML(dsEval, evalOutputFilepath);
            }
        }

        public int GetAttempatedPointsByQuestionType(string practiceSetId, QuestionTemplates questionType, QuestionType type , string specificParameter="")
        {
            string[] files = Directory.GetFiles(baseOutputDirectory, "*_" + practiceSetId + "_" + type.ToString() + "_" + questionType.ToString() + "_eval.xml");
            var xmlEncryptor = new XMLEncryptor(phrase, phrase);
            int attempted = 0;
            foreach (var file in files)
            {
                if(File.Exists(file))
                {
                    DataSet dsResult = xmlEncryptor.ReadEncryptedXML(file);

                    if(dsResult!=null)
                    {
                        var dt = dsResult.Tables[0];
                        if(dt!=null)
                        {
                            string evaluation = Convert.ToString(dt.Rows[0]["evalDc"]);

                            if (evaluation.IndexOf(';') > 0)
                            {
                                //parameterized questions
                                string[] evalArr = evaluation.Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
                                foreach (var item in evalArr)
                                {
                                    if (!string.IsNullOrEmpty(specificParameter))
                                    {
                                        if (item.Split('=')[0].Equals(specificParameter, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            attempted += Convert.ToInt32(item.Split('=')[1]);
                                            break;
                                        }
                                        continue;

                                    }

                                    attempted += Convert.ToInt32(item.Split('=')[1]);
                                }

                            }
                            else
                            {
                                //simple questions
                                attempted += Convert.ToInt32(evaluation);
                            }
                        }
                    }
                }   
            }
            return attempted;

        }

        public static List<EvaluationResult> GetResult(QuestionBase question)
        {
            var xmlEncryptor = new XMLEncryptor(phrase, phrase);
            List<EvaluationResult> result = null;
            string resultFileName = question.Id + "_" + question.CurrentPracticeSetId + "_" + question.CurrentQuestionType.ToString() + "_" + question.QuestionTemplate  + "_eval.xml";
            string resultFilepath = baseOutputDirectory + resultFileName;

            if (File.Exists(resultFilepath))
            {
                result = new List<EvaluationResult>();
                DataSet dsResult = new DataSet();

                dsResult = xmlEncryptor.ReadEncryptedXML(resultFilepath);

                DataTable dtResult = dsResult.Tables["evalDt"];
                string[] evalResult = new string[] { };
                if (dtResult != null)
                {
                    if (!dtResult.Columns.Contains("evalDc"))
                        evalResult = string.Empty.Split(new char[] { ';' },StringSplitOptions.RemoveEmptyEntries);
                    else
                        evalResult = Convert.ToString(dtResult.Rows[0]["evalDc"]).Split(new char[] { ';' },StringSplitOptions.RemoveEmptyEntries);

                }
                if (evalResult.Count() > 1)
                {
                    foreach (var item in evalResult)
                    {
                        result.Add(new EvaluationResult()
                        {
                            ParamName = item.Split('=')[0],
                            ParamScore = item.Split('=')[1]
                        });
                    }
                }
                else if (evalResult.Count() == 1) //not a parameter based question
                {
                    result.Add(new EvaluationResult() { 
                        ParamName="SINGLE",
                        ParamScore=evalResult[0]
                    });
                }
                
            }

            return result;
        }
        private static int NumberOfQuestionsByType(string practiceSetId, FileReader.FileType fileType, QuestionTemplates questionType)
        {
            DataSet dsQuestions = FileReader.ReadFile(fileType);
            DataRow[] dRows = dsQuestions.Tables["question"].Select("practiceSet='" + practiceSetId + "'");
            return dRows.Where(x => Convert.ToString(x["type"]) == questionType.ToString()).Count();
        }
        private static int TotalPointsByFileType(FileReader.FileType fileType, QuestionTemplates template, string practiceSetId)
        {
            DataSet dsQuestions = FileReader.ReadFile(fileType);
            DataRow[] dRows = dsQuestions.Tables["question"].Select("practiceSet='" + practiceSetId + "'");
            var rowsFiltered = dRows.Where(x => Convert.ToString(x["type"]) == template.ToString()).ToList();

            var points = 0;
            foreach (var row in rowsFiltered)
            {
                int questionId = Convert.ToInt32(row["question_Id"]);
                DataRow questionTemplateRow = dsQuestions.Tables["template"].Select("question_Id=" + questionId).FirstOrDefault();

                string correctAnswer = Convert.ToString(questionTemplateRow["answer"]);
                points += correctAnswer.Split(new char[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();
            }
            return points;

        }

        public int PointsByType(DataSet dsEvalParams, QuestionTemplates questionType, string specificParameter)
        {
            int total = 0;
            DataRow[] dRows = dsEvalParams.Tables["template"].Select("key='" + questionType.ToString() + "'");
            DataRow dRow = dRows[0];
            if (dRows != null)
            {
                var evalParams = Convert.ToString(dRow["params"]).Split('|');

                if (string.IsNullOrEmpty(specificParameter))
                {
                    foreach (var prm in evalParams)
                    {
                        var evalParam = prm.Split(',');
                        total += Convert.ToInt32(evalParam[3]);

                    }
                }
                else
                {
                    total = Convert.ToInt32(evalParams.SingleOrDefault(x => x.Contains(specificParameter)).Split(',')[3]); //get only specific parameter score    
                }

            }
            return total;
        }

        public int GetTotalPointsByType(DataSet dsEvalParams, QuestionTemplates questionType, FileReader.FileType fileType, string practiceSetId, string specificParameter="")
        {
            if (fileType == FileReader.FileType.QUESTION_WRITING
                || fileType == FileReader.FileType.QUESTION_SPEAKING
            || questionType == QuestionTemplates.LISTEN_AND_WRITE)
            {
                var numberOfQuestions = NumberOfQuestionsByType(practiceSetId, fileType, questionType);
                if (numberOfQuestions == 0)
                    return 0;
                return numberOfQuestions * PointsByType(dsEvalParams, questionType, specificParameter);
            }
            return TotalPointsByFileType(fileType, questionType, practiceSetId);
        }

    }
}