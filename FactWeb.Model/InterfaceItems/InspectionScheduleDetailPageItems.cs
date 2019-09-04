using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class InspectionScheduleDetailPageItems
    {
        public int InspectionScheduleId { get; set; }

        public string InspectionDate { get; set; }

        public int ApplicationTypeId { get; set; }

        public int ApplcattionId { get; set; }

        public bool ArchiveExist { get; set; }

        public List<OrganizationFacilityItems> WeakFacilities { get; set; }
        public List<OrganizationFacilityItems> SelectedFacilities { get; set; }
        public List<UserItem> Users { get; set; }
        public List<AccreditationRoleItem> Roles { get; set; }
        public List<InspectionCategoryItem> InspectionCategories{ get; set; }
        public List<InspectionScheduleDetailItems> InspectionScheduleDetailItems { get; set; }

        public List<FacilitySiteItems> FacilitySites { get; set; }
    }
}
