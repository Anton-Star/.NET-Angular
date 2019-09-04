using System;

namespace FactWeb.Model.InterfaceItems
{
    public class InspectionScheduleItem
    {
        public  string InspectionScheduleId { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string ApplicationTypeName { get; set; }
        public string ApplicationTypeId { get; set; }
        public string ApplicationId { get; set; }
        public string InspectionDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string IsCompleted { get; set; }
        public string CompletedDate { get; set; }
        public int SiteId { get; set; }
        public string SiteName{ get; set; }
        public string IsActive { get; set; }
        public bool IsArchive { get; set; }
        public string UpdatedDate { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public Guid? ComplianceApplicationId { get; set; }
        public Guid AppUniqueId { get; set; }
            
    }
}
