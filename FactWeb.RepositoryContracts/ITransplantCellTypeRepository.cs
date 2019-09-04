using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ITransplantCellTypeRepository : IRepository<TransplantCellType>
    {
        TransplantCellType GetByName(string name);
        Task<TransplantCellType> GetByNameAsync(string name);
    }
}
