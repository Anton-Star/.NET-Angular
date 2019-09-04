using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class AccessRequestRepository : BaseRepository<AccessRequest>, IAccessRequestRepository
    {
        public AccessRequestRepository(FactWebContext context) : base(context)
        {
        }
    }
}
