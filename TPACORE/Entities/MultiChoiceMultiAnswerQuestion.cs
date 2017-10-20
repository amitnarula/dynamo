using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPA.Entities
{
    public class MultiChoiceMultiAnswerQuestion:QuestionBase
    {
        public List<Option> Options { get; set; }
        public string Picture { get; set; }
    }
}
