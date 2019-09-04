using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactWeb.Model.InterfaceItems
{
    public class ServiceTypeItem
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public int? MasterServiceTypeId { get; set; }
        public MasterServiceTypeItem MasterServiceTypeItem { get; set; }
    }
}
