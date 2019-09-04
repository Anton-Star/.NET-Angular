using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteProcessingTotalRepository : IRepository<SiteProcessingTotal>
    {
        List<SiteProcessingTotal> GetAllBySite(int siteId);
        Task<List<SiteProcessingTotal>> GetAllBySiteAsync(int siteId);
    }
}
