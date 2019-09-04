using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationSection : BaseModel
    {
        [Key, Column("ApplicationSectionId")]
        public Guid Id { get; set; }

        public int ApplicationTypeId { get; set; }
        public Guid? ParentApplicationSectionId { get; set; }
        public Guid ApplicationVersionId { get; set; }
        [Column("ApplicationSectionPartNumber")]
        public int PartNumber { get; set; }
        [Column("ApplicationSectionName"), Required]
        public string Name { get; set; }
        [Column("ApplicationSectionIsActive")]
        public bool IsActive { get; set; }
        [Column("ApplicationSectionHelpText")]
        public string HelpText { get; set; }
        [Column("ApplicationSectionIsVariance")]
        public bool? IsVariance { get; set; }
        [Column("ApplicationSectionVersion"), MaxLength(20)]
        public string Version { get; set; }
        [Column("ApplicationSectionOrder")]
        public string Order { get; set; }
        [Column("ApplicationSectionUniqueIdentifier")]
        public string UniqueIdentifier { get; set; }
                
        public virtual ApplicationSection ParentApplicationSection { get; set; }

        public virtual ApplicationType ApplicationType { get; set; }
        public virtual ApplicationVersion ApplicationVersion { get; set; }

        public virtual ICollection<ApplicationSectionQuestion> Questions { get; set; }
        public virtual ICollection<ApplicationSection> Children { get; set; }

        public virtual ICollection<ApplicationSectionScopeType> ApplicationSectionScopeTypes { get; set; }
    }
}
