using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class NetcordMembershipTypeRepository : BaseRepository<NetcordMembershipType>, INetcordMembershipTypeRepository
    {
        public NetcordMembershipTypeRepository(FactWebContext context) : base(context)
        {
        }

        public NetcordMembershipType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<NetcordMembershipType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
