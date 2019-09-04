using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationSectionQuestionAnswer : BaseModel
    {
        [Key, Column("ApplicationSectionQuestionAnswerId")]
        public Guid Id { get; set; }
        public Guid ApplicationSectionQuestionId { get; set; }
        [Column("ApplicationSectionQuestionAnswerText"), Required, MaxLength(1000)]
        public string Text { get; set; }
        [Column("ApplicationSectionQuestionAnswerIsActive")]
        public bool IsActive { get; set; }
        [Column("ApplicationSectionQuestionAnswerOrder")]
        public int Order { get; set; }
        [Column("ApplicationSectionQuestionAnswerIsExpectedAnswer")]
        public bool? IsExpectedAnswer { get; set; }

        public virtual ApplicationSectionQuestion Question { get; set; }

        public virtual ICollection<ApplicationResponse> ApplicationResponses { get; set; }
        public virtual ICollection<ApplicationSectionQuestionAnswerDisplay> Displays { get; set; }
    }
}
