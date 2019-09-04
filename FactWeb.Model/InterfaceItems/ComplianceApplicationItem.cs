using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ComplianceApplicationItem
    {
        public Guid? Id { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public bool HasQmRestriction { get; set; }
        public string AccreditationGoal { get; set; }
        public string ApplicationStatus { get; set; }
        public string InspectionScope { get; set; }
        public string RejectionComments { get; set; }
        public string ReportReviewStatus { get; set; }
        public string ReportReviewStatusName { get; set; }
        //public string AccreditationStatusName { get; set; }
        public List<ComplianceApplicationSite> ComplianceApplicationSites { get; set; }
        public List<ComplianceApplicationServiceType> ComplianceApplicationServiceTypes { get; set; }
        public UserItem Coordinator { get; set; }
        public ComplianceApplicationApprovalStatusItem ApprovalStatus { get; set; }
        public List<ApplicationItem> Applications { get; set; }
        public string DueDate{ get; set; }
        public bool ShowAccreditationReport { get; set; }
        public bool IsReinspection { get; set; }
        public bool HasRfi { get; set; }
        public bool HasFlag { get; set; }
        public string CreatedDate { get; set; }
        public DateTime? AccreditedSince { get; set; }
        public string TypeDetail { get; set; }
    }
}
