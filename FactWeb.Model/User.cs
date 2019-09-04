using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class User : BaseModel
    {
        [Key, Column("UserId")]
        public Guid Id { get; set; }
        //[ForeignKey("Organization")]
        //public int? OrganizationId { get; set; }
        public int RoleId { get; set; }
        [Column("UserFirstName"), MaxLength(100), Required]
        public string FirstName { get; set; }
        [Column("UserLastName"), MaxLength(100), Required]
        public string LastName { get; set; }
        [Column("UserEmailAddress"), MaxLength(100)]
        [Index("ix_user_emailAddress", 1, IsUnique = true)]
        public string EmailAddress { get; set; }
        [Column("UserPreferredPhoneNumber"), MaxLength(20)]
        public string PreferredPhoneNumber { get; set; }
        [Column("UserPhoneExtension")]
        public string PhoneExtension { get; set; }
        [Column("UserWorkPhoneNumber"), MaxLength(20), MinLength(10)]
        public string WorkPhoneNumber { get; set; }
        [Column("UserPassword"), MaxLength(200)]
        public string Password { get; set; }
        [Column("UserIsActive")]
        public bool IsActive { get; set; }
        [Column("UserIsLocked")]
        public bool IsLocked { get; set; }
        [Column("UserFailedLoginAttempts")]
        public int? FailedLoginAttempts { get; set; }
        [Column("UserLastLoginDate")]
        public DateTime? LastLoginDate { get; set; }
        [Column("UserPasswordChangeDate")]
        public DateTime? PasswordChangeDate { get; set; }

        [Column("UserPasswordResetToken")]
        public string PasswordResetToken { get; set; }

        [Column("UserPasswordResetExpirationDate")]
        public DateTime? PasswordResetExpirationDate { get; set; }

        //public virtual Organization Organization { get; set; }
        public virtual Role Role { get; set; }

        [Column("UserWebPhotoPath"), MaxLength(100)]
        public string WebPhotoPath { get; set; }

        [Column("UserEmailOptOut")]
        public bool? EmailOptOut { get; set; }
        [Column("UserMailOptOut")]
        public bool? MailOptOut { get; set; }

        [Column("UserResumePath"), MaxLength(500)]
        public string ResumePath { get; set; }

        [Column("UserStatementOfCompliancePath"), MaxLength(500)]
        public string StatementOfCompliancePath { get; set; }
        [Column("UserAgreedToPolicyDate")]
        public DateTime? AgreedToPolicyDate { get; set; }
        [Column("UserAnnualProfessionHistoryFormPath"), MaxLength(500)]
        public string AnnualProfessionHistoryFormPath { get; set; }
        [Column("UserMedicalLicensePath"), MaxLength(500)]
        public string MedicalLicensePath { get; set; }
        [Column("UserMedicalLicenseExpiry")]
        public DateTime? MedicalLicenseExpiry { get; set; }
        [Column("UserCompletedStep2")]
        public bool? CompletedStep2 { get; set; }

        [Column("UserIsAuditor")]
        public bool? IsAuditor { get; set; }
        [Column("UserIsObserver")]
        public bool? IsObserver { get; set; }

        [Column("UserCanManageUsers")]
        public bool? CanManageUsers { get; set; }
        [Column("UserDocumentLibraryUserId")]
        public string DocumentLibraryUserId { get; set; }

        [Column("UserTwoFactorCode")]
        public string TwoFactorCode { get; set; }

        [Column("UserTitle")]
        public string Title { get; set; }

        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        public virtual ICollection<UserJobFunction> UserJobFunctions { get; set; }
        public virtual ICollection<UserLanguage> UserLanguages { get; set; }
        public virtual ICollection<UserMembership> UserMemberships { get; set; }
        public virtual ICollection<UserCredential> UserCredentials { get; set; }
        public virtual ICollection<Workflow> Workflows { get; set; }
        public virtual ICollection<UserOrganization> Organizations { get; set; }
        public virtual ICollection<OrganizationConsutant> OrganizationConsutants { get; set; }
    }
}
