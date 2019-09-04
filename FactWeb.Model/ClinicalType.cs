using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ClinicalType : BaseModel
    {
        [Key, Column("ClinicalTypeId")]
        public int Id { get; set; }
        [Column("ClinicalTypeName")]
        public string Name { get; set; }
    }
}
