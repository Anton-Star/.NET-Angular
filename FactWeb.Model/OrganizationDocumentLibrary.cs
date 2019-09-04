using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class OrganizationDocumentLibrary : BaseModel
    {
        [Key, Column("OrganizationDocumentLibraryId")]
        public Guid Id { get; set; }
        public int OrganizationId { get; set; }
        [Column("OrganizationDocumentLibraryVaultId")]
        public string VaultId { get; set; }
        [Column("OrganizationDocumentLibraryCycleNumber")]
        public int CycleNumber { get; set; }
        [Column("OrganizationDocumentLibraryIsCurrent")]
        public bool IsCurrent { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
