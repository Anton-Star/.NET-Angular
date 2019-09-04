using System;

namespace FactWeb.Model.InterfaceItems
{
    public class ApplicationStatusHistoryItem
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationStatusItem ApplicationStatusOld { get; set; }
        public ApplicationStatusItem ApplicationStatusNew { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}