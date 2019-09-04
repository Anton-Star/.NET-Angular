using System;

namespace FactWeb.Model.InterfaceItems
{
    public class ApplicationCacheModels
    {
        public class Key
        {
            public int ApplicationTypeId { get; set; }
            public Guid ApplicationVersionId { get; set; }
        }
    }
}
