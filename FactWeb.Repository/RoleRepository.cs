using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(FactWebContext context) : base(context)
        {
        }

        public Role Get(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<Role> GetAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
