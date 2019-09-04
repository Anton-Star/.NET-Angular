using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ITransplantTypeRepository : IRepository<TransplantType>
    {
        TransplantType GetByName(string name);
        Task<TransplantType> GetByNameAsync(string name);
    }
}
