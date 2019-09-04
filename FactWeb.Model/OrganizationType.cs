using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FactWeb.Model
{
    public class OrganizationType : BaseModel
    {
        [Key, Column("OrganizationTypeId")]
        public int Id { get; set; }
        [Column("OrganizationTypeName")]
        public string Name { get; set; }
    }
}
