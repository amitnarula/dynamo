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

        public static string GetQuestionTemplateFriendlyName(string templateKey)
        {
            QuestionTemplates template;
            Enum.TryParse(templateKey, out template);

            string questionTemplate = string.Empty;
            switch (template)
            {
                case QuestionTemplates.MULTI_CHOICE_SINGLE_ANSWER:
                    questionTemplate = "Multiple choice, choose single answer";
                    break;
                case QuestionTemplates.WRITE_ESSAY:
                    questionTemplate = "Write essay";
                    break;
                case QuestionTemplates.SUMMARIZE_TEXT:
                    questionTemplate = "Summarize written text";
                    break;
                case QuestionTemplates.MULTI_CHOICE_MULTIPLE_ANSWER:
                    questionTemplate = "Multiple choice, choose multiple answer";
                    break;
                case QuestionTemplates.FILL_IN_BLANK_WITH_OPTIONS:
                    questionTemplate = "Reading:Fill in the blanks";
                    break;
                case QuestionTemplates.REORDER:
                    questionTemplate = "Re-order paragraphs";
                    break;
                case QuestionTemplates.FILL_IN_BLANKS:
                    questionTemplate = "Reading and Writing:Fill in the blanks";
                    break;
                case QuestionTemplates.LISTEN_MULTI_CHOICE:
                    questionTemplate = "Multiple choice,choose single answer";
                    break;
                case QuestionTemplates.LISTEN_MULTI_SELECT:
                    questionTemplate = "Multiple choice,choose multiple answer";
                    break;
                case QuestionTemplates.LISTEN_AND_WRITE:
                    questionTemplate = "Summarize spoken text";
                    break;
                case QuestionTemplates.LISTEN_AND_FILL_BLANKS:
                    questionTemplate = "Fill in the blanks";
                    break;
                case QuestionTemplates.LISTEN_AND_DICTATE:
                    questionTemplate = "Write from dictation";
                    break;
                case QuestionTemplates.SPEAK_LISTEN:
                    questionTemplate = "Repeat sentence";
                    break;
                case QuestionTemplates.LOOK_SPEAK_LISTEN:
                    questionTemplate = "Re-tell lecture";
                    break;
                case QuestionTemplates.SPEAK_LOOK:
                    questionTemplate = "Describe image";
                    break;
                case QuestionTemplates.SPEAK_READ:
                    questionTemplate = "Read aloud";
                    break;
                case QuestionTemplates.LISTEN_AND_HIGHLIGHT:
                    questionTemplate = "Highlight incorrect words";
                    break;
                case QuestionTemplates.LISTEN_SELECT_MISSING_WORD:
                    questionTemplate = "Select missing word";
                    break;
                case QuestionTemplates.SPEAK_ANSWER_SHORT_QUESTION:
                    questionTemplate = "Answer short question";
                    break;
                case QuestionTemplates.LISTEN_HIGHLIGHT_CORRECT_SUMMARY:
                    questionTemplate = "Highlight correct summary";
                    break;
                default:
                    break;
            }
            return questionTemplate;
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
