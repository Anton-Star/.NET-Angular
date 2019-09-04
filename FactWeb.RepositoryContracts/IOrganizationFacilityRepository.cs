using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IOrganizationFacilityRepository : IRepository<OrganizationFacility>
    {
        List<OrganizationFacility> GetAllOrganizationFacility();
        /// <summary>
        /// Add organization faclity relation to the database
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="relation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        bool AddRelation(int organizationId, int facilityId, bool relation, string currentUser);
        /// <summary>
        /// Add organization faclity relation to the database asynchronously
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="relation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<bool> AddRelationAsync(int organizationId, int facilityId, bool relation, string currentUser);

        /// <summary>
        /// Add organization faclity relation to the database asynchronously
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="relation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<bool> UpdateRelationAsync(int organizationFacilityId, int organizationId, int facilityId, bool relation, string currentUser);
        
        /// <summary>
        /// Deletes a organization and facility relation from the database against Id
        /// </summary>
        /// <param name="organizationFacilityId"></param>
        /// <returns></returns>
        bool DeleteRelation(int organizationFacilityId);
        /// <summary>
        /// Deletes a organization and facility relation from the database against Id asynchronously
        /// </summary>
        /// <param name="organizationFacilityId"></param>
        /// <returns></returns>
        Task<bool> DeleteRelationAsync(int organizationFacilityId);
        /// <summary>
        /// Check if duplicate ralation exist
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        bool IsDuplicateRelation(int organizationFacilityId, int organizationId, int facilityId);
        /// <summary>
        /// Check if duplicate ralation exist asynchronously
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<bool> IsDuplicateRelationAsync(int organizationFacilityId, int organizationId, int facilityId);
        /// <summary>
        /// Gets all organizationfacilities by organization id or facility id asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of OrganizationFacility objects</returns>
        Task<List<OrganizationFacility>> SearchAsync(int? organizationId, int? facilityId);

        /// <summary>
        /// Gets all organizationfacilities by organization id or facility id
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of OrganizationFacility objects</returns>
        List<OrganizationFacility> Search(int? organizationId, int? facilityId);

        /// <summary>
        /// Get all organization facilities filtered by organization id and facility id
        /// </summary>
        /// <param name="organiziationId"></param>
        /// <param name="facilityID"></param>
        /// <returns></returns>
        List<OrganizationFacility> GetByOrganizationIdFacilityId(int organizationId, int facilityId);

        /// <summary>
        /// Get all organization facilities filtered by organization id and facility id asynchronously
        /// </summary>
        /// <param name="organiziationId"></param>
        /// <param name="facilityID"></param>
        /// <returns></returns>
        Task<List<OrganizationFacility>> GetByOrganizationIdFacilityIdAsync(int organizationId, int facilityId);

        List<OrganizationFacility> GetByOrganization(int organizationId);

        List<int> GetSiteIdsWithStrongRelations(string orgName);

        List<OrganizationFacilityItems> GetOrgFacilities();
    }
}
