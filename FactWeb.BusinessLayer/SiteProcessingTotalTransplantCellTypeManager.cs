using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class SiteProcessingTotalTransplantCellTypeManager : BaseManager<SiteProcessingTotalTransplantCellTypeManager, ISiteProcessingTotalTransplantCellTypeRepository, SiteProcessingTotalTransplantCellType>
    {
        public SiteProcessingTotalTransplantCellTypeManager(ISiteProcessingTotalTransplantCellTypeRepository repository) : base(repository)
        {
        }

        public List<SiteProcessingTotalTransplantCellType> GetAllBySite(int siteId)
        {
            return base.Repository.GetAllBySite(siteId);
        }

        public Task<List<SiteProcessingTotalTransplantCellType>> GetAllBySiteAsync(int siteId)
        {
            return base.Repository.GetAllBySiteAsync(siteId);
        }

        public List<SiteProcessingTotalTransplantCellType> GetByProcessingId(Guid id)
        {
            return base.Repository.GetByProcessingId(id);
        }
    }
}
