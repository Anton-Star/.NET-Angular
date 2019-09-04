using System;

namespace FactWeb.Mvc.Models
{
    public class SendEmailModel
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public bool IncludeAccReport { get; set; }
        public int CycleNumber { get; set; }

        public string OrgName { get; set; }
        public Guid AppId { get; set; }
    }
}