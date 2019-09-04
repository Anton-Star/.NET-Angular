using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class FacilityAddressRepository : BaseRepository<FacilityAddress>, IFacilityAddressRepository
    {
        public FacilityAddressRepository(FactWebContext context) : base(context)
        {
        }

        public List<FacilityAddress> GetByFacility(int facilityId)
        {
            return base.FetchMany(x => x.FacilityId == facilityId);
        }

        public Task<List<FacilityAddress>> GetByFacilityAsync(int facilityId)
        {
            return base.FetchManyAsync(x => x.FacilityId == facilityId);
        }

        public List<FacilityAddress> GetByAddress(int addressId)
        {
            return base.FetchMany(x => x.AddressId == addressId);
        }

        public Task<List<FacilityAddress>> GetByAddressAsync(int addressId)
        {
            return base.FetchManyAsync(x => x.AddressId == addressId);
        }
    }
}
