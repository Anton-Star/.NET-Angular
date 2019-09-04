using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{

    public class OrganizationFacilityManager : BaseManager<OrganizationFacilityManager, IOrganizationFacilityRepository, OrganizationFacility>
    {
        public OrganizationFacilityManager(IOrganizationFacilityRepository repository) : base(repository)
        {
        }

        public List<OrganizationFacility> GetAllOrganizationFacility()
        {
            LogMessage("GetAllOrganizationFacility (OrganizationFacilityManager)");

            return base.Repository.GetAllOrganizationFacility();
        }

        /// <summary>
        /// Adds a organiztion and facility relation in to the database
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="relation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public bool AddRelation(int organizationId, int facilityId, bool relation, string currentUser)
        {
            LogMessage("AddRelation (OrganizationFacilityManager)");

            return base.Repository.AddRelation(organizationId, facilityId, relation, currentUser);
        }

        public List<OrganizationFacility> GetByOrganization(int organizationId)
        {
            return base.Repository.GetByOrganization(organizationId);
        }

        /// <summary>
        /// Adds a organiztion and facility relation in to the database asnychronously
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="relation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public Task<bool> SaveRelationAsync(int organizationFacilityId, int organizationId, int facilityId, bool relation, string currentUser)
        {
            LogMessage("SaveRelationAsync (OrganizationFacilityManager)");

            if (organizationFacilityId == 0)
            {
                return base.Repository.AddRelationAsync(organizationId, facilityId, relation, currentUser);
            }
            else
            {
                return base.Repository.UpdateRelationAsync(organizationFacilityId, organizationId, facilityId, relation, currentUser);
            }

        }

        /// <summary>
        /// Deletes organization facility relation from database against Id
        /// </summary>
        /// <param name="organizationFacilityId"></param>
        /// <returns></returns>
        public bool DeleteRelation(int organizationFacilityId)
        {
            LogMessage("DeleteRelation (OrganizationFacilityManager)");

            return base.Repository.DeleteRelation(organizationFacilityId);
        }

        /// <summary>
        /// Deletes organization facility relation from database against Id asynchronously
        /// </summary>
        /// <param name="organizationFacilityId"></param>
        /// <returns></returns>
        public Task<bool> DeleteRelationAsync(int organizationFacilityId)
        {
            LogMessage("DeleteRelationAsync (OrganizationFacilityManager)");

            return base.Repository.DeleteRelationAsync(organizationFacilityId);
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
            LogMessage("IsDuplicateRelation (OrganizationFacilityManager)");

            return base.Repository.IsDuplicateRelation(organizationFacilityId, organizationId, facilityId);
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
            LogMessage("IsDuplicateRelationAsync (OrganizationFacilityManager)");

            return base.Repository.IsDuplicateRelationAsync(organizationFacilityId, organizationId, facilityId);
        }

        /// <summary>
        /// Gets all organizationfacilities by organization id or facility id asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of OrganizationFacility objects</returns>
        public Task<List<OrganizationFacility>> SearchAsync(int? organizationId, int? facilityId)
        {
            LogMessage("SearchAsync (OrganizationFacilityManager)");

            return base.Repository.SearchAsync(organizationId, facilityId);
        }

        /// <summary>
        /// Gets all organizationfacilities by organization id or facility id
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of OrganizationFacility objects</returns>
        public List<OrganizationFacility> Search(int? organizationId, int? facilityId)
        {
            LogMessage("Search (OrganizationFacilityManager)");

            return base.Repository.Search(organizationId, facilityId);
        }


        /// <summary>
        /// Get all organization facilities filtered by organization id and facility id
        /// </summary>
        /// <param name="organiziationId"></param>
        /// <param name="facilityID"></param>
        /// <returns></returns>
        public List<OrganizationFacility> GetByOrganizationIdFacilityId(int organizationId, int facilityId)
        {
            LogMessage("GetByOrganizationIdFacilityId (OrganizationFacilityManager)");

            return base.Repository.GetByOrganizationIdFacilityId(organizationId, facilityId);
        }

        /// <summary>
        /// Get all organization facilities filtered by organization id and facility id async
        /// </summary>
        /// <param name="organiziationId"></param>
        /// <param name="facilityID"></param>
        /// <returns></returns>
        public Task<List<OrganizationFacility>> GetByOrganizationIdFacilityIdAsync(int organizationId, int facilityId)
        {
            LogMessage("GetByOrganizationIdFacilityIdAsync (OrganizationFacilityManager)");

            return base.Repository.GetByOrganizationIdFacilityIdAsync(organizationId, facilityId);
        }

        public List<int> GetSiteIdsWithStrongRelations(string orgName)
        {
            return base.Repository.GetSiteIdsWithStrongRelations(orgName);
        }

        public List<OrganizationFacilityItems> GetOrgFacilities()
        {
            return base.Repository.GetOrgFacilities();
        }
    }
}


