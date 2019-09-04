using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Language : BaseModel
    {
        [Key, Column("LanguageId")]
        public Guid Id { get; set; }
        [Column("LanguageName")]
        public string Name { get; set; }
        [Column("LanguageOrder")]
        public int Order { get; set; }
        [Column("LanguageIsActive")]
        public bool IsActive { get; set; }

        public virtual ICollection<UserLanguage> UserLanguages { get; set; }
    }
}
