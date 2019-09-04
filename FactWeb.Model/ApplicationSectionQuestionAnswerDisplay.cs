using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationSectionQuestionAnswerDisplay : BaseModel
    {
        [Key, Column("ApplicationSectionQuestionAnswerDisplayId")]
        public Guid Id { get; set; }
        public Guid ApplicationSectionQuestionAnswerId { get; set; }
        public Guid HidesQuestionId { get; set; }

        public ApplicationSectionQuestionAnswer ApplicationSectionQuestionAnswer { get; set; }
        [ForeignKey("HidesQuestionId")]
        public ApplicationSectionQuestion ApplicationSectionQuestion { get; set; }
    }
}
