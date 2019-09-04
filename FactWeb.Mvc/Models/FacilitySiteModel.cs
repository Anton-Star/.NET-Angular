using FactWeb.Model.InterfaceItems;
using System.Collections.Generic;

namespace FactWeb.Mvc.Models
{
    public class FacilitySiteModel
    {
        public int FacilitySiteId { get; set; }
        public int SiteId { get; set; }
        public int FacilityId { get; set; }
        public string CurrentUser { get; set; }
    }

    public class UpdateFacilityCibmtrOutcomeModel
    {
        public List<CibmtrOutcomeAnalysis> CibmtrOutcomeAnalyses { get; set; }
        public List<CibmtrDataMgmt> CibmtrDataMgmts { get; set; }
    }
}