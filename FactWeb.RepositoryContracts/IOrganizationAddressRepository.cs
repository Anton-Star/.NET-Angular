using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IOrganizationAddressRepository : IRepository<OrganizationAddress>
    {
        /// <summary>
        /// Gets all Organization Addresses by organization
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of Organization Addresses</returns>
        List<OrganizationAddress> GetByOrganization(int organizationId);

        /// <summary>
        /// Gets all Organization Addresses by organization asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of Organization Addresses</returns>
        Task<List<OrganizationAddress>> GetByOrganizationAsync(int organizationId);

        /// <summary>
        /// Gets all Organization Addresses by address
        /// </summary>
        /// <param name="addressId">Id of the address</param>
        /// <returns>Collection of Organization Addresses</returns>
        List<OrganizationAddress> GetByAddress(int addressId);

        /// <summary>
        /// Gets all Organization Addresses by address
        /// </summary>
        /// <param name="addressId">Id of the address</param>
        /// <returns>Collection of Organization Addresses</returns>
        Task<List<OrganizationAddress>> GetByAddressAsync(int addressId);
    }
}
