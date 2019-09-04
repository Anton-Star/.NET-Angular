using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class DocumentType : BaseModel
    {
        [Key, Column("DocumentTypeId")]
        public int Id { get; set; }
        [Column("DocumentTypeName")]
        public string Name { get; set; }
    }
}
