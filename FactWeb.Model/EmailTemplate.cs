using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class EmailTemplate : BaseModel
    {
        [Key, Column("EmailTemplateId")]
        public Guid Id { get; set; }
        [Column("EmailTemplateName")]
        public string Name { get; set; }
        [Column("EmailTemplateHtml")]
        public string Html { get; set; }
    }
}
