using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class OrganizationDocument : BaseModel
    {
        [Key, Column("OrganizationDocumentId")]
        public Guid Id { get; set; }
        public int OrganizationId { get; set; }
        public Guid DocumentId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Document Document { get; set; }
    }
}
