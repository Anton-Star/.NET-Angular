using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class CompApplication
    {
        public Guid? Id { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public bool HasQmRestriction { get; set; }
        public string ApplicationStatus { get; set; }
        //public string AccreditationStatusName { get; set; }
        public List<ApplicationSiteResponse> ComplianceApplicationSites { get; set; }
        public bool ShowAccreditationReport { get; set; }
        public bool HasRfi { get; set; }
        public bool HasFlag { get; set; }
    }
}
