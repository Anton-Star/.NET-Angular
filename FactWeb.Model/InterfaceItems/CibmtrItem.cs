using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class CibmtrItem
    {
        public Guid? Id { get; set; }
        public string CenterNumber { get; set; }
        public string CcnName { get; set; }
        public string TransplantSurvivalReportName { get; set; }
        public string DisplayName { get; set; }
        public bool IsNonCibmtr { get; set; }
        public string FacilityName { get; set; }
        public bool IsActive { get; set; }

        public List<CibmtrOutcomeAnalysis> CibmtrOutcomeAnalyses { get; set; }
        public List<CibmtrDataMgmt> CibmtrDataMgmts { get; set; }
    }
}

