using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class FacilityCibmtrOutcomeAnalysisManager : BaseManager<FacilityCibmtrOutcomeAnalysisManager, IFacilityCibmtrOutcomeAnalysisRepository, FacilityCibmtrOutcomeAnalysis>
    {
        public FacilityCibmtrOutcomeAnalysisManager(IFacilityCibmtrOutcomeAnalysisRepository repository) : base(repository)
        {
        }

        public List<FacilityCibmtrOutcomeAnalysis> GetAllByFacility(int facilityId)
        {
            return base.Repository.GetAllByFacility(facilityId);
        }

        public List<FacilityCibmtrOutcomeAnalysis> GetAllByFacility(string facilityName)
        {
            return base.Repository.GetAllByFacility(facilityName);
        }

        public List<FacilityCibmtrOutcomeAnalysis> GetAllByCibmtr(Guid facilityCibmtrId)
        {
            return base.Repository.GetAllByCibmtr(facilityCibmtrId);
        }
    }
}
