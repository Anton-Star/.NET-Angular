using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IProcessingTypeRepository : IRepository<ProcessingType>
    {
        ProcessingType GetByName(string name);
        Task<ProcessingType> GetByNameAsync(string name);
    }
}
