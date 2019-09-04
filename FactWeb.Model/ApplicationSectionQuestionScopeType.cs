using System;

namespace FactWeb.Model
{
    public class ApplicationSectionQuestionScopeType : BaseModel
    {
        public Guid Id { get; set; }
        public Guid ApplicationSectionQuestionId { get; set; }
        public int ScopeTypeId { get; set; }

        public virtual ApplicationSectionQuestion ApplicationSectionQuestion { get; set; }
        public virtual ScopeType ScopeType { get; set; }
    }
}
