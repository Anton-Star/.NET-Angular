using System;

namespace FactWeb.Model.InterfaceItems
{
    public class Overview
    {
        public string Type { get; set; }
        public string TypeName { get; set; }
        public DateTime? AccreditedSince { get; set; }
        public string InspectionDate { get; set; }
        public string Standards { get; set; }
        public string AccreditationGoal { get; set; }
        public string InspectionScope { get; set; }
        public string SiteDescription { get; set; }
        public string CommendablePractices { get; set; }
        public string OverallImpressions { get; set; }
    }
}