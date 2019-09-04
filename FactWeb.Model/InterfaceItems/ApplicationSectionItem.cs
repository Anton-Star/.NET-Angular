using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ApplicationSectionItem
    {
        public Guid? Id { get; set; }
        public int? PartNumber { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public bool? IsVariance { get; set; }
        public string HelpText { get; set; }
        public string Version { get; set; }
        public string Order { get; set; }
        public List<ApplicationSectionItem> Children { get; set; }
        public List<Question> Questions { get; set; }
        public string UniqueIdentifier { get; set; }
        public List<ScopeTypeItem> ScopeTypes { get; set; }
        public Guid? ParentId { get; set; }
        public string ApplicationTypeName { get; set; }
        public Guid? VersionId { get; set; }

        public Guid AppUniqueId { get; set; }

        public string Circle { get; set; }
        public string CircleStatusName { get; set; }
        public bool IsVisible { get; set; }
    }
}
