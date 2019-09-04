using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationSectionRepository : IRepository<ApplicationSection>
    {
        /// <summary>
        /// Gets an Application Section by id
        /// </summary>
        /// <param name="id">Id of the application section</param>
        /// <returns>ApplicationSection object</returns>
        ApplicationSection GetById(Guid id);

        ApplicationSection GetById(Guid id, int appId);

        /// <summary>
        /// Gets all the sections by the parent
        /// </summary>
        /// <param name="parentId">Id of the parent</param>
        /// <returns>Collection of ApplicationSections</returns>
        List<ApplicationSection> GetByParent(Guid parentId);
        /// <summary>
        /// Gets all the sections by the parent asynchronously
        /// </summary>
        /// <param name="parentId">Id of the parent</param>
        /// <returns>Collection of ApplicationSections</returns>
        Task<List<ApplicationSection>> GetByParentAsync(Guid parentId);

        /// <summary>
        /// Gets all the sections that are root level
        /// </summary>
        /// <returns>Collection of ApplicationSections</returns>
        List<ApplicationSection> GetRootLevel();
        /// <summary>
        /// Gets all the sections that are root level asynchronously
        /// </summary>
        /// <returns>Collection of ApplicationSections</returns>
        Task<List<ApplicationSection>> GetRootLevelAsync();

        /// <summary>
        /// Gets all sections by application type
        /// </summary>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of ApplicationSections</returns>
        List<ApplicationSection> GetAllForApplicationType(int applicationTypeId);
        /// <summary>
        /// Gets all sections by application type asynchronously
        /// </summary>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of ApplicationSections</returns>
        Task<List<ApplicationSection>> GetAllForApplicationTypeAsync(int applicationTypeId);

        /// <summary>
        /// Gets all sections by application type
        /// </summary>
        /// <param name="applicationTypeName">Name of the application type</param>
        /// <returns>Collection of ApplicationSections</returns>
        List<ApplicationSection> GetAllForApplicationType(string applicationTypeName);
        /// <summary>
        /// Gets all sections by application type
        /// </summary>
        /// <param name="applicationTypeName">Name of the application type</param>
        /// <returns>Collection of ApplicationSections</returns>
        Task<List<ApplicationSection>> GetAllForApplicationTypeAsync(string applicationTypeName);

        IEnumerable<ApplicationSection> GetForApplicationType(string applicationTypeName);

        /// <summary>
        /// Gets all sections by application type asynchronously
        /// </summary>
        /// <param name="applicationTypeName">Name of the application type</param>
        /// <param name="version">Version of the application type</param>
        /// <returns>Collection of ApplicationSections</returns>
        Task<List<ApplicationSection>> GetAllForApplicationTypeAsync(string applicationTypeName, string version);

        /// <summary>
        /// Gets all sections by application type
        /// </summary>
        /// <param name="applicationTypeName">Name of the application type</param>
        /// <param name="version">Version of the application type</param>
        /// <returns>Collection of ApplicationSections</returns>
        List<ApplicationSection> GetAllForApplicationType(string applicationTypeName, string version);

        List<ApplicationSection> GetAllWithDocument(Guid appId);
        List<ApplicationSection> GetAllWithDocumentForComp(Guid compId);
        Task<List<ApplicationSection>> GetAllWithDocumentAsync(int organizationId);

        List<ApplicationSection> GetRootItems(int applicationTypeId);
        Task<List<ApplicationSection>> GetRootItemsAsync(int applicationTypeId);

        /// <summary>
        /// Gets all sections by application type
        /// </summary>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of ApplicationSections</returns>
        List<ApplicationSection> GetFlatForApplicationType(int applicationTypeId, Guid? applicationVersionId);

        List<ApplicationSection> GetAllActiveForApplicationType(int applicationTypeId);

        List<ApplicationSection> GetAllForVersion(Guid versionId);

        Task<List<ApplicationSection>> GetAllActiveForApplicationTypeAsync(int applicationTypeId);

        List<ApplicationSection> GetAllForActiveVersionsNoLazyLoad();

        List<ApplicationSection> GetAllForVersionNoLazyLoad(Guid versionId);

        List<ApplicationSection> GetAllForApplicationTypeNoLazyLoad(string applicationTypeName);

        List<ApplicationSectionResponse> GetApplicationSectionsForApplication(Guid? complianceApplicationId, Guid? applicationUniqueId, Guid currentUserId);

        List<ApplicationSection> GetSectionsWithRFIs(int applicationId);
    }
}
