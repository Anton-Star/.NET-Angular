using System;

namespace FactWeb.Model.InterfaceItems
{
    public class CibmtrOutcomeAnalysis
    {
        public Guid? Id { get; set; }
        public Guid? CibmtrId { get; set; }
        public int? ReportYear { get; set; }
        public int? SurvivalScore { get; set; }
        public int? SampleSize { get; set; }
        public decimal? ActualPercent { get; set; }
        public decimal? PredictedPercent { get; set; }
        public decimal? LowerPercent { get; set; }
        public decimal? UpperPercent { get; set; }
        public string ComparativeDataSource { get; set; }
        public string PublishedOneYearSurvival { get; set; }
        public string ProgramOneYearSurvival { get; set; }
        public string Comments { get; set; }
        public string ReportedCausesOfDeath { get; set; }
        public string CorrectiveActions { get; set; }
        public string FactImprovementPlan { get; set; }
        public string AdditionalInformationRequested { get; set; }
        public string ProgressOnImplementation { get; set; }
        public string InspectorInformation { get; set; }
        public string InspectorCommendablePractices { get; set; }
        public string Inspector100DaySurvival { get; set; }
        public string Inspector1YearSurvival { get; set; }
        public bool? IsNotRequired { get; set; }
    }
}
