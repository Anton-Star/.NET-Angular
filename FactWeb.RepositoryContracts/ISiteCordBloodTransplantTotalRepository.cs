using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteCordBloodTransplantTotalRepository : IRepository<SiteCordBloodTransplantTotal>
    {
        List<SiteCordBloodTransplantTotal> GetBySite(int siteId);
        Task<List<SiteCordBloodTransplantTotal>> GetBySiteAsync(int siteId);
    }
}
