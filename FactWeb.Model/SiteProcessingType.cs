using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class SiteProcessingType : BaseModel
    {
        [Key, Column("SiteProcessingTypeId")]
        public Guid Id { get; set; }

        public int SiteId { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        public int ProcessingTypeId { get; set; }

        [ForeignKey("ProcessingTypeId")]
        public virtual ProcessingType ProcessingType { get; set; }
    }
}