using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;

namespace FactWeb.Mvc.Models
{
    public class SaveCoordinatorViewModel
    {
        public Guid ComplianceApplicationId { get; set; }
        public string AccreditationGoal { get; set; }
        public string InspectionScope { get; set; }
        public DateTime? AccreditedSinceDate { get; set; }
        public string TypeDetail { get; set; }
    }

    public class SavePersonnelModel
    {
        public int OrgId { get; set; }
        public List<Personnel> Personnel { get; set; }
    }
}