using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPA.Entities
{
    public class FillInTheBlanksWithOptionsQuestion:QuestionBase
    {
        public List<List<Option>> Options { get; set; }
    }
}
