using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ScopeType : BaseModel
    {
        [Key, Column("ScopeTypeId")]
        public int Id { get; set; }
        [Column("ScopeTypeName")]
        public string Name { get; set; }
        [Column("ScopeTypeImportName")]
        public string ImportName { get; set; }
        [Column("ScopeTypeIsActive")]
        public bool IsActive { get; set; }
        [Column("ScopeTypeIsArchived")]
        public bool IsArchived { get; set; }

    }
}
