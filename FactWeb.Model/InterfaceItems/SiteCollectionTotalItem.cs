using System;

namespace FactWeb.Model.InterfaceItems
{
    public class SiteCollectionTotalItem
    {
        public Guid? Id { get; set; }
        public int SiteId { get; set; }
        public CollectionTypeItem CollectionType { get; set; }
        public ClinicalPopulationTypeItem ClinicalPopulationType { get; set; }
        public int NumberOfUnits { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
