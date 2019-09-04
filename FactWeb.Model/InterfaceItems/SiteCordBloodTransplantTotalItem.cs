using System;

namespace FactWeb.Model.InterfaceItems
{
    public class SiteCordBloodTransplantTotalItem
    {
        public Guid? Id { get; set; }
        public int SiteId { get; set; }
        public CBUnitTypeItem CbUnitType { get; set; }
        public CBCategoryItem CbCategory { get; set; }
        public int NumberOfUnits { get; set; }
        public string AsOfDate { get; set; }
    }
}
