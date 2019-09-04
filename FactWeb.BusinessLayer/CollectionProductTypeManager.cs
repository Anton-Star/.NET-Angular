using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class CollectionProductTypeManager : BaseManager<CollectionProductTypeManager, ICollectionProductTypeRepository, CollectionProductType>
    {
        public CollectionProductTypeManager(ICollectionProductTypeRepository repository) : base(repository)
        {
        }
    }
}
