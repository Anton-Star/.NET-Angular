using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class QuestionType : BaseModel
    {
        [Key, Column("QuestionTypeId")]
        public int Id { get; set; }
        [Column("QuestionTypeName"), Required, MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<ApplicationSectionQuestion> Questions { get; set; }
    }
}
