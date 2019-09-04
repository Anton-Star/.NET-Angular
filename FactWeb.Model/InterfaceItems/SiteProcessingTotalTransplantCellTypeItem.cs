using System;

namespace FactWeb.Model.InterfaceItems
{
    public class SiteProcessingTotalTransplantCellTypeItem
    {
        public Guid Id { get; set; }
        public Guid SiteProcessingTotalId { get; set; }
        public TransplantCellTypeItem TransplantCellType { get; set; }
    }
}
