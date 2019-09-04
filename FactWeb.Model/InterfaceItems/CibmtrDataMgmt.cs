using System;

namespace FactWeb.Model.InterfaceItems
{
    public class CibmtrDataMgmt
    {
        public Guid? Id { get; set; }
        public Guid CibmtrId { get; set; }
        public DateTime? AuditDate { get; set; }
        public float? CriticalFieldErrorRate { get; set; }
        public float? RandomFieldErrorRate { get; set; }
        public float? OverallFieldErrorRate { get; set; }
        public bool? IsCapIdentified { get; set; }
        public string AuditorComments { get; set; }
        public DateTime? CpiLetterDate { get; set; }
        public Guid? CpiTypeId { get; set; }
        public string CpiTypeName { get; set; }
        public string CpiComments { get; set; }
        public string CorrectiveActions { get; set; }
        public string FactProgressDetermination { get; set; }
        public bool? IsAuditAccuracyRequired { get; set; }
        public string AdditionalInformation { get; set; }
        public string ProgressOnImplementation { get; set; }
        public string InspectorInformation { get; set; }
        public string InspectorCommendablePractices { get; set; }
        public string Inspector100DaySurvival { get; set; }
        public string Inspector1YearSurvival { get; set; }
    }
}
