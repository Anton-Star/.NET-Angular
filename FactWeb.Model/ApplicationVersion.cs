using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationVersion : BaseModel
    {
        [Key, Column("ApplicationVersionId")]
        public Guid Id { get; set; }
        public int ApplicationTypeId { get; set; }
        [Column("ApplicationVersionTitle"), MaxLength(200), Required]
        public string Title { get; set; }
        [Column("ApplicationVersionNumber")]
        public string VersionNumber { get; set; }
        [Column("ApplicationVersionIsActive")]
        public bool IsActive { get; set; }

        [Column("ApplicationVersionIsDeleted")]
        public bool? IsDeleted { get; set; }

        public virtual ApplicationType ApplicationType { get; set; }

        public virtual ICollection<ApplicationSection> ApplicationSections { get; set; }
        //public virtual ICollection<SiteApplicationVersion> SiteApplicationVersions { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
    }
}
