using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using log4net;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ApplicationManager = FactWeb.BusinessLayer.ApplicationManager;

namespace FactWeb.BusinessFacade
{
    public class ApplicationFacade
    {
        private readonly Container container;
        private readonly ILog log = LogManager.GetLogger(typeof(ApplicationFacade));

        public static Dictionary<int, ApplicationVersion> ActiveVersions = new Dictionary<int, ApplicationVersion>();

        private List<Application> GetApplicationsWithSingleCompliance(List<Application> applications)
        {
            var result = new List<Application>();

            foreach (var application in applications)
            {
                if (application.ComplianceApplication != null)
                {
                    if (result.All(x => x.ComplianceApplicationId != application.ComplianceApplicationId))
                    {
                        application.ApplicationType.Name = "Compliance Application";
                        result.Add(application);
                    }
                }
                // Commented because of bug 631. Show only comliance application
                else
                {
                    result.Add(application);
                }
            }

            return result;
        }

        private List<CoordinatorApplication> GetApplicationsWithSingleCompliance(List<CoordinatorApplication> applications)
        {
            var result = new List<CoordinatorApplication>();

            //Fix for bug 1021: sort the applications by most recent outcomestatus created date
            applications = applications.OrderByDescending(apps => apps.AccreditationOutcomeDate).ToList();

            foreach (var application in applications)
            {
                if (application.ComplianceApplicationId != null)
                {
                    if (result.All(x => x.ComplianceApplicationId != application.ComplianceApplicationId))
                    {
                        application.ApplicationTypeName = "Compliance Application";
                        result.Add(application);
                    }
                }
                // Commented because of bug 631. Show only comliance application
                else
                {
                    result.Add(application);
                }
            }

            return result;
        }

        public SubmittedComplianceModel GetSubmittedComplainceApplication(Guid applicationUniqueId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();

            var application = applicationManager.GetByUniqueId(applicationUniqueId, false, false);

            if (application == null)
            {
                return null;
            }

            var complianceApplications = complianceApplicationManager.GetByOrg(application.OrganizationId);

            var complianceApplication =
                complianceApplications.FirstOrDefault(x => x.ComplianceApplicationApprovalStatus.Name == Constants.ComplianceApplicationApprovalStatuses.Submitted);

            return new SubmittedComplianceModel
            {
                SubmittedComplianceId = complianceApplication?.Id,
                OrgDirectorId = application.Organization.Users.FirstOrDefault(x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector)?.UserId
            };
        }

        public async Task<List<Application>> GetApplicationsByComplianceId(string complianceId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = await applicationManager.GetApplicationsByComplianceId(complianceId);

            return applications;
        }

        private List<Application> GetApplicationsWithSingleComplianceForInspectionScheduler(List<Application> applications)
        {
            var result = new List<Application>();

            foreach (var application in applications)
            {
                if (application.ComplianceApplication != null)
                {
                    if (result.All(x => x.ComplianceApplicationId != application.ComplianceApplicationId))
                    {
                        application.ApplicationType.Name = "Compliance Application";
                        result.Add(application);
                    }
                }
            }

            return result;
        }
        public ApplicationFacade(Container container)
        {
            this.container = container;
        }

        public async Task<List<Application>> GetAllAsync()
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = await applicationManager.GetAllAsync();

            return this.GetApplicationsWithSingleCompliance(applications);
        }

        public ApplicationStatus GetApplicationStatusByUniqueId(Guid appUniqueId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applicationStatus = applicationManager.GetApplicationStatusByUniqueId(appUniqueId);

            return applicationStatus;
        }

        public Application getApplicationById(int id)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var application = applicationManager.getApplicationById(id);

