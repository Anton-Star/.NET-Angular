using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteProcessingTypeRepository : IRepository<SiteProcessingType>
    {
        List<SiteProcessingType> GetAllBySiteId(int? id);

        Task<List<SiteProcessingType>> GetAllBySiteIdAsync(int? id);
    }
}