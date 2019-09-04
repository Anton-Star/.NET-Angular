using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ApplicationResponseCommentItem
    {        
        public int ApplicationResponseCommentId { get; set; }
        public int? ApplicationId { get; set; }
        public Guid? QuestionId { get; set; }
        public string Comment { get; set; }
        public string CommentOverride { get; set; }
        public bool IsOverridden { get; set; }
        public string OverridenBy { get; set; }
        public Guid? FromUser { get; set; }
        public Guid? ToUser { get; set; }
        public Guid? DocumentId { get; set; }
        public UserItem CommentFrom { get; set; }        
        public string CommentTo { get; set; }        
        public CommentTypeItem CommentType { get; set; }        
        public DocumentItem Document { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public int? AnswerResponseStatusId { get; set; }
        public bool IncludeInReporting { get; set; }
        public bool VisibleToApplicant { get; set; }

        public List<CommentDocument> CommentDocuments { get; set; }
    }
}
