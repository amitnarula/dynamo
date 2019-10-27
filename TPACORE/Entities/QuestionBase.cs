using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPA.Entities
{
    public class QuestionBase
    {
        public string Id { get; set; }
        public string Instruction { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int QuestionIndex { get; set; }
        public string CurrentPracticeSetId { get; set; }
        public QuestionType CurrentQuestionType { get; set; }
        public string QuestionTemplate { get; set; }
        public int TotalQuestionsCount { get; set; }
        public string[] CorrectAnswers { get; set; }
        public Mode Mode { get; set; }
        public Mode SetMode { get; set; }
        public TestMode TestMode { get; set; }
        public string[] UserAnswers { get; set; }
        public TimeSpan AttemptTime { get; set; }
        public AttemptTimeType AttemptTimeType { get; set; }
        public TimeSpan DelayTime { get; set; }
        
    }
}
