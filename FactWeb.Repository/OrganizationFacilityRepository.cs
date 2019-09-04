using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;


namespace FactWeb.Repository
{
    public class OrganizationFacilityRepository : BaseRepository<OrganizationFacility>, IOrganizationFacilityRepository
    {
        public OrganizationFacilityRepository(FactWebContext context) : base(context)
        {
        }

        public List<OrganizationFacility> GetAllOrganizationFacility()
        {
            return
                base.Context.OrganizationFacilities
                    .Include(x => x.Organization)
                    .Include(x => x.Facility)
                    .ToList();
        }

        /// <summary>
        /// Add organization faclity relation to the database
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="relation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public bool AddRelation(int organizationId, int facilityId, bool relation, string currentUser)
        { 

            OrganizationFacility organizationFacility = new OrganizationFacility();
            organizationFacility.OrganizationId = organizationId;
            organizationFacility.FacilityId = facilityId;
            organizationFacility.StrongRelation = relation;
            organizationFacility.CreatedBy = currentUser;
            organizationFacility.CreatedDate = DateTime.Now;
            organizationFacility.UpdatedBy = string.Empty;
            organizationFacility.UpdatedDate = DateTime.Now;

            base.Add(organizationFacility);
            base.SaveChanges();
            
            return true;            
        }

        /// <summary>
        /// Add organization faclity relation to the database asynchronously
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="relation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public Task<bool> AddRelationAsync(int organizationId, int facilityId, bool relation, string currentUser)
        {

            OrganizationFacility organizationFacility = new OrganizationFacility();
            organizationFacility.OrganizationId = organizationId;
            organizationFacility.FacilityId = facilityId;
            organizationFacility.StrongRelation = relation;
            organizationFacility.CreatedBy = currentUser;
            organizationFacility.CreatedDate = DateTime.Now;
            organizationFacility.UpdatedBy = string.Empty;
            organizationFacility.UpdatedDate = DateTime.Now;

            base.Add(organizationFacility);
            base.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<bool> UpdateRelationAsync(int organizationFacilityId, int organizationId, int facilityId,bool relation, string currentUser)
        {
            var organizationFacility = base.GetById(organizationFacilityId);
            organizationFacility.OrganizationId = organizationId;
            organizationFacility.FacilityId = facilityId;
            organizationFacility.StrongRelation = relation;
            organizationFacility.UpdatedBy = currentUser;
            organizationFacility.UpdatedDate = DateTime.Now;

            base.SaveAsync(organizationFacility);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Deletes a organization and facility relation from the database against Id
        /// </summary>
        /// <param name="organizationFacilityId"></param>
        /// <returns></returns>
        public bool DeleteRelation(int organizationFacilityId)
        {

            OrganizationFacility organizationFacility = new OrganizationFacility();
            organizationFacility = base.GetById(organizationFacilityId);
            
            base.Remove(organizationFacility);
            base.SaveChanges();

            return true;
        }

        /// <summary>
        /// Deletes a organization and facility relation from the database against Id asynchronously
        /// </summary>
        /// <param name="organizationFacilityId"></param>
        /// <returns></returns>
        public Task<bool> DeleteRelationAsync(int organizationFacilityId)
        {

            OrganizationFacility organizationFacility = new OrganizationFacility();
            organizationFacility = base.GetById(organizationFacilityId);

            base.Remove(organizationFacility);
            base.SaveChanges();

            return Task.FromResult(true);
        }

        /// <summary>
        /// Check if duplicate ralation exist
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public bool IsDuplicateRelation(int organizationFacilityId, int organizationId, int facilityId)
        {
            var result = base.FetchMany(x => x.OrganizationId == organizationId && x.FacilityId == facilityId);

            if (organizationFacilityId == 0)
            {
                return result.Count > 0 ? true : false;
            }
            else if (result.Count > 0)
            {
                return result[0].Id == organizationFacilityId ? false : true;
            }
            return false;
        }

        /// <summary>
        /// Check if duplicate ralation exist asynchronously
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public Task<bool> IsDuplicateRelationAsync(int organizationFacilityId, int organizationId, int facilityId)
        {
            var result = base.FetchMany(x => x.OrganizationId == organizationId && x.FacilityId == facilityId);

            if (organizationFacilityId == 0)
            {
                return Task.FromResult(result.Count > 0 ? true : false);
            }
            else if (result.Count > 0)
            {
                return Task.FromResult(result[0].Id == organizationFacilityId ? false : true);
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// Get all organization facility asynchronously
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        public Task<List<OrganizationFacility>> SearchAsync(int? organizationId, int? facilityId)
        {
            return base.FetchManyAsync(x => x.OrganizationId == organizationId || x.FacilityId == facilityId);                        
        }

        /// <summary>
        /// Get all organization facility 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        public List<OrganizationFacility> Search(int? organizationId, int? facilityId)
        {
            return base.FetchMany(x => x.OrganizationId == organizationId || x.FacilityId == facilityId);            
        }

        /// <summary>
        /// Get all organization facilities filtered by organization id and facility id
        /// </summary>
        /// <param name="organiziationId"></param>
        /// <param name="facilityID"></param>
        /// <returns></returns>
        public List<OrganizationFacility> GetByOrganizationIdFacilityId(int organizationId, int facilityId)
        {
            return base.FetchMany(x => x.OrganizationId == organizationId && x.FacilityId == facilityId);
        }

        /// <summary>
        /// Get all organization facilities filtered by organization id and facility id asynchronously
        /// </summary>
        /// <param name="organiziationId"></param>
        /// <param name="facilityID"></param>
        /// <returns></returns>
        public Task<List<OrganizationFacility>> GetByOrganizationIdFacilityIdAsync(int organizationId, int facilityId)
        {
            return base.FetchManyAsync(x => x.OrganizationId == organizationId && x.FacilityId == facilityId);
        }

        public List<OrganizationFacility> GetByOrganization(int organizationId)
        {
            return
                base.Context.OrganizationFacilities
                    .Where(x => x.OrganizationId == organizationId)
                    .Include(x => x.Facility)
                    .Include(x => x.Facility.FacilitySites)
                    .Include(x => x.Facility.FacilitySites.Select(y => y.Site))
                    .Include(x => x.Facility.FacilitySites.Select(y => y.Site.InspectionScheduleSites))
                    .Include(x => x.Facility.FacilitySites.Select(y => y.Site.SiteClinicalTypes))
                    .Include(x => x.Facility.FacilityDirector)
                    .Include(x => x.Facility.PrimaryOrganization)
                    .Include(x => x.Facility.PrimaryOrganization.OrganizationType)
                    .Include(x => x.Facility.MasterServiceType)
                    .ToList();
        }

        public List<int> GetSiteIdsWithStrongRelations(string orgName)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[1];
            paramList[0] = orgName;

            var rows = objectContext.ExecuteStoreQuery<int>(
                "EXEC usp_getStrongRelationshipsByOrg @OrgName={0}", paramList).ToList();

            return rows;
        }

        public List<OrganizationFacilityItems> GetOrgFacilities()
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[0];

            var data = objectContext.ExecuteStoreQuery<OrganizationFacilityItems>(
                "EXEC usp_getOrganizationFacilities", paramList).ToList();

            return data;
        }
    }
}
