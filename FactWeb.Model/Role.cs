using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Role : BaseModel
    {
        [Key, Column("RoleId")]
        public int Id { get; set; }
        [Column("RoleName")]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
