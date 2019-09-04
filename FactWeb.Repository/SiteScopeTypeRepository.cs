using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class SiteScopeTypeRepository : BaseRepository<SiteScopeType>, ISiteScopeTypeRepository
    {
        public SiteScopeTypeRepository(FactWebContext context) : base(context)
        {
        }

        public List<SiteScopeType> GetAllBySiteId(int? id)
        {
            int? siteId = id.GetValueOrDefault();
            return base.FetchMany(x => x.SiteId == siteId);
        }

        public Task<List<SiteScopeType>> GetAllBySiteIdAsync(int? id)
        {
            int? siteId = id.GetValueOrDefault();
            return base.FetchManyAsync(x => x.SiteId == siteId);
        }
    }
}