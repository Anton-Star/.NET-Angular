using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class SiteAddress : BaseModel
    {
        [Key, Column("SiteAddressId")]
        public int Id { get; set; }
        
        public int SiteId { get; set; }
        
        public int AddressId { get; set; }

        public bool?  IsPrimaryAddress { get; set; }
        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        public virtual ICollection<Distance> Distances { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
