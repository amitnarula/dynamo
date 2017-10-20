using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPA.Entities
{
    public class LookSpeakListenQuestion : QuestionBase
    {
        public int Delay { get; set; }
        public int RecordingTime { get; set; }
        public string Media { get; set; }
        public string OutputFile { get; set; }
        public int AudioDelay { get; set; }
        public string Picture { get; set; }
        public bool PlayBeep { get; set; }
        public bool ShowTranscript { get; set; }
        public string SampleTranscript { get; set; }
    }
}
