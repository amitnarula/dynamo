using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPA.Entities;
using TPACORE.CoreFramework;

namespace TPA.CoreFramework
{
    public class CommonUtilities
    {
        public static int GetWordCount(string text)
        {
            return text.Trim().Split(new char[] { ' ' }).Count();
        }

        public static FileReader.FileType GetFileTypeByQuestionType(QuestionType questionType)
        {
            FileReader.FileType fileType=FileReader.FileType.PRACTICE_SET;

            switch (questionType)
            {
                case QuestionType.READING:
                    fileType = FileReader.FileType.QUESTION_READING;
                    break;
                case QuestionType.WRITING:
                    fileType = FileReader.FileType.QUESTION_WRITING;
                    break;
                case QuestionType.SPEAKING:
                    fileType = FileReader.FileType.QUESTION_SPEAKING;
                    break;
                case QuestionType.LISTENING:
                    fileType = FileReader.FileType.QUESTION_LISTENING;
                    break;
                default:
                    break;
            }

            return fileType;
        }

        public static QuestionType GetQuestionTypeByFileType(FileReader.FileType fileType)
        {
            QuestionType questionType;
            switch (fileType)
            {
                case FileReader.FileType.QUESTION_READING:
                    questionType = QuestionType.READING;
                    break;
                case FileReader.FileType.QUESTION_WRITING:
                    questionType = QuestionType.WRITING;
                    break;
                case FileReader.FileType.QUESTION_LISTENING:
                    questionType = QuestionType.LISTENING;
                    break;
                case FileReader.FileType.QUESTION_SPEAKING:
                    questionType = QuestionType.SPEAKING;
                    break;
                default:
                    throw new Exception("Unsupported file type conversion");
            }
            return questionType;
        }

        public static string ResolveTargetUserFolder() {
            //string.empty is the default folder //default folder data/temp
            //{userfolderid} is student folder // data/temp/{userfolderid}
            var studentLoginInfo = TPACache.GetItem(TPACache.STUDENT_LOGIN_INFO) as User;
            if (studentLoginInfo!=null) {
                return studentLoginInfo.UserId;
            }

            var studentSetForEvaluationInfo = TPACache.GetItem(TPACache.STUDENT_ID_TO_EVALUATE) as User;
            if (studentSetForEvaluationInfo != null)
            {
                return studentSetForEvaluationInfo.UserId;
            }
            
            return string.Empty; 
        }
        
        public static string ResolveTargetEvaluationFolder() {
            //string.empty is the default folder //default folder data/temp
            //{userfolderid} is student folder // data/temp/{userfolderid}
            
            var teacherLoginInfo = TPACache.GetItem(TPACache.LOGIN_KEY) as LoginState;
            if (teacherLoginInfo !=null && teacherLoginInfo.CurrentStatus == LoginStatus.OK)
            {
                var studentToEvaluate = TPACache.GetItem(TPACache.STUDENT_ID_TO_EVALUATE) as User;
                return studentToEvaluate != null ? studentToEvaluate.UserId.ToString() : string.Empty;
            }
            return string.Empty;
            
        }
        public static string ResolveTargetFolder() {
            var targetFolder = ResolveTargetUserFolder();

            if (string.IsNullOrEmpty(targetFolder))
                targetFolder = ResolveTargetEvaluationFolder();

            return targetFolder;
        }

    }
}
