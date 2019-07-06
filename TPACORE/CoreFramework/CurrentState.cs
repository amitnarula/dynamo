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

    public enum LoginStatus
    {
        OK,
        Failed,
        SessionExpired
    }

    public class ScreenTime
    {
        public string QuestionId { get; set; }
        public string QuestionTemplate { get; set; }
        public string MaxScore { get; set; }
        public string QuestionType { get; set; }
        public string PracticeSetId { get; set; }
        public string TimeSpent { get; set; }
        public string PracticeSetName { get; set; }
    }

    public class LoginState
    {
        public LoginStatus CurrentStatus { get; set; } 
    }
}
