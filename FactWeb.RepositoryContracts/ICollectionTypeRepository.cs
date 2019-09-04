using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ICollectionTypeRepository : IRepository<CollectionType>
    {
        CollectionType GetByName(string name);
        Task<CollectionType> GetByNameAsync(string name);
    }
}
