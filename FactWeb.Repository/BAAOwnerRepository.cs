using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class BAAOwnerRepository : BaseRepository<BAAOwner>, IBAAOwnerRepository
    {
        public BAAOwnerRepository(FactWebContext context) : base(context)
        {
        }
    }
}
