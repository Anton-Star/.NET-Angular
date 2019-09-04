using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class CommentType : BaseModel
    {
        [Key, Column("CommentTypeId")]
        public int Id { get; set; }
        [Column("CommentTypeName")]
        public string Name { get; set; }        
    }
}
