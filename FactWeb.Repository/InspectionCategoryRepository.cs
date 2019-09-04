using FactWeb.Model;
using FactWeb.RepositoryContracts;
namespace FactWeb.Repository
{
    public class InspectionCategoryRepository : BaseRepository<InspectionCategory>, IInspectionCategoryRepository
    {
        public InspectionCategoryRepository(FactWebContext context) : base(context)
        {
        }
    }
}
