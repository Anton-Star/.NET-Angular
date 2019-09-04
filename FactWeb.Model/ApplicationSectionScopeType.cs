using System;

namespace FactWeb.Model
{
    public class ApplicationSectionScopeType : BaseModel
    {
        public Guid Id { get; set; }
        public Guid ApplicationSectionId { get; set; }
        public int ScopeTypeId { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActual { get; set; }

        public virtual ApplicationSection ApplicationSection { get; set; }
        public virtual ScopeType ScopeType { get; set; }
    }
}
