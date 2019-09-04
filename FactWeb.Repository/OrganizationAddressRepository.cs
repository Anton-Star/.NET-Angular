using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class OrganizationAddressRepository : BaseRepository<OrganizationAddress>, IOrganizationAddressRepository
    {
        public OrganizationAddressRepository(FactWebContext context) : base(context)
        {
        }

        public List<OrganizationAddress> GetByOrganization(int organizationId)
        {
            return base.FetchMany(x => x.OrganizationId == organizationId);
        }

        public Task<List<OrganizationAddress>> GetByOrganizationAsync(int organizationId)
        {
            return base.FetchManyAsync(x => x.OrganizationId == organizationId);
        }

        public List<OrganizationAddress> GetByAddress(int addressId)
        {
            return base.FetchMany(x => x.AddressId == addressId);
        }

        public Task<List<OrganizationAddress>> GetByAddressAsync(int addressId)
        {
            return base.FetchManyAsync(x => x.AddressId == addressId);
        }
    }
}
