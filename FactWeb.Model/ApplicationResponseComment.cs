using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationResponseComment : BaseModel
    {
        [Key, Column("ApplicationResponseCommentId")]
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public Guid? QuestionId { get; set; }
        
        public string Comment { get; set; }
        public string CommentOverride { get; set; }

        //[Column("ApplicationResponseCommentRFIComment")]
        //public string RFIComment { get; set; }

        //[Column("ApplicationResponseCommentCitationComment")]
        //public string CitationComment { get; set; }

        //[Column("ApplicationResponseCommentCoordinatorComment")]
        //public string CoordinatorComment { get; set; }

        public int? CommentTypeId { get; set; }

        public Guid? FromUser { get; set; }
        
        public Guid? ToUser { get; set; }
        
        public Guid? DocumentId { get; set; }
        public string OverridenBy { get; set; }
        [Column("ApplicationResponseCommentIncludeInReporting")]
        public bool? IncludeInReporting { get; set; }

        [Column("ApplicationResponseCommentVisibleToApplicant")]
        public bool? VisibleToApplicant { get; set; }

        [ForeignKey("FromUser")]
        public virtual User CommentFrom { get; set; }

        [ForeignKey("ToUser")]
        public virtual User CommentTo { get; set; }

        [ForeignKey("CommentTypeId")]
        public virtual CommentType CommentType { get; set; }

        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual Application Application{ get; set; }

        [ForeignKey("QuestionId")]
        public virtual ApplicationSectionQuestion Question { get; set; }

        public virtual List<ApplicationResponseCommentDocument> ApplicationResponseCommentDocuments { get; set; }
    }
}

