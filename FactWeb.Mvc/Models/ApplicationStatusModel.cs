using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FactWeb.Mvc.Models
{
    public class ApplicationStatusModel
    {
        public int ApplicationStatusId { get; set; }
        public string StatusForFACT { get; set; }
        public string StatusForApplicant { get; set; }
    }
}