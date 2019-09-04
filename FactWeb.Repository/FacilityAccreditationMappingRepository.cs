using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FactWeb.Repository
{
    public class FacilityAccreditationMappingRepository : BaseRepository<FacilityAccreditationMapping>, IFacilityAccreditationMappingRepository
    {
        public FacilityAccreditationMappingRepository(FactWebContext context) : base(context)
        {
        }

        public List<FacilityAccreditationMapping> GetByFacilityId(int facilityId)
        {
            return base.FetchMany(x => x.FacilityId == facilityId);
        }


        public Task<List<FacilityAccreditationMapping>> GetByFacilityIdAsync(int facilityId)
        {
            return base.FetchManyAsync(x => x.FacilityId == facilityId);
        }
    }
}