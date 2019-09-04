using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class AddressManager : BaseManager<AddressManager, IAddressRepository, Address>
    {
        public AddressManager(IAddressRepository repository) : base(repository)
        {
        }
    }
}
