using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationRepository : IRepository<Application>
    {
        Application GetByUniqueIdAnyStatus(Guid id);
        List<Application> GetAllApplications();

            /// <summary>
        /// Gets all applications for a user
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <returns></returns>
        List<Application> GetAllForUser(Guid userId);
        /// <summary>
        /// Gets all applications for a user
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <returns></returns>
        Task<List<Model.Application>> GetAllForUserAsync(Guid userId);

        /// <summary>
        /// Gets all applications for a consultant
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <returns></returns>
        List<Model.Application> GetAllForConsultant(Guid userId);

        /// <summary>
        /// Gets all applications for a consultant asynchronously
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <returns></returns>
        Task<List<Model.Application>> GetAllForConsultantAsync(Guid userId);

        /// <summary>
        /// Gets all applications by organization id
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of applications</returns>
        List<Application> GetAllByOrganization(int organizationId);
        /// <summary>
        /// Gets all applications by organization id asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of applications</returns>
        Task<List<Application>> GetAllByOrganizationAsync(int organizationId);
        /// <summary>
        /// Gets all applications by application type id
        /// </summary>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of applications</returns>
        List<Application> GetAllByApplicationType(int applicationTypeId);
        /// <summary>
        /// Gets all applications by application type id asynchronously
        /// </summary>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of applications</returns>
        Task<List<Application>> GetAllByApplicationTypeAsync(int applicationTypeId);
        /// <summary>
        /// Gets an application by organization and application type
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Application entity object</returns>
        Application GetByOrganizationAndType(int organizationId, int applicationTypeId);

        Application GetByOrganizationAndTypeIgnoreActive(int organizationId, int applicationTypeId);
        /// <summary>
        /// Gets an application by organization and application type asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Application entity object</returns>
        Task<Application> GetByOrganizationAndTypeAsync(int organizationId, int applicationTypeId);

        Application GetByUniqueIdIgnoreActive(Guid id);

        /// <summary>
        /// Gets an application by its unique id
        /// </summary>
        /// <param name="id">Unique Id of the record</param>
        /// <param name="includeResponses">Pre-fill responses</param>
        /// /// <param name="includeNotApplicables">Pre-fill notapplicables</param>
        /// <returns>Application entity object</returns>
        Application GetByUniqueId(Guid id, bool includeResponses = true, bool includeNotApplicables = true, bool includeTraineeResponses = false);

        /// <summary>
        /// Gets an application by compliance application id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Application> GetByComplianceApplicationId(Guid id);
        /// <summary>
        /// Gets an application by its unique id asynchronously
        /// </summary>
        /// <param name="id">Unique Id of the record</param>
        /// <returns>Application entity object</returns>
        Task<Application> GetByUniqueIdAsync(Guid id);
        /// <summary>
        /// Gets all applications by compliance id asynchronously
        /// </summary>
        /// <param name="complianceId">compliance id</param>
        /// <returns>Application entity object</returns>
        Task<List<Application>> GetApplicationsByComplianceId(string complianceId);

        /// <summary>
        /// Gets all applications assigned to the coordinator
        /// </summary>
        /// <param name="coordinatorId">Id of the Coordinator</param>
        /// <returns>Collection of Applications</returns>
        List<CoordinatorApplication> GetCoordinatorApplications(Guid? coordinatorId);

        /// <summary>
        /// Gets all applications that arent cancelled
        /// </summary>
        /// <returns>Collection of Applications</returns>
        List<Application> GetNotCancelled();
        /// <summary>
        /// Gets all applications that arent cancelled asynchronously
        /// </summary>
        /// <returns>Collection of Applications</returns>
        Task<List<Application>> GetNotCancelledAsync();

        List<Application> GetAllComplianceApplications(int organizationId);

        /// <summary>
        /// Gets all the compliance applications for an organization
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of Applications</returns>
        List<Application> GetComplianceApplications(int organizationId);
        /// <summary>
        /// Gets all the compliance applications for an organization asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of Applications</returns>
        Task<List<Application>> GetComplianceApplicationsAsync(int organizationId);

        /// <summary>
        /// Gets all the applications the inspector is assigned to
        /// </summary>
        /// <param name="inspectorUserId">Id of the User</param>
        /// <returns>Collection of applications</returns>
        List<Application> GetInspectorApplications(Guid inspectorUserId);
        /// <summary>
        /// Gets all the applications the inspector is assigned to asynchronously
        /// </summary>
        /// <param name="inspectorUserId">Id of the User</param>
        /// <returns>Collection of applications</returns>
        Task<List<Application>> GetInspectorApplicationsAsync(Guid inspectorUserId);

        /// <summary>
        /// Get application status by application unique id
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>        
        ApplicationStatus GetApplicationStatusByUniqueId(Guid appId);

        Application GetApplicationById(int id);

        void CreateApprovals(int applicationId, string createdBy);

        List<AppReportModel> GetApplicationReport(Guid complianceApplicationId, string siteName);
        List<AppReportModel> GetAppReport(Guid appUniqueId);
        List<BlankReport> GetBlankReport(int applicationTypeId);
    }
}
