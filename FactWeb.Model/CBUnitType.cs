using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class CBUnitType : BaseModel
    {
        [Key, Column("CBUnitTypeId")]
        public int Id { get; set; }
        [Column("CBUnitTypeName")]
        public string Name { get; set; }
    }
}
