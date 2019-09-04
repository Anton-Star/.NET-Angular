using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class SiteItems
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string SiteStartDate { get; set; }
        public string SitePhone { get; set; }
        public string SiteStreetAddress1 { get; set; }
        public string SiteStreetAddress2 { get; set; }
        public string SiteCity { get; set; }
        public bool SiteIsPrimarySite { get; set; }
        public string SiteProvince { get; set; }
        public StateItem SiteState { get; set; }
        public string SiteZip { get; set; }
        public CountryItem SiteCountry { get; set; }
        public bool? IsPrimaryAddress { get; set; }
        //public ClinicalTypeItem SiteClinicalType { get; set; }
        //public ProcessingTypeItem SiteProcessingType { get; set; }
        public CollectionProductTypeItem SiteCollectionProductType { get; set; }
        public CBCollectionTypeItem SiteCBCollectionType { get; set; }
        public CBUnitTypeItem SiteCBUnitType { get; set; }
        public ClinicalPopulationTypeItem SiteClinicalPopulationType { get; set; }
        //public TransplantTypeItem SiteTransplantType { get; set; }
        public int SiteUnitsBanked { get; set; }
        public string SiteUnitsBankedDate { get; set; }
        public List<ScopeTypeItem> ScopeTypes { get; set; }
        public List<TransplantTypeItem> TransplantTypes { get; set; }
        public List<ClinicalTypeItem> ClinicalTypes { get; set; }
        public List<ProcessingTypeItem> ProcessingTypes { get; set; }
        public int CurrentFacilityId { get; set; }
        //public List<AddressItem> Addresses { get; set; }
        public int SiteFacilityId { get; set; }
        public string SiteInspectionDate { get; set; }
        public string SiteDescription { get; set; }
        public bool IsStrong { get; set; }
        public List<FacilityItems> Facilities { get; set; }
        public List<SiteCordBloodTransplantTotalItem> SiteCordBloodTransplantTotals { get; set; }
        public List<SiteTransplantTotalItem> SiteTransplantTotals { get; set; }
        public List<SiteCollectionTotalItem> SiteCollectionTotals { get; set; }
        public List<SiteProcessingTotalItem> SiteProcessingTotals { get; set; }
        public List<SiteProcessingMethodologyTotalItem> SiteProcessingMethodologyTotals { get; set; }
        public List<SiteAddressItem> SiteAddresses { get; set; }
    }
}
