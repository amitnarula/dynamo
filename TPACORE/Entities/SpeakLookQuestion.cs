using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPA.Entities
{
    public class SpeakLookQuestion : QuestionBase
    {
        public string OutputFile { get; set; }
        public string Picture { get; set; }
        public int RecordingTime { get; set; }
        public int Delay { get; set; }
        public bool PlayBeep { get; set; }
    }
}
