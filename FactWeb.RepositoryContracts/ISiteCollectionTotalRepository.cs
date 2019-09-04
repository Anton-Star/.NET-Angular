using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteCollectionTotalRepository : IRepository<SiteCollectionTotal>
    {
        List<SiteCollectionTotal> GetAllBySite(int siteId);
        Task<List<SiteCollectionTotal>> GetAllBySiteAsync(int siteId);
    }
}
