using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ApplicationItem
    {
        public int ApplicationId { get; set; }
        public string ApplicationStatusName { get; set; }
        public string ApplicantApplicationStatusName { get; set; }
        public int ApplicationStatusId { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public Guid UniqueId { get; set; }
        public string CreatedBy { get; set; }
        //public List<SiteApplicationVersionItem> SiteApplications { get; set; }
        public SiteItems Site { get; set; }        
        public int CycleNumber { get; set; }
        public int CurrentCycleNumber { get; set; }                
        public DateTime? SubmittedDate { get; set; }
        public string SubmittedDateString { get; set; }
        public DateTime? DueDate { get; set; }
        public string DueDateString { get; set; }
        public string Template { get; set; }
        public bool IncludeAccreditationReport { get; set; }

        public DateTime? InspectionDate { get; set; }
        public string RFIDueDate { get; set; }
        public UserItem Coordinator { get; set; }
        public SiteItems PrimarySite { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OutcomeStatusName { get; set; }
        public Guid ApplicationVersionId { get; set; }
        public string ApplicationVersionTitle { get; set; }
        public List<QuestionNotApplicable> NotApplicables { get; set; }
        public Guid? ComplianceApplicationId { get; set; }
        public bool IsInspectorComplete { get; set; }
        public string CreatedDateString { get; set; }
        public List<ApplicationSectionItem> Sections { get; set; }
        public List<ApplicationWithRfi> ApplicationsWithRfis { get; set; }
        public SubmittedComplianceModel ComplianceApproval { get; set; }
        public bool ShowAccredReport { get; set; }

        public bool IsClinical { get; set; }
        public bool HasOutcome { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool HasQmRestriction { get; set; }
        public string Circle { get; set; }
        public string Standards { get; set; }
        public string TypeDetail { get; set; }
    }

    public class ApplicationWithRfi
    {
        public Guid UniqueId { get; set; }
        public Guid? ComplianceAppId { get; set; }
        public Guid ApplicationSectionId { get; set; }
        public int SiteId { get; set; }
        public string Status { get; set; }
        public string ApplicationTypeName { get; set; }
        public string RequirementNumber { get; set; }
    }
}
