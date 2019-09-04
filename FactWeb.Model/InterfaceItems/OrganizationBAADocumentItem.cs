using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactWeb.Model.InterfaceItems
{
    public class OrganizationBAADocumentItem
    {
        public Guid Id { get; set; }
        public int OrganizationId { get; set; }
        public Guid DocumentId { get; set; }
        public virtual DocumentItem DocumentItem { get; set; }
    }
}
