using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class SiteTransplantTotalRepository : BaseRepository<SiteTransplantTotal>, ISiteTransplantTotalRepository
    {
        public SiteTransplantTotalRepository(FactWebContext context) : base(context)
        {
        }

        public List<SiteTransplantTotal> GetAllBySite(int siteId)
        {
            return base.FetchMany(x => x.SiteId == siteId);
        }

        public Task<List<SiteTransplantTotal>> GetAllBySiteAsync(int siteId)
        {
            return base.FetchManyAsync(x => x.SiteId == siteId);
        }
    }
}
