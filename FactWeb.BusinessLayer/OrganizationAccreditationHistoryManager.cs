using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class OrganizationAccreditationHistoryManager : BaseManager<OrganizationAccreditationHistoryManager, IOrganizationAccreditationHistoryRepository, OrganizationAccreditationHistory>
    {
        public OrganizationAccreditationHistoryManager(IOrganizationAccreditationHistoryRepository repository) : base(repository)
        {
        }
    }
}
