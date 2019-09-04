using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class CollectionType : BaseModel
    {
        [Key, Column("CollectionTypeId")]
        public Guid Id { get; set; }
        [Column("CollectionTypeName")]
        public string Name { get; set; }

    }
}
