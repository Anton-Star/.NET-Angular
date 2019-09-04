using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class AddressTypeRepository : BaseRepository<AddressType>, IAddressTypeRepository
    {
        public AddressTypeRepository(FactWebContext context) : base(context)
        {
        }

        public AddressType GetByName(string name)
        {
            return base.Fetch(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
