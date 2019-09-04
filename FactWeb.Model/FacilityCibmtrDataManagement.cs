using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class FacilityCibmtrDataManagement : BaseModel
    {
        [Key, Column("FacilityCibmtrDataManagementId")]
        public Guid Id { get; set; }
        public Guid FacilityCibmtrId { get; set; }
        [Column("FacilityCibmtrDataManagementAuditDate")]
        public DateTime? AuditDate { get; set; }
        [Column("FacilityCibmtrDataManagementCriticalFieldErrorRate")]
        public float? CriticalFieldErrorRate { get; set; }
        [Column("FacilityCibmtrDataManagementRandomFieldErrorRate")]
        public float? RandomFieldErrorRate { get; set; }
        [Column("FacilityCibmtrDataManagementOverallFieldErrorRate")]
        public float? OverallFieldErrorRate { get; set; }
        [Column("FacilityCibmtrDataManagementIsCapIdentified")]
        public bool? IsCapIdentified { get; set; }
        [Column("FacilityCibmtrDataManagementAuditorComments")]
        public string AuditorComments { get; set; }
        [Column("FacilityCibmtrDataManagementCpiLetterDate")]
        public DateTime? CpiLetterDate { get; set; }
        public Guid? CpiTypeId { get; set; }
        [Column("FacilityCibmtrDataManagementCpiComments")]
        public string CpiComments { get; set; }

        [Column("FacilityCibmtrDataManagementCorrectiveActions")]
        public string CorrectiveActions { get; set; }
        [Column("FacilityCibmtrDataManagementFactProgressDetermination")]
        public string FactProgressDetermination { get; set; }
        [Column("FacilityCibmtrDataManagementIsAuditAccuracyRequired")]
        public bool? IsAuditAccuracyRequired { get; set; }
        [Column("FacilityCibmtrDataManagementAdditionalInformation")]
        public string AdditionalInformation { get; set; }

        [Column("FacilityCibmtrDataManagementProgressOnImplementation")]
        public string ProgressOnImplementation { get; set; }
        [Column("FacilityCibmtrDataManagementInspectorInformation")]
        public string InspectorInformation { get; set; }
        [Column("FacilityCibmtrDataManagementInspectorCommendablePractices")]
        public string InspectorCommendablePractices { get; set; }
        [Column("FacilityCibmtrDataManagementInspector100DaySurvival")]
        public string Inspector100DaySurvival { get; set; }
        [Column("FacilityCibmtrDataManagementInspector1YearSurvival")]
        public string Inspector1YearSurvival { get; set; }

        public virtual FacilityCibmtr FacilityCibmtr { get; set; }
        public virtual CpiType CpiType { get; set; }
    }
}
