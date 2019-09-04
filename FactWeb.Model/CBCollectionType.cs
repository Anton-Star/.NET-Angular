using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class CBCollectionType : BaseModel
    {
        [Key, Column("CBCollectionTypeId")]
        public int Id { get; set; }
        [Column("CBCollectionTypeName")]
        public string Name { get; set; }
    }
}
