using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class FacilityAddress : BaseModel
    {
        [Key, Column("FacilityAddressId")]
        public int Id { get; set; }
        
        public int FacilityId { get; set; }
        
        public int AddressId { get; set; }
        [ForeignKey("FacilityId")]
        public virtual Facility Facility { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
    }
}
