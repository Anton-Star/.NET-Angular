using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactWeb.Model.InterfaceItems
{
    public class FacilitySiteItems
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }

        public int FacilitySiteId { get; set; }

        public int SiteId { get; set; }

        public string SiteName { get; set; }

        public int FacilityId { get; set; }

        public string FacilityName { get; set; }
        public string Relation { get; set; }

        public string InspectionDate { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedOn { get; set; }

    }
}
