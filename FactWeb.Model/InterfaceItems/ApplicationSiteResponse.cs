using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class ApplicationSiteResponse
    {
        public string SiteName { get; set; }
        public int SiteId { get; set; }
        public List<AppResponse> AppResponses { get; set; }
        public string StatusName { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? InspectionDate { get; set; }
        public bool IsInspectionResponsesComplete { get; set; }
        [JsonIgnore]
        public bool HasRfis { get; set; }
        [JsonIgnore]
        public bool HasFlags { get; set; }
        [JsonIgnore]
        public bool IsReviewer { get; set; }
    }
}
