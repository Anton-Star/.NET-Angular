using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class SiteProcessingTotal : BaseModel
    {
        [Key, Column("SiteProcessingTotalId")]
        public Guid Id { get; set; }
        public int SiteId { get; set; }
        [Column("SiteProcessingTotalNumberOfUnits")]
        public int NumberOfUnits { get; set; }
        [Column("SiteProcessingTotalStartDate")]
        public DateTime StartDate { get; set; }
        [Column("SiteProcessingTotalEndDate")]
        public DateTime EndDate { get; set; }

        public virtual Site Site { get; set; }

        public Guid? TransplantCellTypeId { get; set; }

        public virtual ICollection<SiteProcessingTotalTransplantCellType> SiteProcessingTotalTransplantCellTypes { get; set; }
        public virtual TransplantCellType TransplantCellType { get; set; }
    }
}
