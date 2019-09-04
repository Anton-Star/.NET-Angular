using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class SiteProcessingMethodologyTotalRepository : BaseRepository<SiteProcessingMethodologyTotal>, ISiteProcessingMethodologyTotalRepository
    {
        public SiteProcessingMethodologyTotalRepository(FactWebContext context) : base(context)
        {
        }

        public List<SiteProcessingMethodologyTotal> GetAllBySite(int siteId)
        {
            return base.FetchMany(x => x.SiteId == siteId);
        }

        public Task<List<SiteProcessingMethodologyTotal>> GetAllBySiteAsync(int siteId)
        {
            return base.FetchManyAsync(x => x.SiteId == siteId);
        }
    }
}
