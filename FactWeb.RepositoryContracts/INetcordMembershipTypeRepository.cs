using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface INetcordMembershipTypeRepository : IRepository<NetcordMembershipType>
    {
        NetcordMembershipType GetByName(string name);
        Task<NetcordMembershipType> GetByNameAsync(string name);
    }
}
