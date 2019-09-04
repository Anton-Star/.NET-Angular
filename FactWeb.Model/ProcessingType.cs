using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ProcessingType : BaseModel
    {
        [Key, Column("ProcessingTypeId")]
        public int Id { get; set; }
        [Column("ProcessingTypeName")]
        public string Name { get; set; }
    }
}
