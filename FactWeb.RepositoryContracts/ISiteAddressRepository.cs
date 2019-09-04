using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteAddressRepository : IRepository<SiteAddress>
    {
        /// <summary>
        /// Gets all Site Addresses by site
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <returns>Collection of Site Addresses</returns>
        List<SiteAddress> GetBySite(int siteId);

        /// <summary>
        /// Gets all Site Addresses by site asynchronously
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <returns>Collection of Site Addresses</returns>
        Task<List<SiteAddress>> GetBySiteAsync(int siteId);

        /// <summary>
        /// Gets all Site Addresses by address
        /// </summary>
        /// <param name="addressId">Id of the address</param>
        /// <returns>Collection of Site Addresses</returns>
        List<SiteAddress> GetByAddress(int addressId);

        /// <summary>
        /// Gets all Site Addresses by site
        /// </summary>
        /// <param name="addressId">Id of the address</param>
        /// <returns>Collection of Site Addresses</returns>
        Task<List<SiteAddress>> GetByAddressAsync(int addressId);
    }
}
