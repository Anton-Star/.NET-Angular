using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Template : BaseModel
    {
        [Key, Column("TemplateId")]
        public Guid Id { get; set; }
        [Column("TemplateName"), Required, MaxLength(100)]
        public string Name { get; set; }
        [Column("TemplateText"), Required]
        public string Text { get; set; }
    }
}
