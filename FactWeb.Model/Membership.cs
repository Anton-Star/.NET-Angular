using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Membership : BaseModel
    {
        [Key, Column("MembershipId")]
        public Guid Id { get; set; }
        [Column("MembershipName")]
        public string Name { get; set; }
        [Column("MembershipOrder")]
        public int Order { get; set; }
        [Column("MembershipIsActive")]
        public bool IsActive { get; set; }

        public virtual ICollection<UserMembership> UserMemberships { get; set; }
    }
}
