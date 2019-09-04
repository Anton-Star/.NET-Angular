using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class UserAddress : BaseModel
    {
        [Key, Column("UserAddressId")]
        public int Id { get; set; }
        public Guid UserId { get; set; }
       
        public int AddressId { get; set; }

        public virtual User User { get; set; }
        public virtual Address Address { get; set; }
    }
}
