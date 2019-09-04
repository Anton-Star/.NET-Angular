using System;

namespace FactWeb.Model.InterfaceItems
{
    public class FlatSite
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public DateTime SiteStartDate { get; set; }
        public int? FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public bool IsPrimarySite { get; set; }
    }
}
