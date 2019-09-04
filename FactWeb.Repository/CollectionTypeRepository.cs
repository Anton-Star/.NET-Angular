using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class CollectionTypeRepository : BaseRepository<CollectionType>, ICollectionTypeRepository
    {
        public CollectionTypeRepository(FactWebContext context) : base(context)
        {
        }

        public CollectionType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<CollectionType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
