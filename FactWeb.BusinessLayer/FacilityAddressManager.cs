using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class FacilityAddressManager : BaseManager<FacilityAddressManager, IFacilityAddressRepository, FacilityAddress>
    {
        public FacilityAddressManager(IFacilityAddressRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Get FacilityAddress by Facility Id 
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        public List<FacilityAddress> GetByFacilityId(int facilityId)
        {
            LogMessage("GetByFacilityId (FacilityAddressManager)");

            return base.Repository.GetByFacility(facilityId);
        }
    }
}
