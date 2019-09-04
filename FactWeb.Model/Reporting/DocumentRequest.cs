using System.Collections.Generic;

namespace FactWeb.Model.Reporting
{
    public class DocumentRequest
    {
        public string ReportId { get; set; }
        public string Format { get; set; }
        public List<string> ParameterValues { get; set; }
    }
}
