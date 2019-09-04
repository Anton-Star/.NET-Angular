using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ICacheStatusRepository : IRepository<CacheStatus>
    {
        CacheStatus GetByObjectName(string objectName);
        Task<CacheStatus> GetByObjectNameAsync(string objectName);
    }
}
