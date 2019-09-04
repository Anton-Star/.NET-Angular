using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class AppResponse
    {
        public int ApplicationId { get; set; }
        public Guid ApplicationUniqueId { get; set; }
        public string ApplicationTypeName { get; set; }
        public List<ApplicationSectionResponse> ApplicationSectionResponses { get; set; }
        public string ApplicationTypeStatusName { get; set; }
    }
}
