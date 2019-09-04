using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class OrganizationAccreditationHistory : BaseModel
    {
        [Key, Column("OrganizationAccreditationHistoryId")]
        public Guid Id { get; set; }
        public int OrganizationId { get; set; }
        public int? AccreditationStatusId { get; set; }
        [Column("OrganizationAccreditationHistoryAccreditationDate")]
        public DateTime? AccreditationDate { get; set; }
        [Column("OrganizationAccreditationHistoryExpirationDate")]
        public DateTime? ExpirationDate { get; set; }
        [Column("OrganizationAccreditationHistoryExtensionDate")]
        public DateTime? ExtensionDate { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual AccreditationStatus AccreditationStatus { get; set; }
    }
}
