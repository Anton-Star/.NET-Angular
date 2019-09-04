using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class SiteTransplantType : BaseModel
    {
        [Key, Column("SiteTransplantTypeId")]
        public Guid Id { get; set; }

        public int SiteId { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        public int TransplantTypeId { get; set; }

        [ForeignKey("TransplantTypeId")]
        public virtual TransplantType TransplantType { get; set; }

    }
}

