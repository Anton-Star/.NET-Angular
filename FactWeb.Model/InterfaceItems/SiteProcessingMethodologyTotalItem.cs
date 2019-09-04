using System;

namespace FactWeb.Model.InterfaceItems
{
    public class SiteProcessingMethodologyTotalItem
    {
        public Guid? Id { get; set; }
        public int SiteId { get; set; }
        public ProcessingTypeItem ProcessingType { get; set; }
        public int PlatformCount { get; set; }
        public int ProtocolCount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
