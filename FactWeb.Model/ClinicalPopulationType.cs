using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ClinicalPopulationType : BaseModel
    {
        [Key, Column("ClinicalPopulationTypeId")]
        public int Id { get; set; }
        [Column("ClinicalPopulationTypeName")]
        public string Name { get; set; }
    }
}
