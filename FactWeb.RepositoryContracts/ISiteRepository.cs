using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteRepository : IRepository<Site>
    {
        List<Site> GetAllSites();
        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values
        /// </summary>
        /// <param name="siteName">Name of the site</param>
        /// <returns>Collection of site objects</returns>
        Task<List<Site>> SearchAsync(string siteName);

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values asynchronously
        /// </summary>
        /// <param name="siteName">Name of the site</param>
        /// <returns>Collection of site objects</returns>
        List<Site> Search(string siteName);

        /// <summary>
        /// Gets an site by its name
        /// </summary>
        /// <param name="siteName">Name of the Site</param>
        /// <returns>Site entity object</returns>
        Site GetByName(string siteName);

        /// <summary>
        /// Gets an site by its name
        /// </summary>
        /// <param name="siteName">Name of the Site</param>
        /// <returns>Site entity object</returns>
        Task<Site> GetByNameAsync(string siteName);

        /// <summary>
        /// Gets site scope types of an organization by its id
        /// </summary>
        /// <param name="organizationId">Id of the Organization</param>
        List<SiteScopeType> GetScopeTypes(long organizationId);

        List<SiteScopeType> GetScopeTypesBySite(int siteId);

        List<Site> GetSitesByOrganizationId(long organizationId);
        Site GetPrimarySite(long organizationId);

        List<FlatSite> GetFlatSites();

        List<Site> GetSitesByOrganizationInclusive(string orgName);

        List<Site> GetSitesByComplianceNoFacility(Guid complianceApplicationId);
    }
}
