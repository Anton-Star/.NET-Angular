using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ApplicationQuestionNotApplicableItem
    {
        public Guid Id { get; set; }
        public ApplicationVersionItem ApplicationVersion { get; set; }

        public List<QuestionNotApplicable> QuestionsNotApplicable { get; set; }
    }
}
