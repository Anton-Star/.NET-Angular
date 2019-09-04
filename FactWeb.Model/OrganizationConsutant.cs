using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class OrganizationConsutant : BaseModel
    {
        [Key, Column("OrganizationConsutantId")]
        public int Id { get; set; }

        public int OrganizationId { get; set; }

        public Guid ConsultantId { get; set; }

        [Column("OrganizationConsutantStartDate")]
        public DateTime? StartDate { get; set; }
        [Column("OrganizationConsutantEndDate")]
        public DateTime? EndDate { get; set; }

        [ForeignKey("ConsultantId")]
        public virtual User User { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

    }
}
