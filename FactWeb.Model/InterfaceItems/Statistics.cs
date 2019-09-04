using System;

namespace FactWeb.Model.InterfaceItems
{
    public class Statistics
    {
        public string SiteName { get; set; }
        public DateTime? AsOfDate { get; set; }
        public int RelatedBanked { get; set; }
        public int RelatedReleased { get; set; }
        public int RelatedOutcome { get; set; }
        public int UnrelatedBanked { get; set; }
        public int UnrelatedReleased { get; set; }
        public int UnrelatedOutcome { get; set; }

    }
}