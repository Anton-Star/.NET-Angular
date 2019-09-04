using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Facility : BaseModel
    {
        [Key, Column("FacilityId")]
        public int Id { get; set; }
        [Column("FacilityName")]
        public string Name { get; set; }
        public Guid? FacilityNumber { get; set; }
        [Column("FacilityIsActive")]
        public bool IsActive { get; set; }
        public Guid? FacilityDirectorId { get; set; }
        [Column("FacilityOtherSiteAccreditationDetails")]
        public string OtherSiteAccreditationDetails { get; set; }
        [Column("FacilityMaxtrixMax")]
        public string MaxtrixMax { get; set; }
        [Column("FacilityQMRestrictions")]
        public bool QMRestrictions { get; set; }
        [Column("FacilityNetCordMembership")]
        public bool NetCordMembership { get; set; }
        [Column("FacilityHRSA")]
        public bool HRSA { get; set; }
        
        public int? MasterServiceTypeId { get; set; }
        public int? FacilityAccreditationId { get; set; }
        
        public int ServiceTypeId { get; set; }        

        public int? PrimaryOrganizationId { get; set; }

        public Guid? NetcordMembershipTypeId { get; set; }

        [Column("FacilityProvisionalMembershipDate")]
        public DateTime? ProvisionalMembershipDate { get; set; }

        [Column("FacilityAssociateMembershipDate")]
        public DateTime? AssociateMembershipDate { get; set; }

        [Column("FacilityFullMembershipDate")]
        public DateTime? FullMembershipDate { get; set; }

        [ForeignKey("FacilityAccreditationId")]
        public virtual FacilityAccreditation FacilityAccreditation { get; set; }

        [ForeignKey("FacilityDirectorId")]
        public virtual User FacilityDirector { get; set; }

        [ForeignKey("ServiceTypeId")]
        public virtual ServiceType ServiceType { get; set; }

        [ForeignKey("PrimaryOrganizationId")]
        public virtual Organization PrimaryOrganization { get; set; }

        [ForeignKey("MasterServiceTypeId")]
        public virtual MasterServiceType MasterServiceType { get; set; }

        [ForeignKey("NetcordMembershipTypeId")]
        public virtual NetcordMembershipType NetcordMembershipType { get; set; }

        public virtual ICollection<FacilityAddress> FacilityAddresses { get; set; }

        public virtual ICollection<OrganizationFacility> OrganizationFacilities { get; set; }

        public virtual ICollection<FacilityAccreditationMapping> FacilityAccreditationMapping { get; set; }

        public virtual ICollection<FacilitySite> FacilitySites { get; set; }

        public virtual ICollection<FacilityCibmtr> FacilityCibmtrs { get; set; }
    }
}
