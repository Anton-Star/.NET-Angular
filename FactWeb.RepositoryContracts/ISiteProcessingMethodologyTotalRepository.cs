using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteProcessingMethodologyTotalRepository : IRepository<SiteProcessingMethodologyTotal>
    {
        List<SiteProcessingMethodologyTotal> GetAllBySite(int siteId);
        Task<List<SiteProcessingMethodologyTotal>> GetAllBySiteAsync(int siteId);
    }
}
