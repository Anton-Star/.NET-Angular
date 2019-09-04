using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Address : BaseModel
    {
        [Key, Column("AddressId")]
        public int Id { get; set; }
        public int AddressTypeId { get; set; }
        [ForeignKey("AddressTypeId")]
        public virtual AddressType AddressType { get; set; }
        [Column("AddressStreet1")]
        public string Street1 { get; set; }
        [Column("AddressStreet2")]
        public string Street2 { get; set; }
        [Column("AddressCity")]
        public string City { get; set; }
        [Column("AddressState")]
        public int? StateId { get; set; }
        [ForeignKey("StateId")]
        public virtual State State { get; set; }
        [Column("AddressProvince")]
        public string Province { get; set; }
        [Column("AddressZipCode")]
        public string ZipCode { get; set; }
        [Column("AddressPhone")]
        public string Phone { get; set; }
        [Column("AddressCountry")]
        public int? CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        [Column("AddressLogitude")]
        public string Logitude { get; set; }
        [Column("AddressLatitude")]
        public string Latitude { get; set; }
        
        public virtual ICollection<OrganizationAddress> OrganizationAddresses { get; set; }
        //public virtual ICollection<SiteAddress> SiteAddresses { get; set; }
        
    }
}
