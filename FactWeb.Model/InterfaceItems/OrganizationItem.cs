using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class OrganizationItem
    {
        public int OrganizationId { get; set; }
        public string OrganizationNumber { get; set; }
        public string OrganizationName { get; set; }        
        public string OrganizationPhoneUS { get; set; }        
        public int OrganizationPhoneUSExt { get; set; }        
        public string OrganizationPhone { get; set; }        
        public int OrganizationPhoneExt { get; set; }        
        public string OrganizationFaxUS { get; set; }        
        public int OrganizationFaxUSExt { get; set; }        
        public string OrganizationFax { get; set; }        
        public int OrganizationFaxExt { get; set; }        
        public string OrganizationEmail { get; set; }        
        public string OrganizationWebSite { get; set; }
        public OrganizationTypeItem OrganizationTypeItem { get; set; }
        public OrganizationAddressItem OrganizationAddressItem { get; set; }
        public UserItem PrimaryUser { get; set; }
        public List<OrganizationFacilityItems> Facilities { get; set; }

        public List<UserItem> OrganizationDirectors { get; set; }

        public AccreditationStatusItem AccreditationStatusItem { get; set; }

        public BAAOwnerItem BAAOwnerItem { get; set; }
        public Guid? EligibilityApplicationUniqueId { get; set; }
        public Guid? ComplianceApplicationUniqueId { get; set; }
        public Guid? ApplicationUniqueId { get; set; }

        public Guid? RenewalApplicationUniqueId { get; set; }

        public string BAAExecutionDate { get; set; }

        public string BAADocumentVersion { get; set; }
        public string BaaDocumentPath { get; set; }

        public string BAAVerifiedDate { get; set; }

        public string AccreditationDate { get; set; }

        public string AccreditationExpirationDate { get; set; }

        public string AccreditationExtensionDate { get; set; }

        public string AccreditedSince { get; set; }

        public string Comments { get; set; }

        public int? CycleNumber { get; set; }
        public DateTime? CycleEffectiveDate { get; set; }
        public string Description { get; set; }
        public string SpatialRelationship { get; set; }
        public string DocumentLibraryGroupId { get; set; }
        public string DocumentLibraryVaultId { get; set; }
        public string DocumentLibraryAccessToken { get; set; }
        public bool UseTwoYearCycle { get; set; }
        public string CcEmailAddresses { get; set; }

        public List<FacilityItems> FacilityItems { get; set; }
        public List<DocumentItem> BAADocumentItems { get; set; }
        public List<string> FacilityDirectors { get; set; }

    }
}
