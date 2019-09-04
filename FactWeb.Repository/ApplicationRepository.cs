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
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(FactWebContext context) : base(context)
        {
        }

        public List<Application> GetAllApplications()
        {
            return base.Context.Applications
                .Include(x => x.AccreditationOutcomes.Select(y => y.OutcomeStatus))
                .Include(x => x.Coordinator)
                .Include(x => x.ApplicationResponses)
                .Include(x => x.ApplicationResponsesTrainee)
                .Include(x => x.ApplicationStatus)
                .Include(x => x.ApplicationType)
                .Include(x => x.ApplicationVersion)
                .Include(x => x.ComplianceApplication)
                .Include(x => x.InspectionSchedules.Select(y => y.InspectionScheduleDetails))
                .Include(x => x.Inspections)
                .Include(x => x.Organization)
                .Include(
                    x => x.Organization.OrganizationFacilities.Select(y => y.Facility.FacilitySites.Select(z => z.Site.InspectionScheduleSites)))
                .ToList();
        }

        public List<Application> GetAllForUser(Guid userId)
        {
            return base.Context.Applications
                .Include(x => x.AccreditationOutcomes.Select(y => y.OutcomeStatus))
                .Include(x => x.Coordinator)
                .Include(x => x.ApplicationResponses)
                .Include(x => x.ApplicationResponsesTrainee)
                .Include(x => x.ApplicationStatus)
                .Include(x => x.ApplicationType)
                .Include(x => x.ApplicationVersion)
                .Include(x => x.ComplianceApplication)
                .Include(x => x.InspectionSchedules.Select(y => y.InspectionScheduleDetails))
                .Include(x => x.Inspections)
                .Include(x => x.Organization)
                .Include(
                    x => x.Organization.OrganizationFacilities.Select(y => y.Facility.FacilitySites.Select(z => z.Site.InspectionScheduleSites)))
                .Where(x => x.Organization.Users.Any(y => y.UserId == userId) && x.IsActive == true && x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled)
                .ToList();
        }

        public Task<List<Application>> GetAllForUserAsync(Guid userId)
        {            
            return base.FetchManyAsync(x => x.Organization.Users.Any(y => y.UserId == userId) && x.IsActive == true);
        }

        public List<Application> GetAllForConsultant(Guid userId)
        {
            return base.Context.Applications
                .Include(x => x.AccreditationOutcomes.Select(y => y.OutcomeStatus))
                .Include(x => x.Coordinator)
                .Include(x => x.ApplicationResponses)
                .Include(x => x.ApplicationResponsesTrainee)
                .Include(x => x.ApplicationStatus)
                .Include(x => x.ApplicationType)
                .Include(x => x.ApplicationVersion)
                .Include(x => x.ComplianceApplication)
                .Include(x => x.InspectionSchedules.Select(y => y.InspectionScheduleDetails))
                .Include(x => x.Inspections)
                .Include(x => x.Organization)
                .Include(
                    x => x.Organization.OrganizationFacilities.Select(y => y.Facility.FacilitySites.Select(z => z.Site.InspectionScheduleSites)))
                .Where(x => x.Organization.OrganizationConsutants.Any(y => y.ConsultantId == userId) && x.IsActive == true)
                .ToList();
        }

        public Task<List<Application>> GetAllForConsultantAsync(Guid userId)
        {
            return  base.FetchManyAsync(x => x.Organization.OrganizationConsutants.Any(y => y.ConsultantId == userId) && x.IsActive == true);
        }

        public Task<List<Application>> GetApplicationsByComplianceId(string complianceId)
        {
            return base.FetchManyAsync(x => x.ComplianceApplicationId == new Guid(complianceId) && x.IsActive == true);
        }

        public ApplicationStatus GetApplicationStatusByUniqueId(Guid appId)
        {
            return base.FetchAsync(x => x.UniqueId == appId).Result.ApplicationStatus;
        }
        public Application GetApplicationById(int appId)
        {
            return base.FetchAsync(x => x.Id == appId).Result;
        }
        public List<Application> GetAllByOrganization(int organizationId)
        {
            return base.FetchMany(x => x.OrganizationId == organizationId && x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive == true);
        }

        public Task<List<Application>> GetAllByOrganizationAsync(int organizationId)
        {
            return base.FetchManyAsync(x => x.OrganizationId == organizationId && x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive == true);
        }

        public List<Application> GetAllByApplicationType(int applicationTypeId)
        {
            return base.FetchMany(x => x.ApplicationTypeId == applicationTypeId && x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive == true);
        }

        public Task<List<Application>> GetAllByApplicationTypeAsync(int applicationTypeId)
        {
            return base.FetchManyAsync(x => x.ApplicationTypeId == applicationTypeId && x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive == true);
        }

        public Application GetByOrganizationAndType(int organizationId, int applicationTypeId)
        {
            return base.Fetch(x => x.OrganizationId == organizationId && x.ApplicationTypeId == applicationTypeId && x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive == true);
        }

        public Application GetByOrganizationAndTypeIgnoreActive(int organizationId, int applicationTypeId)
        {
            return base.Fetch(x => x.OrganizationId == organizationId && x.ApplicationTypeId == applicationTypeId && x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled);
        }

        public Task<Application> GetByOrganizationAndTypeAsync(int organizationId, int applicationTypeId)
        {
            return base.FetchAsync(x => x.OrganizationId == organizationId && x.ApplicationTypeId == applicationTypeId && x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive == true);
        }

        public Application GetByUniqueIdIgnoreActive(Guid id)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            var application = base.Context.Applications
                .Include(x => x.Coordinator)
                .Include(x => x.Coordinator.UserCredentials.Select(y => y.Credential))
                .Include(x => x.ApplicationStatus)
                .Include(x => x.ApplicationType)
                .Include(x => x.ComplianceApplication)
                .Include(x => x.ComplianceApplication.Coordinator)
                .Include(x => x.ComplianceApplication.Coordinator.UserCredentials.Select(y => y.Credential))
                .Include(x => x.ComplianceApplication.ReportReviewStatus)
                .Include(x => x.ComplianceApplication.ComplianceApplicationApprovalStatus)
                .Include(x => x.Organization)
                .Include(x => x.Organization.Users)
                .Include(x => x.InspectionSchedules)
                .Include(x => x.Organization.OrganizationAccreditationCycles)
                .Include(x => x.Organization.OrganizationFacilities)
                .Include(x => x.Organization.OrganizationFacilities.Select(y => y.Facility.FacilitySites.Select(z => z.Site.SiteAddresses.Select(zz => zz.Address))))
                .Include(x => x.ApplicationVersion)
                .Include(x => x.AccreditationOutcomes)
                .Include(x => x.AccreditationOutcomes.Select(y => y.OutcomeStatus))
                .Include(x => x.Site)
                .SingleOrDefault(
                    x =>
                        x.UniqueId == id && x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled);

            if (application != null)
            {
                application.ApplicationQuestionNotApplicables =
                    base.Context.ApplicationQuestionNotApplicables.Where(x => x.ApplicationId == application.Id)
                        .ToList();

                var responses =
                    base.Context.ApplicationResponses
                    .Include(x => x.ApplicationResponseStatus)
                    .Include(x => x.ApplicationSectionQuestion)
                    .Include(x => x.Document)
                    .Include(x => x.Document.AssociationTypes)
                    .Where(x => x.ApplicationId == application.Id)
                    .ToList();

                application.ApplicationResponses = responses;
            }

            return application;
        }

        public Application GetByUniqueId(Guid id, bool includeResponses = true, bool includeNotApplicables = true, bool includeTraineeResponses = false)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            var application = base.Context.Applications
                .Include(x => x.Coordinator)
                .Include(x=>x.Coordinator.UserCredentials.Select(y=>y.Credential))
                .Include(x => x.ApplicationStatus)
                .Include(x => x.ApplicationType)
                .Include(x => x.ComplianceApplication)
                .Include(x => x.ComplianceApplication.Coordinator)
                .Include(x => x.ComplianceApplication.Coordinator.UserCredentials.Select(y => y.Credential))
                .Include(x => x.ComplianceApplication.ReportReviewStatus)
                .Include(x=>x.ComplianceApplication.ComplianceApplicationApprovalStatus)
                .Include(x=>x.Organization)
                .Include(x => x.Organization.Users)
                .Include(x=>x.InspectionSchedules)
                .Include(x=>x.Organization.OrganizationAccreditationCycles)
                .Include(x=>x.Organization.OrganizationFacilities)
                .Include(x => x.Organization.OrganizationFacilities.Select(y=>y.Facility.FacilitySites.Select(z=>z.Site.SiteAddresses.Select(zz=>zz.Address))))
                .Include(x=>x.ApplicationVersion)
                .Include(x=>x.AccreditationOutcomes)
                .Include(x => x.AccreditationOutcomes.Select(y=>y.OutcomeStatus))
                .Include(x=>x.Site)
                .SingleOrDefault(
                    x =>
                        x.UniqueId == id && x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled &&
                        x.IsActive == true);

            if (application != null && includeNotApplicables)
            {
                application.ApplicationQuestionNotApplicables =
                    base.Context.ApplicationQuestionNotApplicables.Where(x => x.ApplicationId == application.Id)
                        .ToList();
            }

            if (application != null && includeResponses)
            {
                var responses =
                    base.Context.ApplicationResponses
                    .Include(x=>x.ApplicationResponseStatus)
                    .Include(x=>x.ApplicationSectionQuestion)
                    .Include(x=>x.Document)
                    .Include(x=>x.Document.AssociationTypes)
                    .Where(x => x.ApplicationId == application.Id)
                    .ToList();

                application.ApplicationResponses = responses;
            }

            if (application != null && includeTraineeResponses)
            {
                var responses = base.Context.ApplicationResponseTrainee
                    .Include(x => x.ApplicationResponseStatus)
                    .Include(x => x.ApplicationSectionQuestion)
                    .Where(x => x.ApplicationId == application.Id)
                    .ToList();

                application.ApplicationResponsesTrainee = responses;
            }

            return application;
        }

        public List<Application> GetByComplianceApplicationId(Guid id)
        {
            return base.FetchMany(x => x.ComplianceApplicationId == id && x.IsActive == true);
        }

        public Task<Application> GetByUniqueIdAsync(Guid id)
        {
            return base.FetchAsync(x => x.UniqueId == id && x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive == true);
        }

        public Application GetByUniqueIdAnyStatus(Guid id)
        {
            return base.Fetch(x => x.UniqueId == id);
        }

        public List<CoordinatorApplication> GetCoordinatorApplications(Guid? coordinatorId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = coordinatorId;

            var data = objectContext.ExecuteStoreQuery<CoordinatorApplication>(
                "EXEC usp_GetCoordinatorApplications @CoordinatorId={0}", paramList).ToList();

            return data;
        }

        public List<Application> GetNotCancelled()
        {
            return base.FetchMany(x => x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive == true);
        }

        public Task<List<Application>> GetNotCancelledAsync()
        {
            return base.FetchManyAsync(x => x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive == true);
        }

        public List<Application> GetAllComplianceApplications(int organizationId)
        {
            return
                base.Context.Applications
                    .Include(x => x.AccreditationOutcomes.Select(y => y.OutcomeStatus))
                    .Include(x => x.Coordinator)
                    .Include(x => x.ApplicationResponses)
                    .Include(x => x.ApplicationResponsesTrainee)
                    .Include(x => x.ApplicationStatus)
                    .Include(x => x.ApplicationType)
                    .Include(x => x.ApplicationVersion)
                    .Include(x => x.ComplianceApplication)
                    .Include(x => x.InspectionSchedules.Select(y => y.InspectionScheduleDetails))
                    .Include(x => x.Inspections)
                    .Include(x => x.Organization)
                    .Include(x => x.Site)
                    .Where(
                        x =>
                            x.OrganizationId == organizationId &&
                            x.ApplicationType.Name != Constants.ApplicationTypes.Eligibility &&
                            x.ApplicationType.Name != Constants.ApplicationTypes.Annual &&
                            x.ApplicationType.Name != Constants.ApplicationTypes.Renewal &&
                            x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled)
                    .ToList();
        }

        public List<Application> GetComplianceApplications(int organizationId)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            var notApplicables =
                base.Context.ApplicationQuestionNotApplicables
                    .Where(x => x.Application.OrganizationId == organizationId &&
                                x.Application.ComplianceApplicationId != null &&
                                x.Application.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled &&
                                (x.Application.IsActive == true || x.Application.IsActive == null))
                                .ToList();

            var responses = base.Context.ApplicationResponses
                .Include(x=>x.ApplicationSectionQuestion)
                .Include(x=>x.ApplicationResponseStatus)
                .Where(x => x.Application.OrganizationId == organizationId &&
                                x.Application.ComplianceApplicationId != null &&
                                x.Application.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled &&
                                (x.Application.IsActive == true || x.Application.IsActive == null))
                                .ToList();


            var apps =
                base.Context.Applications
                    .Include(x => x.AccreditationOutcomes.Select(y => y.OutcomeStatus))
                    .Include(x => x.Coordinator)
                    .Include(x => x.ApplicationStatus)
                    .Include(x => x.ApplicationType)
                    .Include(x => x.ApplicationVersion)
                    .Include(x => x.ComplianceApplication)
                    .Include(x => x.ComplianceApplication.Coordinator)
                   // .Include(x => x.InspectionSchedules.Select(y => y.InspectionScheduleDetails))
                    //.Include(x => x.Inspections)
                    .Include(x => x.Organization)
                    //.Include(x=>x.Organization.OrganizationAccreditationCycles)
                    .Include(x => x.Site)
                    //.Include(x=>x.Site.InspectionScheduleSites)
                    //.Include(x=>x.Site.SiteTransplantTypes)
                    //.Include(x=>x.Site.FacilitySites)
                    //.Include(x=>x.Site.FacilitySites.Select(y=>y.Facility.ServiceType))
                    //.Include(x=>x.Site.SiteAddresses.Select(y=>y.Address))
                    //.Include(x=>x.Site.InspectionScheduleSites)
                    .Where(
                        x =>
                            x.OrganizationId == organizationId &&
                            x.ComplianceApplicationId != null &&
                            x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && (x.IsActive == true || x.IsActive == null))
                    .ToList();

            foreach (var app in apps)
            {
                app.ApplicationQuestionNotApplicables = notApplicables.Where(x => x.ApplicationId == app.Id).ToList();
                app.ApplicationResponses = responses.Where(x => x.ApplicationId == app.Id).ToList();
            }


            return apps;
        }

        public Task<List<Application>> GetComplianceApplicationsAsync(int organizationId)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.OrganizationId == organizationId &&
                        x.ApplicationType.Name != Constants.ApplicationTypes.Eligibility &&
                        x.ApplicationType.Name != Constants.ApplicationTypes.Annual &&
                        x.ApplicationType.Name != Constants.ApplicationTypes.Renewal &&
                        x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive == true);
        }

        public List<Application> GetInspectorApplications(Guid inspectorUserId)
        {
            return
                base.FetchMany(
                    x =>
                        x.InspectionSchedules.Any(y => y.InspectionScheduleDetails.Any(z => z.UserId == inspectorUserId)) && x.IsActive == true);
        }

        public Task<List<Application>> GetInspectorApplicationsAsync(Guid inspectorUserId)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.InspectionSchedules.Any(y => y.InspectionScheduleDetails.Any(z => z.UserId == inspectorUserId)) && x.IsActive == true);
        }

        public void CreateApprovals(int applicationId, string createdBy)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[2];
            paramList[0] = applicationId;
            paramList[1] = createdBy;

            objectContext.ExecuteStoreCommand(
                "EXEC usp_createApplicationApprovals @ApplicationId={0}, @CreatedBy={1}", paramList);
        }

        public List<AppReportModel> GetApplicationReport(Guid complianceApplicationId, string siteName)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[2];
            paramList[0] = complianceApplicationId;
            paramList[1] = siteName;

            return objectContext.ExecuteStoreQuery<AppReportModel>(
                "EXEC usp_getComplianceApplicationSections @complianceApplicationId={0}, @siteName={1}", paramList).ToList();
        }

        public List<AppReportModel> GetAppReport(Guid appUniqueId)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[1];
            paramList[0] = appUniqueId;

            return objectContext.ExecuteStoreQuery<AppReportModel>(
                "EXEC usp_getApplicationSectionsReporting @AppUniqueId={0}", paramList).ToList();
        }

        public List<BlankReport> GetBlankReport(int applicationTypeId)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[1];
            paramList[0] = applicationTypeId;

            return objectContext.ExecuteStoreQuery<BlankReport>(
                "EXEC usp_getBlankReport @TypeId={0}", paramList).ToList();
        }
    }
}
