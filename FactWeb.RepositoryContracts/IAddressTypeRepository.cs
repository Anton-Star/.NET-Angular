using FactWeb.Model;

namespace FactWeb.RepositoryContracts
{
    public interface IAddressTypeRepository : IRepository<AddressType>
    {
        AddressType GetByName(string name);
    }
}
