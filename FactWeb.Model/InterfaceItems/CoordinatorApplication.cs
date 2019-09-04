using System;

namespace FactWeb.Model.InterfaceItems
{
    public class CoordinatorApplication
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Location { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; }
        public Guid? CoordinatorId { get; set; }
        public string Coordinator { get; set; }
        public DateTime? InspectionScheduleInspectionDate { get; set; }
        public DateTime? ApplicationDueDate { get; set; }
        public string OutcomeStatusName { get; set; }
        public DateTime? AccreditationOutcomeDate { get; set; }
        public int ApplicationStatusId { get; set; }
        public string ApplicationStatusName { get; set; }
        public int ApplicationId { get; set; }
        public Guid? ComplianceApplicationId { get; set; }
        public string ApplicationVersionTitle { get; set; }
        public Guid ApplicationUniqueId { get; set; }
        public bool? ApplicationIsActive { get; set; }
    }
}
