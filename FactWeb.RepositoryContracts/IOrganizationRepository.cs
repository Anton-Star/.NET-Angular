using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        List<Organization> GetAllOrganizations(bool includeFacilities);
        List<Organization> GetAllOrganizationWithApplications();
            /// <summary>
            /// Gets all the records that match a contains search on any of the provided values
            /// </summary>
            /// <param name="organizationName">Name of the organization</param>
            /// <param name="city">City the organization is located in</param>
            /// <param name="state">State the organization is located in</param>
            /// <returns>Collection of organization objects</returns>
        Task<List<Organization>> SearchAsync(string organizationName, string city, string state);

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values asynchronously
        /// </summary>
        /// <param name="organizationName">Name of the organization</param>
        /// <param name="city">City the organization is located in</param>
        /// <param name="state">State the organization is located in</param>
        /// <returns>Collection of organization objects</returns>
        List<Organization> Search(string organizationName, string city, string state);

        /// <summary>
        /// Gets all organizations with the given facility
        /// </summary>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of Organization objects</returns>
        List<Organization> GetByFacility(int facilityId);

        /// <summary>
        /// Gets all organizations with the given facility asynchronously
        /// </summary>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of Organization objects</returns>
        Task<List<Organization>> GetByFacilityAsync(int facilityId);

        Task<List<Organization>> GetByFacilityIdAndRelationAsync(int facilityId, bool strongRelation);

        /// <summary>
        /// Gets an organization by its name
        /// </summary>
        /// <param name="organizationName">Name of the Organization</param>
        /// <returns>Organization entity object</returns>
        Organization GetByName(string organizationName);

        /// <summary>
        /// Gets an organization by its name
        /// </summary>
        /// <param name="organizationName">Name of the Organization</param>
        /// <returns>Organization entity object</returns>
        Task<Organization> GetByNameAsync(string organizationName);

        /// <summary>
        /// Gets accredited services of an organization by its id
        /// </summary>
        /// <param name="organizationId">Id of the Organization</param>
        string GetAccreditedServices(int organizationId);

        /// <summary>
        /// Gets all organizations that have submitted eligibility applications
        /// </summary>
        /// <returns>Collection of Organizations</returns>
        List<Organization> GetAllWithSubmittedEligibility();
        /// <summary>
        /// Gets all organizations that have submitted eligibility applications asynchronously
        /// </summary>
        /// <returns>Collection of Organizations</returns>
        Task<List<Organization>> GetAllWithSubmittedEligibilityAsync();

        /// <summary>
        /// Returns whether the user has access to the org data or not
        /// </summary>
        /// <param name="orgName">Name of the organization</param>
        /// <param name="userId">Id of the user</param>
        /// <returns>Boolean if the user has access or not</returns>
        bool DoesHaveAccess(string orgName, Guid userId);

        List<Organization> GetOrganizationsWithoutDocumentLibrary();

        Task<List<Organization>> GetOrganizationsWithoutDocumentLibraryAsync();

        bool HasQmRestriction(int organizationId);

        void CreateDocumentLibrary(int organizationId, int cycleNumber, string vaultId, string groupId, string createdBy);

        List<SimpleOrganization> GetSimpleOrganizations();
        Organization GetByCompAppId(Guid compAppId);
    }
}
