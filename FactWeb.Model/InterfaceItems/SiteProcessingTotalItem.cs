using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class SiteProcessingTotalItem
    {
        public Guid? Id { get; set; }
        public int SiteId { get; set; }
        public int NumberOfUnits { get; set; }
        public List<string> SelectedTypes { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<SiteProcessingTotalTransplantCellTypeItem> SiteProcessingTotalTransplantCellTypes { get; set; }
    }
}
