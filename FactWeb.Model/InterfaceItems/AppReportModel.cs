using System;

namespace FactWeb.Model.InterfaceItems
{
    public class AppReportModel
    {
        public int ApplicationId { get; set; }
        public Guid? ApplicationSectionId { get; set; }
        public string HasSection { get; set; }
        public string Text { get; set; }
        public string Response { get; set; }
        public string Comments { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public int Order2 { get; set; }
        public int Order3 { get; set; }
        public int Order4 { get; set; }
        public int Order5 { get; set; }
    }
}
