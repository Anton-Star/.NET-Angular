using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Distance : BaseModel
    {
        [Key, Column("DistanceId")]
        public int Id { get; set; }        
        
        public int SiteAddressId { get; set; }
        
        public int UserAddressId { get; set; }                
       
        public double DistanceInMiles { get; set; }

        [ForeignKey("SiteAddressId")]
        public virtual SiteAddress SiteAddress { get; set; }

        [ForeignKey("UserAddressId")]
        public virtual UserAddress UserAddress { get; set; }
    }
}
