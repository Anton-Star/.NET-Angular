using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteTransplantTypeRepository : IRepository<SiteTransplantType>
    {
        List<SiteTransplantType> GetAllBySiteId(int? id);

        Task<List<SiteTransplantType>> GetAllBySiteIdAsync(int? id);
    }
}