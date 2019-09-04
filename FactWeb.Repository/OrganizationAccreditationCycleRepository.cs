using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class OrganizationAccreditationCycleRepository : BaseRepository<OrganizationAccreditationCycle>, IOrganizationAccreditationCycleRepository
    {
        public OrganizationAccreditationCycleRepository(FactWebContext context) : base(context)
        {
        }

        public List<OrganizationAccreditationCycle> GetByOrganization(int organizationId)
        {
            return base.FetchMany(x => x.OrganizationId == organizationId);
        }

        public Task<List<OrganizationAccreditationCycle>> GetByOrganizationAsync(int organizationId)
        {
            return base.FetchManyAsync(x => x.OrganizationId == organizationId);
        }

        public OrganizationAccreditationCycle GetCurrentByOrganization(int organizationId)
        {
            return base.Fetch(x => x.OrganizationId == organizationId && x.IsCurrent);
        }

        public Task<OrganizationAccreditationCycle> GetCurrentByOrganizationAsync(int organizationId)
        {
            return base.FetchAsync(x => x.OrganizationId == organizationId && x.IsCurrent);
        }
    }
}
