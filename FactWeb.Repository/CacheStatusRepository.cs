using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class CacheStatusRepository : BaseRepository<CacheStatus>, ICacheStatusRepository
    {
        public CacheStatusRepository(FactWebContext context) : base(context)
        {
        }

        public CacheStatus GetByObjectName(string objectName)
        {
            return base.Fetch(x => x.ObjectName == objectName);
        }

        public Task<CacheStatus> GetByObjectNameAsync(string objectName)
        {
            return base.FetchAsync(x => x.ObjectName == objectName);
        }
    }
}
