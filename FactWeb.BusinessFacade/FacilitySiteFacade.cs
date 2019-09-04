using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using static FactWeb.Infrastructure.Constants;
using FactWeb.Infrastructure;

namespace FactWeb.BusinessFacade
{
    public class FacilitySiteFacade
    {
        private readonly Container container;

        public FacilitySiteFacade(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets all of the records for the entity object
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<FacilitySite> GetAll()
        {
            var facilitySiteManager = this.container.GetInstance<FacilitySiteManager>();

            return facilitySiteManager.GetAll();
        }

        /// <summary>
        /// Gets all of the records for the entity object asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<List<FacilitySite>> GetAllAsync()
        {
            var facilitySiteManager = this.container.GetInstance<FacilitySiteManager>();

            return facilitySiteManager.GetAllAsync();
        }

        /// <summary>
        /// Gets records for the entity object against user id
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<FacilitySite> GetFacilitySiteByUserId(string userId)
        {
            var facilitySiteManager = this.container.GetInstance<FacilitySiteManager>();

            return facilitySiteManager.GetFacilitySiteByUserId(userId);
        }

        /// <summary>
        /// Gets all of the records for the entity object against user id asychronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<List<FacilitySite>> GetFacilitySiteByUserIdAsync(string userId)
        {
            var facilitySiteManager = this.container.GetInstance<FacilitySiteManager>();

            return facilitySiteManager.GetFacilitySiteByUserIdAsync(userId);
        }

        /// <summary>
        /// Add facility site relation
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public bool AddRelation(int siteId, int facilityId, string currentUser)
        {
            var facilitySiteManager = this.container.GetInstance<FacilitySiteManager>();
            return facilitySiteManager.AddRelation(siteId, facilityId, currentUser);
        }

        /// <summary>
        /// Add facility site relation asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<bool> SaveRelationAsync(int facilitySiteId, int siteId, int facilityId, string currentUser)
        {
            var facilitySiteManager = this.container.GetInstance<FacilitySiteManager>();
            return facilitySiteManager.SaveRelationAsync(facilitySiteId, siteId, facilityId, currentUser);
        }

        /// <summary>
        /// Delete a facility site record from database against Id
        /// </summary>
        /// <param name="facilitySiteId"></param>
        /// <returns></returns>
        public bool DeleteRelation(int facilitySiteId)
        {
            var facilitySiteManager = this.container.GetInstance<FacilitySiteManager>();
            return facilitySiteManager.DeleteRelation(facilitySiteId);
        }

        /// <summary>
        /// Delete a facility site record from database against Id asynchronously
        /// </summary>
        /// <param name="facilitySiteId"></param>
        /// <returns></returns>
        public Task<bool> DeleteRelationAsync(int facilitySiteId)
        {
            var facilitySiteManager = this.container.GetInstance<FacilitySiteManager>();
            return facilitySiteManager.DeleteRelationAsync(facilitySiteId);
        }

        /// <summary>
        /// Gets all facilitiesSites by site id or facility id asynchronously
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of FacilitySite objects</returns>
        public Task<List<FacilitySite>> SearchAsync(int? siteId, int? facilityId)
        {
            var facilitySiteManager = this.container.GetInstance<FacilitySiteManager>();

            return facilitySiteManager.SearchAsync(siteId, facilityId);
        }

        /// <summary>
        /// Gets all facilitiesSites by site id or facility id
        /// </summary>
        /// <param name="siteId">Id of the site</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of FacilitySite objects</returns>
        public List<FacilitySite> Search(int? siteId, int? facilityId)
        {
            var facilitySiteManager = this.container.GetInstance<FacilitySiteManager>();

            return facilitySiteManager.Search(siteId, facilityId);
        }
    }
}

