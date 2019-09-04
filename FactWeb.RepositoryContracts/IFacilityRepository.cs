using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IFacilityRepository : IRepository<Facility>
    {
        List<Facility> GetAllFacilities();

        /// <summary>
        /// Gets a facility by name
        /// </summary>
        /// <param name="name">Name of the facility</param>
        /// <returns>Facility object</returns>
        Facility GetByName(string name);

        /// <summary>
        /// Gets a facility by name asynchronously
        /// </summary>
        /// <param name="name">Name of the facility</param>
        /// <returns>Facility object</returns>
        Task<Facility> GetByNameAsync(string name);

        /// <summary>
        /// Get all active facilities asynchronously
        /// </summary>
        Task<List<Facility>> GetAllActiveAsync();

        /// <summary>
        /// Get all active facilities 
        /// </summary>
        List<Facility> GetAllActive();

        Task<string> GetCBCollectionSiteTypes(int facilityId);

        List<Facility> GetAllByOrg(int organizationId);
    }
}