            return application;
        }



        public List<ApplicationItem> GetAll()
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = applicationManager.GetAllApplications();

            var result = ModelConversions.Convert(this.GetApplicationsWithSingleCompliance(applications), false);

            foreach (var application in result.Where(x => x.ApplicationTypeName == Constants.ApplicationTypes.Annual ||
                    x.ApplicationTypeName == Constants.ApplicationTypes.Eligibility ||
                    x.ApplicationTypeName == Constants.ApplicationTypes.Renewal))
            {
                application.ComplianceApproval = this.GetSubmittedComplainceApplication(application.UniqueId);
            }

            return result;
        }

        public async Task<List<Application>> GetAllByOrgAsync(long organizationId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = await applicationManager.GetAllByOrganizationOrApplicationTypeAsync((int)organizationId, null);

            return this.GetApplicationsWithSingleCompliance(applications);
        }

        public List<ApplicationItem> GetAllForUser(Guid userId, bool isFact)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = applicationManager.GetAllForUser(userId);

            var apps = this.GetApplicationsWithSingleCompliance(applications);

            var result = ModelConversions.Convert(apps, false);

            foreach (var application in result.Where(x => x.ApplicationTypeName == Constants.ApplicationTypes.Annual ||
                    x.ApplicationTypeName == Constants.ApplicationTypes.Eligibility ||
                    x.ApplicationTypeName == Constants.ApplicationTypes.Renewal))
            {
                if (isFact)
                {
                    application.ComplianceApproval = this.GetSubmittedComplainceApplication(application.UniqueId);
                } else
                {
                    var app = applications.Single(x => x.Id == application.ApplicationId);

                    if (app.Organization.Users.Any(x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector && x.UserId == userId))
                    {
                        application.ComplianceApproval = this.GetSubmittedComplainceApplication(application.UniqueId);
                    }
                }

            }


            return result;
        }

        public async Task<List<Application>> GetAllForUserAsync(Guid userId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = await applicationManager.GetAllForUserAsync(userId);

            return this.GetApplicationsWithSingleCompliance(applications);
        }

        public List<ApplicationItem> GetAllForConsultant(Guid userId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = applicationManager.GetAllForConsultant(userId);

            var result = ModelConversions.Convert(this.GetApplicationsWithSingleCompliance(applications), false);

            foreach (var application in result.Where(x => x.ApplicationTypeName == Constants.ApplicationTypes.Annual ||
                    x.ApplicationTypeName == Constants.ApplicationTypes.Eligibility ||
                    x.ApplicationTypeName == Constants.ApplicationTypes.Renewal))
            {
                application.ComplianceApproval = this.GetSubmittedComplainceApplication(application.UniqueId);
            }

            return result;
        }

        public async Task<List<Application>> GetAllForConsultantAsync(Guid userId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = await applicationManager.GetAllForConsultantAsync(userId);

            return this.GetApplicationsWithSingleCompliance(applications);
        }

        public void SendToFact(Guid app, string url, string factPortalEmailAddress, string leadName, string orgName, string appTypeName, string coordinatorEmail)
        {
            List<String> ccs = new List<string>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var application = applicationManager.GetByUniqueId(app);

            if (application == null)
            {
                throw new Exception("Cannot find application");
            }

            var inspection = application.InspectionSchedules.FirstOrDefault(x => x.IsActive);

            List<InspectionScheduleDetail> inspectors;

            if (application.ComplianceApplicationId.HasValue)
            {
                inspectors =
                    this.GetAllInspectionScheduleDetailsForComplianceApplication(
                        application.ComplianceApplicationId.Value);
            }
            else
            {
                inspectors = this.GetAllInspectionScheduleDetailsForApplication(app);
            }
            

            var reminderHtml = url + "app/email.templates/inspectorRfi.html";
            var appUrl = string.Format("{0}{1}", url, "#/Application?app=" + app);

            var html = WebHelper.GetHtml(reminderHtml);
            html = html.Replace("{OrgName}", orgName);
            html = html.Replace("{Inspector}", leadName);
            html = html.Replace("{InspectionDate}", inspection?.InspectionDate.ToShortDateString());
            html = html.Replace("{ApplicationType}", appTypeName);
            html = html.Replace("{URL}", appUrl);

            var tos = new List<string>
            {
                coordinatorEmail,
                factPortalEmailAddress
            };

            tos.AddRange(inspectors.Where(x => x.User.EmailAddress != coordinatorEmail && x.IsLead)
                        .Select(x => x.User.EmailAddress)
                        .ToList());

            EmailHelper.Send(tos, ccs, "FACT Inspector RFIs Pending -" + application.Organization.Name, html);

        }

        /// <summary>
        /// Returns all application types
        /// </summary>
        /// <returns></returns>
        public List<ApplicationType> GetAllApplicationTypes()
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();

            return applicationTypeManager.GetAll();
        }

        /// <summary>
        /// Get all response statuses from database
        /// </summary>
        /// <returns></returns>
        public List<ApplicationResponseStatus> GetAllApplicationResponseStatus()
        {
            var applicationResponseStatusManager = this.container.GetInstance<ApplicationResponseStatusManager>();

            return applicationResponseStatusManager.GetAll();
        }

        public void DeleteVersion(Guid versionId, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationVersionManager>();

            var version = manager.GetById(versionId);

            if (version == null)
            {
                throw new Exception("Cannot find version");
            }

            if (version.IsActive)
            {
                throw new Exception("Active version cannot be deleted");
            }

            version.IsDeleted = true;
            version.UpdatedDate = DateTime.Now;
            version.UpdatedBy = updatedBy;

            manager.Save(version);
        }

        public ApplicationStatusView GetApplicationStatusView(Guid applicationUniqueIdentifier, string roleName)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var siteManager = this.container.GetInstance<SiteManager>();
            var applicationStatus = "";

            var application = applicationManager.GetByUniqueId(applicationUniqueIdentifier);

            if (application == null)
            {
                throw new Exception("Cannot find application");
            }

            var scopeTypeList = siteManager.GetScopeTypes(application.OrganizationId);

            var items = applicationSectionManager.GetRootItemStatuses(application, roleName, scopeTypeList);

            if (application.ApplicationStatus != null)
            {
                applicationStatus = (roleName == Constants.Roles.User) ? application.ApplicationStatus.NameForApplicant : application.ApplicationStatus.Name;
            }

            var result = new ApplicationStatusView
            {
                ApplicationType = application.ApplicationType.Name,
                ApplicationSectionRootItems = items,
                SubmittedDate = application.SubmittedDate,
                DueDate = application.DueDate,
                OrganizationName = application.Organization.Name,
                ApplicationStatus = applicationStatus
            };

            if (application.InspectionSchedules != null)
            {
                result.InspectionDate = application.InspectionSchedules.LastOrDefault()?.InspectionDate;
            }

            if (application.ComplianceApplication != null)
            {
                result.Coordinator = ModelConversions.Convert(application.ComplianceApplication.Coordinator);
            }

            return result;
        }

        /// <summary>
        /// Get all response statuses from database asynchrously
        /// </summary>
        /// <returns></returns>
        public Task<List<ApplicationResponseStatus>> GetAllApplicationResponseStatusAsync()
        {
            var applicationResponseStatusManager = this.container.GetInstance<ApplicationResponseStatusManager>();

            return applicationResponseStatusManager.GetAllAsync();
        }

        public ApplicationType GetApplicationType(int id)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();

            return applicationTypeManager.GetById(id);
        }

        public async Task<bool> ApplicationTypeNameAlreadyExistsAsync(int currentId, string name)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();

            var appType = await applicationTypeManager.GetByNameAsync(name);

            return appType != null && appType.Id != currentId;
        }

        public bool ApplicationTypeNameAlreadyExists(int currentId, string name)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();

            var appType = applicationTypeManager.GetByName(name);

            return appType != null && appType.Id != currentId;
        }

        public async Task<ApplicationType> AddOrEditApplicationTypeAsync(ApplicationTypeItem model, string savedBy)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            ApplicationType applicationType = null;

            var exists = await this.ApplicationTypeNameAlreadyExistsAsync(model.ApplicationTypeId.GetValueOrDefault(), model.ApplicationTypeName);

            if (exists)
            {
                throw new Exception($"Module Name {model.ApplicationTypeName} already exists.");
            }

            if (model.ApplicationTypeId.HasValue)
            {
                applicationType = applicationTypeManager.GetById(model.ApplicationTypeId.Value);

                if (applicationType == null)
                {
                    throw new Exception("Cannot find application type");
                }

                applicationType.Name = model.ApplicationTypeName;
                applicationType.UpdatedBy = savedBy;
                applicationType.UpdatedDate = DateTime.Now;

                await applicationTypeManager.SaveAsync(applicationType);


                return applicationType;
            }

            applicationType = new ApplicationType()
            {
                Name = model.ApplicationTypeName,
                IsManageable = true,
                CreatedDate = DateTime.Now,
                CreatedBy = savedBy
            };

            applicationTypeManager.Add(applicationType);

            return applicationType;
        }

        public ApplicationType AddOrEditApplicationType(ApplicationTypeItem model, string savedBy)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            ApplicationType applicationType = null;

            var exists = this.ApplicationTypeNameAlreadyExists(model.ApplicationTypeId.GetValueOrDefault(), model.ApplicationTypeName);

            if (exists)
            {
                throw new Exception($"Module Name {model.ApplicationTypeName} already exists.");
            }

            if (model.ApplicationTypeId.HasValue)
            {
                applicationType = applicationTypeManager.GetById(model.ApplicationTypeId.Value);

                if (applicationType == null)
                {
                    throw new Exception("Cannot find application type");
                }

                applicationType.Name = model.ApplicationTypeName;
                applicationType.UpdatedBy = savedBy;
                applicationType.UpdatedDate = DateTime.Now;

                applicationTypeManager.Save(applicationType);


                return applicationType;
            }

            applicationType = new ApplicationType()
            {
                Name = model.ApplicationTypeName,
                IsManageable = true,
                CreatedDate = DateTime.Now,
                CreatedBy = savedBy
            };

            applicationTypeManager.Add(applicationType);

            return applicationType;
        }

        public async Task SendComplianceToInspection(Guid complianceApplicationId, string organizationName, Guid coordinatorId, bool isCb, string url,
            string scheduleInspectionEmail, string sentBy)
        {
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var applicationStatus = await applicationStatusManager.GetByNameAsync(Constants.ApplicationStatus.SchedulingInspection);

            await
                complianceApplicationManager.SetApplicationStatusAsync(complianceApplicationId, applicationStatus.Id, true, sentBy);

            var compApp = complianceApplicationManager.GetById(complianceApplicationId);

            var app = compApp.Applications.First();

            var coordinator = userManager.GetById(coordinatorId);

            var qualityManagers = userManager.GetByRole(Constants.Roles.QualityManager);

            var htmlUrl = url + "app/email.templates/sendToInspection.html";

            var html = WebHelper.GetHtml(htmlUrl);
            html = html.Replace("{OrgName}", organizationName);

            if (isCb)
            {
                html = html.Replace("{InspectionRequestFormURL}",
                    string.Format("{0}#/Reporting?c={1}&r=CB%20Inspection%20Request", url, complianceApplicationId));
            }
            else
            {
                html = html.Replace("{InspectionRequestFormURL}", string.Format("{0}#/Reporting?c={1}&r=CT%20Inspection%20Request", url, complianceApplicationId));
            }

            var ccs = new List<string>
            {
                coordinator.EmailAddress, "portal@factwebsite.org"
            };

            ccs.AddRange(qualityManagers.Select(x=>x.EmailAddress));

            html = html.Replace("{InspectionSchedulerURL}", string.Format("{0}#/InspectionScheduler", url));
            html = html.Replace("{Url}", string.Format("{0}#/Reviewer/View?app={1}&c={2}", url, app.UniqueId, complianceApplicationId));
            html = html.Replace("{CoordinatorName}",
                string.Format("{0} {1}", coordinator.FirstName, coordinator.LastName));
            html = html.Replace("{CoordinatorCredentials}",
                string.Join(", ", coordinator.UserCredentials.Select(x => x.Credential.Name)));
            html = html.Replace("{CoordinatorTitle}", coordinator.Title);
            html = html.Replace("{CoordinatorPhoneNumber}", coordinator.PreferredPhoneNumber);
            html = html.Replace("{CoordinatorEmailAddress}", coordinator.EmailAddress);

            EmailHelper.Send(new List<string> { scheduleInspectionEmail }, ccs,
                string.Format("Inspection Request - {0}", organizationName), html);
        }

        public async Task SetInspectorCompleteAsync(int applicationId, Guid userId, string url, string completedBy)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var appSettingManager = this.container.GetInstance<ApplicationSettingManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var qualityManagers = userManager.GetByRole(Constants.Roles.QualityManager);

            var factPortalSetting = appSettingManager.GetByName(Constants.ApplicationSettings.AutoCcEmailAddress);

            var application = applicationManager.GetById(applicationId);
            if (application == null || !application.ComplianceApplicationId.HasValue) return;

            var compApp = this.GetComplianceResponses(userId, false, (int)Constants.Role.Inspector, false, application.ComplianceApplicationId.GetValueOrDefault(), application.UniqueId);

            var siteResp = compApp.ComplianceApplicationSites.FirstOrDefault(x => x.SiteId == application.SiteId);

            var app = siteResp?.AppResponses.FirstOrDefault(x => x.ApplicationId == application.Id);
            
            if (app != null)
            {
                var found = app.ApplicationSectionResponses.Where(x => x.IsVisible &&
                                                             x.SectionStatusName !=
                                                             Constants.ApplicationResponseStatus.NotCompliant &&
                                                             x.SectionStatusName !=
                                                             Constants.ApplicationResponseStatus.Compliant &&
                                                             x.SectionStatusName !=
                                                             Constants.ApplicationResponseStatus.NoResponseRequested).
                ToList();

                if (found.Count > 0)
                {
                    throw new Exception("Your site's checklist is incomplete. Please mark all questions compliant, not compliant, or not applicable.");
                }
            }

            var details = inspectionScheduleDetailManager.GetAllActiveByComplianceApplication(application.ComplianceApplicationId.Value);

            var dets = details.Where(x => x.UserId == userId && x.SiteId == application.SiteId).ToList();

            if (dets == null || dets.Count == 0)
            {
                throw new Exception("Cannot find user associated with application");
            }

            foreach (var detail in dets)
            {
                detail.IsInspectionComplete = true;
                detail.InspectorCompletionDate = DateTime.Now;
                detail.UpdatedBy = completedBy;
                detail.UpdatedDate = DateTime.Now;

                var allComplete = details.All(x => (x.IsActive && !x.IsArchive && (x.IsInspectionComplete == true || x.Id == detail.Id)) || x.IsArchive || !x.IsActive || x.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees);

                if (detail.IsLead && allComplete)
                {
                    detail.InspectionSchedule.CompletionDate = DateTime.Now;
                    detail.InspectionSchedule.IsCompleted = true;
                    detail.InspectionSchedule.UpdatedBy = completedBy;
                    detail.InspectionSchedule.UpdatedDate = DateTime.Now;
                }

                await inspectionScheduleDetailManager.SaveAsync(detail);

                if (detail.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees)
                {
                    var mentor = details.FirstOrDefault(x => x.IsMentor && x.IsActive);

                    if (mentor != null)
                    {
                        var coordinator = application.ComplianceApplication?.Coordinator ?? application.Coordinator ?? new User
                        {
                            UserCredentials = new List<UserCredential>(),
                            UserJobFunctions = new List<UserJobFunction>()
                        };

                        var ccs = new List<string>
                        {
                            coordinator.EmailAddress,
                            detail.User.EmailAddress,
                            factPortalSetting.Value
                        };

                        var htmlUrl = url + "app/email.templates/traineeSubmitted.html";
                        var appUrl = string.Format("{0}#/Compliance?c={1}", url, application.ComplianceApplication.Id);
                        var traineeUrl =
                            $"{url}#/Reporting?app={application.UniqueId}&c={application.ComplianceApplicationId}&org={application.Organization.Name}&r=Trainee%20Inspection%20Summary";

                        var html = WebHelper.GetHtml(htmlUrl);
                        html = html.Replace("{OrgName}", application.Organization.Name);
                        html = html.Replace("{MentorName}", $"{mentor.User.FirstName} {mentor.User.LastName}");
                        html = html.Replace("{TraineeName}", $"{detail.User.FirstName} {detail.User.LastName}");
                        html = html.Replace("{Url}", appUrl);
                        html = html.Replace("{TraineeUrl}", traineeUrl);

                        if (coordinator.UserCredentials != null && coordinator.UserCredentials.Count > 0)
                        {
                            html = html.Replace("{CoordinatorName}",
                                string.Format("{0} {1}, {2}", coordinator.FirstName, coordinator.LastName, string.Join(", ", coordinator.UserCredentials.Select(x => x.Credential.Name))));
                        }
                        else
                        {
                            html = html.Replace("{CoordinatorName}",
                                string.Format("{0} {1}", coordinator.FirstName, coordinator.LastName));
                        }
                        html = html.Replace("{CoordinatorTitle}", coordinator.Title);
                        html = html.Replace("{CoordinatorPhoneNumber}", coordinator.PreferredPhoneNumber);
                        html = html.Replace("{CoordinatorEmailAddress}", coordinator.EmailAddress);

                        EmailHelper.Send(new List<string> { mentor.User.EmailAddress }, ccs,
                            $"FACT Trainee Inspection Report Complete - {application.Organization.Name}", html, null, true);
                    }
                    
                }

                if (detail.IsLead && allComplete)
                {
                    var coordinator = application.ComplianceApplication?.Coordinator ?? application.Coordinator ?? new User
                    {
                        UserCredentials = new List<UserCredential>(),
                        UserJobFunctions = new List<UserJobFunction>()
                    };

                    var htmlUrl = url + "app/email.templates/inspectorReportSubmitted.html";
                    var appUrl = string.Format("{0}#/Compliance?c={1}", url, application.ComplianceApplication.Id);

                    var html = WebHelper.GetHtml(htmlUrl);
                    html = html.Replace("{OrgName}", application.Organization.Name);
                    html = html.Replace("{SubmittedDate}", DateTime.Now.ToString("G"));
                    html = html.Replace("{InspectionSchedulerDateRange}",
                        string.Format("{0} - {1}", detail.InspectionSchedule.StartDate.ToShortDateString(),
                            detail.InspectionSchedule.EndDate.ToShortDateString()));
                    html = html.Replace("{Url}", appUrl);
                    if (coordinator.UserCredentials != null && coordinator.UserCredentials.Count > 0)
                    {
                        html = html.Replace("{CoordinatorName}",
                            string.Format("{0} {1}, {2}", coordinator.FirstName, coordinator.LastName, string.Join(", ", coordinator.UserCredentials.Select(x => x.Credential.Name))));
                    }
                    else
                    {
                        html = html.Replace("{CoordinatorName}",
                            string.Format("{0} {1}", coordinator.FirstName, coordinator.LastName));
                    }
                    html = html.Replace("{CoordinatorTitle}", coordinator.Title);
                    html = html.Replace("{CoordinatorPhoneNumber}", coordinator.PreferredPhoneNumber);
                    html = html.Replace("{CoordinatorEmailAddress}", coordinator.EmailAddress);

                    var inspectors = details.Select(x => x.User.EmailAddress).ToList();

                    var ccs = new List<string>
                    {
                        coordinator.EmailAddress, factPortalSetting.Value
                    };

                    ccs.AddRange(qualityManagers.Select(x=>x.EmailAddress));

                    EmailHelper.Send(inspectors, ccs,
                        string.Format("FACT Inspection Report Submitted - {0}", application.Organization.Name), html, null, true);

                    await this.UpdateApplicationStatusAsync(application.ApplicationTypeId, Constants.ApplicationStatus.PostInspectionReview,
                        application.OrganizationId, null, string.Empty, completedBy);

                    var reportingFacade = this.container.GetInstance<ReportingFacade>();

                    var cycleNumber = 1;

                    if (application.Organization.OrganizationAccreditationCycles != null &&
                        application.Organization.OrganizationAccreditationCycles.Count > 0)
                    {
                        cycleNumber = application.Organization.OrganizationAccreditationCycles.Max(c => c.Number);
                    }

                    reportingFacade.CopyReport(Constants.Reports.InspectionSummary,
                        application.Organization.DocumentLibraryVaultId, application.ComplianceApplicationId.GetValueOrDefault(),
                        application.Organization.Name, cycleNumber, true);

                }
            }
           
        }

        public Task<List<ApplicationType>> GetAllApplicationTypesAsync()
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();

            return applicationTypeManager.GetAllAsync();
        }

        public Task<List<ApplicationStatus>> GetApplicationStatusAsync()
        {
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();

            return applicationStatusManager.GetAllAsync();
        }

        public List<ApplicationStatusHistory> GetApplicationStatusHistory(Guid applicationUniqueId)
        {
            var applicationStatusHistoryManager = this.container.GetInstance<ApplicationStatusHistoryManager>();

            return applicationStatusHistoryManager.GetAllByApplicationId(applicationUniqueId);
        }

        public List<ApplicationStatusHistory> GetCompAppStatusHistory(Guid compApplicationId)
        {
            var result = new List<ApplicationStatusHistory>();
            var compAppManager = this.container.GetInstance<ComplianceApplicationManager>();

            var compApp = compAppManager.GetById(compApplicationId);

            foreach (var app in compApp.Applications)
            {
                result.AddRange(this.GetApplicationStatusHistory(app.UniqueId));
            }

            return result;
        }

        public Application GetByOrgAndType(long organizationId, string applicationTypeName)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applicationType = applicationTypeManager.GetByName(applicationTypeName);

            if (applicationType == null)
            {
                throw new ObjectNotFoundException(string.Format("Cannot find application type {0}", applicationTypeName));
            }

            var items = applicationManager.GetAllByOrganizationOrApplicationType(
                System.Convert.ToInt32(organizationId), applicationType.Id);

            return items.FirstOrDefault();
        }

        public List<Application> GetAllByOrgAndType(long organizationId, string applicationTypeName)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applicationType = applicationTypeManager.GetByName(applicationTypeName);

            if (applicationType == null)
            {
                throw new ObjectNotFoundException(string.Format("Cannot find application type {0}", applicationTypeName));
            }

            var items = applicationManager.GetAllByOrganizationOrApplicationType(
                System.Convert.ToInt32(organizationId), applicationType.Id);

            return items;
        }

        public async Task<Application> GetByOrgAndTypeAsync(string orgName, string applicationTypeName)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var orgManager = this.container.GetInstance<OrganizationManager>();

            var applicationType = await applicationTypeManager.GetByNameAsync(applicationTypeName);
            var org = await orgManager.GetByNameAsync(orgName);

            if (applicationType == null || org == null)
            {
                throw new ObjectNotFoundException("Cannot find data");
            }

            var items = await applicationManager.GetAllByOrganizationOrApplicationTypeAsync(org.Id, applicationType.Id);

            return items.FirstOrDefault();
        }

        public bool IsAccreditationRoleTrainee(Guid? currentUserId, Guid applicationUniqueId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var appManager = this.container.GetInstance<ApplicationManager>();

            var app = appManager.GetByUniqueId(applicationUniqueId, false, false);

            if (app.ComplianceApplicationId.HasValue)
            {
                var schedules =
                    inspectionScheduleDetailManager.GetAllActiveByComplianceApplication(
                        app.ComplianceApplicationId.Value);

                return schedules.Any(x => x.UserId == currentUserId.Value && x.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees && !x.IsInspectionComplete.GetValueOrDefault());               
            }

            var accreditatoinRole = inspectionScheduleDetailManager.GetAccreditationRoleByUserId(currentUserId.Value, applicationUniqueId);
            return accreditatoinRole != null && accreditatoinRole.Name == Constants.AccreditationsRoles.InspectorTrainees;
        }

        public string GetComplianceAccessType(Guid userId, bool isFactStaff, Guid complianceApplicationId)
        {
            if (isFactStaff)
            {
                return Constants.Roles.FACTAdministrator;
            }

            var applicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var application = applicationManager.GetById(complianceApplicationId);
            var user = userManager.GetById(userId);

            if (application == null || user == null || user.Organizations == null) return string.Empty;

            if (user.Organizations.Any(x => x.OrganizationId == application.OrganizationId)) return Constants.Roles.User;

            if (user.OrganizationConsutants.Any(x => x.OrganizationId == application.OrganizationId)) return Constants.Roles.User;

            return
                application.Applications.Any(
                    x => x.InspectionSchedules.Any(y => y.InspectionScheduleDetails.Any(z => z.UserId == userId)))
                    ? Constants.Roles.Inspector
                    : string.Empty;
        }


        public string GetAccessType(Guid userId, bool isFactStaff, Guid applicationUniqueId)
        {
            if (isFactStaff)
            {
                return Constants.Roles.FACTAdministrator;
            }

            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var application = applicationManager.GetByUniqueId(applicationUniqueId, false, false);
            var user = userManager.GetById(userId);


            if (application == null || user == null || user.Organizations == null) return string.Empty;

            if (user.Organizations.Any(x => x.OrganizationId == application.OrganizationId)) return Constants.Roles.User;

            if (user.OrganizationConsutants.Any(x => x.OrganizationId == application.OrganizationId)) return Constants.Roles.FACTConsultant;


            if (application.InspectionSchedules != null && application.InspectionSchedules.Count != 0 &&
                !application.InspectionSchedules.All(x => x.InspectionScheduleDetails.All(y => y.UserId != userId)))
                return Constants.Roles.Inspector;

            return string.Empty;
        }

        public bool HasAccessToApplication(bool isFactStaff, Guid? applicationUniqueId, Guid? complianceApplicationId,
            Guid userId)
        {
            if (isFactStaff) return true;

            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();

            var model = complianceApplicationManager.DoesInspectorHaveAccess(applicationUniqueId, complianceApplicationId,
                userId);

            return model?.Found ?? false;
        }

        public List<ApplicationSectionItem> BuildApplication(string orgName, string applicationTypeName, Guid? currentUserId, bool canSeeCitations)
        {
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();
            var applicationResponseTraineeManager = this.container.GetInstance<ApplicationResponseTraineeManager>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var siteManager = this.container.GetInstance<SiteManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var answerManager = this.container.GetInstance<ApplicationSectionQuestionAnswerManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();

            var answers = answerManager.GetAll();

            var applicationType = this.GetApplicationType(applicationTypeName);
            var org = organizationManager.GetByName(orgName);
            var users = userManager.GetByOrganization(org.Id);
            var application = applicationManager.GetByOrganizationAndApplicationTypeIgnoreActive(org.Id, applicationType.Id);

            var comments = applicationResponseCommentManager.GetByApplication(application.Id);

            var sections = this.GetApplicationSectionItems(org.Id, applicationType.Id, application.ApplicationVersionId.GetValueOrDefault(), application.UniqueId, null, application.ApplicationQuestionNotApplicables);

            var applicationResponses = applicationResponseManager.GetApplicationResponses(org.Id,
                applicationType.Id);

            var isTrainee = this.IsAccreditationRoleTrainee(currentUserId, applicationResponses.FirstOrDefault()?.Application.UniqueId ?? Guid.NewGuid());

            if (application.ApplicationType.Name != Constants.ApplicationTypes.Annual &&
                application.ApplicationType.Name != Constants.ApplicationTypes.Eligibility &&
                application.ApplicationType.Name != Constants.ApplicationTypes.Renewal)
            {
                var scopeTypeList = siteManager.GetScopeTypes(application.OrganizationId);

                HideQuestionsOutofScope(sections, scopeTypeList);
            }

            if (isTrainee)
            {
                var applicationResponsesTrainee =
                    applicationResponseTraineeManager.GetApplicationResponsesTrainee(org.Id,
                        applicationType.Id);

                sections = this.Convert(sections, applicationResponsesTrainee, applicationResponses);
            }
            else
            {
                var accreditationOutcomeManager = this.container.GetInstance<AccreditationOutcomeManager>();
                var outcomes = accreditationOutcomeManager.GetByAppId(application.UniqueId);

                DateTime? appSubmittedDate = null;

                if ((!canSeeCitations || users.Any(x => x.Id == currentUserId)) &&
                    application.ApplicationStatusId != (int) Constants.ApplicationStatuses.Rfi &&
                    application.ApplicationStatusId != (int) Constants.ApplicationStatuses.ApplicantResponse)
                {
                    appSubmittedDate = application.UpdatedDate ?? System.Convert.ToDateTime("1/1/2017");
                }

                sections = this.Convert(appSubmittedDate, sections, applicationResponses, answers, users, comments, outcomes, canSeeCitations);
            }
                

            return sections;
        }

        public List<ApplicationSectionItem> BuildApplication(Guid userId, bool isFactStaff, Guid applicationUniqueId, bool canSeeCitations,
            bool? isTrainee = null, Application application = null, User currentUser = null,
            List<SiteScopeType> scopeTypeList = null, List<ApplicationSectionQuestionAnswer> answers = null,
            List<User> users = null, List<ApplicationResponseComment> comments = null,
            List<ApplicationSection> allApplicationSections = null, bool setColor = false, 
            Guid? complianceAppId = null, ComplianceApplication complianceApplication = null,
            bool isReviewer = false, List<ApplicationResponse> applicationResponses = null)
        {
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();
            var siteManager = this.container.GetInstance<SiteManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var answerManager = this.container.GetInstance<ApplicationSectionQuestionAnswerManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var applicationResponseTraineeManager = this.container.GetInstance<ApplicationResponseTraineeManager>();
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            var accreditationOutcomeManager = this.container.GetInstance<AccreditationOutcomeManager>();

            if (application == null)
            {
                application = applicationManager.GetByUniqueIdIgnoreActive(applicationUniqueId);
            }           

            if (application == null) return null;

            if (!this.HasAccessToApplication(isFactStaff, applicationUniqueId, application.ComplianceApplicationId, userId))
            {
                throw new Exception("Not Authorized");
            }

            if (!isTrainee.HasValue)
            {
                var details = inspectionScheduleDetailManager.GetAllActiveByApplication(applicationUniqueId);

                isTrainee =
                    details.Any(
                        x =>
                            x.UserId == userId &&
                            x.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees);
            }


            answers = answers ?? answerManager.GetAllForVersion(application.ApplicationVersionId.GetValueOrDefault());
            users = users ?? userManager.GetByOrganization(application.OrganizationId);

            if (comments == null)
            {
                

                comments = applicationResponseCommentManager.GetByApplication(application.Id);

                if ((!isTrainee.GetValueOrDefault() || isFactStaff) && application.ComplianceApplicationId != null)
                {
                    var inspectionSchedules =
                        inspectionScheduleManager.GetAllForCompliance(application.ComplianceApplicationId.Value);

                    var traineeIds = new List<Guid>();

                    foreach (var schedule in inspectionSchedules)
                    {
                        traineeIds.AddRange(schedule.InspectionScheduleDetails.Where(x=>x.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees).Select(x=>x.UserId).Distinct());
                    }

                    comments = comments.Where(x => traineeIds.All(y => y != x.FromUser)).ToList();
                }
            }
            

            var sections = this.GetApplicationSectionItems(application.OrganizationId, application.ApplicationTypeId,
                application.ApplicationVersionId.GetValueOrDefault(), application.UniqueId, null,
                application.ApplicationQuestionNotApplicables, allApplicationSections);

            applicationResponses = applicationResponses ?? applicationResponseManager.GetApplicationResponses(applicationUniqueId);

            

            if (isTrainee.GetValueOrDefault())
            {
                var traineeResponses = applicationResponseTraineeManager.GetApplicationResponses(applicationUniqueId);

                foreach (var response in applicationResponses)
                {
                    var traineeResps =
                        traineeResponses.Where(
                                x =>
                                    x.ApplicationId == response.ApplicationId &&
                                    x.ApplicationSectionQuestionId == response.ApplicationSectionQuestionId)
                            .ToList();

                    if (traineeResps.Count > 0)
                    {
                        foreach (var resp in traineeResps)
                        {
                            response.ApplicationResponseStatus = resp.ApplicationResponseStatus;
                            response.ApplicationResponseStatusId = resp.ApplicationResponseStatusId;
                        }
                    }
                    else
                    {
                        response.ApplicationResponseStatus = new ApplicationResponseStatus { Id = 2, Name = Constants.ApplicationResponseStatus.Reviewed };
                        response.ApplicationResponseStatusId = 2;
                    }

                }
            }

            if (application.ApplicationType.Name != Constants.ApplicationTypes.Annual &&
                application.ApplicationType.Name != Constants.ApplicationTypes.Eligibility &&
                application.ApplicationType.Name != Constants.ApplicationTypes.Renewal)
            {
                if (application.SiteId.HasValue)
                {
                    scopeTypeList = scopeTypeList ?? siteManager.GetScopeTypesBySite(application.SiteId.Value);
                }
                else
                {
                    scopeTypeList = scopeTypeList ?? siteManager.GetScopeTypes(application.OrganizationId);
                }
                

                HideQuestionsOutofScope(sections, scopeTypeList);
            }

            var outcomes = accreditationOutcomeManager.GetByAppId(application.UniqueId);

            DateTime? appSubmittedDate = null;

            if ((!canSeeCitations || users.Any(x => x.Id == userId)) &&
                application.ApplicationStatusId != (int) Constants.ApplicationStatuses.Rfi &&
                 application.ApplicationStatusId != (int) Constants.ApplicationStatuses.ApplicantResponse)
            {
                appSubmittedDate = application.UpdatedDate ?? System.Convert.ToDateTime("1/1/2017");
            }

            sections = this.Convert(appSubmittedDate, sections, applicationResponses, answers, users, comments, outcomes, canSeeCitations);

            if (!setColor) return sections;

            DateTime? submittedDate;
            var statusName = string.Empty;

            if (complianceApplication == null && complianceAppId.HasValue)
            {
                var compManager = this.container.GetInstance<ComplianceApplicationManager>();
                complianceApplication = compManager.GetByIdWithApps(complianceAppId.Value);
                submittedDate = complianceApplication.Applications.Max(x => x.SubmittedDate);
                statusName = complianceApplication.ApplicationStatus.Name;
            }
            else
            {
                submittedDate = application.SubmittedDate;
                statusName = application.ApplicationStatus?.Name;
            }

            sections = ModelConversions.SetColor(statusName, submittedDate.GetValueOrDefault(), sections, isReviewer);

            return sections;
        }

        public List<ApplicationSectionItem> GetApplicationSectionItems(long organizationId, int applicationTypeId, Guid applicationVersionId, Guid? appUniqueId, List<ApplicationSectionQuestionAnswerDisplay> displays, ICollection<ApplicationQuestionNotApplicable> notApplicables = null, List<ApplicationSection> allApplicationSections = null)
        {
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var displayManager = this.container.GetInstance<ApplicationSectionQuestionAnswerDisplayManager>();

            displays = displays ?? displayManager.GetAllForVersion(applicationVersionId);

            var applicationSections = applicationSectionManager.GetApplicationSections(applicationTypeId, applicationVersionId, displays, notApplicables, allApplicationSections);

            var sections = applicationSections.OrderBy(x => System.Convert.ToInt32(x.Order)).Select(x => ModelConversions.Convert(x, appUniqueId)).ToList();

            return sections;
        }

        public ApplicationType GetApplicationType(string applicationTypeName)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var applicationType = applicationTypeManager.GetByName(applicationTypeName);

            if (applicationType == null)
            {
                throw new ObjectNotFoundException(string.Format("Cannot find application type {0}", applicationTypeName));
            }

            return applicationType;
        }

        public List<SiteApplicationSection> BuildRFIComments(Guid compAppId, Guid userId, bool isFactStaff, bool canSeeCitations)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var apps = applicationManager.GetByComplianceApplicationId(compAppId);
            //var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            //var app = complianceApplicationManager.Get(compAppId);

            var commentTypeManager = this.container.GetInstance<CommentTypeManager>();
            var commentType = commentTypeManager.GetByName(Constants.CommentTypes.RFI);

            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();
            var comments = applicationResponseCommentManager.GetByCompliance(compAppId);
            comments = comments.Where(x => x.VisibleToApplicant == true).ToList();

            if (!isFactStaff && apps.All(x=>x.ApplicationStatus.Name != Constants.ApplicationStatus.InProgress && x.ApplicationStatus.Name != Constants.ApplicationStatus.ForReview))
                return null;

            var grouped = apps.GroupBy(x => x.Site);

            grouped = grouped.OrderBy(x => Comparer.OrderSite(x.Key)).ToList();

            var complianceApplicationSites = grouped
                .Select(site => new SiteApplicationSection
                {
                    SiteItem = ModelConversions.Convert(site.Key, false, false, false),
                    //ApplicationSectionItem = this.GetApplicationSections(displays, applicationTypes, responses, comments, applicationRfiStatus, commentType, site.ToList(), app.OrganizationId, userId, isFactStaff, canSeeCitations)
                    ApplicationSectionItem = this.BuildRFISections(comments, commentType, site.ToList())
                })
                .ToList();


            return complianceApplicationSites;

        }

        public List<ApplicationSectionItem> BuildRFISections(List<ApplicationResponseComment> allComments, CommentType commentType, List<Application> applicationList)
        {
            var applicationSectionItemList = new List<ApplicationSectionItem>();

            foreach (var application in applicationList)
            {
                var questionManager = this.container.GetInstance<ApplicationSectionQuestionManager>();

                var questions = questionManager.GetQuestionsWithRFIs(application.Id);

                var sectionManager = this.container.GetInstance<ApplicationSectionManager>();
                var sections = sectionManager.GetSectionsWithRFIs(application.Id);

                foreach (var section in sections)
                {
                    var sect = ModelConversions.Convert(section, application.UniqueId, null, false);
                    sect.Questions = new List<Question>();

                    var qs = questions.Where(x => x.ApplicationSectionId == sect.Id).ToList();

                    foreach (var ques in qs)
                    {
                        var comments =
                        allComments.Where(
                            x =>
                                x.ApplicationId == application.Id &&
                                x.QuestionId == ques.Id &&
                                x.CommentTypeId == commentType.Id)
                            .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate))
                            .ToList();

                        var question = ModelConversions.Convert(ques, false);
                        question.ResponseCommentsRFI = ModelConversions.Convert(comments);
                        question.ApplicationResponseComments = question.ResponseCommentsRFI;

                        sect.Questions.Add(question);
                    }

                    sect.Questions =
                        sect.Questions.OrderBy(x => x.ComplianceNumber)
                            .ThenByDescending(x => System.Convert.ToInt32(x.Order))
                            .ToList();

                    applicationSectionItemList.Add(sect);
                }

                //foreach (var response in responses)
                //{
                //    var comments =
                //        allComments.Where(
                //            x =>
                //                x.ApplicationId == application.Id &&
                //                x.QuestionId == response.ApplicationSectionQuestionId &&
                //                x.CommentTypeId == commentType.Id)
                //            .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate))
                //            .ToList();

                //    if (comments.Count != 0)
                //    {
                //        var section =
                //            sections.SingleOrDefault(
                //                x => x.Id == response.ApplicationSectionQuestion.ApplicationSectionId);

                //        var question = ModelConversions.Convert(response.ApplicationSectionQuestion, true);
                //        question.ResponseCommentsRFI = ModelConversions.Convert(comments);
                //        question.ApplicationResponseComments = question.ResponseCommentsRFI;

                //        if (section == null)
                //        {
                //            var sect = ModelConversions.Convert(response.ApplicationSectionQuestion.ApplicationSection, application.UniqueId, null, false);

                //            sect.Questions = new List<Question>
                //            {
                //                question
                //            };
                //            sections.Add(sect);
                //        }
                //        else
                //        {
                //            if (section.Questions.All(x => x.Id != question.Id))
                //            {
                //                section.Questions.Add(question);
                //            }
                //        }

                //    }
                //}

                //foreach (var section in sections)
                //{
                //    section.Questions =
                //        section.Questions.OrderBy(x => x.ComplianceNumber)
                //            .ThenByDescending(x => System.Convert.ToInt32(x.Order))
                //            .ToList();

                //    applicationSectionItemList.Add(section);
                //}

                //foreach (var section in applicationSections)
                //{
                //    if (section.Questions.Count > 0)
                //    {
                //        var questions = section.Questions.ToList();

                //        for (var i = 0; i < questions.Count; i++)
                //        {
                //            questions[i].ApplicationResponseComments =
                //                comments.Where(x => x.QuestionId == questions[i].Id && x.CommentTypeId == commentType.Id).ToList();

                //            if (questions[i].ApplicationResponseComments.Count == 0)
                //            {
                //                questions.RemoveAt(i);
                //                i--;
                //            }
                //        }
                //    }

                //    if (section.Questions.Count > 0)
                //    {
                //        applicationSectionItemList.Add(ModelConversions.Convert(section, application.UniqueId));
                //    }
                //}

                //var roots = applicationSections.Where(x => x.ParentApplicationSectionId == null).ToList();

                //for (var i = 0; i < roots.Count; i++)
                //{
                //    roots[i] = this.BuildSection(roots[i], applicationSections);
                //}

                //var sections = roots.OrderBy(x => System.Convert.ToInt32(x.Order)).Select(x => ModelConversions.Convert(x, application.UniqueId)).ToList();



                //sections = this.ConvertRFIView(sections, application, applicationRFIStatus, commentType, allResponses, comments,
                //    null, canSeeCitations);

                //applicationSectionItemList.AddRange(sections);
            }

            return applicationSectionItemList;
        }

        private ApplicationSection BuildSection(ApplicationSection section, List<ApplicationSection> allSections)
        {
            var children = allSections.Where(x => x.ParentApplicationSectionId == section.Id).ToList();

            for (var i = 0; i < children.Count; i++)
            {
                children[i] = this.BuildSection(children[i], allSections);
            }

            section.Children = children;

            return section;
        }

        private List<ApplicationSectionItem> BuildRfi(ApplicationSectionItem section)
        {
            var items = new List<ApplicationSectionItem>();

            if (section.Questions.Count > 0)
            {
                
            }

            return items;
        }

        public List<SiteApplicationSection> BuildRFICommentsForApplication(Guid applicationUniqueId, Guid userId, bool isFactStaff, bool canSeeCitations)
        {
            var manager = this.container.GetInstance<ApplicationManager>();
            var app = manager.GetByUniqueId(applicationUniqueId);

            var commentTypeManager = this.container.GetInstance<CommentTypeManager>();
            var commentType = commentTypeManager.GetByName(Constants.CommentTypes.RFI);

            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();
            var comments = applicationResponseCommentManager.GetByApplication(app.Id);

            var record = new SiteApplicationSection
            {
                SiteItem = ModelConversions.Convert(app.Site, false),
                ApplicationSectionItem = this.BuildRFISections(comments, commentType, new List<Application> { app })
            };

            return new List<SiteApplicationSection> { record };

        }

        public List<ApplicationSectionItem> GetApplicationSections(Dictionary<Guid, List<ApplicationSectionQuestionAnswerDisplay>> displayDictionary, Dictionary<int, List<ApplicationSection>> applicationTypes, List<ApplicationResponse> allResponses, List<ApplicationResponseComment> allComments, ApplicationResponseStatus applicationRFIStatus, CommentType commentType, List<Application> applicationList, long organizationId, Guid userId, bool isFactStaff, bool canSeeCitations)
        {
            var applicationSectionItemList = new List<ApplicationSectionItem>();
            
            
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();

            if (applicationRFIStatus == null)
            {
                throw new ObjectNotFoundException(string.Format("Cannot find application RFI status {0}", Constants.ApplicationResponseStatus.RFI));
            }

            if (commentType == null)
            {
                throw new ObjectNotFoundException(string.Format("Cannot find Comment Type {0}", Constants.CommentTypes.RFI));
            }

            foreach (var application in applicationList)
            {
                List<ApplicationSection> applicationSections = null;

                if (applicationTypes.ContainsKey(application.ApplicationTypeId))
                {
                    applicationSections = applicationTypes[application.ApplicationTypeId].Select(x=> new ApplicationSection
                    {
                        Id = x.Id,
                        ApplicationTypeId = x.ApplicationTypeId,
                        ApplicationType = x.ApplicationType,
                        ParentApplicationSectionId = x.ParentApplicationSectionId,
                        ParentApplicationSection = x.ParentApplicationSection,
                        ApplicationVersionId = x.ApplicationVersionId,
                        ApplicationVersion = x.ApplicationVersion,
                        PartNumber = x.PartNumber,
                        Name = x.Name,
                        IsActive = x.IsActive,
                        HelpText = x.HelpText,
                        IsVariance = x.IsVariance,
                        Version = x.Version,
                        Order = x.Order,
                        UniqueIdentifier = x.UniqueIdentifier,
                        Questions = x.Questions,
                        Children = x.Children,
                        ApplicationSectionScopeTypes = x.ApplicationSectionScopeTypes

                    })
                    .ToList();
                }
                else
                {
                    var appSections =
                        applicationSectionManager.GetFlatForApplicationType(application.ApplicationTypeId, application.ApplicationVersionId.GetValueOrDefault());
                    applicationTypes.Add(application.ApplicationTypeId, appSections);

                    applicationSections = appSections.Select(x => x).ToList();
                }

                List<ApplicationSectionQuestionAnswerDisplay> displays = null;

                if (displayDictionary.ContainsKey(application.ApplicationVersionId.GetValueOrDefault()))
                {
                    displays = displayDictionary[application.ApplicationVersionId.GetValueOrDefault()];
                }
                else
                {
                    var displayManager = this.container.GetInstance<ApplicationSectionQuestionAnswerDisplayManager>();
                    displays = displayManager.GetAllForVersion(application.ApplicationVersionId.GetValueOrDefault());
                    displayDictionary.Add(application.ApplicationVersionId.GetValueOrDefault(), displays);
                }


                var sections = this.GetApplicationSectionItems(organizationId, application.ApplicationTypeId, application.ApplicationVersionId.GetValueOrDefault(), application.UniqueId, displays, application.ApplicationQuestionNotApplicables, applicationSections);
                var applicationResponses =
                    allResponses.Where(x => x.ApplicationId == application.Id).ToList();
                var comments = allComments.Where(x => x.ApplicationId == application.Id).ToList();
                sections = this.ConvertRFIView(sections, application, applicationRFIStatus, commentType, applicationResponses, comments, null, canSeeCitations);

                foreach (var sect in sections)
                {
                    sect.AppUniqueId = application.UniqueId;
                }

                applicationSectionItemList.AddRange(sections);
            }

            return applicationSectionItemList;
        }

        public ComplianceApplication SaveComplianceApplication(ComplianceApplicationItem model, string savedBy)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var complianceApplicationApprovalStatusManager = this.container.GetInstance<ComplianceApplicationApprovalStatusManager>();
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var notApplicableManager = this.container.GetInstance<ApplicationQuestionNotApplicableManager>();
            var applicationVersionManager = this.container.GetInstance<ApplicationVersionManager>();
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var questionManager = this.container.GetInstance<ApplicationSectionQuestionManager>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            var approvalStatus = complianceApplicationApprovalStatusManager.GetByName(model.ApprovalStatus?.Name ?? Constants.ComplianceApplicationApprovalStatuses.Planning);
            var status = applicationStatusManager.GetByName(Constants.ApplicationStatus.InProgress);
            var cancelledStatus = applicationStatusManager.GetByName(Constants.ApplicationStatus.Cancelled);

            


            //var sections = applicationSectionManager.GetAllForActiveVersionsNoLazyLoad();
            //var questions = questionManager.GetAllForActiveVersionsNoLazyLoad();
            //var notApplicables = notApplicableManager.GetAllForActiveVersionNoLazyLoad();
            //var activeVersions = applicationVersionManager.GetActiveVersions(sections, questions, notApplicables);

            var activeVersions = ActiveVersions.Select(version => version.Value).ToList();
            
            var org = organizationManager.GetById(model.OrganizationId);

            var cycle = org.DocumentLibraries.FirstOrDefault(x => x.IsCurrent);

            var cycleNumber = (cycle?.CycleNumber ?? 1);

            if (cycleNumber == 0) cycleNumber = 1;

            if (!model.Id.HasValue)
            {
                return complianceApplicationManager.Add(cycleNumber, activeVersions, approvalStatus, status, model, savedBy);
            }
            else
            {
                notApplicableManager.RemoveForCompliance(model.Id.GetValueOrDefault(), savedBy);
                return complianceApplicationManager.Update(cancelledStatus, activeVersions, approvalStatus, status, model, cycleNumber, savedBy);
            }

        }

        public Application CopyComplianceApplication(CopyComplianceApplicationModel model, string updatedBy)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var siteManager = this.container.GetInstance<SiteManager>();

            var applicationStatus = applicationStatusManager.GetByName(model.ApplicationStatus);
            var applicationType = applicationTypeManager.GetByName(model.ApplicationType);
            var complianceApplication = complianceApplicationManager.GetById(model.ComplianceApplicationId);
            var site = siteManager.GetByName(model.CopyToSite);

            if (complianceApplication == null)
            {
                throw new Exception("Cannot find Compliance Application");
            }

            if (applicationStatus == null)
            {
                throw new Exception($"Cannot find Application Status {model.ApplicationStatus}");
            }

            if (applicationType == null)
            {
                throw new Exception($"Cannot find app$lication type {model.ApplicationType}");
            }

            if (site == null)
            {
                throw new Exception($"Cannot find site {model.CopyToSite}");
            }

            var copyFromApplication =
                complianceApplication.Applications.SingleOrDefault(
                    x => x.Site.Name == model.CopyFromSite && x.ApplicationTypeId == applicationType.Id);

            if (copyFromApplication == null)
            {
                throw new Exception($"Cannot find application for site {model.CopyFromSite} with application type {model.ApplicationType}");
            }

            var application = new Application
            {
                ApplicationTypeId = copyFromApplication.ApplicationTypeId,
                OrganizationId = copyFromApplication.OrganizationId,
                CreatedDate = DateTime.Now,
                CreatedBy = updatedBy,
                ApplicationStatusId = applicationStatus.Id,
                ComplianceApplicationId = copyFromApplication.ComplianceApplicationId,
                SubmittedDate = copyFromApplication.SubmittedDate,
                DueDate = copyFromApplication.DueDate,
                UniqueId = Guid.NewGuid(),
                CoordinatorId = copyFromApplication.CoordinatorId,
                ApplicationVersionId = copyFromApplication.ApplicationVersionId,
                SiteId = site.Id,
                RFIDueDate = copyFromApplication.RFIDueDate,
                CycleNumber = copyFromApplication.CycleNumber,
                IsActive = true,
                ApplicationResponsesTrainee =
                    copyFromApplication.ApplicationResponsesTrainee.Select(x => new ApplicationResponseTrainee
                    {
                        ApplicationResponseStatusId = x.ApplicationResponseStatusId,
                        CreatedBy = updatedBy,
                        CreatedDate = DateTime.Now,
                        ApplicationSectionQuestionId = x.ApplicationSectionQuestionId,
                        ApplicationSectionQuestionAnswerId = x.ApplicationSectionQuestionAnswerId
                    }).ToList(),
                ApplicationResponseComments =
                    copyFromApplication.ApplicationResponseComments.Select(x => new ApplicationResponseComment
                    {
                        FromUser = x.FromUser,
                        ToUser = x.ToUser,
                        DocumentId = x.DocumentId,
                        CreatedDate = DateTime.Now,
                        CreatedBy = updatedBy,
                        QuestionId = x.QuestionId,
                        Comment = x.Comment,
                        CommentTypeId = x.CommentTypeId,
                        CommentOverride = x.CommentOverride,
                        OverridenBy = x.OverridenBy,
                        VisibleToApplicant = x.VisibleToApplicant
                    }).ToList(),
                ApplicationResponses = copyFromApplication.ApplicationResponses.Select(x => new ApplicationResponse
                {
                    ApplicationSectionQuestionId = x.ApplicationSectionQuestionId,
                    ApplicationSectionQuestionAnswerId = x.ApplicationSectionQuestionAnswerId,
                    Text = x.Text,
                    CreatedDate = DateTime.Now,
                    CreatedBy = updatedBy,
                    Flag = x.Flag,
                    Comments = x.Comments,
                    DocumentId = x.DocumentId,
                    Document = x.Document,
                    ApplicationResponseStatusId = x.ApplicationResponseStatusId,
                    UserId = x.UserId,
                    CoorindatorComment = x.CoorindatorComment,
                    VisibleApplicationResponseStatusId = x.VisibleApplicationResponseStatusId
                }).ToList(),
                ApplicationQuestionNotApplicables =
                    copyFromApplication.ApplicationQuestionNotApplicables.Select(
                        x => new ApplicationQuestionNotApplicable
                        {
                            Id = Guid.NewGuid(),
                            ApplicationSectionQuestionId = x.ApplicationSectionQuestionId,
                            CreatedBy = updatedBy,
                            CreatedDate = DateTime.Now
                        }).ToList()
            };

            applicationManager.Add(application);

            if (model.DeleteOriginal)
            {
                this.Deactivate(copyFromApplication.UniqueId, updatedBy);
            }

            return application;
        }

        /// <summary>
        /// Get inspection status against a user and provided application unique Id
        /// </summary>
        /// <param name="applicationUniqueId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> GetInspectionCompletionStatus(Guid applicationUniqueId, Guid userId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();

            var details = await inspectionScheduleDetailManager.GetAllActiveByApplicationAsync(applicationUniqueId);

            var detail = details.FirstOrDefault(x => x.UserId == userId);

            if (detail == null)
            {
                throw new Exception("Cannot find user associated with application");
            }

            return detail.IsInspectionComplete ?? false;
        }

        private void SendPendingApproval(string url, Guid? eligibilityAppUniqueId, ComplianceApplication complianceApplication, string appType)
        {
            if (complianceApplication == null)
            {
                throw new NullReferenceException("Compliance Application cannot be null");
            }

            var facilityDirectors = complianceApplication.Organization
                .OrganizationFacilities
                .Where(x => x.Facility.FacilityDirectorId.HasValue)
                .Select(orgFacility => orgFacility.Facility.FacilityDirector)
                .ToList();

            var tos = complianceApplication.Organization.Users.Where(
                x => x.JobFunctionId != Constants.JobFunctionIds.OrganizationDirector).Select(x=>x.User.EmailAddress).ToList();

            if (tos.Count == 0)
            {
                throw new Exception($"Organization {complianceApplication.Organization.Name} does not have a director assigned.");
            }
            
            var ccs = facilityDirectors.Select(facDirector => facDirector.EmailAddress).ToList();

            var primaryContact = complianceApplication.Organization.PrimaryUser?.EmailAddress;

            if (primaryContact != null)
            {
                ccs.Add(primaryContact);
            }

            if (!string.IsNullOrEmpty(complianceApplication.Organization.CcEmailAddresses))
            {
                var add = complianceApplication.Organization.CcEmailAddresses.Split(';');

                ccs.AddRange(add.ToList());
            }

            var appTypes = string.Empty;
            var types = new HashSet<string>();
            var includedSites = new HashSet<string>();
            var sites = string.Empty;

            DateTime? dueDate = null;

            foreach (var app in complianceApplication.Applications.Where(x=>x.ApplicationStatusId != (int)Constants.ApplicationStatuses.Cancelled)) 
            {
                if (app.DueDate.HasValue && !dueDate.HasValue)
                {
                    dueDate = app.DueDate;
                }

                var site = app.Site.Name + ": ";

                if (!types.Contains(app.ApplicationType.Name))
                {
                    appTypes += app.ApplicationType.Name + "<br>";
                    types.Add(app.ApplicationType.Name);
                }

                if (app.SiteId.HasValue && !includedSites.Contains(app.Site.Name))
                {
                    var fac = app.Site.FacilitySites.FirstOrDefault();

                    if (fac == null) continue;

                    var serviceType = fac.Facility.ServiceType.Name;

                    if (serviceType.IndexOf("CB", StringComparison.Ordinal) > -1)
                    {
                        if (app.Site.CBCollectionType != null)
                        {
                            site += app.Site.CBCollectionType.Name;
                        }
                        if (app.Site.CBUnitTypeId.HasValue)
                        {
                            if (!site.EndsWith(": "))
                            {
                                site += ", ";
                            }

                            site += app.Site.CBUnitType.Name;
                        }
                    }
                    else if (serviceType.IndexOf("Clinical", StringComparison.Ordinal) > -1)
                    {
                        if (!site.EndsWith(": "))
                        {
                            site += ", ";
                        }
                        
                        foreach (var tt in app.Site.SiteTransplantTypes)
                        {
                            site += tt.TransplantType.Name + "; ";
                        }

                        if (app.Site.SiteTransplantTypes.Count > 0)
                        {
                            site = site.Substring(0, site.Length - 2);
                        }
                    }
                    else if (serviceType.IndexOf("Processing", StringComparison.Ordinal) > -1)
                    {
                        if (!site.EndsWith(": "))
                        {
                            site += ", ";
                        }

                        foreach (var tt in app.Site.SiteProcessingTypes)
                        {
                            site += tt.ProcessingType.Name + "; ";
                        }

                        if (app.Site.SiteProcessingTypes.Count > 0)
                        {
                            site = site.Substring(0, site.Length - 2);
                        }
                    }
                    else if (serviceType.IndexOf("Collection", StringComparison.Ordinal) > -1)
                    {
                        if (app.Site.CollectionProductTypeId.HasValue)
                        {
                            if (!site.EndsWith(": "))
                            {
                                site += ", ";
                            }

                            site += app.Site.CollectionProductType.Name;
                        }
                    }
                }

                if (!sites.Contains(site))
                {
                    sites += "<p>" + site + "</p>";
                }
                
            }

            var coordinator = complianceApplication.Coordinator;

            var creds = string.Empty;
            foreach (var cred in coordinator.UserCredentials)
            {
                creds += cred.Credential.Name + ", ";
            }

            if (creds.Length > 2)
            {
                creds = creds.Substring(0, creds.Length - 2);
            }

            var reminderHtml = url + "app/email.templates/complianceApproval.html";
            var appUrl = string.Format("{0}{1}", url, "#/Application?app=" + eligibilityAppUniqueId);
            var approveUrl = string.Format("{0}#/Compliance/ManageCompliance?app={1}&sub={2}", url,
                complianceApplication.Applications.First().UniqueId, complianceApplication.Id);

            var html = WebHelper.GetHtml(reminderHtml);
            html = html.Replace("{ApplicationUrl}", appUrl);
            html = html.Replace("{OrgName}", complianceApplication.Organization.Name);
            html = html.Replace("{DueDate}", dueDate?.ToString("d") ?? "");
            html = html.Replace("{ApplicationTypeName}", appTypes);
            html = html.Replace("{AccreditationGoal}", complianceApplication.AccreditationGoal);
            html = html.Replace("{InspectionScope}", complianceApplication.InspectionScope);
            html = html.Replace("{Sites}", sites);
            html = html.Replace("{ApproveUrl}", approveUrl);

            html = html.Replace("{CoordinatorName}", $"{coordinator.FirstName} {coordinator.LastName}, {creds}");
            html = html.Replace("{CoordinatorTitle}", coordinator.Title);
            html = html.Replace("{CoordinatorPhoneNumber}", coordinator.WorkPhoneNumber);
            html = html.Replace("{CoordinatorEmailAddress}", coordinator.EmailAddress);
            html = html.Replace("{AppType}", appType);

            ccs.Add("portal@factwebsite.org");
            ccs.Add(coordinator.EmailAddress);

            EmailHelper.Send(tos, ccs, "FACT " + appType + $" Approved [Action Requested] - {complianceApplication.Organization.Name}", html);
        }

        private void SendComplianceApproved(string url, ComplianceApplication complianceApplication)
        {
            var first = complianceApplication.Applications.First();
            var coordinator = complianceApplication.Coordinator;
            //var orgDirector = complianceApplication.Organization.OrganizationDirector;
            var primaryContact = complianceApplication.Organization.PrimaryUserId.HasValue
                ? complianceApplication.Organization.PrimaryUser.EmailAddress
                : "";
            var ccs = new List<string>();

            var creds = string.Empty;
            foreach (var cred in coordinator.UserCredentials)
            {
                creds += cred.Credential.Name + ", ";
            }

            if (creds.Length > 2)
            {
                creds = creds.Substring(0, creds.Length - 2);
            }

            var reminderHtml = url + "app/email.templates/complianceApplicationApproved.html";
            var appUrl = $"{url}#/Compliance?app={first?.UniqueId}&c={complianceApplication.Id}&ver={first?.ApplicationVersion?.Title}";

            var html = WebHelper.GetHtml(reminderHtml);
            html = html.Replace("{OrgName}", complianceApplication.Organization.Name);
            html = html.Replace("{DueDate}", first != null && first.DueDate.HasValue ? first?.DueDate.Value.ToString("d") : "");
            html = html.Replace("{URL}", appUrl);

            html = html.Replace("{CoordinatorName}", $"{coordinator.FirstName} {coordinator.LastName}, {creds}");
            html = html.Replace("{CoordinatorTitle}", coordinator.Title);
            html = html.Replace("{CoordinatorPhoneNumber}", coordinator.WorkPhoneNumber);
            html = html.Replace("{CoordinatorEmailAddress}", coordinator.EmailAddress);

            var tos = complianceApplication.Organization.Users.Where(
                x => x.JobFunctionId != Constants.JobFunctionIds.OrganizationDirector).Select(x => x.User.EmailAddress)
                .ToList();

            tos.Add(primaryContact);

            if (!string.IsNullOrEmpty(complianceApplication.Organization.CcEmailAddresses))
            {
                var add = complianceApplication.Organization.CcEmailAddresses.Split(';');

                ccs.AddRange(add.ToList());
            }

            ccs.Add("portal@factwebsite.org");
            ccs.Add(coordinator.EmailAddress);

            EmailHelper.Send(tos, ccs, $"FACT Compliance Application is Available for {complianceApplication.Organization.Name}", html);
        }

        public void SetComplianceApprovalStatus(string url, string approvalStatusName, Guid complianceApplicationId, string rejectionComments, string serialNumber, string savedBy, Guid userId)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var complianceApplicationApprovalStatusManager =
                this.container.GetInstance<ComplianceApplicationApprovalStatusManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();

            ApplicationStatus rfiStatus = null;

            var compApp = complianceApplicationManager.GetById(complianceApplicationId);

            if (compApp == null)
            {
                throw new Exception("Cannot find compliance application");
            }

            if (!string.IsNullOrEmpty(serialNumber))
            {
                //var k2Manager = this.container.GetInstance<K2NotificationManager>();
                var compAppApprovalManager = this.container.GetInstance<ComplianceApplicationSubmitApprovalManager>();

                //if (approvalStatusName == Constants.ComplianceApplicationApprovalStatuses.Approved)
                //{
                //    k2Manager.SetApproval(serialNumber);

                //}
                //else
                //{
                //    k2Manager.SetDecline(serialNumber);
                //}

                compAppApprovalManager.MarkUserAsApproved(complianceApplicationId, userId, approvalStatusName == Constants.ComplianceApplicationApprovalStatuses.Approved, savedBy);

                var approvals = compAppApprovalManager.GetByCompliance(complianceApplicationId);

                if (approvals == null || !approvals.All(x => x.IsApproved)) return;


                var staffEmailList = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.StaffEmailList];
                var factPortalEmailAddress = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.FactPortalEmailAddress];

                this.ChangeComplainceApplicationStatus(url, staffEmailList, factPortalEmailAddress, complianceApplicationId, Constants.ApplicationStatus.ForReview, savedBy, true);

                var apps = applicationManager.GetAllByOrganizationOrApplicationType(compApp.OrganizationId, (int)Constants.ApplicationType.RenewalApplication);

                foreach (var app in apps.Where(x=>x != null))
                {
                    app.ApplicationStatusId = (int) Constants.ApplicationStatuses.Complete;
                    app.IsActive = false;
                    app.UpdatedBy = savedBy;
                    app.UpdatedDate = DateTime.Now;

                    applicationManager.Save(app);
                }
            }
            else
            {
                //Per bug 1910, we want to set the status to RFI if its an add on
                if (approvalStatusName == Constants.ComplianceApplicationApprovalStatuses.Approved && compApp.ApplicationStatus.Name != Constants.ApplicationStatus.InProgress)
                {
                    rfiStatus = applicationStatusManager.GetByName(Constants.ApplicationStatus.RFI);
                }

                var renewalApp =
                    applicationManager.GetAllByOrganizationOrApplicationType(compApp.OrganizationId,
                        (int)Constants.ApplicationType.RenewalApplication).SingleOrDefault();
                var eligibilityApp = applicationManager.GetAllByOrganizationOrApplicationType(compApp.OrganizationId, (int)Constants.ApplicationType.EligibilityApplication).SingleOrDefault();

                var approvalStatus = complianceApplicationApprovalStatusManager.GetByName(approvalStatusName);

                complianceApplicationManager.SetComplianceApprovalStatus(rfiStatus, approvalStatus, complianceApplicationId, rejectionComments, savedBy);

                switch (approvalStatusName)
                {
                    case Constants.ComplianceApplicationApprovalStatuses.Submitted:
                        Guid? appId = null;
                        var type = Constants.ApplicationTypes.Eligibility;

                        var appStatus =
                            applicationStatusManager.GetByName(Constants.ApplicationStatus.AwaitingDirectorApproval);

                        if (renewalApp != null)
                        {
                            appId = renewalApp.UniqueId;
                            type = Constants.ApplicationTypes.Renewal;
                            renewalApp.ApplicationStatusId = appStatus.Id;
                            renewalApp.UpdatedBy = savedBy;
                            renewalApp.UpdatedDate = DateTime.Now;
                            applicationManager.Save(renewalApp);
                        }
                        else if (eligibilityApp != null)
                        {
                            appId = eligibilityApp.UniqueId;

                            eligibilityApp.ApplicationStatusId = appStatus.Id;
                            eligibilityApp.UpdatedBy = savedBy;
                            eligibilityApp.UpdatedDate = DateTime.Now;
                            applicationManager.Save(eligibilityApp);
                        }

                        this.SendPendingApproval(url, appId, compApp, type);


                        break;
                    case Constants.ComplianceApplicationApprovalStatuses.Approved:
                        this.SendComplianceApproved(url, compApp);

                        //Per Bug 2352 this shouldnt be sent here
                        var completeAppStatus =
                            applicationStatusManager.GetByName(Constants.ApplicationStatus.Complete);

                        if (renewalApp != null)
                        {
                           // this.SendPendingApproval(url, renewalApp.UniqueId, compApp, Constants.ApplicationTypes.Renewal);
                            renewalApp.ApplicationStatusId = completeAppStatus.Id;
                            renewalApp.UpdatedBy = savedBy;
                            renewalApp.UpdatedDate = DateTime.Now;
                            applicationManager.Save(renewalApp);
                        }
                        else if (eligibilityApp != null)
                        {
                            //this.SendPendingApproval(url, eligibilityApp.UniqueId, compApp, Constants.ApplicationTypes.Eligibility);
                            eligibilityApp.ApplicationStatusId = completeAppStatus.Id;
                            eligibilityApp.UpdatedBy = savedBy;
                            eligibilityApp.UpdatedDate = DateTime.Now;
                            applicationManager.Save(eligibilityApp);
                        }

                        complianceApplicationManager.CreateComplianceApplicationSections(complianceApplicationId);

                        break;
                    case Constants.ComplianceApplicationApprovalStatuses.Reject:
                        var reviewStatus =
                            applicationStatusManager.GetByName(Constants.ApplicationStatus.ForReview);

                        if (renewalApp != null)
                        {
                            //this.SendPendingApproval(url, renewalApp.UniqueId, compApp, Constants.ApplicationTypes.Renewal);
                            renewalApp.ApplicationStatusId = reviewStatus.Id;
                            renewalApp.UpdatedBy = savedBy;
                            renewalApp.UpdatedDate = DateTime.Now;
                            applicationManager.Save(renewalApp);
                        }
                        else if (eligibilityApp != null)
                        {
                            //this.SendPendingApproval(url, eligibilityApp.UniqueId, compApp, Constants.ApplicationTypes.Eligibility);
                            eligibilityApp.ApplicationStatusId = reviewStatus.Id;
                            eligibilityApp.UpdatedBy = savedBy;
                            eligibilityApp.UpdatedDate = DateTime.Now;
                            applicationManager.Save(eligibilityApp);
                        }

                        this.ComplianceApplicationRejectedEmail(url, rejectionComments, compApp);
                        break;
                }
            }
        }

        public void SetEligibilityApplicationApprovalStatus(ComplianceApplicationItem model, string savedBy)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            applicationManager.SetEligibilityApplicationApprovalStatus(model.Id, savedBy);
        }

        /// <summary>
        /// Sends an email to coordinator that he/she is assigned to that compliance application
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        public void EligibilityApplicationApprovedEmail(ComplianceApplicationItem model, string url, string type)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var complianceApplication = complianceApplicationManager.GetById((Guid)model.Id);

            string coordicatorEmail = string.Empty;
            string organizationName = string.Empty;

            if (complianceApplication != null)
                coordicatorEmail = complianceApplication.Coordinator.EmailAddress;
            else
                throw new ObjectNotFoundException(string.Format("Cannot find Compliance Application Id {0}", model.Id.Value));

            organizationName = complianceApplication.Organization != null ? complianceApplication.Organization.Name : string.Empty;

            applicationManager.EligibilityApplicationApprovedEmail(url, coordicatorEmail, model.Id.Value, organizationName, type);
        }

        public void NotifyApplicationAboutRFI(string applicantId)
        {
            EmailHelper.Send(applicantId, "RFI Received", "There is an RFI(s) against your Application", true);
        }

        /// <summary>
        /// Saves appliation due date
        /// </summary>
        /// <param name="application"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task SaveApplication(ApplicationItem application, string email)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var app = applicationManager.GetById(application.ApplicationId);

            if (app.ComplianceApplicationId.HasValue)
            {
                foreach (var appl in app.ComplianceApplication.Applications)
                {
                    appl.DueDate = application.DueDate;
                    appl.RFIDueDate = string.IsNullOrEmpty(application.RFIDueDate) ? (DateTime?)null : System.Convert.ToDateTime(application.RFIDueDate);
                    appl.UpdatedBy = email;
                    appl.UpdatedDate = DateTime.Now;

                    await applicationManager.SaveAsync(appl);
                }
            }
            else
            {
                app.DueDate = application.DueDate;
                app.RFIDueDate = string.IsNullOrEmpty(application.RFIDueDate) ? (DateTime?)null : System.Convert.ToDateTime(application.RFIDueDate);
                app.UpdatedBy = email;
                app.UpdatedDate = DateTime.Now;

                await applicationManager.SaveAsync(app);
            }

        }

        //public async Task SaveApplicationAsync(string orgName, string applicationTypeName,
        //    List<ApplicationSectionItem> sections, string updatedBy, int? roleId)
        //{
        //    var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
        //    var applicationManager = this.container.GetInstance<ApplicationManager>();
        //    var orgManager = this.container.GetInstance<OrganizationManager>();

        //    var applicationType = await applicationTypeManager.GetByNameAsync(applicationTypeName);
        //    var org = await orgManager.GetByNameAsync(orgName);

        //    if (applicationType == null || org == null)
        //    {
        //        throw new ObjectNotFoundException("Cannot find data");
        //    }

        //    await
        //        applicationManager.SaveApplicationAsync(org.Id, applicationType.Id,
        //            sections, updatedBy, roleId);
        //}

        public void SaveMultiViewSections(Guid appUniqueId,
            List<ApplicationSectionResponse> sections, string updatedBy, int? roleId)
        {
            foreach (var section in sections)
            {
                this.SaveApplicationSection(appUniqueId, section, updatedBy, roleId);
            }
        }

        public void SaveApplicationSection(Guid applicationUniqueId,
            ApplicationSectionResponse section, string updatedBy, int? roleId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();
            var documentManager = this.container.GetInstance<DocumentManager>();
            var associationTypeManager = this.container.GetInstance<AssociationTypeManager>();
            var applicationResponseStatusManager = this.container.GetInstance<ApplicationResponseStatusManager>();
            var compApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var commentManager = this.container.GetInstance<ApplicationResponseCommentManager>();

            ComplianceApplication compApp = null;

            var association = associationTypeManager.GetByName(Constants.AssociationTypes.Evidence);
            var rfiComplete =
                applicationResponseStatusManager.GetStatusByName(Constants.ApplicationResponseStatus.RFICompleted);
            var rfiStatus = applicationResponseStatusManager.GetStatusByName(Constants.ApplicationResponseStatus.RFI);

            applicationResponseManager.AddToHistory(applicationUniqueId, section.ApplicationSectionId);
            applicationResponseManager.Remove(applicationUniqueId, section);

            var app = applicationManager.GetByUniqueId(applicationUniqueId);

            if (app.ComplianceApplicationId.HasValue)
            {
                compApp = compApplicationManager.GetById(app.ComplianceApplicationId.Value);
            }

            var isNonInspector = true;

            var userManager = this.container.GetInstance<UserManager>();

            var users = userManager.GetByOrganization(app.OrganizationId) ?? new List<User>();

            if (!app.ComplianceApplicationId.HasValue)
            {
                isNonInspector = users.Any(x => x.EmailAddress == updatedBy);
            }

            var responses = applicationManager.GetApplicationResponsesForSection(commentManager, section, updatedBy, roleId, compApp, rfiComplete, app, rfiStatus, isNonInspector, users);

            applicationManager.SaveApplicationSection(applicationUniqueId, responses, updatedBy, roleId);

            if (association == null) return;

            foreach (var response in responses.Where(x=>x.DocumentId != null))
            {
                documentManager.UpdateDocumentAssociations(response.DocumentId.GetValueOrDefault(), association, updatedBy);
            }

            if (compApp != null)
            {
                compApplicationManager.CreateComplianceApplicationSections(compApp.Id);
            }
            

        }

        public void UpdateAnswerResponseStatus(List<ApplicationSectionResponse> applicationSectionItem, string updatedBy)
        {
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();
            applicationResponseManager.UpdateAnswerResponseStatus(applicationSectionItem, updatedBy);
        }
        public void UpdateSection(List<ApplicationSectionItem> applicationSectionItem, string updatedBy)
        {
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();
            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();

            // applicationResponseManager.UpdateSection(applicationSectionItem, updatedBy);

            foreach (var section in applicationSectionItem)
            {
                foreach (var question in section.Questions)
                {
                    foreach (var questionResponse in question.QuestionResponses)
                    {
                        if (question.AnswerResponseStatusId == 0)
                            continue;

                        var response = applicationResponseManager.GetById(questionResponse.Id);
                        response.ApplicationResponseStatusId = question.AnswerResponseStatusId;
                        response.CoorindatorComment = questionResponse.CoordinatorComment;

                        applicationResponseManager.Save(response);
                    }

                    foreach (var responseComment in question.ApplicationResponseComments)
                    {
                        var comment = applicationResponseCommentManager.GetById(responseComment.ApplicationResponseCommentId);

                        comment.Comment = responseComment.Comment;

                        if (comment.CommentOverride != responseComment.CommentOverride)
                        {
                            comment.CommentOverride = responseComment.CommentOverride;
                            comment.OverridenBy = updatedBy;
                        }

                        comment.IncludeInReporting = responseComment.IncludeInReporting;
                        comment.VisibleToApplicant = responseComment.VisibleToApplicant;

                        applicationResponseCommentManager.Save(comment);
                    }
                }
            }
        }

        public void SaveApplicationSectionTrainee(Guid appUniqueId, ApplicationSectionResponse section, string updatedBy, int? roleId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var applicationResponseTraineeManager = this.container.GetInstance<ApplicationResponseTraineeManager>();


            applicationResponseTraineeManager.Remove(appUniqueId, section);

            //var responses = applicationManager.GetApplicationResponsesTraineeForSection(section, updatedBy, roleId);

            applicationManager.SaveApplicationSectionTrainee(appUniqueId, section, updatedBy, roleId);

        }

        public void ChangeComplainceApplicationStatus(string url, string staffEmailList, string factPortalEmailAddress, Guid complianceApplicationId, string applicationStatusName, string updatedBy, bool ignoreStatus)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var complianceApplication = complianceApplicationManager.GetById(complianceApplicationId);

            if (!ignoreStatus && applicationStatusName == Constants.ApplicationStatus.ForReview &&
                complianceApplication.ApplicationStatus.Name == Constants.ApplicationStatus.InProgress)
            {
                var k2Manager = this.container.GetInstance<K2NotificationManager>();

                var user = userManager.GetByEmailAddress(updatedBy);

                var userName = string.Empty;

                if (user == null)
                {
                    userName = updatedBy;
                }
                else
                {
                    userName = $"{user.FirstName} {user.LastName}";
                }

                k2Manager.SetDataFieldsStartProcess(null, complianceApplicationId.ToString(), complianceApplication.Organization.Name, "FACT Web\\Compliance Application Facility Approval v3", userName);

                var complianceApplicationSubmitApprovalManager =
                    this.container.GetInstance<ComplianceApplicationSubmitApprovalManager>();

                complianceApplicationSubmitApprovalManager.CreateApprovals(complianceApplicationId, updatedBy);
            }
            else
            {
                complianceApplicationManager.UpdateComplianceApplicationStatus(complianceApplicationId,
                applicationStatusName, updatedBy);

                var apps = applicationManager.GetByComplianceApplicationId(complianceApplicationId);

                foreach (var application in apps)
                {
                    this.ChangeApplicationStatus(url, staffEmailList, factPortalEmailAddress, application, applicationStatusName, updatedBy, false);
                }

                //if (applicationStatusName == Constants.ApplicationStatus.ForReview)
                //{
                //    HostingEnvironment.QueueBackgroundWorkItem(x => this.ProcessApplicationExpectedAndHiddens(complianceApplicationId, updatedBy));
                //}

                if (applicationStatusName == Constants.ApplicationStatus.ForReview || applicationStatusName == Constants.ApplicationStatus.RFIReview || complianceApplication.ApplicationStatus.Name == Constants.ApplicationStatus.RFI || complianceApplication.ApplicationStatus.Name == Constants.ApplicationStatus.ApplicantResponse)
                {

                    var to = staffEmailList;
                    if (complianceApplication.Coordinator != null && !String.IsNullOrEmpty(complianceApplication.Coordinator.EmailAddress))
                    {
                        to += "," + complianceApplication.Coordinator.EmailAddress;
                    }

                    var app = complianceApplication.Applications.FirstOrDefault();

                    var allApps = applicationManager.GetAllByOrganizationOrApplicationType(app?.OrganizationId ?? 0, null);

                    var appType = Constants.ApplicationTypes.Eligibility;

                    if (allApps.Any(x => x.ApplicationType.Name == Constants.ApplicationTypes.Renewal))
                    {
                        appType = Constants.ApplicationTypes.Renewal;
                    }

                    this.EligibilityApplicationReceivedEmail(url, complianceApplication.Organization.Name, to, factPortalEmailAddress, app?.UniqueId.ToString() ?? string.Empty, complianceApplication.Coordinator ?? new User(), appType, true, complianceApplication.Id);
                    this.ComplianceApplicationSubmittedEmail(url, updatedBy, factPortalEmailAddress, complianceApplication);
                }
            }
        }

        public async Task ProcessAnnualReport(string url, string staffEmailList, string factPortalEmailAddress, string orgName, Guid userId, string updatedBy)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var applicationSubmitApprovalManager = this.container.GetInstance<ApplicationSubmitApprovalManager>();
            var emailTemplateManager = this.container.GetInstance<EmailTemplateManager>();

            var applicationTypeTask = applicationTypeManager.GetByNameAsync(Constants.ApplicationTypes.Annual);
            var applicationStatusTask = applicationStatusManager.GetByNameAsync(Constants.ApplicationStatus.ForReview);
            var org = orgManager.GetByName(orgName);

            await Task.WhenAll(applicationTypeTask, applicationStatusTask);

            var application = applicationManager.GetAllByOrganizationOrApplicationType(org.Id, applicationTypeTask.Result.Id).FirstOrDefault();

            if (application == null)
            {
                throw new Exception("Cannot find application");
            }

            var user = userManager.GetById(userId);

            if (user == null)
            {
                throw new Exception("Cannot find user");
            }

            if (org.OrganizationConsutants.Any(x => x.ConsultantId == user.Id))
            {
                throw new Exception("Consultants cannot submit applications");
            }

            if (application.ApplicationStatus.Name == Constants.ApplicationStatus.InProgress)
            {
                var approvals = applicationSubmitApprovalManager.GetAllForApplication(application.Id);

                if (approvals.Count > 0)
                {
                    var approval = approvals.SingleOrDefault(x => x.UserId == user.Id);

                    if (approval != null)
                    {
                        approval.IsApproved = true;
                        approval.UpdatedBy = updatedBy;
                        approval.UpdatedDate = DateTime.Now;

                        applicationSubmitApprovalManager.Save(approval);
                    }

                    if (approvals.All(x => x.IsApproved))
                    {
                        this.ProcessAppExpectedAndHidden(application.UniqueId, updatedBy);

                        await applicationManager.UpdateApplicationStatusAsync(application, org.Id,
                            applicationTypeTask.Result.Id, applicationStatusTask.Result.Id,
                            Constants.ApplicationStatus.ForReview, null, updatedBy);

                        this.SendApplicationReceivedEmail(url, staffEmailList, factPortalEmailAddress, orgName,
                            Constants.ApplicationTypes.Annual, application, org);
                    }
                }
                else
                {
                    try
                    {
                        applicationManager.CreateApprovals(application.Id, updatedBy);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("UserId"))
                        {
                            throw new Exception("One or more Facility Director(s) are missing.  Please contact your coordinator.");
                        }

                        throw;
                    }
                    

                    var content = emailTemplateManager.GetContent(application.Id, null, "Facility Compliance Approval",
                        null, null, null);

                    var ccs = new List<string>
                    {
                        factPortalEmailAddress
                    };

                    if (!string.IsNullOrEmpty(org.CcEmailAddresses))
                    {
                        ccs.Add(org.CcEmailAddresses);
                    }

                    if (org.PrimaryUserId.HasValue)
                    {
                        var pc = userManager.GetById(org.PrimaryUserId.Value);

                        if (pc != null)
                        {
                            ccs.Add(pc.EmailAddress);
                        }
                    }

                    if (application.CoordinatorId.HasValue)
                    {
                        ccs.Add(application.Coordinator.EmailAddress);
                    }

                    approvals = applicationSubmitApprovalManager.GetAllForApplication(application.Id);

                    var tos = approvals.Select(approval => approval.User.EmailAddress).ToList();

                    EmailHelper.Send(tos, ccs, $"FACT Annual Report Ready for Review - {application.Organization.Name}", content.Body);
                }
            }
            else
            {
                await applicationManager.UpdateApplicationStatusAsync(application, org.Id,
                            applicationTypeTask.Result.Id, applicationStatusTask.Result.Id,
                            Constants.ApplicationStatus.ForReview, null, updatedBy);

                this.SendApplicationReceivedEmail(url, staffEmailList, factPortalEmailAddress, orgName,
                    Constants.ApplicationTypes.Annual, application, org);
            }
        }

        public async Task<Application> ChangeApplicationStatusAsync(string url, string staffEmailList, string factPortalEmailAddress, string orgName, string applicationTypeName,
            string applicationStatusName, string updatedBy)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var applicationTypeTask = applicationTypeManager.GetByNameAsync(applicationTypeName);
            var applicationStatusTask = applicationStatusManager.GetByNameAsync(applicationStatusName);
            var org = orgManager.GetByName(orgName);

            await Task.WhenAll(applicationTypeTask, applicationStatusTask);

            var application = applicationManager.GetAllByOrganizationOrApplicationType(org.Id, applicationTypeTask.Result.Id).FirstOrDefault();

            var user = userManager.GetByEmailAddress(updatedBy);

            if (user == null)
            {
                throw new Exception("Cannot find user");
            }

            if (org.OrganizationConsutants.Any(x => x.ConsultantId == user.Id))
            {
                throw new Exception("Consultants cannot submit applications");
            }

            if (application != null &&
                applicationStatusName == Constants.ApplicationStatus.ForReview)
            {
                this.log.Info($"Processing App Expected and Hidden. AppId: {application.Id}; AppStatus: {application.ApplicationStatus.Name}; New Status: {applicationStatusName} ");
                this.ProcessAppExpectedAndHidden(application.UniqueId, updatedBy);
            }
            else
            {
                this.log.Info($"Not processing Expecteds: {application?.UniqueId} -- {application?.ApplicationStatus?.Name}");
            }

            if (application?.ComplianceApplicationId != null)
            {
                foreach (var app in application.ComplianceApplication.Applications)
                {
                    await applicationManager.UpdateApplicationStatusAsync(app, org.Id, applicationTypeTask.Result.Id, applicationStatusTask.Result.Id, applicationStatusName, null, updatedBy);
                }
            }
            else
            {
                await applicationManager.UpdateApplicationStatusAsync(application, org.Id, applicationTypeTask.Result.Id, applicationStatusTask.Result.Id, applicationStatusName, null, updatedBy);
            }

            if (applicationTypeName == Constants.ApplicationTypes.Eligibility || applicationTypeName == Constants.ApplicationTypes.Renewal || applicationTypeName == Constants.ApplicationTypes.Annual)
            {
                this.SendApplicationReceivedEmail(url, staffEmailList, factPortalEmailAddress, orgName,
                    applicationTypeName, application, org);
            }
            else if (application != null && application.ComplianceApplicationId.HasValue && applicationStatusName == Constants.ApplicationStatus.ForReview)
            {
                var to = staffEmailList;
                if (application.Coordinator != null && !String.IsNullOrEmpty(application.Coordinator.EmailAddress))
                {
                    to += "," + application.Coordinator.EmailAddress;
                }

                this.EligibilityApplicationReceivedEmail(url, orgName, to, factPortalEmailAddress, application.UniqueId.ToString(), application.Coordinator ?? new User(), applicationTypeName, true, application.ComplianceApplicationId);
            }

            return application;

        }

        private void SendApplicationReceivedEmail(string url, string staffEmailList, string factPortalEmailAddress, string orgName, string applicationTypeName, Application application, Organization org)
        {
            var userManager = this.container.GetInstance<UserManager>();

            var subTo = new List<string>();
            var subCc = new List<string>();

            //US: 313
            var to = staffEmailList;
            var receivedCC = factPortalEmailAddress;
            subCc.Add(factPortalEmailAddress);
            if (application.Coordinator != null && !String.IsNullOrEmpty(application.Coordinator.EmailAddress))
            {
                to += "," + application.Coordinator.EmailAddress;
                subCc.Add(application.Coordinator.EmailAddress);
            }

            var coordinator = (applicationTypeName == Constants.ApplicationTypes.Eligibility
                                  ? userManager.GetAccreditationServicesSupervisor()
                                  : application.Coordinator) ?? new User();

            subTo.AddRange(org.Users.Where(x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector).Select(x=>x.User.EmailAddress));
            
            if (org.PrimaryUser != null)
            {
                subTo.Add(org.PrimaryUser.EmailAddress);
            }

            if (!string.IsNullOrEmpty(org.CcEmailAddresses))
            {
                var add = org.CcEmailAddresses.Split(';');

                subCc.AddRange(add.ToList());
            }

            this.EligibilityApplicationReceivedEmail(url, orgName, to, receivedCC, application.UniqueId.ToString(), coordinator, applicationTypeName);
            this.EligibilityApplicationSubmittedEmail(url, subTo, subCc, application.UniqueId.ToString(), applicationTypeName, application.Organization.Name, application.Coordinator ?? new User());
        }

        public void UpdateApplicationStatus(int applicationTypeId, int applicationStatusId, int organizationId, string updatedBy)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var app = applicationManager.GetByOrganizationAndApplicationType(organizationId, applicationTypeId);

            applicationManager.UpdateApplicationStatus(organizationId, applicationTypeId, applicationStatusId, updatedBy);
        }

        public async Task UpdateApplicationStatusAsync(int applicationTypeId, string applicationStatusName, int organizationId, DateTime? dueDate, string emailTemplate, string updatedBy, bool includeAccreditationReport = false, string accessToken = "")
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var applicationStatus = applicationStatusManager.GetByName(applicationStatusName);

            var coordinatorEmail = string.Empty;

            if (applicationStatus == null)
                throw new ObjectNotFoundException(string.Format("Cannot find application status {0}", applicationStatusName));

            var app = await applicationManager.GetByOrganizationAndApplicationTypeAsync(organizationId, applicationTypeId);

            var applicationTypeName = app.ApplicationType.Name;

            var coordinator = new User();
            var creds = "";

            if (app.CoordinatorId.HasValue)
            {
                coordinator = app.Coordinator;
                coordinatorEmail = coordinator.EmailAddress;

                foreach (var cred in coordinator.UserCredentials)
                {
                    creds += cred.Credential.Name + ", ";
                }

                if (creds.Length > 2)
                {
                    creds = creds.Substring(0, creds.Length - 2);
                }
            }

            emailTemplate = emailTemplate.Replace("{CoordinatorName}", $"{coordinator.FirstName} {coordinator.LastName}");
            emailTemplate = emailTemplate.Replace("{Coordinator Name}", $"{coordinator.FirstName} {coordinator.LastName}, {creds}");
            emailTemplate = emailTemplate.Replace("{Coordinator Credentials}", creds);
            emailTemplate = emailTemplate.Replace("{CoordinatorCredentials}", creds);
            emailTemplate = emailTemplate.Replace("{CoordinatorTitle}", coordinator.Title);
            emailTemplate = emailTemplate.Replace("{CoordinatorPhoneNumber}", coordinator.WorkPhoneNumber);
            emailTemplate = emailTemplate.Replace("{CoordinatorPhone}", coordinator.WorkPhoneNumber);
            emailTemplate = emailTemplate.Replace("{Coordinator Phone}", coordinator.WorkPhoneNumber);
            emailTemplate = emailTemplate.Replace("{Coordinator PhoneNumber}", coordinator.WorkPhoneNumber);
            emailTemplate = emailTemplate.Replace("{Coordinator Phone Number}", coordinator.WorkPhoneNumber);
            emailTemplate = emailTemplate.Replace("{CoordinatorEmailAddress}", coordinator.EmailAddress);
            emailTemplate = emailTemplate.Replace("{Coordinator Email Address}", coordinator.EmailAddress);

            if (app.ComplianceApplicationId != null)
            {
                foreach (var appl in app.ComplianceApplication.Applications)
                {
                    await applicationManager.UpdateApplicationStatusAsync(appl, organizationId, applicationTypeId, applicationStatus.Id, applicationStatusName, dueDate, updatedBy);
                }
            }
            else
            {
                await applicationManager.UpdateApplicationStatusAsync(app, organizationId, applicationTypeId, applicationStatus.Id, applicationStatusName, dueDate, updatedBy);
            }

            if (app.ComplianceApplicationId.HasValue)
            {
                var compAppManager = this.container.GetInstance<ComplianceApplicationManager>();

                var compApp = compAppManager.GetById(app.ComplianceApplicationId.Value);

                coordinatorEmail = compApp.Coordinator.EmailAddress;

                compApp.ApplicationStatusId = applicationStatus.Id;
                compApp.UpdatedDate = DateTime.Now;
                compApp.UpdatedBy = updatedBy;

                compAppManager.Save(compApp);
            }

            if (applicationTypeName == Constants.ApplicationTypes.Annual &&
                applicationStatusName == Constants.ApplicationStatus.Complete)
            {
                var reportingFacade = this.container.GetInstance<ReportingFacade>();
                var orgManager = this.container.GetInstance<OrganizationManager>();

                var org = orgManager.GetById(organizationId);

                var cycleNumber = 1;

                if (org.OrganizationAccreditationCycles != null &&
                    org.OrganizationAccreditationCycles.Count > 0)
                {
                    cycleNumber = org.OrganizationAccreditationCycles.Max(c => c.Number);
                }

                //reportingFacade.CopyReport(Constants.Reports.SingleApplication, org.DocumentLibraryVaultId, app.UniqueId, org.Name, cycleNumber, true);
            }

            List<string> ccs = new List<string>();
            var factEmail = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.FactPortalEmailAddress];

            if (!string.IsNullOrEmpty(factEmail))
                ccs.Add(factEmail);

            if (!string.IsNullOrEmpty(coordinatorEmail))
                ccs.Add(coordinatorEmail);

            if ((applicationStatusName == Constants.ApplicationStatus.RFI || applicationStatusName == Constants.ApplicationStatus.RFI) && !string.IsNullOrEmpty(emailTemplate))
            {
                this.SendRfiEmail(ccs, applicationTypeName, organizationId, emailTemplate, includeAccreditationReport, app.ComplianceApplicationId, accessToken);
            }

            if (applicationStatusName == Constants.ApplicationStatus.Complete && !string.IsNullOrEmpty(emailTemplate))
            {
                this.SendAnnualApplicationCompletionEmail(ccs, applicationTypeName, organizationId, emailTemplate);
            }
        }

        private void SendRfiEmail(List<string> ccs, string applicationTypeName, int organizationId, string html, bool includeAccreditationReport, Guid? complianceApplicationId, string accessToken)
        {
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var emailFacade = this.container.GetInstance<EmailFacade>();

            var org = orgManager.GetById(organizationId);

            var included = new List<string>
            {
                Constants.JobFunctions.ApheresisCollectionFacilityDirector,
                Constants.JobFunctions.ApheresisCollectionFacilityMedicalDirector,
                Constants.JobFunctions.BoneMarrowCollectionFacilityDirector,
                Constants.JobFunctions.BoneMarrowCollectionFacilityMedicalDirector,
                Constants.JobFunctions.ClinicalProgramDirector,
                Constants.JobFunctions.CordBloodBankDirector,
                Constants.JobFunctions.CordBloodBankMedicalDirector,
                Constants.JobFunctions.CordBloodCollectionSiteDirector,
                Constants.JobFunctions.CordBloodProcessingFacilityDirector,
                Constants.JobFunctions.ProcessingFacilityDirector
            };

            var tos = org.Users.Where(x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector).Select(x => x.User.EmailAddress).ToList();

            if (org.PrimaryUserId.HasValue)
            {
                ccs.Add(org.PrimaryUser.EmailAddress);
            }

            if (!string.IsNullOrEmpty(org.CcEmailAddresses))
            {
                var add = org.CcEmailAddresses.Split(';');

                ccs.AddRange(add.ToList());
            }

            var users = org.Users.Where(x => included.Any(y => y == x.JobFunction.Name) && x.User.RoleId != (int)Constants.Role.NonSystemUser);

            ccs.AddRange(users.Select(x => x.User.EmailAddress));

            var cycleNumber = 1;

            if (org.OrganizationAccreditationCycles != null &&
                org.OrganizationAccreditationCycles.Count > 0)
            {
                cycleNumber = org.OrganizationAccreditationCycles.Max(c => c.Number);
            }

            emailFacade.Send(tos, ccs, $"FACT {applicationTypeName} requires more information - {org.Name}", html, includeAccreditationReport, cycleNumber, org.Name, complianceApplicationId, accessToken, false);
        }

        private void SendAnnualApplicationCompletionEmail(List<string> ccs, string applicationTypeName, int organizationId, string html)
        {
            var orgManager = this.container.GetInstance<OrganizationManager>();

            var org = orgManager.GetById(organizationId);

            var included = new List<string>
            {
                Constants.JobFunctions.ApheresisCollectionFacilityDirector,
                Constants.JobFunctions.ApheresisCollectionFacilityMedicalDirector,
                Constants.JobFunctions.BoneMarrowCollectionFacilityDirector,
                Constants.JobFunctions.BoneMarrowCollectionFacilityMedicalDirector,
                Constants.JobFunctions.ClinicalProgramDirector,
                Constants.JobFunctions.CordBloodBankDirector,
                Constants.JobFunctions.CordBloodBankMedicalDirector,
                Constants.JobFunctions.CordBloodCollectionSiteDirector,
                Constants.JobFunctions.CordBloodProcessingFacilityDirector,
                Constants.JobFunctions.ProcessingFacilityDirector
            };

            var tos = org.Users.Where(x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector).Select(x => x.User.EmailAddress).ToList();

            var users = org.Users.Where(x => included.Any(y => y == x.JobFunction.Name));

            ccs.AddRange(users.Select(x => x.User.EmailAddress));

            if (org.PrimaryUserId.HasValue && org.PrimaryUser != null)
            {
                tos.Add(org.PrimaryUser.EmailAddress);
            }

            if (!string.IsNullOrEmpty(org.CcEmailAddresses))
            {
                var add = org.CcEmailAddresses.Split(';');

                ccs.AddRange(add.ToList());
            }

            EmailHelper.Send(tos, ccs, $"FACT Annual Report Approved - {org.Name}", html);
        }

        /// <summary>
        /// Eligibility applicaiton received
        /// </summary>
        /// <param name="url">Url of the current server</param>
        public void EligibilityApplicationReceivedEmail(string url, string orgName, string staffEmailList, string factPortalEmailAddress, string applicationUniqueId, User coordinator, string applicationType, bool isCompliance = false, Guid? compAppId = null)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            applicationManager.EligibilityApplicationReceivedEmail(url, orgName, staffEmailList, factPortalEmailAddress, applicationUniqueId, coordinator, applicationType, isCompliance, compAppId);
        }

        public void ComplianceApplicationSubmittedEmail(string url, string userEmail, string cc, ComplianceApplication complianceApplication)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            applicationManager.ComplianceApplicationSubmittedEmail(url, userEmail, cc, complianceApplication);
        }

        /// <summary>
        /// Eligibility applicaiton submitted
        /// </summary>
        /// <param name="url">Url of the current server</param>
        public void EligibilityApplicationSubmittedEmail(string url, List<string> to, List<string> cc, string applicationUniqueId, string appType, string orgName, User coordinator)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            applicationManager.EligibilityApplicationSubmittedEmail(url, to, cc, applicationUniqueId, appType, orgName, coordinator);
        }

        /// <summary>
        /// Compliance Application Rejected
        /// </summary>
        /// <param name="url">Url of the current server</param>
        public void ComplianceApplicationRejectedEmail(string url, string rejectionComments, ComplianceApplication complianceApplication)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var coordicatorEmail = string.Empty;
            var directorEmail = string.Empty;
            var primaryEmail = string.Empty;

            coordicatorEmail = complianceApplication.Coordinator.EmailAddress;

            if (complianceApplication.Organization.PrimaryUserId != null)
            {
                primaryEmail = complianceApplication.Organization.PrimaryUser.EmailAddress;
            }

            var tos =
                complianceApplication.Organization.Users.Where(
                        x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector)
                    .Select(x => x.User.EmailAddress)
                    .ToList();
            tos.Add(primaryEmail);

            complianceApplicationManager.ComplianceApplicationRejectedEmail(url, tos, coordicatorEmail,
                complianceApplication.Organization.CcEmailAddresses, rejectionComments,
                complianceApplication);
        }

        private void ProcessAppExpectedAndHidden(Guid applicationUniqueId, string updatedBy)
        {
            this.log.Info($"Processing App Expected and Hidden. AppUniqueId: {applicationUniqueId}");

            try
            {
                var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();

                applicationResponseManager.ProcessExpectedAndHidden(applicationUniqueId, updatedBy);


                //var applicationResponseStatusManager = this.container.GetInstance<ApplicationResponseStatusManager>();
                //var applicationManager = this.container.GetInstance<ApplicationManager>();

                //var applicationResponses = applicationResponseManager.GetApplicationResponses(applicationUniqueId);
                //var applicationResponseStatus =
                //    applicationResponseStatusManager.GetStatusByName(Constants.ApplicationResponseStatus.Reviewed);

                //var updatedApplicationResponses = applicationManager.SetExpectedAnswers(applicationResponses,
                //    applicationResponseStatus.Id, updatedBy);

                //if (updatedApplicationResponses.Count > 0)
                //{
                //    foreach (var response in updatedApplicationResponses)
                //    {
                //        applicationResponseManager.Save(response);
                //    }
                //}


                //var applicationSectionQuestionAnswerDisplayManager =
                //    this.container.GetInstance<ApplicationSectionQuestionAnswerDisplayManager>();

                //var lstApplicationSectionQuestionAnswerDisplay = applicationSectionQuestionAnswerDisplayManager.GetAll();

                //var lstResponsesToBeRemoved = (from hiddenQuestion in lstApplicationSectionQuestionAnswerDisplay
                //    join appResponses in updatedApplicationResponses on
                //    hiddenQuestion.ApplicationSectionQuestionAnswerId equals
                //    appResponses.ApplicationSectionQuestionAnswerId
                //    select hiddenQuestion.HidesQuestionId).ToList();

                //if (lstResponsesToBeRemoved.Count <= 0) return;

                //foreach (var question in lstResponsesToBeRemoved)
                //{
                //    var appResponse =
                //        updatedApplicationResponses.FirstOrDefault(x => x.ApplicationSectionQuestionId == question);

                //    if (appResponse != null)
                //    {
                //        applicationResponseManager.Remove(appResponse.Id);
                //    }
                //}
            }
            catch (Exception ex)
            {
                this.log.Error($"Error tyying to set Expecteds: {applicationUniqueId}", ex);
                throw;
            }
        }

        public void ChangeApplicationStatus(string url, string staffEmailList, string factPortalEmailAddress, Application application,
            string applicationStatusName, string updatedBy, bool sendEmail = true)
        {
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            
            var userManager = this.container.GetInstance<UserManager>();

            //var applicationTypeTask = applicationTypeManager.GetByName(applicationTypeName);
            var applicationStatusTask = applicationStatusManager.GetByName(applicationStatusName);
            var org = application.Organization;

            if (updatedBy != "System")
            {
                var user = userManager.GetByEmailAddress(updatedBy);

                if (user == null)
                {
                    throw new Exception("Cannot find user");
                }

                if (org.OrganizationConsutants.Any(x => x.ConsultantId == user.Id))
                {
                    throw new Exception("Consultants cannot submit applications");
                }
            }
            
            if (applicationStatusName.Trim() == Constants.ApplicationStatus.ForReview.Trim())
            {
                this.log.Info($"Processing App Expected and Hidden. AppId: {application.Id}; AppStatus: {application.ApplicationStatus.Name}; New Status: {applicationStatusName} " );
                this.ProcessAppExpectedAndHidden(application.UniqueId, updatedBy);
            }
            else
            {
                this.log.Info($"Not processing Expecteds: {application.UniqueId} -- {application.ApplicationStatus?.Name}");
            }


            applicationManager.UpdateApplicationStatus(application.Id, applicationStatusTask.Id, updatedBy);

            if (application.ApplicationType.Name == Constants.ApplicationTypes.Eligibility || application.ApplicationType.Name == Constants.ApplicationTypes.Renewal)
            {
                var subTo = new List<string>();
                var subCc = new List<string>();

                //US: 313
                var to = staffEmailList;
                var receivedCC = factPortalEmailAddress;
                subCc.Add(factPortalEmailAddress);
                if (application.Coordinator != null && !String.IsNullOrEmpty(application.Coordinator.EmailAddress))
                {
                    to += "," + application.Coordinator.EmailAddress;
                    subCc.Add(application.Coordinator.EmailAddress);
                }

                subTo.AddRange(org.Users.Where(
                        x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector)
                    .Select(x => x.User.EmailAddress)
                    .ToList());

                if (org.PrimaryUser != null)
                {
                    subTo.Add(org.PrimaryUser.EmailAddress);
                }

                if (!string.IsNullOrEmpty(org.CcEmailAddresses))
                {
                    var add = org.CcEmailAddresses.Split(';');

                    subCc.AddRange(add.ToList());
                }

                this.EligibilityApplicationReceivedEmail(url, org.Name, to, receivedCC, application.UniqueId.ToString(), application.Coordinator ?? new User(), application.ApplicationType.Name);
                this.EligibilityApplicationSubmittedEmail(url, subTo, subCc, application.UniqueId.ToString(), application.ApplicationType.Name, application.Organization.Name, application.Coordinator ?? new User());
            }

            else if (application.ComplianceApplicationId.HasValue && 
                applicationStatusName == Constants.ApplicationStatus.ForReview && 
                !string.IsNullOrEmpty(application.ComplianceApplication?.ApplicationStatus?.Name) &&
                application.ComplianceApplication?.ApplicationStatus?.Name != Constants.ApplicationStatus.RFI &&
                sendEmail)
            {
                var to = staffEmailList;
                if (application.Coordinator != null && !String.IsNullOrEmpty(application.Coordinator.EmailAddress))
                {
                    to += "," + application.Coordinator.EmailAddress;
                }

                var allApps = applicationManager.GetAllByOrganizationOrApplicationType(application.OrganizationId, null);

                var appType = allApps.Any(x => x.ApplicationType.Name == Constants.ApplicationTypes.Renewal)
                    ? Constants.ApplicationTypes.Renewal
                    : Constants.ApplicationTypes.Eligibility;

                this.EligibilityApplicationReceivedEmail(url, org.Name, to, factPortalEmailAddress, application.UniqueId.ToString(), application.Coordinator ?? new User(), appType, true, application.ComplianceApplicationId);
            }
        }

        public List<Application> GetApplications(string organizationName)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            List<Application> applications = null;

            if (!string.IsNullOrEmpty(organizationName))
            {
                var organization = organizationManager.GetByName(organizationName);

                if (organization == null)
                {
                    throw new Exception($"Cannot find Organization {organizationName}");
                }

                applications = this.GetInspectionScheduleByOrgIdAppId(organization.Id, null);
            }
            else
            {
                applications = this.GetInspectionScheduleByOrgIdAppId(null, null);
            }

            return this.GetApplicationsWithSingleCompliance(applications);
        }


        public List<Application> GetInspectorApplications(Guid inspectorUserId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = applicationManager.GetInspectorApplications(inspectorUserId);

            return applications;
        }

        public async Task<List<Application>> GetInspectorApplicationsAsync(Guid inspectorUserId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = await applicationManager.GetInspectorApplicationsAsync(inspectorUserId);

            return applications;
        }

        public List<CoordinatorApplication> GetCoordinatorApplications(Guid? coordinatorUserId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();

            var applications = applicationManager.GetCoordinatorApplications(coordinatorUserId);

            //foreach (var application in applications)
            //{
            //    if (application.ComplianceApplicationId != null)
            //    {
            //        var coordinator = complianceApplicationManager.GetById(application.ComplianceApplicationId.Value).Coordinator;
            //        application.Coordinator = coordinator.LastName + ", " + coordinator.FirstName;
            //        application.CoordinatorId = coordinator.Id;
            //    }
            //}

            return this.GetApplicationsWithSingleCompliance(applications);
        }

        public async Task<List<Application>> GetApplicationsAsync(string organizationName)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            List<Application> applications = null;

            if (!string.IsNullOrEmpty(organizationName))
            {
                var organization = await organizationManager.GetByNameAsync(organizationName);

                if (organization == null)
                {
                    throw new Exception($"Cannot find Organization {organizationName}");
                }

                applications = await this.GetInspectionScheduleByOrgIdAppIdAsync(organization.Id, null);
            }
            else
            {
                applications = await this.GetInspectionScheduleByOrgIdAppIdAsync(null, null);
            }

            return this.GetApplicationsWithSingleCompliance(applications);

        }

        public Application GetApplication(Guid uniqueId)
        {
            var manager = this.container.GetInstance<ApplicationManager>();

            return manager.GetByUniqueId(uniqueId);
        }

        public Application GetApplicationIgnoreActive(Guid uniqueId)
        {
            var manager = this.container.GetInstance<ApplicationManager>();

            return manager.GetByUniqueIdIgnoreActive(uniqueId);
        }

        public DateTime? GetInspectionDateByCompliance(Guid complianceApplicationId, int siteId)
        {
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();

            var schedules = inspectionScheduleManager.GetAllForCompliance(complianceApplicationId);

            foreach (var sched in schedules.Where(x => x.IsActive && !x.IsCompleted))
            {
                var detail = sched.InspectionScheduleDetails.FirstOrDefault(x => x.IsActive && x.SiteId == siteId);

                if (detail != null)
                {
                    return sched.StartDate;
                }
            }

            return null;
        }

        public Task<Application> GetApplicationAsync(Guid uniqueId)
        {
            var manager = this.container.GetInstance<ApplicationManager>();

            return manager.GetByUniqueIdAsync(uniqueId);
        }

        public bool ShowAccreditationReport(Guid complianceApplicationId)
        {
            var manager = this.container.GetInstance<ComplianceApplicationManager>();

            var app = manager.GetById(complianceApplicationId);

            return app.ShowAccreditationReport.GetValueOrDefault();
        }

        public List<ApplicationWithRfi> GetApplicationsWithRfis(int organizationId, Guid currentApplicationUniqueId, string applicationResponseStatusName)
        {
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();
            var rfiResponses = applicationResponseManager.GetApplicationRfis(organizationId);

            var items =
                rfiResponses.Where(
                    x =>
                        x.Application.UniqueId != currentApplicationUniqueId)
                        .ToList();

            var rfis = new List<ApplicationWithRfi>();

            foreach (var item in items)
            {
                var rfi = rfis.SingleOrDefault(
                    x =>
                        x.ComplianceAppId == item.Application.ComplianceApplicationId &&
                        x.ApplicationSectionId == item.ApplicationSectionQuestion.ApplicationSectionId);
                if (rfi == null)
                {
                    rfis.Add(new ApplicationWithRfi
                    {
                        ComplianceAppId = item.Application.ComplianceApplicationId,
                        UniqueId = item.Application.UniqueId,
                        ApplicationSectionId = item.ApplicationSectionQuestion.ApplicationSectionId,
                        SiteId = item.Application.SiteId.GetValueOrDefault(),
                        Status = item.ApplicationResponseStatus.Name,
                        ApplicationTypeName = item.Application.ApplicationType.Name,
                        RequirementNumber = item.ApplicationSectionQuestion.ApplicationSection.UniqueIdentifier
                    });
                }
                else
                {
                    if (item.ApplicationResponseStatus.Name == Constants.ApplicationResponseStatus.RFI)
                    {
                        rfi.Status = Constants.ApplicationResponseStatus.RFI;
                    }
                }
            }

            return rfis;
        }

        public List<Application> GetInspectionScheduleByOrgIdAppId(int? organizationId, int? applicationTypeId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = applicationManager.GetAllByOrganizationOrApplicationType(organizationId,
                applicationTypeId);

            return this.GetApplicationsWithSingleCompliance(applications);
        }

        public async Task<List<Application>> GetInspectionScheduleByOrgIdAppIdAsync(int? organizationId, int? applicationTypeId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = await applicationManager.GetAllByOrganizationOrApplicationTypeAsync(organizationId, applicationTypeId);

            return this.GetApplicationsWithSingleCompliance(applications);
        }
        public async Task<List<Application>> GetInspectionScheduleByOrgIdAppIdForInspectionSchedulerAsync(int? organizationId, int? applicationTypeId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applications = await applicationManager.GetAllByOrganizationOrApplicationTypeAsync(organizationId, applicationTypeId);

            return this.GetApplicationsWithSingleComplianceForInspectionScheduler(applications);
        }

        private ComplianceApplicationItem GetComplianceApplicationItem(Organization organization, ComplianceApplication complianceApplication, List<Application> applications, List<ApplicationQuestionNotApplicable> notApplicables = null)
        {
            if (complianceApplication != null && notApplicables != null)
            {
                foreach (var app in applications)
                {
                    app.ApplicationQuestionNotApplicables =
                        notApplicables.Where(x => x.ApplicationId == app.Id).ToList();
                }
            }

            var grouped = applications.GroupBy(x => x.Site);

            grouped = grouped.OrderBy(x => Comparer.OrderSite(x.Key));

            var complianceApplicationSites = grouped
                .Select(site => new ComplianceApplicationSite
                {
                    Site = ModelConversions.Convert(site.Key, false, false, false),
                    Applications = site.ToList().Select(x => ModelConversions.Convert(x, true, false, false)).ToList(),
                })
                .ToList();

            //var complianceApplication = complianceApplicationManager.GetByOrg(organizationId);

            var application = applications.FirstOrDefault(x => x.ApplicationType.Name.Contains("Compliance"));
            // var application = applications.Where(x => x.ApplicationType.Name.Contains("Compliance") && x.DueDate != null).FirstOrDefault();


            return new ComplianceApplicationItem
            {
                Id = complianceApplication?.Id,
                AccreditationGoal = complianceApplication?.AccreditationGoal,
                InspectionScope = complianceApplication?.InspectionScope,
                Coordinator = ModelConversions.Convert(complianceApplication?.Coordinator, false, false, true),
                ComplianceApplicationSites = complianceApplicationSites,
                ReportReviewStatus = complianceApplication?.ReportReviewStatus?.Id.ToString() ?? "",
                ReportReviewStatusName = complianceApplication?.ReportReviewStatus?.Name,
                ApprovalStatus = ModelConversions.Convert(complianceApplication?.ComplianceApplicationApprovalStatus),
                OrganizationId = organization.Id,
                OrganizationName = organization.Name,
                ApplicationStatus = complianceApplication?.ApplicationStatus?.Name,
                DueDate = application?.DueDate?.ToShortDateString() ?? "",
                ShowAccreditationReport = complianceApplication?.ShowAccreditationReport.GetValueOrDefault() ?? false,
                CreatedDate = complianceApplication?.CreatedDate.GetValueOrDefault().ToString("MM/dd/yyyy")
            };
        }

        public List<ComplianceApplicationItem> GetAllComplianceApplications(string organizationName, Guid userId)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var applicationNotApplicableManager = this.container.GetInstance<ApplicationQuestionNotApplicableManager>();

            var organization = organizationManager.GetByName(organizationName);

            if (organization == null)
            {
                throw new Exception("Cannot find organization");
            }

            var applications = complianceApplicationManager.GetByOrg(organization.Id);
            var apps = applicationManager.GetComplianceApplications(organization.Id);

            var results = new List<ComplianceApplicationItem>();

            foreach (var app in applications)
            {
                var notApplicables = applicationNotApplicableManager.GetAllForCompliance(app.Id);

                var sections = applicationSectionManager.GetApplicationSectionsForApplication(app.Id, null, userId);
                
                var myApps = apps.Where(x => x.ComplianceApplicationId == app.Id).ToList();

                var compApp = this.GetComplianceApplicationItem(organization, app, myApps, notApplicables);

                foreach (var row in compApp.ComplianceApplicationSites)
                {
                    var site = sections.FirstOrDefault(x => x.SiteId == row.Site.SiteId);

                    if (site != null)
                    {
                        foreach (var record in row.Applications)
                        {
                            var responses = site.AppResponses.Where(x => x.ApplicationId == record.ApplicationId).ToList();

                            foreach (var appResponse in responses)
                            {
                                appResponse.ApplicationSectionResponses = ModelConversions.SetStatuses(app.ApplicationStatus.Name, appResponse.ApplicationSectionResponses, false);
                                appResponse.ApplicationTypeStatusName = ModelConversions.SetStatusForAllSectionsForApplicant(appResponse.ApplicationSectionResponses);

                                var rows = appResponse.ApplicationSectionResponses.Where(x => x.IsVisible).ToList();

                                record.Standards = string.Join(",",
                                    rows.Select(x => x.ApplicationSectionUniqueIdentifier));
                            }
                        }

                    }
                    
                }

                results.Add(compApp);

            }

            return results;
        }

        public ComplianceApplicationItem GetComplianceApplication(Guid complianceAppId)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var application = complianceApplicationManager.GetById(complianceAppId);
            var apps = applicationManager.GetComplianceApplications(application.OrganizationId);

            apps = apps.Where(x => x.ComplianceApplicationId == complianceAppId).ToList();

            return this.GetComplianceApplicationItem(application.Organization, application, apps);
        }

        //public ComplianceApplicationItem GetAllComplianceApplication(string organizationName)
        //{
        //    var organizationManager = this.container.GetInstance<OrganizationManager>();
        //    var applicationManager = this.container.GetInstance<ApplicationManager>();

        //    var organization = organizationManager.GetByName(organizationName);

        //    if (organization == null)
        //    {
        //        throw new Exception("Cannot find organization");
        //    }

        //    var applications = applicationManager.GetAllComplianceApplications(organization.Id);

        //    return this.GetComplianceApplicationItem(organization.Id, applications);
        //}

        //public ComplianceApplicationItem GetComplianceApplication(string organizationName)
        //{
        //    var organizationManager = this.container.GetInstance<OrganizationManager>();
        //    var applicationManager = this.container.GetInstance<ApplicationManager>();

        //    var organization = organizationManager.GetByName(organizationName);

        //    if (organization == null)
        //    {
        //        throw new Exception("Cannot find organization");
        //    }

        //    var applications = applicationManager.GetComplianceApplications(organization.Id);

        //    return this.GetComplianceApplicationItem(organization.Id, applications);
        //}

        public void CancelComplianceApplication(Guid id, string updateBy)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var applicationStatus = applicationStatusManager.GetByName(Constants.ApplicationStatus.Cancelled);

            var complianceApplication = complianceApplicationManager.GetById(id);

            if (complianceApplication != null)
            {
                complianceApplication.ApplicationStatusId = applicationStatus.Id;
                complianceApplication.UpdatedDate = DateTime.Now;
                complianceApplication.UpdatedBy = updateBy;

                complianceApplicationManager.Save(complianceApplication);

                var applications = applicationManager.GetComplianceApplications(complianceApplication.OrganizationId);

                if (applications != null)
                {
                    foreach (var app in applications)
                    {
                        app.ApplicationStatusId = applicationStatus.Id;
                        app.UpdatedDate = DateTime.Now;
                        app.UpdatedBy = updateBy;

                        applicationManager.Save(app);
                    }
                }
            }
        }

        public ComplianceApplicationItem GetComplianceApplicationByAppUniqueId(Guid complianceApplicationId)
        {
            //var applicationManager = this.container.GetInstance<ApplicationManager>();
            //var application = applicationManager.GetByUniqueId(applicationUniqueId);

            ////var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            ////var complianceApplication = complianceApplicationManager.GetById(complianceApplicationId);

            //if (application == null)
            //    throw new ObjectNotFoundException(string.Format("Cannot find application with Unique Id {0}", applicationUniqueId));

            //var complianceApplication = application.ComplianceApplication;
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var complianceApplication = complianceApplicationManager.GetByIdSemiInclusive(complianceApplicationId);

            if (complianceApplication == null)
                throw new ObjectNotFoundException(string.Format("Cannot find compliance application with Unique Application Id {0}", complianceApplicationId));

            var grouped = complianceApplication.Applications.GroupBy(x => x.Site);

            grouped = grouped.OrderBy(x => Comparer.OrderSite(x.Key));

            var complianceApplicationSites = grouped
                .Select(site => new ComplianceApplicationSite
                {
                    Site = ModelConversions.Convert(site.Key, false),
                    Applications = site
                        .Where(x => x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive.GetValueOrDefault())
                        .ToList()
                        .Select(x => ModelConversions.Convert(x, true)).ToList(),
                })
                .ToList();

            var appWithTypeDetail = complianceApplication.Applications.FirstOrDefault(x => x.TypeDetail != null);
            string typeDetail = null;
            if (appWithTypeDetail != null)
            {
                typeDetail = appWithTypeDetail.TypeDetail;
            }


            return new ComplianceApplicationItem
            {
                Id = complianceApplication.Id,
                AccreditationGoal = complianceApplication.AccreditationGoal,
                RejectionComments = complianceApplication.RejectionComments,
                InspectionScope = complianceApplication.InspectionScope,
                OrganizationId = complianceApplication.OrganizationId,
                ComplianceApplicationSites = complianceApplicationSites,
                Coordinator = ModelConversions.Convert(complianceApplication?.Coordinator),
                ReportReviewStatus = complianceApplication.ReportReviewStatus != null ? complianceApplication.ReportReviewStatus.Id.ToString() : "",
                ReportReviewStatusName = complianceApplication.ReportReviewStatus != null ? complianceApplication.ReportReviewStatus.Name : "",
                Applications = ModelConversions.Convert(complianceApplication.Applications.ToList(), false),
                OrganizationName = complianceApplication.Organization.Name,
                ShowAccreditationReport = complianceApplication.ShowAccreditationReport.GetValueOrDefault(),
                AccreditedSince = complianceApplication.Organization.AccreditedSince,
                TypeDetail = typeDetail
            };

        }

        public ComplianceApplicationItem GetComplianceApplicationSortedByServiceType(Guid complianceApplicationId, bool isUser)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var complianceApplication = complianceApplicationManager.GetById(complianceApplicationId);

            if (complianceApplication == null)
                throw new ObjectNotFoundException(string.Format("Cannot find compliance application Id {0}", complianceApplicationId));

            var org = complianceApplication.Applications.First().Organization;
            var facilities = org.OrganizationFacilities.Select(x => x.Facility);
            var serviceTypes = facilities.Select(x => new
            {
                x.ServiceType.Name,
                Sites = x.FacilitySites.Select(y => y.Site)
            })
                .GroupBy(x => x.Name)
                .Where(x => x.Any(y => y.Sites.Any(z => z.Applications.Count > 0)))
                .Select(x => new
                {
                    ServiceType = x.Key,
                    Sites = x.Select(y => y.Sites.ToList()).ToList()
                });

            var byServiceTypes = serviceTypes
                .Select(s => new ComplianceApplicationServiceType
                {
                    ServiceType = s.ServiceType,
                    ComplianceApplicationSites = s.Sites.First().Select(x => new ComplianceApplicationSite
                    {
                        Site = ModelConversions.Convert(x, false),
                        Applications = x.Applications.Where(
                                y => y.ComplianceApplicationId == complianceApplicationId &&
                                    (y.IsActive == true || !y.IsActive.HasValue) &&
                                    y.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled)
                            .Select(y => ModelConversions.Convert(y, true))
                            .ToList()
                    }).ToList()
                }).ToList();

            foreach (var type in byServiceTypes)
            {
                for (var i = 0; i < type.ComplianceApplicationSites.Count; i++)
                {
                    if (type.ComplianceApplicationSites[i].Applications.Count == 0)
                    {
                        type.ComplianceApplicationSites.RemoveAt(i);
                        i--;
                    }
                }
            }

            var apps =
                complianceApplication.Applications.Where(
                    x =>
                        (x.IsActive == true || !x.IsActive.HasValue) &&
                        x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled).ToList();

            return new ComplianceApplicationItem
            {
                Id = complianceApplication.Id,
                ApplicationStatus = isUser ? complianceApplication.ApplicationStatus.NameForApplicant : complianceApplication.ApplicationStatus.Name,
                AccreditationGoal = complianceApplication.AccreditationGoal,
                RejectionComments = complianceApplication.RejectionComments,
                InspectionScope = complianceApplication.InspectionScope,
                OrganizationId = complianceApplication.OrganizationId,
                ComplianceApplicationServiceTypes = byServiceTypes,
                Coordinator = ModelConversions.Convert(complianceApplication?.Coordinator),
                ReportReviewStatus = complianceApplication.ReportReviewStatus != null ? complianceApplication.ReportReviewStatus.Id.ToString() : "",
                ReportReviewStatusName = complianceApplication.ReportReviewStatus != null ? complianceApplication.ReportReviewStatus.Name : "",
                Applications = ModelConversions.Convert(apps, false),
                OrganizationName = complianceApplication.Organization.Name,
                ShowAccreditationReport = complianceApplication.ShowAccreditationReport.GetValueOrDefault()
            };

        }

        

        public List<Question> GetSectionQuestions(Guid applicationUniqueId, Guid applicationSectionId, Guid userId, bool canSeeCitations, bool isTrainee, bool isUser)
        {
            var questionManager = this.container.GetInstance<ApplicationSectionQuestionManager>();
            var answerManager = this.container.GetInstance<ApplicationSectionQuestionAnswerManager>();
            var displayManager = this.container.GetInstance<ApplicationSectionQuestionAnswerDisplayManager>();
            var commentManager = this.container.GetInstance<ApplicationResponseCommentManager>();
            var responseDocumentManager = this.container.GetInstance<ApplicationResponseCommentDocumentManager>();
            var accreditationOutcomeManager = this.container.GetInstance<AccreditationOutcomeManager>();

            var questions = questionManager.GetSectionQuestions(applicationUniqueId, applicationSectionId, userId);
            var answers = answerManager.GetSectionAnswers(applicationSectionId);
            var displays = displayManager.GetDisplaysForSection(applicationSectionId);
            var comments = commentManager.GetSectionComments(applicationUniqueId, applicationSectionId);
            var documents = responseDocumentManager.GetSectionDocuments(applicationUniqueId, applicationSectionId);
            


            DateTime? appSubmittedDate = null;

            var appManager = this.container.GetInstance<ApplicationManager>();

            var app = appManager.GetByUniqueId(applicationUniqueId);
            var detailManager = this.container.GetInstance<InspectionScheduleDetailManager>();

            if (app.ApplicationStatusId != (int)Constants.ApplicationStatuses.Rfi && app.ApplicationStatusId != (int)Constants.ApplicationStatuses.ApplicantResponse)
            {
                appSubmittedDate = app.UpdatedDate ?? System.Convert.ToDateTime("1/1/17");
            }

            var details = app.ComplianceApplicationId.HasValue
                ? detailManager.GetAllActiveByComplianceApplication(app.ComplianceApplicationId.Value)
                : detailManager.GetAllActiveByApplication(applicationUniqueId);

            if (isUser || !isTrainee)
            {
                for (var i = 0; i < comments.Count; i++)
                {
                    var comment = comments[i];

                    if (!details.Any(
                        x =>
                            x.UserId == comment.FromUser.GetValueOrDefault() &&
                            x.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees)) continue;

                    comments.RemoveAt(i);
                    i--;
                }

                //comments = comments.Where(x => details.Any(y => y.UserId == x.FromUser.GetValueOrDefault())).ToList();
            }
            else
            {
                for (var i = 0; i < comments.Count; i++)
                {
                    var comment = comments[i];

                    if (!details.Any(
                        x =>
                            x.UserId == comment.FromUser.GetValueOrDefault() &&
                            x.AccreditationRole.Name != Constants.AccreditationsRoles.InspectorTrainees)) continue;

                    comments.RemoveAt(i);
                    i--;
                }
            }

            var outcomes = accreditationOutcomeManager.GetByAppId(applicationUniqueId);

            var questionGroup = questions.GroupBy(x => x.QuestionId).ToList();

            var result = new List<Question>();

            foreach (var question in questionGroup)
            {
                var first = question.FirstOrDefault();

                if (first == null) continue;

                var q = new Question
                {
                    Id = question.Key,
                    Type = first.QuestionTypeName,
                    Text = first.Text,
                    Description = first.Description,
                    Flag = first.Flag.GetValueOrDefault(),
                    AnswerResponseStatusId = first.ApplicationResponseStatusId ?? (!isUser ? (int)Constants.ApplicationResponseStatuses.ForReview : (int)Constants.ApplicationResponseStatuses.New),
                    AnswerResponseStatusName = first.ApplicationResponseStatusName ?? (!isUser ? Constants.ApplicationResponseStatus.ForReview : Constants.ApplicationResponseStatus.New),
                    VisibleAnswerResponseStatusId = first.VisibleApplicationResponseStatusId ?? (!isUser ? (int)Constants.ApplicationResponseStatuses.ForReview : (int)Constants.ApplicationResponseStatuses.New),
                    VisibleAnswerResponseStatusName = first.VisibleAnswerResponseStatusName ?? (!isUser ? Constants.ApplicationResponseStatus.ForReview : Constants.ApplicationResponseStatus.New),
                    ComplianceNumber = first.ComplianceNumber,
                    Comments = first.ApplicationResponseComments,
                    CommentLastUpdatedBy = first.ApplicationResponseCommentLastUpdatedBy,
                    CommentDate = first.CommentDate,
                    CommentBy = first.CommentBy,
                    IsHidden = !first.IsVisible,
                    Answers = new List<Answer>(),
                    HiddenBy = displays.Where(x=>x.QuestionId == question.Key).ToList(),
                    QuestionResponses = question
                        .Where(x=>x.ApplicationResponseId.HasValue).Select(x =>
                        {
                            var response = new QuestionResponse
                            {
                                Id = x.ApplicationResponseId.GetValueOrDefault(),
                                UserId = x.UserId
                            };

                            if (x.DocumentId.HasValue)
                            {
                                response.Document = new DocumentItem
                                {
                                    Id = x.DocumentId.GetValueOrDefault(),
                                    RequestValues = x.RequestValues,
                                    Name = x.DocumentName
                                };
                            }

                            if (!string.IsNullOrEmpty(x.OtherText))
                            {
                                if (first.QuestionTypeName == Constants.QuestionTypes.DateRange)
                                {
                                    var dates = x.OtherText.Split('-');
                                    response.FromDate = dates[0].Trim();
                                    response.ToDate = dates[1].Trim();
                                }
                                else
                                {
                                    response.OtherText = x.OtherText;
                                }
                            }

                            if (x.UserId.HasValue)
                            {
                                response.UserId = x.UserId;
                                response.User = new UserItem
                                {
                                    UserId = x.UserId,
                                    FirstName = x.UserFirstName,
                                    LastName = x.UserLastName,
                                    FullName = $"{x.UserFirstName} {x.UserLastName}"
                                };
                            }
                            

                            return response;
                        })
                        .ToList(),
                    ScopeTypes = answers.Where(x=>x.ScopeTypeId.HasValue).Select(x => new ScopeTypeItem
                        {
                            ScopeTypeId = x.ScopeTypeId.GetValueOrDefault(),
                            Name = x.ScopeTypeName
                        }
                        
                    ).ToList(),
                    ApplicationResponseComments = comments
                        .Where(x=>x.QuestionId == question.Key)
                        .OrderByDescending(x => x.CreatedDate)
                        .Select(x=> new ApplicationResponseCommentItem
                        {
                            ApplicationResponseCommentId = x.ApplicationResponseCommentId,
                            ApplicationId = x.ApplicationId,
                            QuestionId = x.QuestionId,
                            Comment = x.Comment,
                            CommentOverride = x.CommentOverride,
                            IsOverridden = x.IsOverridden,
                            OverridenBy = x.OverridenBy,
                            FromUser = x.FromUser,
                            ToUser = x.ToUser,
                            DocumentId = x.DocumentId,
                            CommentFrom = new UserItem
                            {
                              FullName  = x.FromName,
                              Role = new RoleItem
                              {
                                  RoleId = x.FromRoleId,
                                  RoleName = x.FromRoleName
                              }
                            },
                            CommentType = new CommentTypeItem
                            {
                                Id = x.CommentTypeId,
                                Name = x.CommentTypeName
                            },
                            CreatedBy = x.CreatedBy,
                            CreatedDate = x.CreatedDate.ToString(CultureInfo.InvariantCulture),
                            UpdatedDate = x.UpdatedDate?.ToShortDateString(),
                            UpdatedBy = x.UpdatedBy,
                            VisibleToApplicant = x.VisibleToApplicant,
                            CommentDocuments = documents.Where(y=>y.CommentId == x.ApplicationResponseCommentId).ToList()
                        })
                        .ToList()
                };

                q.ResponseCommentsRFI = q.ApplicationResponseComments
                    .Where(x => x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.RFI && 
                        ((!isUser && !isTrainee) || appSubmittedDate == null || System.Convert.ToDateTime(x.CreatedDate) <= appSubmittedDate.Value))
                    .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate)).ToList();

                if ((outcomes != null && outcomes.Count > 0) || !isUser)
                {
                    q.ResponseCommentsCitation =
                        q.ApplicationResponseComments.Where(
                                x => (!isTrainee || x.FromUser == userId) && x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.Citation)
                            .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate))
                            .ToList();
                    q.ResponseCommentsSuggestion =
                        q.ApplicationResponseComments.Where(
                                x => x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.Suggestion)
                            .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate))
                            .ToList();
                }
                else
                {
                    q.ResponseCommentsCitation = new List<ApplicationResponseCommentItem>();
                    q.ResponseCommentsSuggestion = new List<ApplicationResponseCommentItem>();
                }
                
                q.ResponseCommentsFactResponse = q.ApplicationResponseComments.Where(x => x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.FACTResponse).OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate)).ToList();
                q.ResponseCommentsFactOnly = q.ApplicationResponseComments.Where(x => x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.FACTOnly).OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate)).ToList();


                if (first.QuestionTypesFlag.HasValue)
                {
                    q.QuestionTypeFlags = QuestionTypeFlags.Set(first.QuestionTypesFlag.GetValueOrDefault());
                }

                if (first.QuestionTypeName == Constants.QuestionTypes.Checkboxes ||
                    first.QuestionTypeName == Constants.QuestionTypes.RadioButtons)
                {
                    var questionAnswers = answers.Where(x => x.QuestionId == question.Key).Select(x=> new
                    {
                        x.Id,
                        x.QuestionId,
                        x.Text
                    }).Distinct().ToList();

                    foreach (var answer in questionAnswers)
                    {
                        var a = new Answer
                        {
                            Id = answer.Id,
                            QuestionId = answer.QuestionId,
                            Text = answer.Text,
                            HidesQuestions = displays.Where(x=>x.AnswerId == answer.Id).ToList()
                        };

                        if (question.Any(x => x.AnswerId == answer.Id))
                        {
                            a.Selected = true;
                        }

                        q.Answers.Add(a);
                    }
                }

                result.Add(q);
            }

            return result;
        }

        public CompApplication GetComplianceResponses(Guid userId, bool isFactStaff, int roleId, bool isUser, Guid complianceApplicationId, Guid? applicationUniqueId)
        {           
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var siteManager = this.container.GetInstance<SiteManager>();

            var hasFlag = false;
            var hasRfis = false;

            var complianceApplication = complianceApplicationManager.GetById(complianceApplicationId);

            Guid? compAppId = null;

            if (!applicationUniqueId.HasValue)
            {
                compAppId = complianceApplicationId;
            }

            var scopeTypes = siteManager.GetScopeTypes(complianceApplication.OrganizationId);

            var appSiteResponses = applicationSectionManager.GetApplicationSectionsForApplication(compAppId, applicationUniqueId, userId, scopeTypes);

            var orgName = "";
            var hasQm = false;

            foreach (var siteResponse in appSiteResponses)
            {
                if (siteResponse.HasFlags)
                {
                    hasFlag = true;
                }

                if (siteResponse.HasRfis)
                {
                    hasRfis = true;
                }

                var isReviewer = siteResponse.IsReviewer;

                foreach (var appResponse in siteResponse.AppResponses)
                {
                    appResponse.ApplicationSectionResponses = ModelConversions.SetStatuses(complianceApplication.ApplicationStatus.Name, appResponse.ApplicationSectionResponses, isReviewer);
                    appResponse.ApplicationTypeStatusName = isReviewer && complianceApplication.ApplicationStatus.Name != Constants.ApplicationStatus.InProgress
                        ? ModelConversions.SetStatusForAllSectionsForReviewer(appResponse.ApplicationSectionResponses)
                        : ModelConversions.SetStatusForAllSectionsForApplicant(appResponse.ApplicationSectionResponses);

                    var resp = appResponse.ApplicationSectionResponses.FirstOrDefault();

                    orgName = resp?.OrganizationName ?? orgName;
                    hasQm = resp?.HasQmRestrictions ?? hasQm;
                }

                siteResponse.StatusName = isReviewer && complianceApplication.ApplicationStatus.Name != Constants.ApplicationStatus.InProgress
                        ? ModelConversions.SetStatusForAllApplicationsReviewer(siteResponse.AppResponses)
                        : ModelConversions.SetStatusForAllApplicationsApplicant(siteResponse.AppResponses);
            }

            return new CompApplication
            {
                ComplianceApplicationSites = appSiteResponses,
                Id = complianceApplication.Id,
                HasRfi = hasRfis,
                HasFlag = hasFlag,
                ApplicationStatus = isUser ? complianceApplication.ApplicationStatus?.NameForApplicant : complianceApplication.ApplicationStatus?.Name,
                OrganizationId = complianceApplication.OrganizationId,
                OrganizationName = orgName,
                ShowAccreditationReport = complianceApplication.ShowAccreditationReport.GetValueOrDefault(),
                HasQmRestriction = hasQm
            };
        }

        public ComplianceApplicationItem GetComplianceApplicationById(Guid userId, bool isFactStaff, int roleId, bool isUser, Guid complianceApplicationId)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var complianceApplication = complianceApplicationManager.GetByIdInclusive(complianceApplicationId);
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var applicationSectionQuestionAnswerManager =
                this.container.GetInstance<ApplicationSectionQuestionAnswerManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();
            var siteScopeTypeManager = this.container.GetInstance<SiteScopeTypeManager>();
            var applicationVersionCacheManager = this.container.GetInstance<ApplicationVersionCacheManager>();

            var anyAppReviewer = false;

            var answers = applicationSectionQuestionAnswerManager.GetAll();

            var inspectionSchedules = inspectionScheduleManager.GetAllForCompliance(complianceApplicationId);

            //var allSites = siteManager.GetAllSites();

            if (complianceApplication == null)
                throw new ObjectNotFoundException(string.Format("Cannot find compliance application Id {0}", complianceApplicationId));

            var submittedDate = complianceApplication.Applications.Max(x => x.SubmittedDate);

            var grouped = complianceApplication.Applications.GroupBy(x => x.Site);

            grouped = grouped.OrderBy(x => Comparer.OrderSite(x.Key));

            var complianceApplicationSites = grouped
                .Select(site => new
                {
                    Site = ModelConversions.Convert(site.Key, false),
                    Applications = site
                        .Where(x => x.ApplicationStatus.Name != Constants.ApplicationStatus.Cancelled && x.IsActive.GetValueOrDefault())
                        .ToList()
                        .Select(x => x).ToList(),
                })
                .ToList();
            

            var users = userManager.GetByOrganization(complianceApplication.OrganizationId);
            var currentUser = userManager.GetById(userId);
            var allComments = applicationResponseCommentManager.GetAllByCompliance(complianceApplicationId);

            var appSites = new List<ComplianceApplicationSite>();
            var hasRfis = false;
            var hasFlag = false;
            var traineeIds = new List<Guid>();

            foreach (var schedule in inspectionSchedules)
            {
                traineeIds.AddRange(
                    schedule.InspectionScheduleDetails.Where(
                            x => x.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees)
                        .Select(x => x.UserId)
                        .Distinct());
            }

            foreach (var site in complianceApplicationSites)
            {
                var scopeTypes = siteScopeTypeManager.GetBySite(site.Site.SiteId);

                var isTrainee = inspectionSchedules.Any(x=> x.InspectionScheduleDetails.Any(y=>y.UserId == userId && y.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees));

                if (isTrainee)
                {
                    InspectionSchedule schedule = null;

                    if (inspectionSchedules.Count > 1)
                    {
                        var endDate = inspectionSchedules.Max(x => x.EndDate);
                        schedule = inspectionSchedules.First(x => x.EndDate == endDate);
                    }
                    else
                    {
                        schedule = inspectionSchedules.First();
                    }


                    var details = schedule.InspectionScheduleDetails.Where(x => x.SiteId == site.Site.SiteId).ToList();

                    if (details.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        var trainee = details.FirstOrDefault(x => x.UserId == userId);
                        if (trainee == null)
                        {
                            continue;
                        }

                        var mentor = details.FirstOrDefault(x => x.IsMentor);

                        if (trainee.AccreditationRole.Name != Constants.AccreditationsRoles.InspectorTrainees ||
                            (trainee.IsInspectionComplete.GetValueOrDefault() &&
                             (mentor == null || mentor.IsInspectionComplete.GetValueOrDefault())))
                        {
                            isTrainee = false;
                        }
                    }
                }

                var appSite = new ComplianceApplicationSite
                {
                    Site = site.Site,
                    Applications = new List<ApplicationItem>()
                };

                foreach (var app in site.Applications)
                {
                    List<ApplicationSection> applicationSections = null;

                    var appSections = applicationSectionManager.GetFlatForApplicationType(app.ApplicationTypeId, app.ApplicationVersionId);
                    
                    applicationSections = appSections
                        .Select(x => new ApplicationSection
                        {
                            Id = x.Id,
                            ApplicationTypeId = x.ApplicationTypeId,
                            ParentApplicationSectionId = x.ParentApplicationSectionId,
                            ApplicationVersionId = x.ApplicationVersionId,
                            PartNumber = x.PartNumber,
                            Name = x.Name,
                            IsActive = x.IsActive,
                            HelpText = x.HelpText,
                            IsVariance = x.IsVariance,
                            Version = x.Version,
                            Order = x.Order,
                            UniqueIdentifier = x.UniqueIdentifier,
                            ParentApplicationSection = x.ParentApplicationSection,
                            ApplicationType = x.ApplicationType,
                            ApplicationVersion = x.ApplicationVersion,
                            Questions =
                                x.Questions.Where(
                                        xx =>
                                            xx.ScopeTypes.Any(
                                                y => scopeTypes.Any(z => z.ScopeTypeId == y.ScopeTypeId)))
                                    .ToList(),
                            Children = x.Children,
                            ApplicationSectionScopeTypes = x.ApplicationSectionScopeTypes,
                        })
                        .ToList();

                    


                    var isReviewer = roleId == (int) Constants.Role.FACTAdministrator ||
                                     roleId == (int) Constants.Role.FACTCoordinator ||
                                     roleId == (int) Constants.Role.QualityManager ||
                                     (roleId == (int) Constants.Role.Inspector && users.All(x => x.Id != userId));

                    if (isReviewer)
                    {
                        anyAppReviewer = true;
                    }

                    var comments = allComments.Where(x => x.ApplicationId == app.Id && (isFactStaff || isReviewer || x.VisibleToApplicant.GetValueOrDefault())).ToList();
                    var applicationResponses = applicationResponseManager.GetApplicationResponses(app.UniqueId);

                    if (!isTrainee || isFactStaff)
                    {
                        comments = comments.Where(x => traineeIds.All(y => y != x.FromUser)).ToList();
                    }

                    var sections = this.BuildApplication(userId, isFactStaff, app.UniqueId, isReviewer, isTrainee, app, currentUser,
                        null, answers, users, comments, applicationSections, false, null, null, false,
                        applicationResponses);

                    var responsesWithFlag = applicationResponses.Where(x => x.Flag).ToList();

                    if (responsesWithFlag.Count > 0)
                    {
                        hasFlag = this.HasFlag(sections, responsesWithFlag);
                    }

                    sections = ModelConversions.SetColor(complianceApplication.ApplicationStatus.Name, submittedDate.GetValueOrDefault(), sections, isReviewer);

                    if (!hasRfis)
                    {
                        hasRfis = this.HasRfiItems(sections);
                    }

                    var appItem = ModelConversions.Convert(app, true);
                    appItem.Standards = string.Empty;

                    foreach (var section in sections.Where(x => x.IsVisible))
                    {
                        appItem.Standards += section.UniqueIdentifier + ",";
                    }

                    if (appItem.Standards.Length > 0)
                    {
                        appItem.Standards = appItem.Standards.Substring(0, appItem.Standards.Length - 1);
                    }

                    appItem.InspectionDate = this.GetInspectionDateByCompliance(complianceApplicationId,
                        app.SiteId.GetValueOrDefault());

                    appItem.Circle = isReviewer && complianceApplication.ApplicationStatus.Name != Constants.ApplicationStatus.InProgress
                        ? ModelConversions.FindColorFromAllSectionsForReviewer(sections)
                        : ModelConversions.FindColorFromAllSectionsForApplicant(sections);

                    appItem.Sections = sections;

                    appSite.Applications.Add(appItem);
                }

                appSite.Circle = anyAppReviewer && complianceApplication.ApplicationStatus.Name != Constants.ApplicationStatus.InProgress
                        ? ModelConversions.FindColorForAllApplicationsReviewer(appSite.Applications)
                        : ModelConversions.FindColorForAllApplicationsApplicant(appSite.Applications);

                appSites.Add(appSite);
            }

            return new ComplianceApplicationItem
            {
                Id = complianceApplication.Id,
                HasRfi = hasRfis,
                HasFlag = hasFlag,
                ApplicationStatus = isUser ? complianceApplication.ApplicationStatus?.NameForApplicant : complianceApplication.ApplicationStatus?.Name,
                AccreditationGoal = complianceApplication.AccreditationGoal,
                RejectionComments = complianceApplication.RejectionComments,
                InspectionScope = complianceApplication.InspectionScope,
                OrganizationId = complianceApplication.OrganizationId,
                ComplianceApplicationSites = appSites,
                Coordinator = ModelConversions.Convert(complianceApplication?.Coordinator, false),
                ReportReviewStatus = complianceApplication.ReportReviewStatus?.Id.ToString() ?? "",
                ReportReviewStatusName = complianceApplication.ReportReviewStatus?.Name ?? "",
                Applications = ModelConversions.Convert(complianceApplication.Applications.ToList(), true),
                OrganizationName = complianceApplication.Organization.Name,
                ShowAccreditationReport = complianceApplication.ShowAccreditationReport.GetValueOrDefault(),
                IsReinspection =
                    complianceApplication.Applications.Any(x => x.InspectionSchedules.Count() > 1),
                HasQmRestriction = orgManager.HasQmRestriction(complianceApplication.OrganizationId)
            };

        }

        private bool HasFlag(List<ApplicationSectionItem> sections, List<ApplicationResponse> responsesWithFlag)
        {
            foreach (var section in sections.Where(x=>x.IsVisible))
            {
                if (section.Questions != null && section.Questions.Count > 0)
                {
                    if (
                        section.Questions.Any(
                            x => x.QuestionResponses.Any(y => responsesWithFlag.Any(z => z.Id == y.Id))))
                    {
                        return true;
                    }
                }

                if (section.Children != null && section.Children.Count > 0)
                {
                    var has = this.HasFlag(section.Children, responsesWithFlag);

                    if (has) return true;
                }
            }

            return false;
        }

        private bool HasRfiItems(List<ApplicationSectionItem> sections)
        {
            foreach (var section in sections)
            {
                if (section.Questions != null && section.Questions.Count > 0)
                {
                    if (section.Questions.Any(x => x.AnswerResponseStatusName == Constants.ApplicationResponseStatus.RFI))
                    {
                        return true;
                    }
                }

                if (section.Children != null && section.Children.Count > 0)
                {
                    var hasRfi = this.HasRfiItems(section.Children);

                    if (hasRfi) return true;
                }
            }

            return false;
        }

        private List<ApplicationSectionItem> Convert(DateTime? appSubmittedDate, List<ApplicationSectionItem> sections, List<ApplicationResponse> responses, List<ApplicationSectionQuestionAnswer> answers, List<User> users, List<ApplicationResponseComment> comments, List<AccreditationOutcome> outcomes, bool canSeeCitations)
        {
            foreach (var section in sections)
            {
                var status = Constants.ApplicationSectionStatusView.NotStarted;

                var anyQuestionNotAnswered = false;
                var responseCommentList = new List<ApplicationResponseComment>();

                if (section.Questions != null && section.Questions.Count > 0)
                {
                    foreach (var question in section.Questions)
                    {

                        var questionResponses =
                            responses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();

                        if (questionResponses.Count > 0)
                            responseCommentList = comments.Where(x => x.QuestionId == question.Id.Value).ToList();

                        if (question.HiddenBy != null && question.HiddenBy.Count > 0)
                        {
                            foreach (var display in question.HiddenBy)
                            {
                                var answer = answers.SingleOrDefault(x => x.Id == display.AnswerId);

                                if (answer != null)
                                {
                                    display.HiddenByQuestionId = answer.ApplicationSectionQuestionId;
                                }
                            }

                            question.IsHidden = question.HiddenBy.Any(q => responses.Any(
                                x =>
                                    x.ApplicationSectionQuestionAnswerId == q.AnswerId));

                        }

                        var qs = sections.Select(
                                x =>
                                    x.Questions.Select(
                                        y => y.Answers.Select(
                                            z => z.HidesQuestions.Where(a => a.QuestionId == question.Id)
                                                .Select(a => a.QuestionId))))
                            .ToList();

                        if (qs.Count > 0)
                        {

                        }

                        if (question.IsHidden)
                        {
                            section.IsVisible = false;
                            continue; //continue to next question
                        }
                        else if (questionResponses.Count > 0)
                        {
                            section.IsVisible = true;
                            status = Constants.ApplicationSectionStatusView.InProgress; //atleast one question was answered.
                            if (questionResponses.Count == 1)
                            {
                                var hasResponse = false;

                                var response = questionResponses.First();

                                question.Flag = response.Flag;
                                question.Comments = response.Comments;
                                question.CommentLastUpdatedBy = response.CommentLastUpdatedBy;
                                question.CommentDate = response.CommentLastUpdatedDate;
                                question.AnswerResponseStatusId = response.ApplicationResponseStatusId ?? 0;
                                question.AnswerResponseStatusName = response.ApplicationResponseStatus != null ? response.ApplicationResponseStatus.Name : string.Empty;
                                question.VisibleAnswerResponseStatusId = response.VisibleApplicationResponseStatusId ?? 0;
                                question.VisibleAnswerResponseStatusName = response.VisibleApplicationResponseStatus != null ? response.VisibleApplicationResponseStatus.Name : string.Empty;

                                var questionResponse = new QuestionResponse { UpdatedDate = response.UpdatedDate ?? response.CreatedDate.GetValueOrDefault()};
                                //If Question Response was Text or Date
                                if (!string.IsNullOrEmpty(response.Text))
                                {
                                    hasResponse = true;
                                    if (question.Type == Constants.QuestionTypes.DateRange)
                                    {
                                        var dates = response.Text.Split('-');
                                        questionResponse.FromDate = dates[0].Trim();
                                        questionResponse.ToDate = dates[1].Trim();
                                    }
                                    else
                                    {
                                        questionResponse.OtherText = response.Text;
                                    }
                                }
                                //If Question Response was Document
                                if (response.DocumentId != null)
                                {
                                    questionResponse.Document = ModelConversions.Convert(response.Document, true);
                                    hasResponse = true;
                                }
                                //If Question Response was user field.
                                if (response.UserId.HasValue)
                                {
                                    var user = users.SingleOrDefault(x => x.Id == response.UserId.GetValueOrDefault());

                                    questionResponse.UserId = response.UserId;
                                    questionResponse.User = ModelConversions.Convert(user, false);

                                    hasResponse = true;
                                }
                                //If Question response was checkbox OR radio
                                var answer =
                                    question.Answers.SingleOrDefault(
                                        x => x.Id == response.ApplicationSectionQuestionAnswerId);

                                if (answer != null) {
                                    answer.Selected = true;
                                    hasResponse = true;
                                }

                                //if (question.Type == Constants.QuestionTypes.RadioButtons ||
                                //    question.Type == Constants.QuestionTypes.Checkboxes)
                                //{
                                //    hasResponse = true;
                                //}

                                if (hasResponse)
                                {
                                    questionResponse.Id = response.Id;
                                    questionResponse.CoordinatorComment = response.CoorindatorComment;

                                    question.QuestionResponses.Add(questionResponse);
                                }
                                else
                                    anyQuestionNotAnswered = true;
                            }
                            else
                            {
                                if (question.Type == Constants.QuestionTypes.Multiple)
                                {
                                    foreach (var response in questionResponses)
                                    {
                                        question.Flag = response.Flag;
                                        question.Comments = response.Comments;
                                        question.CommentLastUpdatedBy = response.CommentLastUpdatedBy;
                                        question.CommentDate = response.CommentLastUpdatedDate;
                                        question.AnswerResponseStatusId = response.ApplicationResponseStatusId ?? 0;
                                        question.AnswerResponseStatusName = response.ApplicationResponseStatus != null ? response.ApplicationResponseStatus.Name : string.Empty;
                                        question.VisibleAnswerResponseStatusId = response.VisibleApplicationResponseStatusId ?? 0;
                                        question.VisibleAnswerResponseStatusName = response.VisibleApplicationResponseStatus != null ? response.VisibleApplicationResponseStatus.Name : string.Empty;

                                        var hasResponse = false;
                                        var questionResponse = new QuestionResponse();

                                        if (!string.IsNullOrEmpty(response.Text))
                                        {
                                            if (question.Type == Constants.QuestionTypes.DateRange)
                                            {
                                                var dates = response.Text.Split('-');
                                                questionResponse.FromDate = dates[0].Trim();
                                                questionResponse.ToDate = dates[1].Trim();
                                            }
                                            else
                                            {
                                                questionResponse.OtherText = response.Text;
                                            }

                                            hasResponse = true;
                                        }

                                        if (response.DocumentId.HasValue)
                                        {
                                            questionResponse.Document = ModelConversions.Convert(response.Document, true);
                                            hasResponse = true;
                                        }

                                        if (response.UserId.HasValue)
                                        {
                                            questionResponse.UserId = response.UserId;
                                            hasResponse = true;
                                        }

                                        var answer =
                                            question.Answers.SingleOrDefault(
                                                x => x.Id == response.ApplicationSectionQuestionAnswerId);

                                        if (answer != null)
                                        {
                                            answer.Selected = true;
                                            hasResponse = true;
                                        }

                                        if (hasResponse)
                                        {
                                            questionResponse.Id = response.Id;
                                            questionResponse.CoordinatorComment = response.CoorindatorComment;

                                            question.QuestionResponses.Add(questionResponse);

                                        }
                                        else anyQuestionNotAnswered = true;
                                    }

                                }
                                else
                                {  //if more then one responses and the question type is not Multiple then it is question type checkbox or radio.
                                    var responseFirst = questionResponses.First();
                                    question.Flag = responseFirst.Flag;
                                    question.Comments = responseFirst.Comments;
                                    question.CommentLastUpdatedBy = responseFirst.CommentLastUpdatedBy;
                                    question.CommentDate = responseFirst.CommentLastUpdatedDate;
                                    question.AnswerResponseStatusId = responseFirst.ApplicationResponseStatusId ?? 0;
                                    question.AnswerResponseStatusName = responseFirst.ApplicationResponseStatus != null ? responseFirst.ApplicationResponseStatus.Name : string.Empty;
                                    question.VisibleAnswerResponseStatusId = responseFirst.VisibleApplicationResponseStatusId ?? 0;
                                    question.VisibleAnswerResponseStatusName = responseFirst.VisibleApplicationResponseStatus != null ? responseFirst.VisibleApplicationResponseStatus.Name : string.Empty;


                                    foreach (var answer in questionResponses
                                        .Select(response => question.Answers
                                            .SingleOrDefault(x => x.Id == response.ApplicationSectionQuestionAnswerId))
                                        .Where(answer => answer != null))
                                    {
                                        answer.Selected = true;
                                    }
                                }
                            }

                            question.ApplicationResponseComments = ModelConversions.Convert(responseCommentList);

                            question.ResponseCommentsRFI =
                                question.ApplicationResponseComments.Where(
                                        x => x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.RFI &&
                                        (appSubmittedDate == null || System.Convert.ToDateTime(x.CreatedDate) == appSubmittedDate.Value))
                                    .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate))
                                    .ToList();

                            if (canSeeCitations || (outcomes != null && outcomes.Count > 0))
                            {
                                question.ResponseCommentsCitation =
                                    question.ApplicationResponseComments.Where(
                                            x =>
                                                x.CommentType != null &&
                                                x.CommentType.Name == Constants.CommentTypes.Citation)
                                        .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate))
                                        .ToList();
                                question.ResponseCommentsSuggestion =
                                    question.ApplicationResponseComments.Where(
                                            x =>
                                                x.CommentType != null &&
                                                x.CommentType.Name == Constants.CommentTypes.Suggestion)
                                        .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate))
                                        .ToList();
                            }
                            else
                            {
                                question.ResponseCommentsCitation = new List<ApplicationResponseCommentItem>();
                                question.ResponseCommentsSuggestion = new List<ApplicationResponseCommentItem>();
                            }
                            
                            //Coordinator = FACT Response
                            question.ResponseCommentsFactResponse =
                                question.ApplicationResponseComments.Where(
                                        x =>
                                            x.CommentType != null &&
                                            x.CommentType.Name == Constants.CommentTypes.FACTResponse)
                                    .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate))
                                    .ToList();

                            question.ResponseCommentsFactOnly =
                                question.ApplicationResponseComments.Where(
                                        x => x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.FACTOnly)
                                    .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate))
                                    .ToList();
                        }
                        else
                        {
                            anyQuestionNotAnswered = true;
                        }
                    }
                }

                if ((status == Constants.ApplicationSectionStatusView.InProgress) && !anyQuestionNotAnswered)
                    status = Constants.ApplicationSectionStatusView.Complete;

                section.Status = status; //set to default status

                if (section.Children.Count > 0)
                {
                    section.Children = this.Convert(appSubmittedDate, section.Children, responses, answers, users, comments, outcomes, canSeeCitations);

                    if (section.Children.All(x => x.Status == Constants.ApplicationSectionStatusView.Complete))
                    {
                        section.Status = Constants.ApplicationSectionStatusView.Complete;
                    }
                    else if (section.Children.Any(x => x.Status == Constants.ApplicationSectionStatusView.InProgress))
                    {
                        section.Status = Constants.ApplicationSectionStatusView.InProgress;
                    }
                }

            }

            return sections;
        }

        private List<ApplicationSectionItem> Convert(List<ApplicationSectionItem> sections, List<ApplicationResponseTrainee> responsesTrainee, List<ApplicationResponse> responses)
        {
            foreach (var section in sections)
            {
                foreach (var question in section.Questions)
                {
                    var traineeResponses = responsesTrainee.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();
                    var questionResponses = responses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();

                    if (traineeResponses.Count > 0)
                    {
                        var tResponse = traineeResponses.First();
                        question.AnswerResponseStatusId = tResponse.ApplicationResponseStatusId;
                        question.AnswerResponseStatusName = tResponse.ApplicationResponseStatus != null ? tResponse.ApplicationResponseStatus.Name : string.Empty;
                    }

                    if (questionResponses.Count > 0)
                    {
                        var qResponse = questionResponses.First();
                        question.Comments = qResponse.Comments;
                        question.CommentLastUpdatedBy = qResponse.CommentLastUpdatedBy;
                        question.CommentDate = qResponse.CommentLastUpdatedDate;
                        question.QuestionResponses.Add(new QuestionResponse { OtherText = qResponse.Text });
                    }

                    if (section.Children.Count > 0)
                    {
                        section.Children = this.Convert(section.Children, responsesTrainee, responses);
                    }
                }
            }

            return sections;
        }


        private List<ApplicationSectionItem> ConvertRFIView(List<ApplicationSectionItem> sections, Application application, ApplicationResponseStatus applicationRFIStatus, CommentType commentType, List<ApplicationResponse> responses, List<ApplicationResponseComment> comments, List<AccreditationOutcome> outcomes, bool canSeeCitations)
        {
            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();
            var accreditationOutcomeManager = this.container.GetInstance<AccreditationOutcomeManager>();

            comments = comments ?? applicationResponseCommentManager.GetByApplication(application.Id);

            outcomes = outcomes ?? accreditationOutcomeManager.GetByAppId(application.UniqueId);

            foreach (var section in sections)
            {
                var status = Constants.ApplicationSectionStatusView.NotStarted;
                var hasNotCompletedQuestion = false;

                foreach (var question in section.Questions)
                {
                    
                    var questionResponses = responses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();
                    var questionResponsesComments =
                        comments.Where(x => x.QuestionId == question.Id.GetValueOrDefault() && x.CommentTypeId == commentType.Id)
                            .OrderByDescending(x => x.CreatedDate)
                            .ToList();
                        
                        
                        //applicationResponseCommentManager.GetByApplicationIdQuestionId(application.Id, question.Id.Value, applicationRFIStatus.Id, commentType.Id).OrderByDescending(x => x.CreatedDate).ToList(); // 7 for RFI Comments , 1 for RFI type

                    if (application.ApplicationStatus.Name == Constants.ApplicationStatus.ForReview ||
                        application.ApplicationStatus.Name == Constants.ApplicationStatus.RFIReview ||
                        application.ApplicationStatus.Name == Constants.ApplicationStatus.InProgress)
                    {
                        questionResponsesComments =
                            questionResponsesComments.Where(
                                x =>
                                    (x.CommentType.Name != Constants.CommentTypes.RFI) ||
                                    x.CreatedDate < application.UpdatedDate).ToList();
                    }
                    

                    question.ApplicationResponseComments = ModelConversions.Convert(questionResponsesComments);

                    if (question.HiddenBy != null && question.HiddenBy.Count > 0)
                    {
                        //foreach (var display in question.HiddenBy)
                        //{
                        //    var answer = answerManager.GetById(display.AnswerId);

                        //    if (answer != null)
                        //    {
                        //        display.HiddenByQuestionId = answer.ApplicationSectionQuestionId;
                        //    }
                        //}

                        question.IsHidden = question.HiddenBy.Any(q => responses.Any(
                            x =>
                                x.ApplicationSectionQuestionAnswerId == q.AnswerId));

                    }

                    if (question.IsHidden)
                    {
                        hasNotCompletedQuestion = false;
                    }
                    else if (questionResponses.Count > 0)
                    {
                        status = Constants.ApplicationSectionStatusView.Complete;
                        if (questionResponses.Count == 1)
                        {
                            var hasResponse = false;
                            var response = questionResponses.First();

                            question.Flag = response.Flag;
                            question.Comments = response.Comments;
                            question.CommentLastUpdatedBy = response.CommentLastUpdatedBy;
                            question.CommentDate = response.CommentLastUpdatedDate;
                            question.AnswerResponseStatusId = response.ApplicationResponseStatusId ?? 0;
                            question.AnswerResponseStatusName = response.ApplicationResponseStatus != null ? response.ApplicationResponseStatus.Name : string.Empty;

                            var questionResponse = new QuestionResponse();

                            if (!string.IsNullOrEmpty(response.Text))
                            {
                                hasResponse = true;
                                if (question.Type == Constants.QuestionTypes.DateRange)
                                {
                                    var dates = response.Text.Split('-');
                                    questionResponse.FromDate = dates[0].Trim();
                                    questionResponse.ToDate = dates[1].Trim();
                                }
                                else
                                {
                                    questionResponse.OtherText = response.Text;
                                }
                            }

                            if (response.DocumentId.HasValue)
                            {
                                questionResponse.Document = ModelConversions.Convert(response.Document, true);
                                hasResponse = true;
                            }

                            if (response.UserId.HasValue)
                            {
                                questionResponse.UserId = response.UserId;
                                hasResponse = true;
                            }

                            var answer = question.Answers.SingleOrDefault(x => x.Id == response.ApplicationSectionQuestionAnswerId);

                            if (answer != null) answer.Selected = true;

                            if (hasResponse)
                            {
                                questionResponse.Id = response.Id;
                                questionResponse.CoordinatorComment = response.CoorindatorComment;
                                question.QuestionResponses.Add(questionResponse);
                            }
                        }
                        else
                        {
                            if (question.Type == Constants.QuestionTypes.Multiple)
                            {
                                foreach (var response in questionResponses)
                                {
                                    var hasResponse = false;
                                    var questionResponse = new QuestionResponse();

                                    if (!string.IsNullOrEmpty(response.Text))
                                    {
                                        if (question.Type == Constants.QuestionTypes.DateRange)
                                        {
                                            var dates = response.Text.Split('-');
                                            questionResponse.FromDate = dates[0].Trim();
                                            questionResponse.ToDate = dates[1].Trim();
                                        }
                                        else
                                        {
                                            questionResponse.OtherText = response.Text;
                                        }

                                        hasResponse = true;
                                    }

                                    if (response.DocumentId.HasValue)
                                    {
                                        questionResponse.Document = ModelConversions.Convert(response.Document, true);
                                        hasResponse = true;
                                    }

                                    if (response.UserId.HasValue)
                                    {
                                        questionResponse.UserId = response.UserId;
                                        hasResponse = true;
                                    }

                                    var answer =
                                            question.Answers.SingleOrDefault(
                                                x => x.Id == response.ApplicationSectionQuestionAnswerId);

                                    if (answer != null) answer.Selected = true;

                                    if (hasResponse)
                                    {
                                        questionResponse.Id = response.Id;
                                        questionResponse.CoordinatorComment = response.CoorindatorComment;
                                        question.QuestionResponses.Add(questionResponse);
                                    }
                                }

                            }
                            else
                            {
                                var responseFirst = questionResponses.First();
                                question.Flag = responseFirst.Flag;
                                question.Comments = responseFirst.Comments;
                                question.CommentLastUpdatedBy = responseFirst.CommentLastUpdatedBy;
                                question.CommentDate = responseFirst.CommentLastUpdatedDate;
                                question.AnswerResponseStatusId = responseFirst.ApplicationResponseStatusId ?? 0;
                                question.AnswerResponseStatusName = responseFirst.ApplicationResponseStatus != null
                                    ? responseFirst.ApplicationResponseStatus.Name
                                    : string.Empty;


                                foreach (var answer in questionResponses
                                    .Select(response => question.Answers
                                        .SingleOrDefault(x => x.Id == response.ApplicationSectionQuestionAnswerId))
                                    .Where(answer => answer != null))
                                {
                                    answer.Selected = true;
                                }
                            }
                        }

                        question.ResponseCommentsRFI = question.ApplicationResponseComments.Where(x => x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.RFI).OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate)).ToList();

                        if (canSeeCitations || (outcomes != null && outcomes.Count > 0))
                        {
                            question.ResponseCommentsCitation =
                                question.ApplicationResponseComments.Where(
                                        x => x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.Citation)
                                    .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate))
                                    .ToList();
                            question.ResponseCommentsSuggestion =
                                question.ApplicationResponseComments.Where(
                                        x =>
                                            x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.Suggestion)
                                    .OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate))
                                    .ToList();
                        }
                        else
                        {
                            question.ResponseCommentsCitation = new List<ApplicationResponseCommentItem>();
                            question.ResponseCommentsSuggestion = new List<ApplicationResponseCommentItem>();
                        }
                        
                        question.ResponseCommentsFactResponse = question.ApplicationResponseComments.Where(x => x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.FACTResponse).OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate)).ToList();
                        question.ResponseCommentsFactOnly = question.ApplicationResponseComments.Where(x => x.CommentType != null && x.CommentType.Name == Constants.CommentTypes.FACTOnly).OrderByDescending(x => System.Convert.ToDateTime(x.CreatedDate)).ToList();

                    }
                    else
                    {
                        hasNotCompletedQuestion = true;
                    }
                }

                section.Status = status;

                if (section.Children.Count > 0)
                {
                    section.Children = this.ConvertRFIView(section.Children, application, applicationRFIStatus, commentType, responses, comments, outcomes, canSeeCitations);

                    if (section.Children.All(x => x.Status == Constants.ApplicationSectionStatusView.Complete))
                    {
                        section.Status = Constants.ApplicationSectionStatusView.Complete;
                    }
                    else if (section.Children.Any(x => x.Status == Constants.ApplicationSectionStatusView.Partial))
                    {
                        section.Status = Constants.ApplicationSectionStatusView.Partial;
                    }
                }
                else
                {
                    if (hasNotCompletedQuestion)
                    {
                        section.Status = Constants.ApplicationSectionStatusView.Partial;
                    }
                }

                //if (section.Children.Count > 0)
                //{
                //    section.Children = this.ConvertRFIView(section.Children, application, applicationRFIStatus, commentType, responses);
                //}

            }

            //var applicationResponses = applicationResponseManager.GetApplicationResponses(application.UniqueId);

            //var sectionConverted = this.Convert(sections, applicationResponses);

            return sections;
        }

        public User UpdateCoordinator(Guid applicationUniqueId, Guid coordinatorId, string savedBy)
        {
            var userManager = this.container.GetInstance<UserManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var coordinator = userManager.GetCoordinator(coordinatorId);
            var application = applicationManager.GetByUniqueId(applicationUniqueId);

            if (application == null)
            {
                throw new Exception("Cannot find application");
            }

            application.CoordinatorId = coordinator.Id;
            application.UpdatedBy = savedBy;
            application.UpdatedDate = DateTime.Now;

            applicationManager.Save(application);

            return coordinator;
        }

        public async Task<User> UpdateCoordinatorAsync(Guid applicationUniqueId, Guid coordinatorId, string applicationStatus, string dueDate, string savedBy)
        {
            var userManager = this.container.GetInstance<UserManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var complianceAppManager = this.container.GetInstance<ComplianceApplicationManager>();
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();

            var coordinator = userManager.GetCoordinator(coordinatorId);
            var application = await applicationManager.GetByUniqueIdAsync(applicationUniqueId);

            if (application == null)
            {
                throw new Exception("Cannot find application");
            }

            if (!application.ApplicationType.Name.Contains("Compliance"))
                application.CoordinatorId = coordinator.Id;

            application.UpdatedBy = savedBy;
            application.UpdatedDate = DateTime.Now;

            if (!string.IsNullOrEmpty(dueDate))
                application.DueDate = System.Convert.ToDateTime(dueDate);

            await applicationManager.UpdateApplicationStatusAsync(application, application.OrganizationId, application.ApplicationTypeId, Int32.Parse(applicationStatus), "", null, savedBy);

            if (!string.IsNullOrEmpty(applicationStatus))
                application.ApplicationStatusId = System.Convert.ToInt32(applicationStatus);

            await applicationManager.SaveAsync(application);

            if (application.ComplianceApplicationId.HasValue)
            {
                var complianceApp = complianceAppManager.GetById(application.ComplianceApplicationId.Value);
                complianceApp.CoordinatorId = coordinator.Id;
                complianceApp.ApplicationStatusId = application.ApplicationStatusId;
                complianceApp.UpdatedBy = savedBy;
                complianceApp.UpdatedDate = DateTime.Now;

                foreach (var app in complianceApp.Applications)
                {
                    app.ApplicationStatusId = application.ApplicationStatusId;
                    app.UpdatedBy = savedBy;
                    app.UpdatedDate = DateTime.Now;
                }

                await complianceAppManager.SaveAsync(complianceApp);
            }

            return coordinator;
        }

        public void CancelApplication(Guid applicationUniqueId, string savedBy)
        {
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applicationStatus = applicationStatusManager.GetByName(Constants.ApplicationStatus.Cancelled);

            if (applicationStatus == null)
            {
                throw new Exception("Cannot find Cancelled Status");
            }

            var application = applicationManager.GetByUniqueId(applicationUniqueId);

            if (application == null)
            {
                throw new Exception("Cannot find application");
            }

            application.ApplicationStatusId = applicationStatus.Id;
            application.UpdatedBy = savedBy;
            application.UpdatedDate = DateTime.Now;

            applicationManager.Save(application);
        }

        public async Task CancelApplicationAsync(Guid applicationUniqueId, string savedBy)
        {
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var applicationStatus = await applicationStatusManager.GetByNameAsync(Constants.ApplicationStatus.Cancelled);

            if (applicationStatus == null)
            {
                throw new Exception("Cannot find Cancelled Status");
            }

            var application = applicationManager.GetByUniqueIdAnyStatus(applicationUniqueId);

            if (application == null)
            {
                throw new Exception("Cannot find application");
            }

            application.ApplicationStatusId = applicationStatus.Id;
            application.IsActive = false;
            application.UpdatedBy = savedBy;
            application.UpdatedDate = DateTime.Now;

            await applicationManager.SaveAsync(application);
        }

        public async Task<Application> CreateApplicationAsync(string organizationName, string applicationTypeName, Guid coordinatorId, string url, string dueDate, string savedBy, string factPortalEmailAddress)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var applicationVersionManager = this.container.GetInstance<ApplicationVersionManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var organizationAccreditationCycleManager = this.container.GetInstance<OrganizationAccreditationCycleManager>();
            var documentFacade = this.container.GetInstance<DocumentFacade>();
            OrganizationAccreditationCycle organizationAccreditationCycle = null;

            var coordinator = userManager.GetCoordinator(coordinatorId);

            if (coordinator == null || Constants.FactRoles.Names.All(x => x != coordinator.Role.Name))
            {
                throw new Exception("Invalid Coordinator");
            }

            var organizationTask = organizationManager.GetByNameAsync(organizationName);
            var applicationTypeTask = applicationTypeManager.GetByNameAsync(applicationTypeName);
            //var applicationStatusTask = applicationStatusManager.GetByNameAsync(Constants.ApplicationStatus.Pending);// InProgress replace by Pending
            var applicationStatusTask = applicationStatusManager.GetByNameAsync(Constants.ApplicationStatus.InProgress);
            var applicationVersions = await applicationVersionManager.GetByTypeAsync(applicationTypeName);

            await Task.WhenAll(organizationTask, applicationStatusTask, applicationTypeTask);

            if (applicationTypeTask.Result == null)
            {
                throw new Exception($"Cannot find application type {applicationTypeName}");
            }

            if (organizationTask.Result == null)
            {
                throw new Exception($"Cannot find organization {organizationName}");
            }

            var applicationVersion = applicationVersions.Where(x => x.IsActive == true).FirstOrDefault();

            if (applicationVersion == null)
            {
                throw new Exception($"Cannot find active version for {applicationTypeName}");
            }

            var applications = this.GetApplications(organizationName);

            var hasApp = applications.Any(x => x.ApplicationTypeId == applicationTypeTask.Result.Id && x.ApplicationStatusId != (int)Constants.ApplicationStatuses.Cancelled && x.ApplicationStatusId != (int)Constants.ApplicationStatuses.Complete);

            if (hasApp)
            {
                throw new Exception($"Organization {organizationName} already has a {applicationTypeName}.");
            }

            if (applicationTypeName == Constants.ApplicationTypes.Renewal)
            {
                organizationAccreditationCycle =
                    await organizationAccreditationCycleManager.RemoveCurrentAndAddCycleAsync(
                        organizationTask.Result.Id, DateTime.Now, savedBy);
            }
            else
            {
                organizationAccreditationCycle =
                    organizationTask.Result.OrganizationAccreditationCycles.FirstOrDefault(x => x.IsCurrent);
            }

            var application = new Application
            {
                ApplicationTypeId = applicationTypeTask.Result.Id,
                OrganizationId = organizationTask.Result.Id,
                ApplicationStatusId = applicationStatusTask.Result.Id,
                ApplicationVersionId = applicationVersion.Id,
                CoordinatorId = coordinator.Id,
                CycleNumber = organizationAccreditationCycle?.Number ?? 1,
                DueDate = DateTime.Parse(dueDate),
                CreatedDate = DateTime.Now,
                UniqueId = Guid.NewGuid(),
                IsActive = true,
                CreatedBy = savedBy
            };

            applicationManager.Add(application);

            application.Coordinator = coordinator;
            application.ApplicationStatus = applicationStatusTask.Result;
            application.Organization = organizationTask.Result;
            application.ApplicationType = applicationTypeTask.Result;

            if (applicationTypeTask.Result.Name == Constants.ApplicationTypes.Eligibility)
                NotifyApplicantAboutNewApplication(url, savedBy, application, factPortalEmailAddress);
            else if (applicationTypeTask.Result.Name == Constants.ApplicationTypes.Renewal)
            {
                documentFacade.MigrateDocumentLibrary(organizationTask.Result.Id, savedBy);
            }
            else if (applicationTypeTask.Result.Name == Constants.ApplicationTypes.Annual)
            {
                this.BuildAnnualEmail(application, factPortalEmailAddress);
            }

            return application;
        }

        private void BuildAnnualEmail(Application application, string factPortalEmailAddress)
        {
            var emailTemplateManager = this.container.GetInstance<EmailTemplateManager>();
            var orgManager = this.container.GetInstance<OrganizationManager>();

            var content = emailTemplateManager.GetContent(application.Id, null, "Annual Report Created",
                null, null, null);

            var org = orgManager.GetById(application.OrganizationId);

            var ccs = new List<string>();

            if (application.CoordinatorId.HasValue)
            {
                ccs.Add(application.Coordinator.EmailAddress);
            }

            var tos = new List<string> { factPortalEmailAddress };

            if (org.PrimaryUserId.HasValue)
            {
                tos.Add(org.PrimaryUser.EmailAddress);
            }

            tos.AddRange(org.Users.Where(
                        x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector)
                    .Select(x => x.User.EmailAddress)
                    .ToList());

            EmailHelper.Send(tos, ccs, $"FACT Annual Report Created for {org.Name}", content.Body);
        }

        private void NotifyApplicantAboutNewApplication(string url, string applicationEmail, Application application, string factPortalEmailAddress)
        {
            List<String> ccs = new List<string>();
            List<String> tos = new List<string>();

            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var templateHtml = url + "app/email.templates/eligibilityApplicationCreated.html";
            var appUrl = string.Format("{0}{1}", url, "#/Application?app=" + application.UniqueId);

            var html = WebHelper.GetHtml(templateHtml);
            html = html.Replace("{OrgName}", application.Organization.Name);
            html = html.Replace("{DueDate}", application.DueDate.Value.ToShortDateString());
            html = html.Replace("{URL}", appUrl);
            html = html.Replace("{Help}", @"http://www.factwebsite.org");

            tos.AddRange(application.Organization.Users.Where(
                        x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector)
                    .Select(x => x.User.EmailAddress)
                    .ToList());

            if (!string.IsNullOrEmpty(applicationEmail))
                ccs.Add(applicationEmail);

            if (!string.IsNullOrEmpty(factPortalEmailAddress))
                ccs.Add(factPortalEmailAddress);

            var coordinator = new User();
            var creds = "";

            if (application.Coordinator != null && !string.IsNullOrEmpty(application.Coordinator.EmailAddress))
            {
                coordinator = application.Coordinator;
                
                foreach (var cred in coordinator.UserCredentials)
                {
                    creds += cred.Credential.Name + ", ";
                }

                if (creds.Length > 2)
                {
                    creds = creds.Substring(0, creds.Length - 2);
                }

                ccs.Add(application.Coordinator.EmailAddress);
            }
                

            if (application.Organization.PrimaryUser != null && !string.IsNullOrEmpty(application.Organization.PrimaryUser.EmailAddress))
                ccs.Add(application.Organization.PrimaryUser.EmailAddress);

            var facilityDirectors = application.Organization.OrganizationFacilities.Select(x => x.Facility.FacilityDirector.EmailAddress).ToList();

            if (facilityDirectors.Any())
            {
                ccs.AddRange(facilityDirectors);
            }

            if (!string.IsNullOrEmpty(application.Organization.CcEmailAddresses))
            {
                var add = application.Organization.CcEmailAddresses.Split(';');

                ccs.AddRange(add.ToList());
            }

            html = html.Replace("{CoordinatorName}", $"{coordinator.FirstName} {coordinator.LastName}, {creds}");
            html = html.Replace("{CoordinatorTitle}", coordinator.Title);
            html = html.Replace("{CoordinatorPhoneNumber}", coordinator.WorkPhoneNumber);
            html = html.Replace("{CoordinatorEmailAddress}", coordinator.EmailAddress);

            EmailHelper.Send(tos, ccs, "FACT Eligibility Application is Available - " + application.Organization.Name, html);

        }

        public List<ApplicationVersion> GetActiveApplicationVersions()
        {
            var applicationVersionManager = this.container.GetInstance<ApplicationVersionManager>();
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var questionManager = this.container.GetInstance<ApplicationSectionQuestionManager>();
            var notApplicableManager = this.container.GetInstance<ApplicationQuestionNotApplicableManager>();

            if (ActiveVersions.Count > 0)
            {
                return ActiveVersions.Values.ToList();
            }

            var sections = applicationSectionManager.GetAllForActiveVersionsNoLazyLoad();
            var questions = questionManager.GetAllForActiveVersionsNoLazyLoad();
            var notApplicables = notApplicableManager.GetAllForActiveVersionNoLazyLoad();

            var versions = applicationVersionManager.GetActiveVersions(sections, questions, notApplicables);

            foreach (var version in versions)
            {
                ActiveVersions.Add(version.ApplicationTypeId, version);
            }

            return ActiveVersions.Values.ToList();
        }

        public void RemoveFromActiveVersionAndReload(int applicationTypeId, Guid versionId)
        {
            var version = this.GetApplicationVersion(versionId);

            if (ActiveVersions.Count > 0 && ActiveVersions.ContainsKey(applicationTypeId))
            {
                ActiveVersions[applicationTypeId] = version;
            }
            else
            {
                ActiveVersions.Add(applicationTypeId, version);
            }
        }

        public List<ApplicationVersion> GetSimpleActiveApplicationVersions()
        {
            var applicationVersionManager = this.container.GetInstance<ApplicationVersionManager>();

            return applicationVersionManager.GetActiveVersions();
        }

        public ApplicationVersion GetApplicationVersion(Guid versionId)
        {
            var applicationVersionManager = this.container.GetInstance<ApplicationVersionManager>();
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var questionManager = this.container.GetInstance<ApplicationSectionQuestionManager>();
            var notApplicableManager = this.container.GetInstance<ApplicationQuestionNotApplicableManager>();

            var sections = applicationSectionManager.GetAllForVersionNoLazyLoad(versionId);
            var questions = questionManager.GetAllForVersionNoLazyLoad(versionId);
            var notApplicables = notApplicableManager.GetAllForVersionNoLazyLoad(versionId);

            return applicationVersionManager.GetVersion(versionId, sections, questions, notApplicables);
        }

        public Task<List<ApplicationVersion>> GetActiveApplicationVersionsAsync()
        {
            var applicationVersionManager = this.container.GetInstance<ApplicationVersionManager>();

            return applicationVersionManager.GetActiveVersionsAsync();
        }

        public List<InspectionScheduleDetail> GetAllInspectionScheduleDetailsForApplication(Guid applicationUniqueId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();

            return inspectionScheduleDetailManager.GetAllActiveByApplication(applicationUniqueId);
        }

        public Task<List<InspectionScheduleDetail>> GetAllInspectionScheduleDetailsForApplicationAsync(
            Guid applicationUniqueId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();

            return inspectionScheduleDetailManager.GetAllActiveByApplicationAsync(applicationUniqueId);
        }

        public List<InspectionScheduleDetail> GetAllInspectionScheduleDetailsForComplianceApplication(Guid complianceApplicationId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();

            var schedules = inspectionScheduleManager.GetAllForCompliance(complianceApplicationId);

            if (schedules != null && schedules.Count > 0)
            {
                var id = schedules.Select(x => x.Id).Max();

                var details = inspectionScheduleDetailManager.GetAllActiveByComplianceApplication(complianceApplicationId);

                var dets = details.Where(x => x.InspectionScheduleId == id && x.IsActive && !x.IsArchive).ToList();

                dets = dets.OrderBy(x => Comparer.OrderSite(x.Site)).ToList();

                return dets;
            }

            return null;
        }

        private void HideQuestionsOutofScope(List<ApplicationSectionItem> sections, List<SiteScopeType> scopeTypeList)
        {
            foreach (var section in sections)
            {
                section.Questions = section.Questions.Where(
                        question =>
                            question != null && question.ScopeTypes != null &&
                            scopeTypeList.Any(x => question.ScopeTypes.Any(y => y != null && y.ScopeTypeId == x.ScopeTypeId)))
                        .ToList();

                if (section.Children.Count > 0)
                {
                    HideQuestionsOutofScope(section.Children, scopeTypeList);
                }
            }
        }

        public void BulkUpdateApplicationResponseStatus(ApplicationSectionItem section, int fromStatus, int toStatus, string updatedBy, Guid userId, Guid appUniqueId)
        {
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            
            var application = this.GetApplication(appUniqueId);

            if (application == null)
            {
                throw new Exception("Cannot find application. Please contact support.");
            }

            var isTrainee = false;

            if (application.ComplianceApplicationId != null)
            {
                var schedules = inspectionScheduleManager.GetAllForCompliance(application.ComplianceApplicationId.Value);

                foreach (var sched in schedules)
                {
                    var found =
                        sched.InspectionScheduleDetails.Any(
                            x =>
                                x.UserId == userId &&
                                x.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees);

                    if (found)
                    {
                        isTrainee = true;
                        break;
                    }
                }
            }
            else
            {
                if (application.InspectionSchedules != null && application.InspectionSchedules.Count > 0)
                {
                    foreach (var sched in application.InspectionSchedules)
                    {
                        var found =
                            sched.InspectionScheduleDetails.Any(
                                x =>
                                    x.SiteId == application.SiteId && x.UserId == userId &&
                                    x.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees);

                        if (found)
                        {
                            isTrainee = true;
                            break;
                        }
                    }
                }
            }

            

            if (isTrainee)
            {
                this.UpdateApplicationResponseForSectionTrainee(section, fromStatus, toStatus, updatedBy, application.Id);
            }
            else
            {
                UpdateApplicationResponseForSection(section, fromStatus, toStatus, updatedBy, application.Id);
            }
        }

        private void UpdateApplicationResponseForSection(ApplicationSectionItem section, int fromStatus, int toStatus, string updatedBy, int applicationId)
        {
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();

            applicationResponseManager.BulkUpdate(applicationId, section.Id.GetValueOrDefault(), fromStatus, toStatus, updatedBy);
        }

        private void UpdateApplicationResponseForSectionTrainee(ApplicationSectionItem section, int fromStatus, int toStatus, string updatedBy, int applicationId)
        {
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseTraineeManager>();

            applicationResponseManager.BulkUpdate(applicationId, section.Id.GetValueOrDefault(), fromStatus, toStatus, updatedBy);
        }

        public void MultiSiiteView(string appUniqueId, string siteId, string email)
        {
            throw new NotImplementedException();
        }

        public void Deactivate(Guid app, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationManager>();

            manager.Deactivate(app, updatedBy);
        }

        public async Task DeactivateAsync(Guid app, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationManager>();

            await manager.DeactivateAsync(app, updatedBy);
        }

        public void ChangeRfiFollowupResponses(int organizationId, string updatedBy)
        {
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();
            var applicationResponseStatusManager = this.container.GetInstance<ApplicationResponseStatusManager>();
            var appManager = this.container.GetInstance<ApplicationManager>();
            var appStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var compAppManager = this.container.GetInstance<ComplianceApplicationManager>();

            var rfiAppStatus = appStatusManager.GetByName(Constants.ApplicationStatus.RFI);

            var statuses = applicationResponseStatusManager.GetAll();

            var rfiStatus = statuses.SingleOrDefault(x => x.Name == Constants.ApplicationResponseStatus.RFI);
            var rfiFollowupStatus =
                statuses.SingleOrDefault(x => x.Name == Constants.ApplicationResponseStatus.RfiFollowup);

            if (rfiStatus == null || rfiFollowupStatus == null)
            {
                throw new Exception("Cannot find statuses");
            }

            var responses = applicationResponseManager.GetApplicationResponses(organizationId, rfiFollowupStatus.Id);

            foreach (var response in responses)
            {
                response.ApplicationResponseStatusId = rfiStatus.Id;
                response.VisibleApplicationResponseStatusId = rfiAppStatus.Id;
                response.UpdatedDate = DateTime.Now;
                response.UpdatedBy = updatedBy;
                applicationResponseManager.BatchSave(response);
            }

            if (responses.Count > 0)
            {
                applicationResponseManager.SaveChanges();

                var appId = responses.First().ApplicationId;

                var compAppId = this.GetComplianceApplicationIdFromAppId(appId);

                if (compAppId != null)
                {
                    var apps = appManager.GetByComplianceApplicationId(compAppId.Value);

                    if (apps.Count > 0)
                    {
                        foreach (var app in apps)
                        {
                            app.ApplicationStatusId = rfiAppStatus.Id;
                            app.UpdatedDate = DateTime.Now;
                            app.RFIDueDate = DateTime.Now.AddMonths(1);
                            app.DueDate = DateTime.Now.AddMonths(1);
                            app.UpdatedBy = updatedBy;
                            appManager.BatchSave(app);
                        }

                        appManager.SaveChanges();
                    }

                    var compApp = compAppManager.GetById(compAppId.Value);
                    compApp.ApplicationStatusId = rfiAppStatus.Id;
                    compApp.UpdatedDate = DateTime.Now;
                    compApp.UpdatedBy = updatedBy;
                    compAppManager.Save(compApp);
                }
            }

        }

        private Guid? GetComplianceApplicationIdFromAppId(int applicationId)
        {
            var appManager = this.container.GetInstance<ApplicationManager>();

            var app = appManager.GetById(applicationId);

            return app?.ComplianceApplicationId;
        }

        public List<CbTotal> GetCbTotals(Guid complianceApplicationId)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();

            return complianceApplicationManager.GetCbTotals(complianceApplicationId);
        }

        public List<CtTotal> GetCtTotals(Guid complianceApplicationId)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();

            return complianceApplicationManager.GetCtTotals(complianceApplicationId);
        }

        public bool HasQmRestrictions(int orgId)
        {
            var orgManager = this.container.GetInstance<OrganizationManager>();

            return orgManager.HasQmRestriction(orgId);
        }

        public List<ComplianceApplicationSubmitApproval> GetApprovalsByCompliance(Guid complianceApplicationId)
        {
            var compAppApprovalManager = this.container.GetInstance<ComplianceApplicationSubmitApprovalManager>();

            return compAppApprovalManager.GetByCompliance(complianceApplicationId);
        }

        public List<ApplicationSubmitApproval> GetApprovals(Guid applicationUniqueId)
        {
            var applicationSubmitApprovalManager = this.container.GetInstance<ApplicationSubmitApprovalManager>();

            return applicationSubmitApprovalManager.GetAllForApplication(applicationUniqueId);
        }

        public List<AppReportModel> GetApplicationReport(Guid complianceApplicationId, string siteName)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            return applicationManager.GetApplicationReport(complianceApplicationId, siteName);
        }

        public List<AppReportModel> GetAppReport(Guid appUniqueId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            return applicationManager.GetAppReport(appUniqueId);
        }

        public List<BlankReport> GetBlankReport(int applicationTypeId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            return applicationManager.GetBlankReport(applicationTypeId);

        }

        public ComplianceApplicationInspectionDetail GetComplianceApplicationInspectionDetails(Guid complianceApplicationId)
        {
            var manager = this.container.GetInstance<ComplianceApplicationInspectionDetailManager>();

            var records = manager.GetByCompApp(complianceApplicationId);

            return records.Count == 0 ? null : records.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        }

        public ComplianceApplicationInspectionDetail GetByInspectionSched(int inspectionScheduleId)
        {
            var manager = this.container.GetInstance<ComplianceApplicationInspectionDetailManager>();

            return manager.GetByInspectionSched(inspectionScheduleId);
        }

        public ComplianceApplicationInspectionDetail SaveCompAppInspectionDetail(CompAppInspectionDetail detail, string createdBy)
        {
            var manager = this.container.GetInstance<ComplianceApplicationInspectionDetailManager>();

            if (detail.Id.HasValue)
            {
                var inspDetail = manager.GetById(detail.Id.Value);

                inspDetail.InspectorsNeeded = detail.InspectorsNeeded;
                inspDetail.ClinicalNeeded = detail.ClinicalNeeded;
                inspDetail.AdultSimpleExperienceNeeded = detail.AdultSimpleExperienceNeeded;
                inspDetail.AdultMediumExperienceNeeded = detail.AdultMediumExperienceNeeded;
                inspDetail.AdultAnyExperienceNeeded = detail.AdultAnyExperienceNeeded;
                inspDetail.PediatricSimpleExperienceNeeded = detail.PediatricSimpleExperienceNeeded;
                inspDetail.PediatricMediumExperienceNeeded = detail.PediatricMediumExperienceNeeded;
                inspDetail.PediatricAnyExperienceNeeded = detail.PediatricAnyExperienceNeeded;
                inspDetail.Comments = detail.Comments;
                inspDetail.UpdatedBy = createdBy;
                inspDetail.UpdatedDate = DateTime.Now;

                manager.Save(inspDetail);

                return inspDetail;
            }
            else
            {
                var inspDetail = new ComplianceApplicationInspectionDetail
                {
                    Id = Guid.NewGuid(),
                    ComplianceApplicationId = detail.ComplianceApplicationId,
                    InspectionScheduleId = detail.InspectionScheduleId,
                    InspectorsNeeded = detail.InspectorsNeeded,
                    ClinicalNeeded = detail.ClinicalNeeded,
                    AdultSimpleExperienceNeeded = detail.AdultSimpleExperienceNeeded,
                    AdultMediumExperienceNeeded = detail.AdultMediumExperienceNeeded,
                    AdultAnyExperienceNeeded = detail.AdultAnyExperienceNeeded,
                    PediatricSimpleExperienceNeeded = detail.PediatricSimpleExperienceNeeded,
                    PediatricMediumExperienceNeeded = detail.PediatricMediumExperienceNeeded,
                    PediatricAnyExperienceNeeded = detail.PediatricAnyExperienceNeeded,
                    Comments = detail.Comments,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now
                };

                manager.Add(inspDetail);

                return inspDetail;
            }

            
        }

        public bool AppHasRfis(Guid complianceApplicationId)
        {
            var manager = this.container.GetInstance<ComplianceApplicationManager>();

            return manager.AppHasRfis(complianceApplicationId);
        }

    }
}