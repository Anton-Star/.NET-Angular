using System;

namespace FactWeb.Model.InterfaceItems
{
    public class DocumentItem
    {
        public Guid Id { get; set; }
        public string RequestValues { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public bool StaffOnly { get; set; }
        public bool HasResponses { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string AssociationTypes { get; set; }
        public string OrganizationName { get; set; }
        public bool? IsBaaDocument { get; set; }
        public Guid? AppUniqueId { get; set; }
        public bool IncludeInReporting { get; set; }
        public bool IsLatestVersion { get; set; }
        public int? ApplicationId { get; set;}
        public string VaultId { get; set; }
        public Guid? ReplacementOfId { get; set; }
    }
}
