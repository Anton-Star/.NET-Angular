using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class TransplantCellType : BaseModel
    {
        [Key, Column("TransplantCellTypeId")]
        public Guid Id { get; set; }
        [Column("TransplantCellTypeName")]
        public string Name { get; set; }

    }
}
