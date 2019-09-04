using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace FactWeb.Model
{
    public class UserAccreditationRole : BaseModel
    {
        [Key, Column("UserAccreditationRoleId")]
        public int Id { get; set; }

        public int AccreditationRoleId { get; set; }

        public Guid UserId { get; set; }

        public virtual AccreditationRole AccreditationRole { get; set; }
        public virtual User User { get; set; }
    }
}
