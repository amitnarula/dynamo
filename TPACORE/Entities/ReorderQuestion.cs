using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPA.Entities
{
    public class ReorderQuestion:QuestionBase
    {
        public List<Option> Options { get; set; }
    }
}
