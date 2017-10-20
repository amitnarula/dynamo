using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPA.Entities
{
    public class ListenMultiChoiceQuestion:QuestionBase
    {
        public List<Option> Options { get; set; }
        public string Media { get; set; }
        public int Delay { get; set; }
    }
}
