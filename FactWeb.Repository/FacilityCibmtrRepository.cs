using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FactWeb.Repository
{
    public class FacilityCibmtrRepository : BaseRepository<FacilityCibmtr>, IFacilityCibmtrRepository
    {
        public FacilityCibmtrRepository(FactWebContext context) : base(context)
        {
        }

        public List<FacilityCibmtr> GetAllByFacility(int facilityId)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.FacilityCibmtrs
                .Include(x => x.FacilityOutcomeAnalyses)
                .Include(x => x.FacilityCibmtrDataManagements)
                .Include(x => x.FacilityCibmtrDataManagements.Select(y => y.CpiType))
                .Where(x => x.FacilityId == facilityId && x.IsActive)
                .ToList();
        }

        public List<FacilityCibmtr> GetAllByFacility(string facilityName)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.FacilityCibmtrs
                .Include(x => x.FacilityOutcomeAnalyses)
                .Include(x => x.FacilityCibmtrDataManagements)
                .Include(x => x.FacilityCibmtrDataManagements.Select(y => y.CpiType))
                .Where(x => x.Facility.Name == facilityName)
                .ToList();
        }

        public FacilityCibmtr GetByName(string centerName)
        {
            return base.Fetch(x => x.CenterNumber == centerName);
        }
    }
}
