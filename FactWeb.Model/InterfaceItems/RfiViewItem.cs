using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactWeb.Model.InterfaceItems
{
    public class RfiViewItem
    {
        public int RFIsBeforeInspection { get; set; }
        public int RFIsAfterInspection { get; set; }
        public int TotalRFIStandards { get; set; }
        public int TotalRFIs { get; set; }
        public int TotalSites { get; set; }
        public List<SiteApplicationSection> SiteApplicationSection { get; set; }
        //public List<ApplicationSectionItem> ApplicationSection { get; set; }
    }
}
