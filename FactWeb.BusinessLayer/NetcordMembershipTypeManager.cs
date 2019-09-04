using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class NetcordMembershipTypeManager : BaseManager<NetcordMembershipTypeManager, INetcordMembershipTypeRepository, NetcordMembershipType>
    {
        public NetcordMembershipTypeManager(INetcordMembershipTypeRepository repository) : base(repository)
        {
        }

        public NetcordMembershipType GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<NetcordMembershipType> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}
