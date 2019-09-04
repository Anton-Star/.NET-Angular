using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class SiteProcessingTotalTransplantCellTypeRepository : BaseRepository<SiteProcessingTotalTransplantCellType>, ISiteProcessingTotalTransplantCellTypeRepository
    {
        public SiteProcessingTotalTransplantCellTypeRepository(FactWebContext context) : base(context)
        {
        }

        public List<SiteProcessingTotalTransplantCellType> GetAllBySite(int siteId)
        {
            return base.FetchMany(x => x.SiteProcessingTotal.SiteId == siteId);
        }

        public Task<List<SiteProcessingTotalTransplantCellType>> GetAllBySiteAsync(int siteId)
        {
            return base.FetchManyAsync(x => x.SiteProcessingTotal.SiteId == siteId);
        }

        public List<SiteProcessingTotalTransplantCellType> GetByProcessingId(Guid id)
        {
            return base.FetchMany(x => x.SiteProcessingTotalId == id);
        }
    }
}
