using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class Answer
    {
        public Guid? Id { get; set; }
        public Guid? QuestionId { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public int Order { get; set; }
        public bool IsExpectedAnswer { get; set; }

        public List<QuestionAnswerDisplay> HidesQuestions { get; set; }
    }
}
