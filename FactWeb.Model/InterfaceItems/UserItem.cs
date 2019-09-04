using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class UserItem
    {
        public Guid? UserId { get; set; }
        public RoleItem Role { get; set; }
        public string Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PreferredPhoneNumber { get; set; }
        public string Extension { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string WebPhotoPath { get; set; }
        public bool EmailOptOut { get; set; }
        public bool MailOptOut { get; set; }
        public string ResumePath { get; set; }
        public string StatementOfCompliancePath { get; set; }
        public DateTime? AgreedToPolicyDate { get; set; }
        public string AnnualProfessionHistoryFormPath { get; set; }
        public string MedicalLicensePath { get; set; }
        public DateTime? MedicalLicenseExpiry { get; set; }
        public bool CompletedStep2 { get; set; }
        public bool IsLocked { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsActive { get; set; }
        public bool IsAuditor { get; set; }
        public bool IsObserver { get; set; }
        public bool CanManageUsers { get; set; }
        public string UserType { get; set; }
        public string DocumentLibraryUserId { get; set; }
        public string DocumentLibraryAccessToken { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }

        public List<JobFunctionItem> JobFunctions { get; set; }
        public List<UserMembershipItem> Memberships { get; set; }
        public List<LanguageItem> Languages { get; set; }
        public List<CredentialItem> Credentials { get; set; }
        public List<UserOrganizationItem> Organizations { get; set; }

    }
}
