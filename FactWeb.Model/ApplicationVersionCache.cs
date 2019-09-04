using System;
using System.ComponentModel.DataAnnotations;

namespace FactWeb.Model
{
    public class ApplicationVersionCache : BaseModel
    {
        [Key]
        public int ApplicationVersionCacheId { get; set; }
        public Guid ApplicationVersionId { get; set; }
        public string ApplicationVersionCacheSections { get; set; }
    }
}
