using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class SiteProcessingMethodologyTotal : BaseModel
    {
        [Key, Column("SiteProcessingMethodologyTotalId")]
        public Guid Id { get; set; }
        public int SiteId { get; set; }
        public int ProcessingTypeId { get; set; }
        [Column("SiteProcessingMethodologyTotalPlatformCount")]
        public int PlatformCount { get; set; }
        [Column("SiteProcessingMethodologyTotalProtocolCount")]
        public int ProtocolCount { get; set; }
        [Column("SiteProcessingMethodologyTotalStartDate")]
        public DateTime StartDate { get; set; }
        [Column("SiteProcessingMethodologyTotalEndDate")]
        public DateTime EndDate { get; set; }

        public virtual Site Site { get; set; }
        public virtual ProcessingType ProcessingType { get; set; }
    }
}
