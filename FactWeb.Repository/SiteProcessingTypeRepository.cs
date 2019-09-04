using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class SiteProcessingTypeRepository : BaseRepository<SiteProcessingType>, ISiteProcessingTypeRepository
    {
        public SiteProcessingTypeRepository(FactWebContext context) : base(context)
        {
        }

        public List<SiteProcessingType> GetAllBySiteId(int? id)
        {
            int? siteId = id.GetValueOrDefault();
            return base.FetchMany(x => x.SiteId == siteId);
        }

        public Task<List<SiteProcessingType>> GetAllBySiteIdAsync(int? id)
        {
            int? siteId = id.GetValueOrDefault();
            return base.FetchManyAsync(x => x.SiteId == siteId);
        }
    }
}
