using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class FacilityCibmtrOutcomeAnalysis : BaseModel
    {
        [Key, Column("FacilityOutcomeAnalysisId")]
        public Guid Id { get; set; }
        public Guid FacilityCibmtrId { get; set; }
        [Column("FacilityOutcomeAnalysisReportYear")]
        public int? ReportYear { get; set; }
        [Column("FacilityOutcomeAnalysisSurvivalScore")]
        public int? SurvivalScore { get; set; }
        [Column("FacilityOutcomeAnalysisSampleSize")]
        public int? SampleSize { get; set; }
        [Column("FacilityOutcomeAnalysisActualPercent")]
        public decimal? ActualPercent { get; set; }
        [Column("FacilityOutcomeAnalysisPredictedPercent")]
        public decimal? PredictedPercent { get; set; }
        [Column("FacilityOutcomeAnalysisLowerPercent")]
        public decimal? LowerPercent { get; set; }
        [Column("FacilityOutcomeAnalysisUpperPercent")]
        public decimal? UpperPercent { get; set; }
        [Column("FacilityOutcomeAnalysisComparativeDataSource")]
        public string ComparativeDataSource { get; set; }
        [Column("FacilityOutcomeAnalysisPublishedOneYearSurvival")]
        public string PublishedOneYearSurvival { get; set; }
        [Column("FacilityOutcomeAnalysisProgramOneYearSurvival")]
        public string ProgramOneYearSurvival { get; set; }
        [Column("FacilityOutcomeAnalysisComments")]
        public string Comments { get; set; }
        [Column("FacilityOutcomeAnalysisReportedCausesOfDeath")]
        public string ReportedCausesOfDeath { get; set; }
        [Column("FacilityOutcomeAnalysisCorrectiveActions")]
        public string CorrectiveActions { get; set; }
        [Column("FacilityOutcomeAnalysisFactImprovementPlan")]
        public string FactImprovementPlan { get; set; }
        [Column("FacilityOutcomeAnalysisAdditionalInformationRequested")]
        public string AdditionalInformationRequested { get; set; }

        [Column("FacilityOutcomeAnalysisProgressOnImplementation")]
        public string ProgressOnImplementation { get; set; }
        [Column("FacilityOutcomeAnalysisInspectorInformation")]
        public string InspectorInformation { get; set; }
        [Column("FacilityOutcomeAnalysisInspectorCommendablePractices")]
        public string InspectorCommendablePractices { get; set; }
        [Column("FacilityOutcomeAnalysisInspector100DaySurvival")]
        public string Inspector100DaySurvival { get; set; }
        [Column("FacilityOutcomeAnalysisInspector1YearSurvival")]
        public string Inspector1YearSurvival { get; set; }
        [Column("FacilityCibmtrOutcomeAnalysisIsNotRequired")]
        public bool? IsNotRequired { get; set; }

        public virtual FacilityCibmtr FacilityCibmtr { get; set; }
    }
}
