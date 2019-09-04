using System;

namespace FactWeb.Model
{
    public class DocumentAssociationType : BaseModel
    {
        public Guid Id { get; set; }
        public Guid AssociationTypeId { get; set; }
        public Guid DocumentId { get; set; }

        public virtual Document Document { get; set; }
        public virtual AssociationType AssociationType { get; set; }

    }
}
