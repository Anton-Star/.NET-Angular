using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IDocumentRepository : IRepository<Document>
    {
        /// <summary>
        /// Gets a list of documents for an organization
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of Documents</returns>
        List<Document> GetByOrg(int organizationId);

        /// <summary>
        /// Gets a list of documents for an organization asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of Documents</returns>
        Task<List<Document>> GetByOrgAsync(int organizationId);

        /// <summary>
        /// Gets a list of documents for an organization
        /// </summary>
        /// <param name="orgName">Name of the organization</param>
        /// <returns>Collection of Documents</returns>
        List<Document> GetByOrg(string orgName);

        /// <summary>
        /// Gets a list of documents for an organization asynchronously
        /// </summary>
        /// <param name="orgName">Name of the organization</param>
        /// <returns>Collection of Documents</returns>
        Task<List<Document>> GetByOrgAsync(string orgName);

        List<Document> GetByDocumentLibrary(Guid documentLibraryId);

        List<Document> GetPostInspection(string orgName);

            /// <summary>
        /// Gets a list of BAA documents for an organization asynchronously
        /// </summary>
        /// <param name="orgName">Name of the organization</param>
        /// <returns>Collection of Documents</returns>
        Task<List<Document>> GetBAAByOrgAsync(string orgName);

        /// <summary>
        /// Gets by organization and cycle.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="cycleNumber">The cycle number.</param>
        /// <returns>Collection of Documents</returns>
        List<Document> GetByOrgCycle(int organizationId, int cycleNumber);

        /// <summary>
        /// Gets by organization and cycle asynchronously
        /// </summary>
        /// <param name="organizationId">The organization identifier</param>
        /// <param name="cycleNumber">The cycle number</param>
        /// <returns>Collection of Documents</returns>
        Task<List<Document>> GetByOrgCycleAsync(int organizationId, int cycleNumber);

        OrganizationDocumentLibraryItem GetAccessToken(Guid appUniqueId);

        void Delete(Guid documentId);
    }
}
