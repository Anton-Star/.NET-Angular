using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class FacilityCibmtr : BaseModel
    {
        [Key, Column("FacilityCibmtrId")]
        public Guid Id { get; set; }
        public int FacilityId { get; set; }
        [Column("FacilityCibmtrCenterNumber"), Required]
        public string CenterNumber { get; set; }
        [Column("FacilityCibmtrCcnName")]
        public string CcnName { get; set; }
        [Column("FacilityCibmtrTransplantSurvivalReportName")]
        public string TransplantSurvivalReportName { get; set; }
        [Column("FacilityCibmtrDisplayName")]
        public string DisplayName { get; set; }
        [Column("FacilityCibmtrIsNonCibmtr")]
        public bool IsNonCibmtr { get; set; }
        [Column("FacilityCibmtrIsActive")]
        public bool IsActive { get; set; }

        public virtual Facility Facility { get; set; }

        public virtual ICollection<FacilityCibmtrOutcomeAnalysis> FacilityOutcomeAnalyses { get; set; }
        public virtual ICollection<FacilityCibmtrDataManagement> FacilityCibmtrDataManagements { get; set; }
    }
}
