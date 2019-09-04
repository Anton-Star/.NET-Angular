using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class SiteAddressRepository : BaseRepository<SiteAddress>, ISiteAddressRepository
    {
        public SiteAddressRepository(FactWebContext context) : base(context)
        {
        }

        public List<SiteAddress> GetBySite(int siteId)
        {
            return base.FetchMany(x => x.SiteId == siteId);
        }

        public Task<List<SiteAddress>> GetBySiteAsync(int siteId)
        {
            return base.FetchManyAsync(x => x.SiteId == siteId);
        }

        public List<SiteAddress> GetByAddress(int addressId)
        {
            return base.FetchMany(x => x.AddressId == addressId);
        }

        public Task<List<SiteAddress>> GetByAddressAsync(int addressId)
        {
            return base.FetchManyAsync(x => x.AddressId == addressId);
        }
    }
}
