using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationResponseRepository : IRepository<ApplicationResponse>
    {
        /// <summary>
        /// Gets the application responses for an organization and application type
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of application responses</returns>
        List<ApplicationResponse> GetApplicationResponses(long organizationId, int applicationTypeId);
        /// <summary>
        /// Gets the application responses for an organization and application type asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of application responses</returns>
        Task<List<ApplicationResponse>> GetApplicationResponsesAsync(long organizationId, int applicationTypeId);

        /// <summary>
        /// Gets the responses for an organization and document asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="documentId">Id of the document</param>
        /// <returns>Collection of application responses</returns>
        Task<List<ApplicationResponse>> GetApplicationResponsesWithDocumentsAsync(int organizationId, Guid documentId);
        /// <summary>
        /// Gets the responses for an organization and document
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="documentId">Id of the document</param>
        /// <returns>Collection of application responses</returns>
        List<ApplicationResponse> GetApplicationResponsesWithDocuments(int organizationId, Guid documentId);

        List<ApplicationResponse> GetApplicationResponses(Guid applicationUniqueId);

        List<ApplicationResponse> GetApplicationRfis(int organizationId);

        List<ApplicationResponse> GetApplicationResponses(int organizationId, int applicationResponseStatusId);
        List<ApplicationResponse> GetResponsesByAppIdQuestionId(Guid? id, int applicationId);

        List<ApplicationResponse> GetAllForCompliance(Guid complianceId);

        void AddToHistory(Guid applicationUniqueId, Guid sectionId);

        void BulkUpdate(int applicationId, Guid applicationSectionId, int fromStatusId, int toStatusId, string updatedBy);

        void RemoveForSection(Guid applicationUniqueId, Guid sectionId);

        void ProcessExpectedAndHidden(Guid applicationUniqueId, string updatedBy);
    }
}
