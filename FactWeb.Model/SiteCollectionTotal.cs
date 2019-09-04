using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class SiteCollectionTotal : BaseModel
    {
        [Key, Column("SiteCollectionTotalId")]
        public Guid Id { get; set; }
        public int SiteId { get; set; }
        public Guid CollectionTypeId { get; set; }
        public int ClinicalPopulationTypeId { get; set; }
        [Column("SiteCollectionTotalNumberOfUnits")]
        public int NumberOfUnits { get; set; }
        [Column("SiteCollectionTotalStartDate")]
        public DateTime StartDate { get; set; }
        [Column("SiteCollectionTotalEndDate")]
        public DateTime EndDate { get; set; }

        public virtual Site Site { get; set; }
        public virtual CollectionType CollectionType { get; set; }
        public virtual ClinicalPopulationType ClinicalPopulationType { get; set; }

    }
}
