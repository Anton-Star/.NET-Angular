using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class CollectionTypeManager : BaseManager<CollectionTypeManager, ICollectionTypeRepository, CollectionType>
    {
        public CollectionTypeManager(ICollectionTypeRepository repository) : base(repository)
        {
        }

        public CollectionType GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<CollectionType> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}
