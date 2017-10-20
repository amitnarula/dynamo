using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPA.Entities
{
    public class PracticeSet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PracticeSetItem Items { get; set; }
        public PracticeSetAttemptTime PracticeSetAttemptTime { get; set; }
    }
    public class PracticeSetItem
    {
        public bool Speaking { get; set; }
        public bool Listening { get; set; }
        public bool Writing { get; set; }
        public bool Reading { get; set; }

    }

    /// <summary>
    /// A CUSTOM value calculates the practice set overall attempt time automatically
    /// Formula = Number of questions in practice set for particular item (READING,LISTENING, WRITING, SPEAKING) + 2 Minutes
    /// </summary>
    public class PracticeSetAttemptTime
    {
        public string ItemType { get; set; }
        public string AttemptTime { get; set; }
    }
}
