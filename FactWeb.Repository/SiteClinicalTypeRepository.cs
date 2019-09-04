using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class SiteClinicalTypeRepository : BaseRepository<SiteClinicalType>, ISiteClinicalTypeRepository
    {
        public SiteClinicalTypeRepository(FactWebContext context) : base(context)
        {
        }

        public List<SiteClinicalType> GetAllBySiteId(int? id)
        {
            int? siteId = id.GetValueOrDefault();
            return base.FetchMany(x => x.SiteId == siteId);
        }

        public Task<List<SiteClinicalType>> GetAllBySiteIdAsync(int? id)
        {
            int? siteId = id.GetValueOrDefault();
            return base.FetchManyAsync(x => x.SiteId == siteId);
        }
    }
}
