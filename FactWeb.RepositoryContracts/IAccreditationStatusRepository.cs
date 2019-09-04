using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IAccreditationStatusRepository : IRepository<AccreditationStatus>
    {
        AccreditationStatus GetByName(string name);
        Task<AccreditationStatus> GetByNameAsync(string name);
    }
}
