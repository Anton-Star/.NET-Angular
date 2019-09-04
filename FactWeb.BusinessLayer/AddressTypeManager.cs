using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class AddressTypeManager : BaseManager<AddressTypeManager, IAddressTypeRepository, AddressType>
    {
        public AddressTypeManager(IAddressTypeRepository repository) : base(repository)
        {
        }

        public AddressType GetByName(string name)
        {
            LogMessage("GetByName (AddressTypeManager)");

            return base.Repository.GetByName(name);
        }
    }
}
