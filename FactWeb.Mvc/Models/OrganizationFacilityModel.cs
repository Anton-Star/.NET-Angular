using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FactWeb.Mvc.Models
{
    public class OrganizationFacilityModel
    {
        public int OrganizationFacilityId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public bool Relation { get; set; }
        public string CurrentUser { get; set; }
        public string InspectionDate { get; set; }
}
    
}