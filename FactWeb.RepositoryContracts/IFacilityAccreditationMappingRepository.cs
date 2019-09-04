using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IFacilityAccreditationMappingRepository : IRepository<FacilityAccreditationMapping>
    {
        List<FacilityAccreditationMapping> GetByFacilityId(int facilityId);

        Task<List<FacilityAccreditationMapping>> GetByFacilityIdAsync(int facilityId);
    }
}