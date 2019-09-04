using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class CBCategoryManager : BaseManager<CBCategoryManager, ICBCategoryRepository, CBCategory>
    {
        public CBCategoryManager(ICBCategoryRepository repository) : base(repository)
        {
        }

        public CBCategory GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<CBCategory> GetByNameAsync(string name)
        {
            return base.Repository.GetBynameAsync(name);
        }
    }
}
