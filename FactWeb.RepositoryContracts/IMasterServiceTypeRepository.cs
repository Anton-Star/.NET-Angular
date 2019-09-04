using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IMasterServiceTypeRepository : IRepository<MasterServiceType>
    {
        MasterServiceType GetByName(string name);
        Task<MasterServiceType> GetByNameAsync(string name);
    }
}
