using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ApplicationVersionItem
    {
        public Guid Id { get; set; }
        public ApplicationTypeItem ApplicationType { get; set; }
        public string VersionNumber { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public Guid? CopyFromId { get; set; }
        public List<ApplicationSectionItem> ApplicationSections { get; set; }
    }
}
