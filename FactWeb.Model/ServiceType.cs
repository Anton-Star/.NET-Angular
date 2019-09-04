using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ServiceType:BaseModel
    {
        [Key, Column("ServiceTypeId")]
        public int Id { get; set; }
        [Column("ServiceTypeName")]
        public string Name { get; set; }
        public int? MasterServiceTypeId { get; set; }

        [ForeignKey("MasterServiceTypeId")]
        public virtual MasterServiceType MasterServiceType { get; set; }
    }
}
