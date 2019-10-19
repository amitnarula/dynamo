using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPA.Entities
{
    public class Question
    {
        public string PracticeSetId { get; set; }
        public QuestionType QuestionType { get; set; }
        public bool IsPreviousQuestionSelected { get; set; }
        public bool IsNextQuestionSelected { get; set; }
        public Mode QuestionMode { get; set; }
        public Mode SetMode { get; set; }
        public TestMode TestMode { get; set; }
        public SetAttemptTime PracticeSetAttemptTime { get; set; }
    }
    public enum QuestionType { 
        READING,
        WRITING,
        SPEAKING,
        LISTENING,
        NONE
    }
    public enum AttemptTimeType { 
        INDIVIDUAL_QUESTION,
        PRACTICE_SET_ITEM
    }
    public enum Mode { 
        QUESTION,
        ANSWER,
        ANSWER_KEY,
        TIME_OUT
    }

    public enum TestMode
    {
        Practice=0,
        Mock
    }

    public class ModeSetting
    {
        public TestMode TestMode { get; set; }
        public Mode QuestionMode { get; set; }
        public Mode SetMode { get; set; }
    }

    public enum QuestionTemplates
    {
        MULTI_CHOICE_SINGLE_ANSWER,
        WRITE_ESSAY,
        SUMMARIZE_TEXT,
        MULTI_CHOICE_MULTIPLE_ANSWER,
        FILL_IN_BLANK_WITH_OPTIONS,
        REORDER,
        FILL_IN_BLANKS,
        LISTEN_MULTI_CHOICE,
        LISTEN_MULTI_SELECT,
        LISTEN_AND_WRITE,
        LISTEN_AND_FILL_BLANKS,
        SPEAK_LISTEN,
        SPEAK_LOOK,
        SPEAK_READ,
        LOOK_SPEAK_LISTEN,
        LISTEN_AND_HIGHLIGHT,
        SPEAK_ANSWER_SHORT_QUESTION,
        LISTEN_HIGHLIGHT_CORRECT_SUMMARY,
        LISTEN_SELECT_MISSING_WORD,
        LISTEN_AND_DICTATE
    }
}
