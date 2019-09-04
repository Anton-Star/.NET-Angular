using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class SiteTransplantTypeRepository : BaseRepository<SiteTransplantType>, ISiteTransplantTypeRepository
    {
        public SiteTransplantTypeRepository(FactWebContext context) : base(context)
        {
        }

        public List<SiteTransplantType> GetAllBySiteId(int? id)
        {
            int? siteId = id.GetValueOrDefault();
            return base.FetchMany(x => x.SiteId == siteId);
        }

        public Task<List<SiteTransplantType>> GetAllBySiteIdAsync(int? id)
        {
            int? siteId = id.GetValueOrDefault();
            return base.FetchManyAsync(x => x.SiteId == siteId);
        }
    }
}
