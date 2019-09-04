using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class AccreditationRoleRepository : BaseRepository<AccreditationRole>, IAccreditationRoleRepository
    {
        public AccreditationRoleRepository(FactWebContext context) : base(context)
        {
        }
    }
}
