using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class OrganizationAccreditationHistoryRepository : BaseRepository<OrganizationAccreditationHistory>, IOrganizationAccreditationHistoryRepository
    {
        public OrganizationAccreditationHistoryRepository(FactWebContext context) : base(context)
        {
        }
    }
}
