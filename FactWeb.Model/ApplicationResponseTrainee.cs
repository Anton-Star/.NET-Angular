using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationResponseTrainee : BaseModel
    {
        [Key, Column("ApplicationResponseTraineeId")]
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Guid ApplicationSectionQuestionId { get; set; }
        public Guid? ApplicationSectionQuestionAnswerId { get; set; }        
        public int ApplicationResponseStatusId { get; set; }
        public virtual Application Application { get; set; }
        public virtual ApplicationSectionQuestion ApplicationSectionQuestion { get; set; }
        public virtual ApplicationSectionQuestionAnswer ApplicationSectionQuestionAnswer { get; set; }
        [ForeignKey("ApplicationResponseStatusId")]
        public virtual ApplicationResponseStatus ApplicationResponseStatus { get; set; }
        
    }
}
