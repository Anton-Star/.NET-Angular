using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationSectionQuestion : BaseModel
    {
        [Key, Column("ApplicationSectionQuestionId")]
        public Guid Id { get; set; }
        public Guid ApplicationSectionId { get; set; }
        public int QuestionTypeId { get; set; }
        [Column("ApplicationSectionQuestionText"), Required]
        public string Text { get; set; }
        [Column("ApplicationSectionQuestionDescription")]
        public string Description { get; set; }
        [Column("ApplicationSectionQuestionIsActive")]
        public bool IsActive { get; set; }
        [Column("ApplicationSectionQuestionOrder")]
        public int Order { get; set; }
        [Column("ApplicationSectionQuestionComplianceNumber")]
        public int? ComplianceNumber { get; set; }

        public byte? QuestionTypesFlag { get; set; }

        public virtual ApplicationSection ApplicationSection { get; set; }
        public virtual QuestionType QuestionType { get; set; }

        public virtual ICollection<ApplicationSectionQuestionAnswer> Answers { get; set; }
        public virtual ICollection<ApplicationResponse> ApplicationResponses { get; set; }
        public virtual ICollection<ApplicationSectionQuestionScopeType> ScopeTypes { get; set; }
        public virtual ICollection<ApplicationSectionQuestionAnswerDisplay> HiddenBy { get; set; }
        public virtual ICollection<ApplicationResponseComment> ApplicationResponseComments { get; set; }
        //public virtual ICollection<SiteApplicationVersionQuestionNotApplicable> SiteNotApplicables { get; set; }
        public virtual ICollection<ApplicationQuestionNotApplicable> ApplicationQuestionNotApplicables { get; set; }
    }
}

