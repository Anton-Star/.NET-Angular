using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class OrganizationFacility : BaseModel
    {
        [Key, Column("OrganizationFacilityId")]
        public int Id { get; set; }
                
        public int OrganizationId { get; set; }
                
        public int FacilityId { get; set; }
        [Column("OrganizationFacilityStrongRelation")]
        public bool StrongRelation { get; set; }
        [ForeignKey("FacilityId")]
        public virtual Facility Facility { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

    }
}
