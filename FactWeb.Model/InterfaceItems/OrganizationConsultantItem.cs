using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactWeb.Model.InterfaceItems
{
    public class OrganizationConsultantItem
    {
        public int OrganizationConsultantId { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ConsultantId { get; set; }

        public string ConsultantName { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedOn { get; set; }    
        
        public UserItem User { get; set; }
    }
}
