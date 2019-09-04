using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class ApplicationTypeCategoryRepository : BaseRepository<ApplicationTypeCategory>, IApplicationTypeCategoryRepository
    {
        public ApplicationTypeCategoryRepository(FactWebContext context) : base(context)
        {
        }
    }
}
