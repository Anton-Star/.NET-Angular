using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class ApplicationTypeCategoryManager : BaseManager<ApplicationTypeCategoryManager, IApplicationTypeCategoryRepository, ApplicationTypeCategory>
    {
        public ApplicationTypeCategoryManager(IApplicationTypeCategoryRepository repository) : base(repository)
        {
        }
    }
}
