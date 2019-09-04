using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationManager : BaseManager<ApplicationManager, IApplicationRepository, Application>
    {
        private readonly ApplicationStatusHistoryManager applicationStatusHistoryManager;

        public ApplicationManager(IApplicationRepository repository, ApplicationStatusHistoryManager historyManager) : base(repository)
        {
            this.applicationStatusHistoryManager = historyManager;
        }

        public List<Application> GetAllApplications()
        {
            return base.Repository.GetAllApplications();
        }

        /// <summary>
        /// Gets an application by its unique id
        /// </summary>
        /// <param name="id">Unique Id of the record</param>
        /// <returns>Application entity object</returns>
        public Application GetByUniqueId(Guid id, bool includeResponses = true, bool includeNotApplicables = true)
        {
            return base.Repository.GetByUniqueId(id, includeResponses, includeNotApplicables);
        }

        public Application GetByUniqueIdIgnoreActive(Guid id)
        {
            return base.Repository.GetByUniqueIdIgnoreActive(id);
        }

        /// <summary>
        /// Gets an application by its unique id asynchronously
        /// </summary>
        /// <param name="id">Unique Id of the record</param>
        /// <returns>Application entity object</returns>
        public Task<Application> GetByUniqueIdAsync(Guid id)
        {
            return base.Repository.GetByUniqueIdAsync(id);
        }

        public List<Application> GetAllForUser(Guid userId)
        {
            return base.Repository.GetAllForUser(userId);
        }

        public Task<List<Application>> GetAllForUserAsync(Guid userId)
        {
            return base.Repository.GetAllForUserAsync(userId);
        }

        public List<Application> GetAllForConsultant(Guid userId)
        {
            return base.Repository.GetAllForConsultant(userId);
        }

        public Task<List<Application>> GetAllForConsultantAsync(Guid userId)
        {
            return base.Repository.GetAllForConsultantAsync(userId);
        }

        public ApplicationStatus GetApplicationStatusByUniqueId(Guid appId)
        {
            return base.Repository.GetApplicationStatusByUniqueId(appId);
        }

        public Application getApplicationById(int id)
        {
            return base.Repository.GetApplicationById(id);
        }

        public Application GetByUniqueIdAnyStatus(Guid id)
        {
            return base.Repository.GetByUniqueIdAnyStatus(id);
        }
        /// <summary>
        /// Gets all applications by organization id, application type id, or neither
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of applications</returns>
        public List<Application> GetAllByOrganizationOrApplicationType(int? organizationId, int? applicationTypeId)
        {
            LogMessage("GetAllByOrganizationOrApplicationType (ApplicationManager)");

            if (organizationId.HasValue && applicationTypeId.HasValue)
            {
                return new List<Application>
                {
                    base.Repository.GetByOrganizationAndType(organizationId.Value, applicationTypeId.Value)
                };
            }
            else if (organizationId.HasValue)
            {
                return base.Repository.GetAllByOrganization(organizationId.Value);
            }
            else if (applicationTypeId.HasValue)
            {
                return base.Repository.GetAllByApplicationType(applicationTypeId.Value);
            }

            return base.Repository.GetNotCancelled();
        }

        /// <summary>
        /// Gets all applications against provided compliance id
        /// </summary>
        /// <param name="complianceId"></param>
        /// <returns></returns>
        public Task<List<Application>> GetApplicationsByComplianceId(string complianceId)
        {
            LogMessage("GetApplicationsByComplianceId (ApplicationManager)");
            return base.Repository.GetApplicationsByComplianceId(complianceId);
        }

        /// <summary>
        /// Gets application by organization id and application type id
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>        
        /// <param name="applicationTypeId">Id of the application type</param>        
        /// <returns>Application</returns>
        public Application GetByOrganizationAndApplicationType(int organizationId, int applicationTypeId)
        {
            LogMessage("GetByOrganizationAndApplicationType (ApplicationManager)");

            return base.Repository.GetByOrganizationAndType(organizationId, applicationTypeId);
        }

        public Application GetByOrganizationAndApplicationTypeIgnoreActive(int organizationId, int applicationTypeId)
        {
            LogMessage("GetByOrganizationAndTypeIgnoreActive (ApplicationManager)");

            return base.Repository.GetByOrganizationAndTypeIgnoreActive(organizationId, applicationTypeId);
        }

        /// <summary>
        /// Gets application by organization id and application type id
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>        
        /// <param name="applicationTypeId">Id of the application type</param>        
        /// <returns>Application</returns>
        public async Task<Application> GetByOrganizationAndApplicationTypeAsync(int organizationId, int applicationTypeId)
        {
            LogMessage("GetByOrganizationAndApplicationTypeAsync (ApplicationManager)");

            return await base.Repository.GetByOrganizationAndTypeAsync(organizationId, applicationTypeId);
        }

        /// <summary>
        /// Gets all applications by organization id, application type id, or neither
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>        
        /// <param name="applicationTypeId">Id of the application type</param>        
        /// <returns>Collection of applications</returns>
        public async Task<List<Application>> GetAllByOrganizationOrApplicationTypeAsync(int? organizationId, int? applicationTypeId)
        {
            LogMessage("GetAllByOrganizationOrApplicationTypeAsync (ApplicationManager)");

            if (organizationId.HasValue && applicationTypeId.HasValue)
            {
                return new List<Application>
                {
                    await base.Repository.GetByOrganizationAndTypeAsync(organizationId.Value, applicationTypeId.Value)
                };
            }
            else if (organizationId.HasValue)
            {
                return await base.Repository.GetAllByOrganizationAsync(organizationId.Value);
            }
            else if (applicationTypeId.HasValue)
            {
                return await base.Repository.GetAllByApplicationTypeAsync(applicationTypeId.Value);
            }

            return await base.Repository.GetNotCancelledAsync();
        }

        /// <summary>
        /// Update old application version with new ones in application table
        /// </summary>
        /// <param name="oldApplicationVersion"></param>
        /// <param name="newApplicationVersion"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public async Task UpdateCurrentApplicaitonsVersion(ApplicationVersion oldApplicationVersion, ApplicationVersion newApplicationVersion, string updatedBy)
        {
            var applications = this.GetAllByOrganizationOrApplicationType(null, oldApplicationVersion.ApplicationTypeId);

            foreach (var record in applications.Where(x => x.ApplicationVersionId == oldApplicationVersion.Id))
            {
                record.ApplicationVersionId = newApplicationVersion.Id;
                record.UpdatedBy = updatedBy;
                record.UpdatedDate = DateTime.Now;
                base.Repository.BatchSave(record);
            }

            await base.Repository.SaveChangesAsync();
        }

        public List<CoordinatorApplication> GetCoordinatorApplications(Guid? coordinatorId)
        {
            return base.Repository.GetCoordinatorApplications(coordinatorId);
        }

        public List<Application> GetInspectorApplications(Guid inspectorUserId)
        {
            return base.Repository.GetInspectorApplications(inspectorUserId);
        }

        public Task<List<Application>> GetInspectorApplicationsAsync(Guid inspectorUserId)
        {
            return base.Repository.GetInspectorApplicationsAsync(inspectorUserId);
        }

        public void SetEligibilityApplicationApprovalStatus(Guid? id, string savedBy)
        {
            LogMessage("SetEligibilityApplicationApprovalStatus (ApplicationManager)");

            var applications = base.Repository.GetByComplianceApplicationId(id.Value);

            foreach (var application in applications)
            {
                application.ApplicationStatusId = 5;
                application.UpdatedBy = savedBy;
                application.UpdatedDate = DateTime.Now;

                this.Repository.Save(application);
            }
        }

        //public async Task SaveApplicationAsync(int organizationId, int applicationTypeId, List<ApplicationSectionItem> sections, string updatedBy, int? roleId)
        //{
        //    LogMessage("SaveApplicationAsync (ApplicationManager)");

        //    var application = await base.Repository.GetByOrganizationAndTypeAsync(organizationId, applicationTypeId);

        //    if (application == null)
        //    {
        //        throw new ObjectNotFoundException("Cannot find application");
        //    }

        //    var responses = this.GetApplicationResponses(sections, updatedBy, roleId);

        //    foreach (var response in responses)
        //    {
        //        application.ApplicationResponses.Add(response);
        //    }

        //    await this.Repository.SaveAsync(application);
        //}

        public void SaveApplicationSection(int organizationId, int applicationTypeId, IEnumerable<ApplicationResponse> responses, string updatedBy, int? roleId)
        {
            LogMessage("SaveApplicationSection (ApplicationManager)");

            var application = base.Repository.GetByOrganizationAndType(organizationId, applicationTypeId);

            if (application == null)
            {
                throw new ObjectNotFoundException("Cannot find application");
            }

            foreach (var response in responses)
            {
                application.ApplicationResponses.Add(response);
            }

            this.Repository.Save(application);
        }

        public void SaveApplicationSection(Guid applicationUniqueId, IEnumerable<ApplicationResponse> responses,
            string updatedBy, int? roleId)
        {
            LogMessage("SaveApplicationSection (ApplicationManager)");

            var application = base.Repository.GetByUniqueId(applicationUniqueId, false);

            if (application == null)
            {
                throw new ObjectNotFoundException("Cannot find application");
            }

            foreach (var response in responses)
            {
                //if (application.ApplicationResponses.Any(x => x.ApplicationSectionQuestionId == response.ApplicationSectionQuestionId && x.Text == response.Text))
                if (
                    application.ApplicationResponses.Any(
                        x =>
                            x.ApplicationSectionQuestionId == response.ApplicationSectionQuestionId &&
                            x.ApplicationSectionQuestionAnswerId == response.ApplicationSectionQuestionAnswerId &&
                            x.DocumentId == response.DocumentId &&
                            x.UserId == response.UserId))
                {
                    continue;
                }

                response.UpdatedBy = updatedBy;
                response.UpdatedDate = DateTime.Now;

                application.ApplicationResponses.Add(response);
                this.Repository.Save(application);
            }
        }

        public void SaveApplicationSectionTrainee(Guid appUniqueId, ApplicationSectionResponse section, string updatedBy, int? roleId)
        {
            LogMessage("SaveApplicationSection (ApplicationManager)");

            var application = base.Repository.GetByUniqueId(appUniqueId, false, false, true);

            if (application == null)
            {
                throw new ObjectNotFoundException("Cannot find application");
            }

            foreach (var question in section.Questions.Where(x=>x.AnswerResponseStatusId != 0))
            {
                var applicationResponseTrainee = new ApplicationResponseTrainee
                {
                    ApplicationId = application.Id,
                    ApplicationSectionQuestionId = question.Id.GetValueOrDefault(),
                    ApplicationResponseStatusId = question.AnswerResponseStatusId,
                    CreatedBy = updatedBy,
                    CreatedDate = DateTime.Now
                };
                //applicationResponseTrainee.ApplicationSectionQuestionAnswerId = question.an


                application.ApplicationResponsesTrainee.Add(applicationResponseTrainee);
            }

            this.Repository.Save(application);
        }

        public async Task SaveApplicationSectionAsync(int organizationId, int applicationTypeId, IEnumerable<ApplicationResponse> responses, string updatedBy, int? roleId)
        {
            LogMessage("SaveApplicationSectionAsync (ApplicationManager)");

            var application = await base.Repository.GetByOrganizationAndTypeAsync(organizationId, applicationTypeId);

            if (application == null)
            {
                throw new ObjectNotFoundException("Cannot find application");
            }

            if (application.ApplicationResponses == null)
            {
                application.ApplicationResponses = new List<ApplicationResponse>();
            }

            foreach (var response in responses)
            {
                application.ApplicationResponses.Add(response);
            }

            await this.Repository.SaveAsync(application);
        }

        public async Task UpdateApplicationStatusAsync(Application application, int organizationId, int applicationTypeId, int applicationStatusId, string applicationStatusName, DateTime? dueDate, string updatedBy)
        {
            LogMessage("UpdateApplicationStatusAsync (ApplicationManager)");

            if (application == null)
            {
                throw new ObjectNotFoundException("Cannot find application");
            }

            var applicationStatusHistory = new ApplicationStatusHistory();

            applicationStatusHistory.ApplicationId = application.Id;
            applicationStatusHistory.ApplicationStatusIdOld = application.ApplicationStatusId;
            applicationStatusHistory.ApplicationStatusIdNew = applicationStatusId;
            applicationStatusHistory.CreatedBy = updatedBy;
            applicationStatusHistory.CreatedDate = DateTime.Now;

            applicationStatusHistoryManager.Add(applicationStatusHistory);

            application.ApplicationStatusId = applicationStatusId;

            if (applicationStatusId == 8 || applicationStatusId == 10)
            {
                application.SubmittedDate = DateTime.Now;
            }

            switch (applicationStatusName)
            {
                case Constants.ApplicationStatus.ForReview:
                    foreach (
                        var response in
                        application.ApplicationResponses.Where(x => x.ApplicationResponseStatusId == null))
                    {
                        response.ApplicationResponseStatusId = (int)Constants.ApplicationResponseStatuses.ForReview;
                    }
                    break;
                case Constants.ApplicationStatus.RFI:
                    foreach (
                    var response in
                    application.ApplicationResponses.Where(
                        x =>
                            x.ApplicationResponseStatusId != null &&
                            x.ApplicationResponseStatus.Name == Constants.ApplicationResponseStatus.RFI &&
                            !x.VisibleApplicationResponseStatusId.HasValue))
                    {
                        response.VisibleApplicationResponseStatusId = response.ApplicationResponseStatusId;
                    }

                    if (dueDate.HasValue)
                    {
                        application.RFIDueDate = dueDate;
                    }
                    break;
            }

            if (dueDate.HasValue)
            {
                application.DueDate = dueDate;
            }

            application.UpdatedBy = updatedBy;
            application.UpdatedDate = DateTime.Now;

            await this.Repository.SaveAsync(application);

        }

        public void UpdateApplicationStatus(int applicationId, int applicationStatusId, string updatedBy)
        {
            var application = this.GetById(applicationId);

            if (application == null)
            {
                throw new ObjectNotFoundException("Cannot find application");
            }

            var applicationStatusHistory = new ApplicationStatusHistory
            {
                ApplicationId = application.Id,
                ApplicationStatusIdOld = application.ApplicationStatusId,
                ApplicationStatusIdNew = applicationStatusId,
                CreatedBy = updatedBy,
                CreatedDate = DateTime.Now
            };

            this.applicationStatusHistoryManager.Add(applicationStatusHistory);

            application.ApplicationStatusId = applicationStatusId;

            if (applicationStatusId == 8 || applicationStatusId == 10)
            {
                application.SubmittedDate = DateTime.Now;
            }

            application.UpdatedBy = updatedBy;
            application.UpdatedDate = DateTime.Now;

            this.Repository.Save(application);
        }

        public void UpdateApplicationStatus(int organizationId, int applicationTypeId, int applicationStatusId, string updatedBy)
        {
            LogMessage("UpdateApplicationStatus (ApplicationManager)");

            var application = base.Repository.GetByOrganizationAndType(organizationId, applicationTypeId);

            if (application == null)
            {
                throw new ObjectNotFoundException("Cannot find application");
            }

            var applicationStatusHistory = new ApplicationStatusHistory();
            applicationStatusHistory.ApplicationId = application.Id;
            applicationStatusHistory.ApplicationStatusIdOld = application.ApplicationStatusId;
            applicationStatusHistory.ApplicationStatusIdNew = applicationStatusId;
            applicationStatusHistory.CreatedBy = updatedBy;
            applicationStatusHistory.CreatedDate = DateTime.Now;

            applicationStatusHistoryManager.Add(applicationStatusHistory);

            application.ApplicationStatusId = applicationStatusId;

            if (applicationStatusId == 8 || applicationStatusId == 10)
            {
                application.SubmittedDate = DateTime.Now;
            }

            application.UpdatedBy = updatedBy;
            application.UpdatedDate = DateTime.Now;

            this.Repository.Save(application);
        }

        /// <summary>
        /// Checks the isExpected answer flag and update the status of response
        /// </summary>
        /// <param name="applicationResponses"></param>
        /// <returns></returns>
        public List<ApplicationResponse> SetExpectedAnswers(List<ApplicationResponse> applicationResponses, int applicationResponseStatusId, string updatedBy)
        {
            foreach (var response in applicationResponses.Where(x=>x.ApplicationSectionQuestionAnswerId.HasValue && x.ApplicationResponseStatusId != applicationResponseStatusId))
            {
                if (!response.ApplicationSectionQuestion.Answers.Any(
                    x =>
                        x.IsExpectedAnswer.GetValueOrDefault() &&
                        response.ApplicationSectionQuestionAnswerId == x.Id)) continue;

                response.ApplicationResponseStatusId = applicationResponseStatusId;
                response.UpdatedDate = DateTime.Now;
                response.UpdatedBy = updatedBy;
            }

            return applicationResponses;

        }

        /// <summary>
        /// Eligibility applicaiton received
        /// </summary>
        /// <param name="url">Url of the current server</param>
        public void EligibilityApplicationReceivedEmail(string url, string orgName, string staffEmailList, string factPortalEmailAddress, string applicationUniqueId, User coordinator, string applicationType, bool isCompliance = false, Guid? compAppId = null)
        {
            LogMessage("EligibilityApplicationReceivedEmailAsync (ApplicationManager)");

            var reminderHtml = url + "app/email.templates/eligibilityApplicationReceived.html";
            var reminderUrl = string.Format("{0}{1}", url, "#/Application?app=" + applicationUniqueId);
            var title = $"{applicationType} Received";

            if (isCompliance)
            {
                reminderUrl = $"{url}#/Reviewer/View?app={applicationUniqueId}&c={compAppId}";
                title = "Compliance Application Received";
            }

            title = title.Replace("Application", "Submission");
            title += " - " + orgName;

            var html = WebHelper.GetHtml(reminderHtml);
            html = html.Replace("{Url}", reminderUrl);
            html = html.Replace("{OrgName}", orgName);
            html = html.Replace("{CoordinatorName}",
                string.Format("{0} {1}", coordinator.FirstName, coordinator.LastName));
            html = html.Replace("{CoordinatorCredentials}",
                coordinator.UserCredentials != null ? string.Join(", ",  coordinator.UserCredentials.Select(x => x.Credential.Name)) : "");
            html = html.Replace("{CoordinatorTitle}", coordinator.Title);
            html = html.Replace("{CoordinatorPhoneNumber}", coordinator.PreferredPhoneNumber);
            html = html.Replace("{CoordinatorEmailAddress}", coordinator.EmailAddress);

            EmailHelper.Send(staffEmailList, factPortalEmailAddress, title, html);

        }

        public void ComplianceApplicationSubmittedEmail(string url, string userEmail, string cc, ComplianceApplication complianceApplication)
        {
            LogMessage("ComplianceApplicationSubmittedEmail (ApplicationManager)");

            var app = complianceApplication.Applications.FirstOrDefault();

            var coordinator = complianceApplication.Coordinator ??
                              app?.Coordinator ?? new User();

            var reminderHtml = url + "app/email.templates/complianceApplicationSubmitted.html";

            var html = WebHelper.GetHtml(reminderHtml);
            html = html.Replace("{Url}", $"{url}#/Compliance?app={app?.UniqueId}&c={complianceApplication.Id}");
            html = html.Replace("{AppType}", "Compliance Application");
            html = html.Replace("{CoordinatorName}",
                string.Format("{0} {1}", coordinator.FirstName, coordinator.LastName));
            html = html.Replace("{CoordinatorCredentials}",
                string.Join(", ", coordinator.UserCredentials.Select(x => x.Credential.Name)));
            html = html.Replace("{CoordinatorTitle}", coordinator.Title);
            html = html.Replace("{CoordinatorPhoneNumber}", coordinator.PreferredPhoneNumber);
            html = html.Replace("{CoordinatorEmailAddress}", coordinator.EmailAddress);


            EmailHelper.Send(userEmail, cc, $"FACT Compliance Application Submitted - {complianceApplication.Organization.Name}", html);
        }

        /// <summary>
        /// Eligibility applicaiton submitted
        /// </summary>
        /// <param name="url">Url of the current server</param>
        public void EligibilityApplicationSubmittedEmail(string url, List<string> to, List<string> cc, string applicationUniqueId, string appType, string orgName, User coordinator)
        {
            LogMessage("EligibilityApplicationSubmittedEmailAsync (ApplicationManager)");

            var reminderHtml = url + "app/email.templates/eligibilityApplicationSubmitted.html";
            var reminderUrl = string.Format("{0}{1}", url, "#/Application?app=" + applicationUniqueId);

            var html = WebHelper.GetHtml(reminderHtml);
            html = html.Replace("{Url}", reminderUrl);
            html = html.Replace("{AppType}", appType);
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


            EmailHelper.Send(to, cc, $"FACT {appType} Submitted - {orgName}", html);
        }

        public List<ApplicationResponse> GetApplicationResponsesForSection(ApplicationResponseCommentManager commentManager, ApplicationSectionResponse section,
            string updatedBy, int? roleId, ComplianceApplication complianceApplication, ApplicationResponseStatus rfiCompletedStatus, Application application, 
            ApplicationResponseStatus rfiStatus, bool isNonInspector, List<User> orgUsers)
        {
            LogMessage("GetApplicationResponsesForSection (ApplicationManager)");

            var applicationResponses = new List<ApplicationResponse>();
            string rfiCreatedBy = string.Empty;
            foreach (var question in section.Questions)
            {
                if (question.IsHidden)
                    continue; //TFS:1295

                //if (roleId != (int) Constants.Role.User && string.IsNullOrEmpty(question.RFICreatedBy))
                //{
                //    rfiCreatedBy = updatedBy;
                //}

                int? statusId = null;
                int? visibleStatus = null;

                if (question.AnswerResponseStatusId != 0)
                {
                    statusId = question.AnswerResponseStatusId;
                    visibleStatus = question.AnswerResponseStatusId;
                }

                if (rfiStatus != null && application != null && application.ApplicationStatus.Name != Constants.ApplicationStatus.RFI && statusId == rfiStatus.Id)
                {
                    visibleStatus = null;
                }

                if((question.Type != "Radio Buttons") && (question.Type != "Checkboxes")) //TFS:1295 - This check prevents saving NULL against appsecquesanswerid in case of radio/checkboxes.
                {
                    if ((question.AnswerResponseStatusName == Constants.ApplicationResponseStatus.NoResponseRequested || !string.IsNullOrEmpty(question.Comments)) && question.QuestionResponses.Count == 0)
                    {
                        var applicationResponse = new ApplicationResponse
                        {
                            ApplicationSectionQuestionId = question.Id.Value,
                            Flag = question.Flag,
                            ApplicationResponseStatusId = statusId,
                            VisibleApplicationResponseStatusId = visibleStatus,
                            Comments = question.Comments,
                            CommentLastUpdatedBy = question.CommentLastUpdatedBy,
                            CommentLastUpdatedDate = question.CommentDate,
                            ApplicationId = application.Id,
                            CreatedDate = DateTime.Now,
                            CreatedBy = updatedBy,
                        };

                        applicationResponses.Add(applicationResponse);
                    }
                    else
                    {
                        foreach (var response in question.QuestionResponses)
                        {
                            var hasResponse = question.Flag || !string.IsNullOrEmpty(question.Comments);
                            var applicationResponse = new ApplicationResponse
                            {
                                ApplicationSectionQuestionId = question.Id.Value,
                                Flag = question.Flag,
                                ApplicationResponseStatusId = statusId,
                                VisibleApplicationResponseStatusId = visibleStatus,
                                Comments = question.Comments,
                                CommentLastUpdatedBy = question.CommentLastUpdatedBy,
                                CommentLastUpdatedDate = question.CommentDate,
                                ApplicationId = application.Id,
                                CreatedDate = DateTime.Now,
                                CreatedBy = updatedBy,
                            };

                            if (!string.IsNullOrEmpty(response.OtherText))
                            {
                                applicationResponse.Text = response.OtherText;
                                hasResponse = true;
                            }

                            if (!string.IsNullOrEmpty(response.FromDate))
                            {
                                applicationResponse.Text = string.Format("{0} - {1}", response.FromDate, response.ToDate);
                                hasResponse = true;
                            }

                            if (response.UserId.HasValue)
                            {
                                applicationResponse.UserId = response.UserId;
                                hasResponse = true;
                            }

                            if (response.Document != null)
                            {
                                applicationResponse.DocumentId = response.Document.Id;
                                hasResponse = true;
                            }

                            if (hasResponse)
                            {
                                applicationResponses.Add(applicationResponse);
                            }

                            var statusDate = complianceApplication?.Applications?.Max(x => x.UpdatedDate.GetValueOrDefault()) ?? application?.UpdatedDate.GetValueOrDefault();

                            var status = complianceApplication?.ApplicationStatus.Name ?? application?.ApplicationStatus.Name;

                            if (roleId != (int)Constants.Role.FACTAdministrator &&
                                roleId != (int)Constants.Role.FACTCoordinator &&
                                roleId != (int)Constants.Role.QualityManager &&
                                rfiCompletedStatus != null && application != null &&
                                (status == Constants.ApplicationStatus.RFI ||
                                status == Constants.ApplicationStatus.ApplicantResponse ||
                                application.ApplicationStatus.Name.Contains("Committee") ||
                                status == Constants.ApplicationStatus.Complete) &&
                                question.AnswerResponseStatusName == Constants.ApplicationResponseStatus.RFI)
                            {
                                var comments =
                                    commentManager.GetByApplicationIdQuestionId(application.Id,
                                        applicationResponse.ApplicationSectionQuestionId);

                                var userComments = comments.Where(
                                                           x =>
                                                               Convert.ToDateTime(x.CreatedDate) > statusDate &&
                                                               x.CommentFrom.Role.Id !=
                                                               (int) Constants.Role.FACTAdministrator &&
                                                               x.CommentFrom.Role.Id != (int) Constants.Role.FACTCoordinator &&
                                                               x.CommentFrom.Role.Id != (int) Constants.Role.QualityManager &&
                                                               (x.CommentFrom.Role.Id != (int) Constants.Role.Inspector || orgUsers.Any(y => y.Id == x.FromUser.GetValueOrDefault())))
                                                       .ToList() ?? new List<ApplicationResponseComment>();

                                var maxUserDate = userComments.Select(x => x.CreatedDate).Max();

                                var maxFactDate =
                                    question.ApplicationResponseComments.Where(
                                            x => x.CommentFrom.Role.RoleId == (int) Constants.Role.FACTAdministrator ||
                                                 x.CommentFrom.Role.RoleId == (int) Constants.Role.FACTCoordinator ||
                                                 x.CommentFrom.Role.RoleId == (int) Constants.Role.QualityManager ||
                                                 (x.CommentFrom.Role.RoleId == (int) Constants.Role.Inspector &&
                                                  orgUsers.All(y => y.Id != x.FromUser.GetValueOrDefault())))
                                        .Select(x => x.CreatedDate)
                                        .Max();

                                if (Convert.ToDateTime(maxUserDate) > Convert.ToDateTime(maxFactDate) || (maxUserDate.HasValue && string.IsNullOrEmpty(maxFactDate)))
                                {
                                    applicationResponse.ApplicationResponseStatusId = rfiCompletedStatus.Id;
                                    applicationResponse.VisibleApplicationResponseStatusId = rfiCompletedStatus.Id;
                                }
                            }
                        }
                    }
                }
                
                 if (question.Answers.Count > 0)
                {
                    //TFS:1497
                    var statusDate = application?.UpdatedDate.GetValueOrDefault();
                    var status = complianceApplication?.ApplicationStatus.Name ?? application?.ApplicationStatus.Name;

                    if (roleId != (int)Constants.Role.FACTAdministrator && 
                        roleId != (int)Constants.Role.FACTCoordinator && 
                        roleId != (int)Constants.Role.QualityManager &&
                        rfiCompletedStatus != null && application != null &&
                        (status == Constants.ApplicationStatus.RFI ||
                        status == Constants.ApplicationStatus.ApplicantResponse ||
                        status == Constants.ApplicationStatus.Complete ||
                        status.Contains("Committee") ||
                         application.ApplicationStatus.Name == Constants.ApplicationStatus.RFI ||
                         application.ApplicationStatus.Name == Constants.ApplicationStatus.ApplicantResponse ||
                         application.ApplicationStatus.Name == Constants.ApplicationStatus.Complete ||
                         application.ApplicationStatus.Name.Contains("Committee")) &&
                        question.AnswerResponseStatusName == Constants.ApplicationResponseStatus.RFI)
                    {
                        var comments =
                                    commentManager.GetByApplicationIdQuestionId(application.Id,
                                        question.Id.GetValueOrDefault());

                        var userComments = comments.Where(
                        x =>
                            Convert.ToDateTime(x.CreatedDate) > statusDate &&
                            x.CommentFrom.Role.Id != (int)Constants.Role.FACTAdministrator &&
                            x.CommentFrom.Role.Id != (int)Constants.Role.FACTCoordinator &&
                            x.CommentFrom.Role.Id != (int)Constants.Role.QualityManager &&
                           (x.CommentFrom.Role.Id != (int)Constants.Role.Inspector || orgUsers.Any(y => y.Id == x.FromUser.GetValueOrDefault()))).ToList() ?? new List<ApplicationResponseComment>();

                        var maxUserDate = userComments.Select(x => x.CreatedDate).Max();

                        var maxFactDate =
                            question.ApplicationResponseComments.Where(
                                    x => x.CommentFrom.Role.RoleId == (int) Constants.Role.FACTAdministrator ||
                                         x.CommentFrom.Role.RoleId == (int) Constants.Role.FACTCoordinator ||
                                         x.CommentFrom.Role.RoleId == (int) Constants.Role.QualityManager ||
                                         (x.CommentFrom.Role.RoleId == (int) Constants.Role.Inspector &&
                                          orgUsers.All(y => y.Id != x.FromUser.GetValueOrDefault())))
                                .Select(x => x.CreatedDate)
                                .Max();

                        if (Convert.ToDateTime(maxUserDate) > Convert.ToDateTime(maxFactDate) || (maxUserDate.HasValue && string.IsNullOrEmpty(maxFactDate)))
                        {
                            statusId = rfiCompletedStatus.Id;
                            visibleStatus = rfiCompletedStatus.Id;
                        }
                    }

                    var answers = question.Answers
                            .Where(x => x.Selected)
                            .Select(answer => new ApplicationResponse
                            {
                                ApplicationSectionQuestionId = question.Id.Value,
                                ApplicationSectionQuestionAnswerId = answer.Id,//TFS:1295 - This should handle the Checkboxes responses
                                Flag = question.Flag,
                                //Text = answer.Text, //TFS:1295 - Handles the Checkboxes responses
                                ApplicationResponseStatusId = statusId,
                                VisibleApplicationResponseStatusId = visibleStatus,
                                Comments = question.Comments,
                                CommentLastUpdatedBy = question.CommentLastUpdatedBy,
                                CommentLastUpdatedDate = question.CommentDate,
                                CreatedDate = DateTime.Now,
                                CreatedBy = updatedBy
                            })
                            .ToList();

                    if (answers.Count > 0)
                    {
                        applicationResponses.AddRange(answers);
                    }
                    else if (question.AnswerResponseStatusName == Constants.ApplicationResponseStatus.NoResponseRequested || question.Flag || !string.IsNullOrEmpty(question.Comments))
                    {
                        var applicationResponse = new ApplicationResponse
                        {
                            ApplicationSectionQuestionId = question.Id.Value,
                            Flag = question.Flag,
                            ApplicationResponseStatusId = statusId,
                            VisibleApplicationResponseStatusId = visibleStatus,
                            Comments = question.Comments,
                            CommentLastUpdatedBy = question.CommentLastUpdatedBy,
                            CommentLastUpdatedDate = question.CommentDate,
                            ApplicationId = application.Id,
                            CreatedDate = DateTime.Now,
                            CreatedBy = updatedBy,
                        };

                        applicationResponses.Add(applicationResponse);
                    }
                    
                }
            }

            //TFS: 1610. The following code was commented to fix 1610
            //if (section.Children != null && section.Children.Count > 0)
            //{
            //    applicationResponses.AddRange(this.GetApplicationResponses(section.Children, updatedBy, roleId));
            //}

            return applicationResponses;
        }

        public List<ApplicationResponseTrainee> GetApplicationResponsesTraineeForSection(ApplicationSectionItem section, string updatedBy, int? roleId)
        {
            LogMessage("GetApplicationResponsesTraineeForSection (ApplicationManager)");

            var applicationResponsesTrainee = new List<ApplicationResponseTrainee>();
            string rfiCreatedBy = string.Empty;
            foreach (var question in section.Questions)
            {
                int statusId = 0;

                if (question.AnswerResponseStatusId != 0)
                {
                    statusId = question.AnswerResponseStatusId;
                }

                foreach (var response in question.QuestionResponses)
                {
                    var hasResponse = false;
                    var applicationResponseTrainee = new ApplicationResponseTrainee
                    {
                        ApplicationSectionQuestionId = question.Id.Value,
                        ApplicationResponseStatusId = statusId,
                        CreatedDate = DateTime.Now,
                        CreatedBy = updatedBy,
                    };

                    if (hasResponse)
                    {
                        applicationResponsesTrainee.Add(applicationResponseTrainee);
                    }
                }

                if (question.Answers.Count > 0)
                {
                    applicationResponsesTrainee.AddRange(
                        question.Answers
                            .Where(x => x.Selected)
                            .Select(answer => new ApplicationResponseTrainee
                            {
                                ApplicationSectionQuestionId = question.Id.Value,
                                ApplicationSectionQuestionAnswerId = answer.Id,
                                ApplicationResponseStatusId = statusId,
                                CreatedDate = DateTime.Now,
                                CreatedBy = updatedBy
                            }));
                }
            }

            if (section.Children != null && section.Children.Count > 0)
            {
                applicationResponsesTrainee.AddRange(this.GetApplicationResponsesTrainee(section.Children, updatedBy, roleId));
            }

            return applicationResponsesTrainee;
        }

        private IEnumerable<ApplicationResponseTrainee> GetApplicationResponsesTrainee(IEnumerable<ApplicationSectionItem> sections, string updatedBy, int? roleId)
        {
            LogMessage("GetApplicationResponsesTrainee (ApplicationManager)");

            var applicationResponseTrainee = new List<ApplicationResponseTrainee>();

            foreach (var section in sections)
            {
                applicationResponseTrainee.AddRange(this.GetApplicationResponsesTraineeForSection(section, updatedBy, roleId));
            }

            return applicationResponseTrainee;
        }

        /// <summary>
        /// Sends email to coordinator when Compliance Application approval status is changed to Rejected
        /// </summary>
        /// <param name="url">Url of the current server</param>
        public void EligibilityApplicationApprovedEmail(string url, string userEmail, Guid? complianceApplicationId, string organizationName, string applicationTypeName)
        {
            LogMessage("EligibilityApplicationApprovedEmail (ApplicationManager)");

            var rejectionHtml = url + "app/email.templates/adminEligibilityApplicationApprovalStatus.html";
            var rejectionPageUrl = string.Format("{0}{1}", url, "#/Compliance/ManageCompliance?org=" + organizationName);

            var html = WebHelper.GetHtml(rejectionHtml);
            html = string.Format(html, organizationName, rejectionPageUrl, rejectionPageUrl);
            html = html.Replace("{ApplicationType", applicationTypeName);

            EmailHelper.Send(userEmail, $"FACT {applicationTypeName} Approved [Action Requested] - {organizationName}", html, true);
        }

        /// <summary>
        /// Gets an application by compliance applicationid
        /// </summary>
        /// <param name="id">Unique Id of the record</param>
        /// <returns>Application entity object</returns>
        public List<Application> GetByComplianceApplicationId(Guid id)
        {
            return base.Repository.GetByComplianceApplicationId(id);
        }

        public List<Application> GetAllComplianceApplications(int organizationId)
        {
            return base.Repository.GetAllComplianceApplications(organizationId);
        }

        public List<Application> GetComplianceApplications(int organizationId)
        {
            return base.Repository.GetComplianceApplications(organizationId);
        }

        public Task<List<Application>> GetComplianceApplicationsAsync(int organizationId)
        {
            return base.Repository.GetComplianceApplicationsAsync(organizationId);
        }

        public void Deactivate(Guid app, string updatedBy)
        {
            var application = this.GetByUniqueId(app);

            if (application == null)
            {
                throw new Exception("Cannot find application");
            }

            application.IsActive = false;
            application.UpdatedBy = updatedBy;
            application.UpdatedDate = DateTime.Now;

            base.Repository.Save(application);
        }

        public async Task DeactivateAsync(Guid app, string updatedBy)
        {
            var application = await this.GetByUniqueIdAsync(app);

            if (application == null)
            {
                throw new Exception("Cannot find application");
            }

            application.IsActive = false;
            application.UpdatedBy = updatedBy;
            application.UpdatedDate = DateTime.Now;

            await base.Repository.SaveAsync(application);
        }

        public void CreateApprovals(int applicationId, string createdBy)
        {
            base.Repository.CreateApprovals(applicationId, createdBy);
        }

        public List<AppReportModel> GetApplicationReport(Guid complianceApplicationId, string siteName)
        {
            return base.Repository.GetApplicationReport(complianceApplicationId, siteName);
        }

        public List<AppReportModel> GetAppReport(Guid appUniqueId)
        {
            return base.Repository.GetAppReport(appUniqueId);
        }

        public List<BlankReport> GetBlankReport(int applicationTypeId)
        {
            return base.Repository.GetBlankReport(applicationTypeId);
        }
    }
}

