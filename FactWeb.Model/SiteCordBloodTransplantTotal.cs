using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class SiteCordBloodTransplantTotal : BaseModel
    {
        [Key, Column("SiteCordBloodTransplantTotalId")]
        public Guid Id { get; set; }
        public int SiteId { get; set; }

        public int CBUnitTypeId { get; set; }
        public int CBCategoryId { get; set; }
        [Column("SiteCordBloodTransplantTotalNumberOfUnits")]
        public int NumberOfUnits { get; set; }
        [Column("SiteCordBloodTransplantTotalAsOfDate")]
        public DateTime AsOfDate { get; set; }

        public virtual Site Site { get; set; }
        public virtual CBUnitType CbUnitType { get; set; }
        public virtual CBCategory CbCategory { get; set; }
    }
}
