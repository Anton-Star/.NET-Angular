using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class FacilityAccreditation : BaseModel
    {
        [Key, Column("FacilityAccreditationId")]
        public int Id { get; set; }
        [Column("FacilityAccreditationName")]
        public string Name { get; set; }

        public virtual ICollection<FacilityAccreditationMapping> FacilityAccreditationMapping { get; set; }
    }
}
