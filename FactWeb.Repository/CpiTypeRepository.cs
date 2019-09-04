using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class CpiTypeRepository : BaseRepository<CpiType>, ICpiTypeRepository
    {
        public CpiTypeRepository(FactWebContext context) : base(context)
        {
        }
    }
}
