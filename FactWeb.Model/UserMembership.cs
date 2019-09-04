using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class UserMembership : BaseModel
    {
        [Key, Column("UserMembershipId")]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MembershipId { get; set; }

        [Column("UserMembershipNumber"), MaxLength(100)]
        public string MembershipNumber { get; set; }

        public virtual User User { get; set; }
        public virtual Membership Membership { get; set; }
    }
}
