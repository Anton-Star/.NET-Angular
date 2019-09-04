using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class OrganizationTypeManager : BaseManager<OrganizationTypeManager, IOrganizationTypeRepository, OrganizationType>
    {
        public OrganizationTypeManager(IOrganizationTypeRepository repository) : base(repository)
        {
        }
    }
}
