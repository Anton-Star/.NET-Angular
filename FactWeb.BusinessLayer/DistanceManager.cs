using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class DistanceManager : BaseManager<DistanceManager, IDistanceRepository, Distance>
    {
        public DistanceManager(IDistanceRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Get all users within the radius of 500 miles of facility
        /// </summary>
        /// <param name="facilityAddressId"></param>
        /// <returns></returns>
        public Task<List<Distance>> GetAllWithInRadiusAsync(int facilityAddressId, int radius)
        {
            LogMessage("GetAllWithInRadiusAsync (DistanceManager)");

            return base.Repository.GetAllWithInRadiusAsync(facilityAddressId, radius);
        }

        public List<SiteAddressDistance> GetInspectorsWithoutDistance()
        {
            LogMessage("GetInspectorsWithoutDistance (DistanceManager)");

            return base.Repository.GetInspectorsWithoutDistance();
        }

    }
}


