using System;

namespace FactWeb.Model.InterfaceItems
{
    public class CopyComplianceApplicationModel
    {
        public Guid ComplianceApplicationId { get; set; }
        public string CopyFromSite { get; set; }
        public string ApplicationType { get; set; }
        public string CopyToSite { get; set; }
        public bool DeleteOriginal { get; set; }
        public string ApplicationStatus { get; set; }
    }
}
