using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IFacilitySiteRepository : IRepository<FacilitySite>
    {
        /// <summary>
        /// Gets all Facilities Sites against user
        /// </summary>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        List<FacilitySite> GetFacilitySiteByUserId(string userId);

        /// <summary>
        /// Gets all Facilities sites against user asynchronously
        /// </summary>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        Task<List<FacilitySite>> GetFacilitySiteByUserIdAsync(string userId);
        
        /// <summary>
        /// Add facility site relation to the database
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="facilityId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        bool AddRelation(int siteId, int facilityId, string currentUser);
        
        /// <summary>
        /// Add facility site relation to the database asynchronously
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="facilityId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<bool> AddRelationAsync(int siteId, int facilityId, string currentUser);

        /// <summary>
        /// Add facility site relation to the database asynchronously
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="facilityId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<bool> UpdateRelationAsync(int facilitySiteId, int siteId, int facilityId, string currentUser);

        /// <summary>
        /// Deletes a site and facility relation from the database against Id
        /// </summary>
        /// <param name="facilitySiteId"></param>
        /// <returns></returns>
        bool DeleteRelation(int facilitySiteId);
        
        /// <summary>
        /// Deletes a site and facility relation from the database against Id asynchronously
        /// </summary>
        /// <param name="facilitySiteId"></param>
        /// <returns></returns>
        Task<bool> DeleteRelationAsync(int facilitySiteId);
               
        /// <summary>
        /// Gets all facilitiesSites by site id or facility id asynchronously
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of FacilitySite objects</returns>
        Task<List<FacilitySite>> SearchAsync(int? siteId, int? facilityId);

        /// <summary>
        /// Gets all facilitiesSites by site id or facility id
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of FacilitySite objects</returns>
        List<FacilitySite> Search(int? siteId, int? facilityId);

        /// <summary>
        /// Get by site id
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        List<FacilitySite> GetBySiteId(int siteId);

        /// <summary>
        /// Get by site id asynchronously
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<List<FacilitySite>> GetBySiteIdAsync(int siteId);
    }
}
