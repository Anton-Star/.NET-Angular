using System;

namespace FactWeb.Model.InterfaceItems
{
    public class SimpleOrganization
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public DateTime? AccreditationDate { get; set; }
        public DateTime? AccreditationExpirationDate { get; set; }
        public int? AccreditationStatusId { get; set; }
        public string AccreditationStatusName { get; set; }
        public Guid? ComplianceApplicationId { get; set; }
        public Guid? ApplicationUniqueId { get; set; }
        public Guid? EligibilityApplicationUniqueId { get; set; }
        public Guid? RenewalApplicationUniqueId { get; set; }
    }
}
