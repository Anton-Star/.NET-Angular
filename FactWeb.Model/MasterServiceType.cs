using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class MasterServiceType : BaseModel
    {
        [Key, Column("MasterServiceTypeId")]
        public int Id { get; set; }
        [Column("MasterServiceTypeName")]
        public string Name { get; set; }
        [Column("MasterServiceTypeShortName")]
        public string ShortName { get; set; }
    }
}
