using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class InspectionCategoryManager : BaseManager<InspectionCategoryManager, IInspectionCategoryRepository, InspectionCategory>
    {
        public InspectionCategoryManager(IInspectionCategoryRepository repository) : base(repository)
        {
        }

       
    }
}
