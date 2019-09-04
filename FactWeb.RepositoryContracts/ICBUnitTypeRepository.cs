using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ICBUnitTypeRepository : IRepository<CBUnitType>
    {

        CBUnitType GetByName(string name);
        Task<CBUnitType> GetByNameAsync(string name);
    }
}
