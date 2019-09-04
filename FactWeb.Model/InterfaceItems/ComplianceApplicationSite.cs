using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ComplianceApplicationSite
    {
        public SiteItems Site { get; set; }
        //public List<SiteApplicationVersionItem> ApplicationVersions { get; set; }
        public List<ApplicationItem> Applications { get; set; }
        public string Circle { get; set; }
    }
}
