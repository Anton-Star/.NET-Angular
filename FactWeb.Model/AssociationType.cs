using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class AssociationType : BaseModel
    {
        [Key, Column("AssociationTypeId")]
        public Guid Id { get; set; }
        [Column("AssociationTypeName")]
        public string Name { get; set; }

        public virtual ICollection<DocumentAssociationType> DocumentAssociationTypes { get; set; }
    }
}
