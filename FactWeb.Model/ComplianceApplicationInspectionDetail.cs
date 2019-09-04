using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ComplianceApplicationInspectionDetail : BaseModel
    {
        [Key]
        [Column("ComplianceApplicationInspectionDetailId")]
        public Guid Id { get; set; }
        public Guid ComplianceApplicationId { get; set; }
        public int? InspectionScheduleId { get; set; }
        [Column("ComplianceApplicationInspectionDetailInspectorsNeeded")]
        public int InspectorsNeeded { get; set; }
        [Column("ComplianceApplicationInspectionDetailClinicalNeeded")]
        public int ClinicalNeeded { get; set; }
        [Column("ComplianceApplicationInspectionDetailAdultSimpleExperienceNeeded")]
        public bool? AdultSimpleExperienceNeeded { get; set; }
        [Column("ComplianceApplicationInspectionDetailAdultMediumExperienceNeeded")]
        public bool? AdultMediumExperienceNeeded { get; set; }
        [Column("ComplianceApplicationInspectionDetailAdultAnyExperienceNeeded")]
        public bool? AdultAnyExperienceNeeded { get; set; }
        [Column("ComplianceApplicationInspectionDetailPediatricSimpleExperienceNeeded")]
        public bool? PediatricSimpleExperienceNeeded { get; set; }
        [Column("ComplianceApplicationInspectionDetailPediatricMediumExperienceNeeded")]
        public bool? PediatricMediumExperienceNeeded { get; set; }
        [Column("ComplianceApplicationInspectionDetailPediatricAnyExperienceNeeded")]
        public bool? PediatricAnyExperienceNeeded { get; set; }
        [Column("ComplianceApplicationInspectionDetailComments")]
        public string Comments { get; set; }

        public virtual ComplianceApplication ComplianceApplication { get; set; }
        public virtual InspectionSchedule InspectionSchedule { get; set; }
    }
}
