using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationVersionRepository : IRepository<ApplicationVersion>
    {
        /// <summary>
        /// Gets all the application version by their type
        /// </summary>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of ApplicationVersion objects</returns>
        List<ApplicationVersion> GetByType(int applicationTypeId);
        /// <summary>
        /// Gets all the application version by their type asynchronously
        /// </summary>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of ApplicationVersion objects</returns>
        Task<List<ApplicationVersion>> GetByTypeAsync(int applicationTypeId);

        /// <summary>
        /// Gets all the application version by their type
        /// </summary>
        /// <param name="applicationTypeName">Name of the application type</param>
        /// <returns>Collection of ApplicationVersion objects</returns>
        List<ApplicationVersion> GetByType(string applicationTypeName);
        /// <summary>
        /// Gets all the application version by their type asynchronously
        /// </summary>
        /// <param name="applicationTypeName">Name of the application type</param>
        /// <returns>Collection of ApplicationVersion objects</returns>
        Task<List<ApplicationVersion>> GetByTypeAsync(string applicationTypeName);

        /// <summary>
        /// Gets all the active application version
        /// </summary>
        /// <returns>Collection of ApplicationVersion objects</returns>
        List<ApplicationVersion> GetActive();

        ApplicationVersion GetVersion(Guid versionId);

        /// <summary>
        /// Gets all the active application version asynchronously
        /// </summary>
        /// <returns>Collection of ApplicationVersion objects</returns>
        Task<List<ApplicationVersion>> GetActiveAsync();

        /// <summary>
        /// Gets a flat application by its version id
        /// </summary>
        /// <param name="versionId">Id of the version</param>
        /// <returns>Collection of FlatApplication entity objects</returns>
        List<FlatApplication> GetFlatApplication(Guid versionId);
        /// <summary>
        /// Gets a flat application by its version id asynchronously
        /// </summary>
        /// <param name="versionId">Id of the version</param>
        /// <returns>Collection of FlatApplication entity objects</returns>
        Task<List<FlatApplication>> GetFlatApplicationAsync(Guid versionId);

        /// <summary>
        /// Gets the application version by a specific section
        /// </summary>
        /// <param name="applicationSectionId">Id of the application section</param>
        /// <returns>ApplicationVersion entity object</returns>
        ApplicationVersion GetByApplicationSection(Guid applicationSectionId);

        /// <summary>
        /// Gets the application version by a specific section asynchronously
        /// </summary>
        /// <param name="applicationSectionId">Id of the application section</param>
        /// <returns>ApplicationVersion entity object</returns>
        Task<ApplicationVersion> GetByApplicationSectionAsync(Guid applicationSectionId);

        List<ExportModel> Export(Guid applicationVersionId);
    }
}
