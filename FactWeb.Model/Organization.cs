using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Organization : BaseModel
    {
        [Key, Column("OrganizationId")]
        public int Id { get; set; }
        public Guid? PrimaryUserId { get; set; }
        public int? OrganizationTypeId { get; set; }
        //public Guid? OrganizationDirectorId { get; set; }
        public int? AccreditationStatusId { get; set; }
        public int? BAAOwnerId { get; set; }
        [Column("OrganizationNumber")]
        public string Number { get; set; }
        [Column("OrganizationName")]
        public string Name { get; set; }
        [Column("OrganizationPhoneUS")]
        public string PhoneUS { get; set; }
        [Column("OrganizationPhoneUSExt")]
        public int PhoneUSExt { get; set; }
        [Column("OrganizationPhone")]
        public string Phone { get; set; }
        [Column("OrganizationPhoneExt")]
        public int PhoneExt { get; set; }
        [Column("OrganizationFaxUS")]
        public string FaxUS { get; set; }
        [Column("OrganizationFaxUSExt")]
        public int FaxUSExt { get; set; }
        [Column("OrganizationFax")]
        public string Fax { get; set; }
        [Column("OrganizationFaxExt")]
        public int FaxExt { get; set; }
        [Column("OrganizationEmail")]
        public string Email { get; set; }
        [Column("OrganizationWebSite")]
        public string WebSite { get; set; }
        [Column("OrganizationBAAExecutionDate")]
        public DateTime? BAAExecutionDate { get; set; }
        [Column("OrganizationBAADocumentVersion")]
        public string BAADocumentVersion { get; set; }
        [Column("OrganizationBaaDocumentPath")]
        public string BaaDocumentPath { get; set; }
        [Column("OrganizationBAAVerifiedDate")]
        public DateTime? BAAVerifiedDate { get; set; }
        [Column("OrganizationAccreditationDate")]
        public DateTime? AccreditationDate { get; set; }        
        [Column("OrganizationAccreditedSinceDate")]
        public DateTime? AccreditedSince { get; set; }
        [Column("OrganizationAccreditationExpirationDate")]
        public DateTime? AccreditationExpirationDate { get; set; }
        [Column("OrganizationAccreditationExtensionDate")]
        public DateTime? AccreditationExtensionDate { get; set; }
        [Column("OrganizationComments")]
        public string Comments { get; set; }
        [Column("OrganizationDescription")]
        public string Description { get; set; }
        [Column("OrganizationSpatialRelationship")]
        public string SpatialRelationship { get; set; }
        [Column("OrganizationDocumentLibraryGroupId")]
        public string DocumentLibraryGroupId { get; set; }
        [Column("OrganizationDocumentLibraryVaultId")]
        public string DocumentLibraryVaultId { get; set; }
        [Column("OrganizationDocumentLibraryAccessToken")]
        public string DocumentLibraryAccessToken { get; set; }
        [Column("OrganizationDocumentLibraryAccessTokenExpirationDate")]
        public DateTime? DocumentLibraryAccessTokenExpirationDate { get; set; }
        [Column("OrganizationUseTwoYearCycle")]
        public bool? UseTwoYearCycle { get; set; }
        [Column("OrganizationCcEmailAddresses")]
        public string CcEmailAddresses { get; set; }
        

        [ForeignKey("PrimaryUserId")]
        public virtual User PrimaryUser { get; set; }
        //public virtual User OrganizationDirector { get; set; }
        public virtual AccreditationStatus AccreditationStatus { get; set; }
        public virtual BAAOwner BAAOwner { get; set; }
        public virtual OrganizationType OrganizationType { get; set; }
        
        public virtual ICollection<OrganizationAddress> OrganizationAddresses { get; set; }
        public virtual ICollection<UserOrganization> Users { get; set; }
        public virtual ICollection<OrganizationFacility> OrganizationFacilities { get; set; }
        public virtual ICollection<OrganizationDocument> Documents { get; set; }
        public virtual ICollection<OrganizationBAADocument> BAADocuments { get; set; }
        public virtual ICollection<OrganizationConsutant> OrganizationConsutants { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<OrganizationAccreditationCycle> OrganizationAccreditationCycles { get; set; }
        public virtual ICollection<OrganizationAccreditationHistory> OrganizationAccreditationHistories { get; set; }
        public virtual ICollection<OrganizationDocumentLibrary> DocumentLibraries { get; set; }
        
    }
}
