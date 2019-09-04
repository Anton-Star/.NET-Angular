using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class FacilityItems
    {
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public Guid? FacilityNumber { get; set; }
        public bool isActive { get; set; }
        public string FacilityAddress{ get; set; }
        public Guid? FacilityDirectorId { get; set; }
        public int? PrimaryOrganizationId { get; set; }
        public string OtherSiteAccreditationDetails { get; set; }
        public string MaxtrixMax { get; set; }
        public bool QMRestrictions { get; set; }
        public bool NetCordMembership { get; set; }
        public bool HRSA { get; set; }
        public int? MasterServiceTypeId { get; set; }        
        public int ServiceTypeId { get; set; }
        public string CBCollectionSiteType { get; set; }
        public string PrimaryOrganizationName { get; set; }

        public Guid? NetcordMembershipTypeId { get; set; }

        public string ProvisionalMembershipDate { get; set; }
        
        public string AssociateMembershipDate { get; set; }
        public string FullMembershipDate { get; set; }

        public List<SiteTotalItem> SiteTotals { get; set; }
        public List<SiteItems> Sites { get; set; }
        public List<FacilityAccreditationItem> FacilityAccreditation { get; set; }
        public UserItem FacilityDirector { get; set; }
        public ServiceTypeItem ServiceType { get; set; }
        public MasterServiceTypeItem MasterServiceType { get; set; }
        public List<OrganizationFacilityItems> Organizations { get; set; }
        public OrganizationItem PrimaryOrganization { get; set; }
        public NetcordMembershipTypeItem NetcordMembershipType { get; set; }
 
    }
}
