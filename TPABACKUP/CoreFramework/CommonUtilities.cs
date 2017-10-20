using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPA.Entities;

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

    }
}
