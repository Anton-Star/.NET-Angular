using System;

namespace FactWeb.Model.InterfaceItems
{
    public class CompAppInspectionDetail
    {
        public Guid? Id { get; set; }
        public Guid ComplianceApplicationId { get; set; }
        public int? InspectionScheduleId { get; set; }
        public int InspectorsNeeded { get; set; }
        public int ClinicalNeeded { get; set; }
        public bool? AdultSimpleExperienceNeeded { get; set; }
        public bool? AdultMediumExperienceNeeded { get; set; }
        public bool? AdultAnyExperienceNeeded { get; set; }
        public bool? PediatricSimpleExperienceNeeded { get; set; }
        public bool? PediatricMediumExperienceNeeded { get; set; }
        public bool? PediatricAnyExperienceNeeded { get; set; }
        public string Comments { get; set; }
    }
}
