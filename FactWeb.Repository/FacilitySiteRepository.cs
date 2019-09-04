using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class FacilitySiteRepository : BaseRepository<FacilitySite>, IFacilitySiteRepository
    {
        public FacilitySiteRepository(FactWebContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all Facilities Sites against user
        /// </summary>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        public List<FacilitySite> GetFacilitySiteByUserId(string userId)
        {
            return base.FetchMany(x => x.CreatedBy == userId);
        }

        /// <summary>
        /// Gets all Facilities Sites against user asynchronously
        /// </summary>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        public Task<List<FacilitySite>> GetFacilitySiteByUserIdAsync(string userId)
        {
            return base.FetchManyAsync(x => x.CreatedBy == userId);
        }

        /// <summary>
        /// Add facility site relation to the database
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="facilityId"></param>
        /// <param name="relation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public bool AddRelation(int siteId, int facilityId, string currentUser)
        {
            FacilitySite facilitySite = new FacilitySite();
            facilitySite.SiteId = siteId;
            facilitySite.FacilityId = facilityId;
            facilitySite.CreatedBy = currentUser;
            facilitySite.CreatedDate = DateTime.Now;
            facilitySite.UpdatedBy = string.Empty;
            facilitySite.UpdatedDate = DateTime.Now;

            base.Add(facilitySite);
            base.SaveChanges();

            return true;
        }

        /// <summary>
        /// Add facility site relation to the database asynchronously
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="facilityId"></param>
        /// <param name="relation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public Task<bool> AddRelationAsync(int siteId, int facilityId, string currentUser)
        {
            FacilitySite facilitySite = new FacilitySite();
            facilitySite.SiteId = siteId;
            facilitySite.FacilityId = facilityId;
            facilitySite.CreatedBy = currentUser;
            facilitySite.CreatedDate = DateTime.Now;
            facilitySite.UpdatedBy = string.Empty;
            facilitySite.UpdatedDate = DateTime.Now;

            base.Add(facilitySite);
            base.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<bool> UpdateRelationAsync(int facilitySiteId, int siteId, int facilityId, string currentUser)
        {
            var facilitySite = base.GetById(facilitySiteId);
            facilitySite.SiteId = siteId;
            facilitySite.FacilityId = facilityId;
            facilitySite.UpdatedBy = currentUser;
            facilitySite.UpdatedDate = DateTime.Now;

            base.Save(facilitySite);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Deletes a site and facility relation from the database against Id
        /// </summary>
        /// <param name="facilitySiteId"></param>
        /// <returns></returns>
        public bool DeleteRelation(int facilitySiteId)
        {

            FacilitySite facilitySite = new FacilitySite();
            facilitySite = base.GetById(facilitySiteId);

            base.Remove(facilitySite);
            base.SaveChanges();

            return true;
        }

        /// <summary>
        /// Deletes a site and facility relation from the database against Id asynchronously
        /// </summary>
        /// <param name="facilitySiteId"></param>
        /// <returns></returns>
        public Task<bool> DeleteRelationAsync(int facilitySiteId)
        {

            FacilitySite facilitySite = new FacilitySite();
            facilitySite = base.GetById(facilitySiteId);

            base.Remove(facilitySite);
            base.SaveChanges();

            return Task.FromResult(true);
        }

        /// <summary>
        /// Get all facility site asynchronously
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        public Task<List<FacilitySite>> SearchAsync(int? siteId, int? facilityId)
        {
            return base.FetchManyAsync(x => x.SiteId == siteId || x.FacilityId == facilityId);
        }

        /// <summary>
        /// Get all facility site 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        public List<FacilitySite> Search(int? siteId, int? facilityId)
        {
            return base.FetchMany(x => x.SiteId == siteId || x.FacilityId == facilityId);
        }

        /// <summary>
        /// Get by site id
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<FacilitySite> GetBySiteId(int siteId)
        {
            return base.FetchMany(x => x.SiteId == siteId);
        }

        /// <summary>
        /// Get by site id asynchronously
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public Task<List<FacilitySite>> GetBySiteIdAsync(int siteId)
        {
            return base.FetchManyAsync(x => x.SiteId == siteId);
        }
    }
}
