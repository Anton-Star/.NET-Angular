using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class OrganizationTypeRepository : BaseRepository<OrganizationType>, IOrganizationTypeRepository
    {
        public OrganizationTypeRepository(FactWebContext context) : base(context)
        {
        }
    }
}
