using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Document : BaseModel
    {
        [Key, Column("DocumentId")]
        public Guid Id { get; set; }

        public Guid? OrganizationDocumentLibraryId { get; set; }
        [Column("DocumentName"), MaxLength(500), Required]
        public string Name { get; set; }
        [Column("DocumentOriginalName"), MaxLength(500)]
        public string OriginalName { get; set; }
        [Column("DocumentFactStaffOnly")]
        public bool FactStaffOnly { get; set; }
        public int? DocumentTypeId { get; set; }
        public int? ApplicationId { get; set; }
        public string RequestValues { get; set; }
        [Column("DocumentIncludeInReporting")]
        public bool? IncludeInReporting { get; set; }

        [Column("DocumentIsLatestVersion")]
        public bool IsLatestVersion { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        public virtual Application Application { get; set; }
        public virtual ICollection<OrganizationDocument> OrganizationDocuments { get; set; }
        public virtual ICollection<OrganizationBAADocument> OrganizationBAADocuments { get; set; }        
        public virtual ICollection<DocumentAssociationType> AssociationTypes { get; set; }
        public virtual ICollection<ApplicationResponse> ApplicationResponses { get; set; }
        public virtual OrganizationDocumentLibrary OrganizationDocumentLibrary { get; set; }
    }
}
