using System.Collections.Generic;
using FactWeb.Model.InterfaceItems;

namespace FactWeb.Mvc.Models
{
    public class MigrateDocumentsModel
    {
        public string OrgName { get; set; }
        public string AuthKey { get; set; }
    }

    public class IncludeInReportingModel
    {
        public string OrgName { get; set; }
        public List<DocumentItem> Documents { get; set; }
    }
}