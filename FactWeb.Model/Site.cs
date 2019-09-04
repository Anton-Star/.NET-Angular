using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Site : BaseModel
    {
        [Key, Column("SiteId")]
        public int Id { get; set; }
        [Column("SiteName")]
        public string Name { get; set; }

        [Column("SiteStartDate")]
        public DateTime StartDate { get; set; }

        //[Column("SitePhone")]
        //public string Phone { get; set; }

        //[Column("SiteStreetAddress1")]
        //public string StreetAddress1 { get; set; }

        //[Column("SiteStreetAddress2")]
        //public string StreetAddress2 { get; set; }

        //[Column("SiteCity")]
        //public string City { get; set; }

        //[Column("SiteProvince")]
        //public string Province { get; set; }

        //public int? StateId { get; set; }

        //[ForeignKey("StateId")]
        //public virtual State State { get; set; }

        //[Column("SiteZip")]
        //public string Zip { get; set; }

        //public int? CountryId { get; set; }

        //[ForeignKey("CountryId")]
        //public virtual Country Country { get; set; }

        [Column("SiteIsPrimarySite")]
        public bool IsPrimarySite { get; set; }

        //public int? ClinicalTypeId { get; set; }

        //[ForeignKey("ClinicalTypeId")]
        //public virtual ClinicalType ClinicalType { get; set; }

        //public int? ProcessingTypeId { get; set; }

        //[ForeignKey("ProcessingTypeId")]
        //public virtual ProcessingType ProcessingType { get; set; }

        public int? CollectionProductTypeId { get; set; }

        [ForeignKey("CollectionProductTypeId")]
        public virtual CollectionProductType CollectionProductType { get; set; }

        public int? ClinicalPopulationTypeId { get; set; }

        [ForeignKey("ClinicalPopulationTypeId")]
        public virtual ClinicalPopulationType ClinicalPopulationType { get; set; }

        //public int? TransplantTypeId { get; set; }

        //[ForeignKey("TransplantTypeId")]
        //public virtual TransplantType TransplantType { get; set; }

        public int? CBCollectionTypeId { get; set; }

        [ForeignKey("CBCollectionTypeId")]
        public virtual CBCollectionType CBCollectionType { get; set; }

        public int? CBUnitTypeId { get; set; }

        [ForeignKey("CBUnitTypeId")]
        public virtual CBUnitType CBUnitType { get; set; }

        public int? CBUnitsBanked { get; set; }

        public DateTime? CBUnitsBankDate { get; set; }

        [Column("SiteDescription")]
        public string Description { get; set; }

        [Column("SiteOverridenDescription")]
        public string OverridenDescription { get; set; }

        public virtual ICollection<SiteScopeType> SiteScopeTypes { get; set; }
        public virtual ICollection<SiteTransplantType> SiteTransplantTypes { get; set; }
        public virtual ICollection<SiteClinicalType> SiteClinicalTypes { get; set; }
        public virtual ICollection<SiteProcessingType> SiteProcessingTypes { get; set; }

        public virtual ICollection<FacilitySite> FacilitySites { get; set; }
        public virtual ICollection<SiteCordBloodTransplantTotal> SiteCordBloodTransplantTotals { get; set; }
        public virtual ICollection<SiteTransplantTotal> SiteTransplantTotals { get; set; }
        public virtual ICollection<SiteCollectionTotal> SiteCollectionTotals { get; set; }
        public virtual ICollection<SiteProcessingTotal> SiteProcessingTotals { get; set; }
        public virtual ICollection<SiteProcessingMethodologyTotal> SiteProcessingMethodologyTotals { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<InspectionScheduleSite> InspectionScheduleSites { get; set; }
        public virtual ICollection<SiteAddress> SiteAddresses { get; set; }
        //public virtual ICollection<SiteApplicationVersion> SiteApplicationVersions { get; set; }
    }
}

