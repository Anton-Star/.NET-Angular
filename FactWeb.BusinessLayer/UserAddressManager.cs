using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class UserAddressManager : BaseManager<UserAddressManager, IUserAddressRepository, UserAddress>
    {
        public UserAddressManager(IUserAddressRepository repository) : base(repository)
        {
        }
    }
}
