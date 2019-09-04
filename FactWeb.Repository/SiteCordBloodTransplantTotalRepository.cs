using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class SiteCordBloodTransplantTotalRepository : BaseRepository<SiteCordBloodTransplantTotal>, ISiteCordBloodTransplantTotalRepository
    {
        public SiteCordBloodTransplantTotalRepository(FactWebContext context) : base(context)
        {
        }

        public List<SiteCordBloodTransplantTotal> GetBySite(int siteId)
        {
            return base.FetchMany(x => x.SiteId == siteId);
        }

        public Task<List<SiteCordBloodTransplantTotal>> GetBySiteAsync(int siteId)
        {
            return base.FetchManyAsync(x => x.SiteId == siteId);
        }
    }
}
