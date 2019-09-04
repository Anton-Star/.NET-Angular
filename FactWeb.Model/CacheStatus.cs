using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class CacheStatus : BaseModel
    {
        [Key, Column("CacheStatusObjectName")]
        public string ObjectName { get; set; }
        [Column("CacheStatusLastChangeDate")]
        public DateTime LastChangeDate { get; set; }
    }
}
