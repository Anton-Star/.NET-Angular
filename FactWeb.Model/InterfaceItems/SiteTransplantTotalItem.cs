using System;

namespace FactWeb.Model.InterfaceItems
{
    public class SiteTransplantTotalItem
    {
        public Guid? Id { get; set; }
        public int SiteId { get; set; }
        public TransplantCellTypeItem TransplantCellType { get; set; }
        public ClinicalPopulationTypeItem ClinicalPopulationType { get; set; }
        public TransplantTypeItem TransplantType { get; set; }
        public bool IsHaploid { get; set; }
        public int NumberOfUnits { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
