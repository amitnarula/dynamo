using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPA.Entities
{
    public class SpeakReadQuestion : QuestionBase
    {
        public int Delay { get; set; }
        public int RecordingTime { get; set; }
        public string OutputFile { get; set; }
        public bool PlayBeep { get; set; }
    }
}
