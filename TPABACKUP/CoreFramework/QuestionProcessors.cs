using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPA.Entities;

namespace TPA.CoreFramework
{
    public class QuestionProcessors
    {
        public MultiChoiceSingleAnswerQuestion ProcessMultiChoiceSingleAnswerQuestion(string title, string description, List<Option> options)
        {
            MultiChoiceSingleAnswerQuestion question = new MultiChoiceSingleAnswerQuestion();
            question.Title = title;
            question.Description = description;
            question.Options = options;

            return question;
        }
    }
}
