using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.Repository
{
    public class FacilityCibmtrOutcomeAnalysisRepository : BaseRepository<FacilityCibmtrOutcomeAnalysis>, IFacilityCibmtrOutcomeAnalysisRepository
    {
        public FacilityCibmtrOutcomeAnalysisRepository(FactWebContext context) : base(context)
        {
        }

        public List<FacilityCibmtrOutcomeAnalysis> GetAllByFacility(int facilityId)
        {
            return base.FetchMany(x => x.FacilityCibmtr.FacilityId == facilityId);
        }

        public List<FacilityCibmtrOutcomeAnalysis> GetAllByFacility(string facilityName)
        {
            return base.FetchMany(x => x.FacilityCibmtr.Facility.Name == facilityName);
        }

        public List<FacilityCibmtrOutcomeAnalysis> GetAllByCibmtr(Guid facilityCibmtrId)
        {
            return base.FetchMany(x => x.FacilityCibmtrId == facilityCibmtrId);
        }
    }
}
