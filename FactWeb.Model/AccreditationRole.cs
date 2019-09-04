using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class AccreditationRole:BaseModel
    {
        [Key,Column("AccreditationRoleId")]
        public int Id { get; set; }
        [Column("AccreditationRoleName")]
        public string Name{ get; set; }
    }
}
