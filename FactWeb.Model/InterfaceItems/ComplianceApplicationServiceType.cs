using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ComplianceApplicationServiceType
    {
        public string ServiceType { get; set; }
        public List<ComplianceApplicationSite> ComplianceApplicationSites { get; set; }
    }
}
