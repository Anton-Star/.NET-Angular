using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class AccreditationOutcomeItem
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public int OutcomeStatusId { get; set; }
        public string OutcomeStatusName { get; set; }
        public int ReportReviewStatusId { get; set; }
        public string ReportReviewStatusName { get; set; }
        public string CommitteeDate { get; set; }
        public bool SendEmail { get; set; }
        public string EmailContent { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public bool IncludeAccreditationReport { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public List<DocumentItem> AttachedDocuments { get; set; }
        public bool UseTwoYearCycle { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
