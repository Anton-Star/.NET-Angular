using System;

namespace FactWeb.Model.InterfaceItems
{
    public class QuestionNotApplicable
    {
        public Guid Id { get; set; }
        public int ApplicationId { get; set; }
        public Guid QuestionId { get; set; }
    }
}
