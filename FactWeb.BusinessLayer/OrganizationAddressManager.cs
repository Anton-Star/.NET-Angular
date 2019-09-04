using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class OrganizationAddressManager : BaseManager<OrganizationAddressManager, IOrganizationAddressRepository, OrganizationAddress>
    {
        public OrganizationAddressManager(IOrganizationAddressRepository repository) : base(repository)
        {
        }
    }
}
