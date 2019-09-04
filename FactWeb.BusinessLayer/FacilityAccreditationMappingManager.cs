using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{

    public class FacilityAccreditationMappingManager : BaseManager<FacilityAccreditationMappingManager, IFacilityAccreditationMappingRepository, FacilityAccreditationMapping>
    {
        public FacilityAccreditationMappingManager(IFacilityAccreditationMappingRepository repository) : base(repository)
        {

        }

        public List<FacilityAccreditationMapping> GetByFacilityId(int facilityId)
        {
            LogMessage("GetByFacilityId (FacilityAccreditationMappingManager)");

            return base.Repository.GetByFacilityId(facilityId);
        }


        public async Task<List<FacilityAccreditationMapping>> GetByFacilityIdAsync(int facilityId)
        {
            LogMessage("GetByFacilityId (FacilityAccreditationMappingManager)");

            return await base.Repository.GetByFacilityIdAsync(facilityId);
        }
    }
}


