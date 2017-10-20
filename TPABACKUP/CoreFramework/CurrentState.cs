using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPA.Entities;

namespace TPA.CoreFramework
{
    public class CurrentState
    {
        public int QuestionIndex { get; set; }
        public string PracticeSetId { get; set; }
        public QuestionType QuestionType { get; set; }

    }
}
