using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;

namespace FactWeb.Mvc.Models
{
    public class CreateApplicationModel
    {
        public string OrganizationName { get; set; }
        public string ApplicationTypeName { get; set; }        
        public Guid Coordinator { get; set; }
        public string DueDate { get; set; }
    }

    public class SaveApplicationCoordinatorModel
    {
        public Guid UniqueId { get; set; }
        public Guid Coordinator { get; set; }
        public string ApplicationStatus { get; set; }
        public string DueDate { get; set; }
    }

    public class SaveSectionModel
    {
        public string OrgName { get; set; }
        public string Type { get; set; }
        public Guid AppUniqueId { get; set; }
        public ApplicationSectionResponse Section { get; set; }
        public bool IsRfi { get; set; }
    }

    public class SaveMultiViewSectionModel
    {
        public Guid AppUniqueId { get; set; }
        public string OrgName { get; set; }
        public string Type { get; set; }
        public List<ApplicationSectionResponse> Sections { get; set; }
    }

    public class SendForInspectionModel
    {
        public Guid CompId { get; set; }
        public string OrgName { get; set; }
        public Guid CoordinatorId { get; set; }
        public bool IsCb { get; set; }
    }

    public class SaveAppModel
    {
        public string OrgName { get; set; }
        public List<ApplicationSectionItem> Sections { get; set; }
    }

    public class BulkUpdateAppStatusModel
    {
        public ApplicationSectionItem section{ get; set; }
        public int fromStatus { get; set; }
        public int toStatus { get; set; }
        public string appType { get; set; }
        public string organization { get; set; }
        public string appUniqueId { get; set; }
    }

    public class SendToFactModel
    {
        public Guid App { get; set; }
        public string LeadName { get; set; }             
        public string CoordinatorEmail { get; set; }
        public int ApplicationTypeId { get; set; }
        public string AppTypeName { get; set; }
        public int OrganizationId { get; set; }
        public string OrgName { get; set; }

    }

    public class SetComplianceApplicationApprovalStatusModel
    {
        public ComplianceApplicationItem ComplianceApplication { get; set; }
        public string SerialNumber { get; set; }
    }

    public class SetRfiFollowupModel
    {
        public int OrganizationId { get; set; }
        public string Key { get; set; }
    }
}