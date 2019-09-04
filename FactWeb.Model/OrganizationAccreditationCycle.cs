using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class OrganizationAccreditationCycle : BaseModel
    {
        [Key, Column("OrganizationAccreditationCycleId")]
        public Guid Id { get; set; }
        public int OrganizationId { get; set; }
        [Column("OrganizationAccreditationCycleNumber")]
        public int Number { get; set; }
        [Column("OrganizationAccreditationCycleEffectiveDate")]
        public DateTime EffectiveDate { get; set; }
        [Column("OrganizationAccreditationCycleIsCurrent")]
        public bool IsCurrent { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
