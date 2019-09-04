using System;

namespace FactWeb.Model.InterfaceItems
{
    public class InspectionItem
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int SiteId { get; set; }
        public string SiteDescription { get; set; }
        public string CommendablePractices { get; set; }
        public string OverallImpressions { get; set; }
        public Guid ApplicationUniqueId { get; set; }
        public bool AllSitesWithDetails { get; set; }
        public UserItem User { get; set; }
        public bool IsReinspection { get; set; }
        public bool IsOverride { get; set; }
    }
}
