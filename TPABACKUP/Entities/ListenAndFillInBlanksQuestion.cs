using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPA.Entities
{
    public class ListenAndFillInBlanksQuestion:QuestionBase
    {
        public string Media { get; set; }
        public int Delay { get; set; }
    }
}
