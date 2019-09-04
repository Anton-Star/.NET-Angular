using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class UserCredential : BaseModel
    {
        [Key, Column("UserCredentialId")]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int CredentialId { get; set; }

        [ForeignKey("CredentialId")]
        public virtual Credential Credential { get; set; }

    }
}

