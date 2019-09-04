using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationType : BaseModel
    {
        [Key, Column("ApplicationTypeId")]
        public int Id { get; set; }
        [Column("ApplicationTypeName")]
        public string Name { get; set; }

        public bool IsManageable { get; set; }

        public virtual ICollection<ApplicationSection> ApplicationSections { get; set; }
        public virtual ICollection<ApplicationVersion> ApplicationVersions { get; set; }
    }
}
