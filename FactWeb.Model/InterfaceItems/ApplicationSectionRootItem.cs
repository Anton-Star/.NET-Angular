using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ApplicationSectionRootItem
    {
        public Guid ApplicationSectionId { get; set; }
        public string UniqueIdentifier { get; set; }
        public string Name { get; set; }
        public List<ApplicationSectionItemStatus> Items { get; set; }
    }
}
