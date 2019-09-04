using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteProcessingTotalTransplantCellTypeRepository : IRepository<SiteProcessingTotalTransplantCellType>
    {
        List<SiteProcessingTotalTransplantCellType> GetAllBySite(int siteId);
        Task<List<SiteProcessingTotalTransplantCellType>> GetAllBySiteAsync(int siteId);
        List<SiteProcessingTotalTransplantCellType> GetByProcessingId(Guid id);
    }
}
