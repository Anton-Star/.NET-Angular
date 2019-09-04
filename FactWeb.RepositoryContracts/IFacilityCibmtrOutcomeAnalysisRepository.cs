using FactWeb.Model;
using System;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IFacilityCibmtrOutcomeAnalysisRepository : IRepository<FacilityCibmtrOutcomeAnalysis>
    {
        List<FacilityCibmtrOutcomeAnalysis> GetAllByFacility(int facilityId);
        List<FacilityCibmtrOutcomeAnalysis> GetAllByFacility(string facilityName);
        List<FacilityCibmtrOutcomeAnalysis> GetAllByCibmtr(Guid facilityCibmtrId);
    }
}
