using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class SiteCollectionTotalRepository : BaseRepository<SiteCollectionTotal>, ISiteCollectionTotalRepository 
    {
        public SiteCollectionTotalRepository(FactWebContext context) : base(context)
        {
        }

        public List<SiteCollectionTotal> GetAllBySite(int siteId)
        {
            return base.FetchMany(x => x.SiteId == siteId);
        }

        public Task<List<SiteCollectionTotal>> GetAllBySiteAsync(int siteId)
        {
            return base.FetchManyAsync(x => x.SiteId == siteId);
        }
    }
}
