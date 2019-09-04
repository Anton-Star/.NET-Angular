using System;

namespace FactWeb.Model
{
    public class ApplicationQuestionNotApplicable : BaseModel
    {
        public Guid Id { get; set; }
        public int ApplicationId { get; set; }
        public Guid ApplicationSectionQuestionId { get; set; }

        public virtual ApplicationSectionQuestion ApplicationSectionQuestion { get; set; }
        public virtual Application Application { get; set; }
    }
}
