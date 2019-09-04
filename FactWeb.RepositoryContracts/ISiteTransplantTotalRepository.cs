using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteTransplantTotalRepository : IRepository<SiteTransplantTotal>
    {
        List<SiteTransplantTotal> GetAllBySite(int siteId);
        Task<List<SiteTransplantTotal>> GetAllBySiteAsync(int siteId);
    }
}
