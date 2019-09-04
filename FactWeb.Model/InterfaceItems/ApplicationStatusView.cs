using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ApplicationStatusView
    {
        public List<ApplicationSectionRootItem> ApplicationSectionRootItems { get; set; }
        public string ApplicationType { get; set; }
        public string OrganizationName { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string ApplicationStatus { get; set; }
        public UserItem Coordinator { get; set; }
    }
}
