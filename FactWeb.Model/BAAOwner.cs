using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class BAAOwner : BaseModel
    {
        [Key, Column("BAAOwnerId")]
        public int Id { get; set; }
        [Column("BAAOwnerName")]
        public string Name { get; set; }
    }
}


