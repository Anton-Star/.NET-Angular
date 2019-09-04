using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class SiteScopeType : BaseModel
    {
        [Key, Column("SiteScopeTypeId")]
        public Guid Id { get; set; }

        public int SiteId { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        public int ScopeTypeId { get; set; }

        [ForeignKey("ScopeTypeId")]
        public virtual ScopeType ScopeType { get; set; }

    }
}