using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class SiteClinicalType : BaseModel
    {
        [Key, Column("SiteClinicalTypeId")]
        public Guid Id { get; set; }

        public int SiteId { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        public int ClinicalTypeId { get; set; }

        [ForeignKey("ClinicalTypeId")]
        public virtual ClinicalType ClinicalType { get; set; }

    }
}