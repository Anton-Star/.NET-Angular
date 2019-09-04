using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class CBCategory : BaseModel
    {
        [Key, Column("CBCategoryId")]
        public int Id { get; set; }
        [Column("CBCategoryName")]
        public string Name { get; set; }

    }
}
