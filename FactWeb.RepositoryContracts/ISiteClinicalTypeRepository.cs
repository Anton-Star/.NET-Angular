using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteClinicalTypeRepository : IRepository<SiteClinicalType>
    {
        List<SiteClinicalType> GetAllBySiteId(int? id);

        Task<List<SiteClinicalType>> GetAllBySiteIdAsync(int? id);
    }
}