using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.Repository
{
    public class FacilityCibmtrDataManagementRepository : BaseRepository<FacilityCibmtrDataManagement>, IFacilityCibmtrDataManagementRepository
    {
        public FacilityCibmtrDataManagementRepository(FactWebContext context) : base(context)
        {
        }

        public List<FacilityCibmtrDataManagement> GetAllByFacility(int facilityId)
        {
            return base.FetchMany(x => x.FacilityCibmtr.FacilityId == facilityId);
        }

        public List<FacilityCibmtrDataManagement> GetAllByFacility(string facilityName)
        {
            return base.FetchMany(x => x.FacilityCibmtr.Facility.Name == facilityName);
        }

        public List<FacilityCibmtrDataManagement> GetAllByCibmtr(Guid facilityCibmtrId)
        {
            return base.FetchMany(x => x.FacilityCibmtrId == facilityCibmtrId);
        }
    }
}
