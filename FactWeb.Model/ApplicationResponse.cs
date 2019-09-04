using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationResponse : BaseModel
    {
        [Key, Column("ApplicationResponseId")]
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Guid ApplicationSectionQuestionId { get; set; }
        public Guid? ApplicationSectionQuestionAnswerId { get; set; }        
        public int? ApplicationResponseStatusId { get; set; }
        public int? VisibleApplicationResponseStatusId { get; set; }
        [Column("ApplicationResponseFlag")]
        public bool Flag { get; set; }        
        [Column("ApplicationResponseText")]
        public string Text { get; set; }
        [Column("ApplicationResponseComments")]
        public string Comments { get; set; }
        [Column("ApplicationResponseCoorindatorComment")]
        public string CoorindatorComment { get; set; }
        public Guid? UserId { get; set; }
        public Guid? DocumentId { get; set; }
        [Column("ApplicationResponseCommentLastUpdatedBy")]
        public string CommentLastUpdatedBy { get; set; }
        [Column("ApplicationResponseCommentLastUpdatedDate")]
        public DateTime? CommentLastUpdatedDate { get; set; }

        [ForeignKey("ApplicationResponseStatusId")]
        public virtual ApplicationResponseStatus ApplicationResponseStatus { get; set; }
        [ForeignKey("VisibleApplicationResponseStatusId")]
        public virtual ApplicationResponseStatus VisibleApplicationResponseStatus { get; set; }
        public virtual Application Application { get; set; }
        public virtual ApplicationSectionQuestion ApplicationSectionQuestion { get; set; }
        public virtual ApplicationSectionQuestionAnswer ApplicationSectionQuestionAnswer { get; set; }
        public virtual Document Document { get; set; }
        public virtual User User { get; set; }
        
    }
}
