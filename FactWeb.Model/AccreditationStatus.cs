using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class AccreditationStatus : BaseModel
    {
        [Key, Column("AccreditationStatusId")]
        public int Id { get; set; }
        [Column("AccreditationStatusName")]
        public string Name { get; set; }
    }
}

