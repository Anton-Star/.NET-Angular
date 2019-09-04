using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IDistanceRepository : IRepository<Distance>
    {
        Task<List<Distance>> GetAllWithInRadiusAsync(int facilityAddressId, int radius);
        List<SiteAddressDistance> GetInspectorsWithoutDistance();
    }

}
