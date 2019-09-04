using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class OrganizationAddress : BaseModel
    {
        [Key, Column(Order=0)]
        public int OrganizationId { get; set; }
        [Key, Column(Order = 1)]
        public int AddressId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
    }
}
