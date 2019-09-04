using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class FacilityAccreditationMapping : BaseModel
    {
        [Key, Column("FacilityAccreditationMappingId")]
        public int Id { get; set; }

        public int FacilityId { get; set; }

        public int FacilityAccreditationId { get; set; }

        [ForeignKey("FacilityId")]
        public virtual Facility Facility { get; set; }
        [ForeignKey("FacilityAccreditationId")]
        public virtual FacilityAccreditation FacilityAccreditation { get; set; }

    }
}

