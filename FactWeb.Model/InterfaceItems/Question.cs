using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class Question
    {
        public Guid? Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public bool Flag{ get; set; }
        public string Comments{ get; set; }
        public string CommentLastUpdatedBy { get; set; }
        public DateTime? CommentDate { get; set; }
        public string CommentBy { get; set; }
        public int Order { get; set; }
        public int AnswerResponseStatusId { get; set; }
        public string AnswerResponseStatusName { get; set; }
        public int VisibleAnswerResponseStatusId { get; set; }
        public string VisibleAnswerResponseStatusName { get; set; }
        public bool Active { get; set; }
        public int? ComplianceNumber { get; set; }
        public Guid? SectionId { get; set; }
        public string SectionName { get; set; }
        public string SectionUniqueIdentifier { get; set; }
        public QuestionTypeFlags QuestionTypeFlags { get; set; }
        public List<Answer> Answers { get; set; }
        public List<ScopeTypeItem> ScopeTypes { get; set; }
        public List<QuestionResponse> QuestionResponses { get; set; }
        public List<QuestionAnswerDisplay> HiddenBy { get; set; }
        public List<ApplicationResponseCommentItem> ApplicationResponseComments { get; set; }
        public List<ApplicationResponseCommentItem> ResponseCommentsRFI { get; set; }
        public List<ApplicationResponseCommentItem> ResponseCommentsCitation { get; set; }
        public List<ApplicationResponseCommentItem> ResponseCommentsSuggestion { get; set; }
        public List<ApplicationResponseCommentItem> ResponseCommentsFactResponse { get; set; }
        public List<ApplicationResponseCommentItem> ResponseCommentsFactOnly { get; set; }
        public List<ApplicationResponseCommentItem> ResponseCommentsCoordinator { get; set; }
        public List<SiteQuestionResponse> SiteQuestionResponse { get; set; }
        public bool IsHidden { get; set; }
        public bool? IsNotApplicable { get; set; }
    }
}
