using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class CBCategoryRepository : BaseRepository<CBCategory>, ICBCategoryRepository
    {
        public CBCategoryRepository(FactWebContext context) : base(context)
        {
        }

        public CBCategory GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<CBCategory> GetBynameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
