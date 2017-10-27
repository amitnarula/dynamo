using CryptoXML;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using TPACORE.Entities;

namespace TPACORE.CoreFramework
{
    public class EvaluationManager
    {
        private const string phrase = "myKey123";
        static string baseOutputDirectory = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "//Data//Temp//";

        public static void Evaluate(string questionId, List<EvaluationResult> result)
        {
            var xmlEncryptor = new XMLEncryptor(phrase, phrase);
            string evalOutputFilename = questionId + "_eval.xml";
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

        public static List<EvaluationResult> GetResult(string questionId)
        {
            var xmlEncryptor = new XMLEncryptor(phrase, phrase);
            List<EvaluationResult> result = null;
            string resultFileName = questionId + "_eval.xml";
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

                foreach (var item in evalResult)
                {
                    result.Add(new EvaluationResult()
                    {
                        ParamName = item.Split('=')[0],
                        ParamScore = item.Split('=')[1]
                    });
                }
            }

            return result;
        }
    }
}
