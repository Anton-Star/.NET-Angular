using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class SiteTransplantTotal : BaseModel
    {
        [Key, Column("SiteTransplantTotalId")]
        public Guid Id { get; set; }
        public int SiteId { get; set; }
        public Guid TransplantCellTypeId { get; set; }
        public int ClinicalPopulationTypeId { get; set; }
        public int TransplantTypeId { get; set; }
        [Column("SiteTransplantTotalIsHaploid")]
        public bool IsHaploid { get; set; }
        [Column("SiteTransplantTotalNumberOfUnits")]
        public int NumberOfUnits { get; set; }
        [Column("SiteTransplantTotalStartDate")]
        public DateTime StartDate { get; set; }
        [Column("SiteTransplantTotalEndDate")]
        public DateTime EndDate { get; set; }

        public virtual Site Site { get; set; }
        public virtual TransplantCellType TransplantCellType { get; set; }
        public virtual ClinicalPopulationType ClinicalPopulationType { get; set; }
        public virtual TransplantType TransplantType { get; set; }
    }
}
