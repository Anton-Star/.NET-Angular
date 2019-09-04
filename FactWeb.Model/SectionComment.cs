using System;

namespace FactWeb.Model
{
    public class SectionComment
    {
        public int ApplicationResponseCommentId { get; set; }
        public int ApplicationId { get; set; }
        public Guid QuestionId { get; set; }
        public string Comment { get; set; }
        public string CommentOverride { get; set; }
        public bool IsOverridden { get; set; }
        public string OverridenBy { get; set; }
        public Guid? FromUser { get; set; }
        public Guid? ToUser { get; set; }
        public Guid? DocumentId { get; set; }
        public int CommentTypeId { get; set; }
        public string CommentTypeName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string FromName { get; set; }
        public int FromRoleId { get; set; }
        public string FromRoleName { get; set; }
        public bool VisibleToApplicant { get; set; }
    }
}
