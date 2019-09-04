using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Country : BaseModel
    {
        [Key, Column("CountryId")]
        public int Id { get; set; }
        [Column("CountryName")]
        public string Name { get; set; }
    }
}
