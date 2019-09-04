using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Credential : BaseModel
    {
        [Key, Column("CredentialId")]
        public int Id { get; set; }
        [Column("CredentialName")]
        public string Name { get; set; }
    }
}

