using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class FacilitySiteManager : BaseManager<FacilitySiteManager, IFacilitySiteRepository, FacilitySite>
    {
        public FacilitySiteManager(IFacilitySiteRepository repository) : base(repository)
        {

        }

        /// <summary>
        /// Gets all FacilitiesSites against current user 
        /// </summary>
        /// <param name="userId">Id of the current user</param>
        /// <returns>FacilitySite entity object</returns>
        public List<FacilitySite> GetFacilitySiteByUserId(string userId)
        {
            LogMessage("GetFacilitySiteByUserId (FacilitySiteManager)");

            return base.Repository.GetFacilitySiteByUserId(userId);
        }

        /// <summary>
        /// Gets all FacilitiesSites against current user asynchronously
        /// </summary>
        /// <param name="userId">Id of the current user</param>
        /// <returns>Asynchronous Task with a FacilitySite entity object</returns>
        public Task<List<FacilitySite>> GetFacilitySiteByUserIdAsync(string userId)
        {
            LogMessage("GetFacilitySiteByUserIdAsync (FacilitySiteManager)");

            return base.Repository.GetFacilitySiteByUserIdAsync(userId);
        }

        /// <summary>
        /// Adds a site and facility relation in to the database
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="facilityId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public bool AddRelation(int siteId, int facilityId, string currentUser)
        {
            LogMessage("AddRelation (FacilitySiteManager)");

            return base.Repository.AddRelation(siteId, facilityId, currentUser);
        }

        /// <summary>
        /// Adds a site and facility relation in to the database asnychronously
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="facilityId"></param>
        /// <param name="relation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public Task<bool> SaveRelationAsync(int facilitySiteId, int siteId, int facilityId, string currentUser)
        {
            LogMessage("SaveRelationAsync (FacilitySiteManager)");

            if (facilitySiteId == 0)
            {
                return base.Repository.AddRelationAsync(siteId, facilityId, currentUser);
            }
            else
            {
                return base.Repository.UpdateRelationAsync(facilitySiteId, siteId, facilityId, currentUser);
            }

        }

        /// <summary>
        /// Deletes facility site relation from database against Id
        /// </summary>
        /// <param name="facilitySiteId"></param>
        /// <returns></returns>
        public bool DeleteRelation(int facilitySiteId)
        {
            LogMessage("DeleteRelation (FacilitySiteManager)");

            return base.Repository.DeleteRelation(facilitySiteId);
        }

        /// <summary>
        /// Deletes facility site relation from database against Id asynchronously
        /// </summary>
        /// <param name="facilitySiteId"></param>
        /// <returns></returns>
        public Task<bool> DeleteRelationAsync(int facilitySiteId)
        {
            LogMessage("DeleteRelationAsync (FacilitySiteManager)");

            return base.Repository.DeleteRelationAsync(facilitySiteId);
        }

        /// <summary>
        /// Gets all facilities sites by site id or facility id asynchronously
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of FacilitySite objects</returns>
        public Task<List<FacilitySite>> SearchAsync(int? siteId, int? facilityId)
        {
            LogMessage("SearchAsync (FacilitySiteManager)");

            return base.Repository.SearchAsync(siteId, facilityId);
        }

        /// <summary>
        /// Gets all facilitiessites by site id or facility id
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of FacilitySite objects</returns>
        public List<FacilitySite> Search(int? siteId, int? facilityId)
        {
            LogMessage("Search (FacilitySiteManager)");

            return base.Repository.Search(siteId, facilityId);
        }


        /// <summary>
        /// Get all filtered by site id
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<FacilitySite> GetBySiteId(int siteId)
        {
            LogMessage("GetBySiteId (FacilitySiteManager)");

            return base.Repository.GetBySiteId(siteId);
        }

        /// <summary>
        /// Get all filtered by site id async
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public Task<List<FacilitySite>> GetBySiteIdAsync(int siteId)
        {
            LogMessage("GetBySiteIdAsync (FacilitySiteManager)");

            return base.Repository.GetBySiteIdAsync(siteId);
        }
    }
}