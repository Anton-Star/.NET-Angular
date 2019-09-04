using System;

namespace FactWeb.Model
{
    public class SectionQuestion
    {
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public int? ComplianceNumber { get; set; }
        public byte? QuestionTypesFlag { get; set; }
        public int? ApplicationResponseId { get; set; }
        public string OtherText { get; set; }
        public bool? Flag { get; set; }
        public int? ApplicationResponseStatusId { get; set; }
        public string ApplicationResponseStatusName { get; set; }
        public Guid? UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public int? VisibleApplicationResponseStatusId { get; set; }
        public string VisibleAnswerResponseStatusName { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionTypeName { get; set; }
        public Guid? DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string RequestValues { get; set; }
        public Guid? OrganizationDocumentLibraryId { get; set; }
        public Guid? AnswerId { get; set; }
        public bool IsVisible { get; set; }
        public string ApplicationResponseComments { get; set; }
        public string ApplicationResponseCommentLastUpdatedBy { get; set; }
        public DateTime? CommentDate { get; set; }
        public string CommentBy { get; set; }
        public bool IsAnswered { get; set; }
    }
}
