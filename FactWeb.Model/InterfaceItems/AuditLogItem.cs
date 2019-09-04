using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactWeb.Model.InterfaceItems
{
    public class AuditLogItem
    {
        public int AuditLogId { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
        public string DateTime { get; set; }
        public string Description { get; set; }
    }
}
