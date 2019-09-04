using System;

namespace FactWeb.Model.InterfaceItems
{
    public class QuestionAnswerDisplay
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public string RequirementNumber { get; set; }
        public int? ComplianceNumber { get; set; }
        public string QuestionText { get; set; }
        public Guid? HiddenByQuestionId { get; set; }
    }
}
