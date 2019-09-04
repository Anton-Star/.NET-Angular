using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class AddressType : BaseModel
    {
        [Key, Column("AddressTypeId")]
        public int Id { get; set; }
        [Column("AddressTypeName")]
        public string Name { get; set; }
    }
}
