using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class CollectionProductType : BaseModel
    {
        [Key, Column("CollectionProductTypeId")]
        public int Id { get; set; }
        [Column("CollectionProductTypeName")]
        public string Name { get; set; }
    }
}
