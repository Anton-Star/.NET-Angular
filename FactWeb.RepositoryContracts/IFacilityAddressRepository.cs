using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IFacilityAddressRepository : IRepository<FacilityAddress>
    {
        /// <summary>
        /// Gets all Facility Addresses by facility
        /// </summary>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of Facility Addresses</returns>
        List<FacilityAddress> GetByFacility(int facilityId);

        /// <summary>
        /// Gets all Facility Addresses by facility asynchronously
        /// </summary>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of Facility Addresses</returns>
        Task<List<FacilityAddress>> GetByFacilityAsync(int facilityId);

        /// <summary>
        /// Gets all Facility Addresses by address
        /// </summary>
        /// <param name="addressId">Id of the address</param>
        /// <returns>Collection of Facility Addresses</returns>
        List<FacilityAddress> GetByAddress(int addressId);

        /// <summary>
        /// Gets all Facility Addresses by facility
        /// </summary>
        /// <param name="addressId">Id of the address</param>
        /// <returns>Collection of Facility Addresses</returns>
        Task<List<FacilityAddress>> GetByAddressAsync(int addressId);
    }
}
