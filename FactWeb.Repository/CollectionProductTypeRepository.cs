using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class CollectionProductTypeRepository : BaseRepository<CollectionProductType>, ICollectionProductTypeRepository
    {
        public CollectionProductTypeRepository(FactWebContext context) : base(context)
        {
        }
    }
}
