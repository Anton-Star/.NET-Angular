using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class TransplantType : BaseModel
    {
        [Key, Column("TransplantTypeId")]
        public int Id { get; set; }
        [Column("TransplantTypeName")]
        public string Name { get; set; }
    }
}
