using System;

namespace FactWeb.Model.InterfaceItems
{
    public class DocumentAssociationTypeItem
    {
        public Guid Id { get; set; }
        public DocumentItem Document { get; set; }
        public AssociationTypeItem AssociationType { get; set; }
    }
}
