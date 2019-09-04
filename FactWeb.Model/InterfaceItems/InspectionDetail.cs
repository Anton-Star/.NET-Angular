namespace FactWeb.Model.InterfaceItems
{
    public class InspectionDetail
    {
        public int InspectionId { get; set; }
        public int SiteId { get; set; }
        public string SiteDescription { get; set; }
        public string OverallImpressions { get; set; }
        public string CommendablePractices { get; set; }
        public string Inspector { get; set; }
        public string SiteName { get; set; }
        public int InspectionScheduleId { get; set; }
    }
}
