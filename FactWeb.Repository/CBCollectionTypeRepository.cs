using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class CBCollectionTypeRepository : BaseRepository<CBCollectionType>, ICBCollectionTypeRepository
    {
        public CBCollectionTypeRepository(FactWebContext context) : base(context)
        {
        }
    }
}
