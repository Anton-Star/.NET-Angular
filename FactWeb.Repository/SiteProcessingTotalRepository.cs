using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class SiteProcessingTotalRepository : BaseRepository<SiteProcessingTotal>, ISiteProcessingTotalRepository
    {
        public SiteProcessingTotalRepository(FactWebContext context) : base(context)
        {
        }

        public List<SiteProcessingTotal> GetAllBySite(int siteId)
        {
            return base.FetchMany(x => x.SiteId == siteId);
        }

        public Task<List<SiteProcessingTotal>> GetAllBySiteAsync(int siteId)
        {
            return base.FetchManyAsync(x => x.SiteId == siteId);
        }
    }
}
