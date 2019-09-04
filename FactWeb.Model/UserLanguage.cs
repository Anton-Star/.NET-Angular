using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class UserLanguage : BaseModel
    {
        [Key, Column("UserLanguageId")]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid LanguageId { get; set; }

        public virtual User User { get; set; }
        public virtual Language Language { get; set; }

    }
}
