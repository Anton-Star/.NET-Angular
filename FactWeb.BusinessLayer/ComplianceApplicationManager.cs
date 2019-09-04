using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ComplianceApplicationManager : BaseManager<ComplianceApplicationManager, IComplianceApplicationRepository, ComplianceApplication>
    {
        private readonly IApplicationQuestionNotApplicableRepository notApplicableRepository;

        public ComplianceApplicationManager(IComplianceApplicationRepository repository, IApplicationQuestionNotApplicableRepository notApplicableRepository) : base(repository)
        {
            this.notApplicableRepository = notApplicableRepository;
        }

        public DoesInspectorHaveAccessModel DoesInspectorHaveAccess(Guid? applicationUniqueId, Guid? complianceApplicationId, Guid userId)
        {
            return base.Repository.DoesInspectorHaveAccess(applicationUniqueId, complianceApplicationId, userId);
        }

        public void UpdateComplianceApplicationStatus(Guid complianceApplicationId, string statusName, string updatedBy)
        {
            base.Repository.UpdateComplianceApplicationStatus(complianceApplicationId, statusName, updatedBy);
        }

        public ComplianceApplication GetByIdWithApps(Guid id)
        {
            return base.Repository.GetByIdWithApps(id);
        }

        public ComplianceApplication GetByIdInclusive(Guid id)
        {
            return base.Repository.GetByIdInclusive(id);
        }

        public ComplianceApplication GetByIdSemiInclusive(Guid id)
        {
            return base.Repository.GetByIdSemiInclusive(id);
        }

        public List<ComplianceApplication> GetByOrg(int organizationId)
        {
            return base.Repository.GetByOrg(organizationId);
        }

        public ComplianceApplication Add(int cycle, List<ApplicationVersion> activeVersions, ComplianceApplicationApprovalStatus approvalStatus, ApplicationStatus status, ComplianceApplicationItem model, string savedBy)
        {
            var complianceApp = new ComplianceApplication
            {
                Id = Guid.NewGuid(),
                OrganizationId = model.OrganizationId,
                AccreditationGoal = model.AccreditationGoal,
                ApplicationStatusId = status.Id,
                CoordinatorId = model.Coordinator.UserId.GetValueOrDefault(),
                InspectionScope = model.InspectionScope,
                ComplianceApplicationApprovalStatusId = approvalStatus.Id,
                Applications = new List<Application>(),
                IsActive = true,
                ReportReviewStatusId = string.IsNullOrEmpty(model.ReportReviewStatus) ? (Int32?)null : Convert.ToInt32(model.ReportReviewStatus),
                CreatedBy = savedBy,
                CreatedDate = DateTime.Now
            };

            foreach (var site in model.ComplianceApplicationSites)
            {
                if (site.Site == null) continue;

                foreach (var myApp in site.Applications)
                {
                    var app =
                        complianceApp.Applications.SingleOrDefault(
                            x => x.ApplicationTypeId == myApp.ApplicationTypeId && x.SiteId == site.Site.SiteId);

                    var isComplianceApp = myApp.ComplianceApplicationId.HasValue;//myApp.ApplicationTypeName.Contains("Compliance");

                    if (app == null)
                    {
                        var notApplicables =
                            myApp.NotApplicables.Select(x => new ApplicationQuestionNotApplicable
                            {
                                Id = Guid.NewGuid(),
                                ApplicationSectionQuestionId = x.QuestionId,
                                CreatedDate = DateTime.Now,
                                CreatedBy = savedBy
                            }).ToList();

                        var version =
                            activeVersions.SingleOrDefault(
                                x => x.IsActive && x.ApplicationTypeId == myApp.ApplicationTypeId);

                        var application = new Application
                        {
                            ApplicationTypeId = myApp.ApplicationTypeId,
                            OrganizationId = model.OrganizationId,
                            ApplicationStatusId = status.Id,
                            UniqueId = Guid.NewGuid(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = savedBy,
                            CoordinatorId = isComplianceApp == true ? model.Coordinator.UserId.GetValueOrDefault() : (Guid?)null,
                            SiteId = site.Site.SiteId,
                            DueDate = !string.IsNullOrEmpty(model.DueDate) ? Convert.ToDateTime(model.DueDate) : (DateTime?)null,
                            ApplicationVersionId = version?.Id,
                            ApplicationQuestionNotApplicables = notApplicables,
                            CycleNumber = cycle == 0 ? 1 : cycle
                            //SiteApplicationVersions = new List<SiteApplicationVersion>
                            //{
                            //    new SiteApplicationVersion
                            //    {
                            //        Id = Guid.NewGuid(),
                            //        SiteId = site.Site.SiteId,
                            //        ApplicationVersionId = version.ApplicationVersion.Id,
                            //        ApplicationStatusId = status.Id,
                            //        CreatedDate = DateTime.Now,
                            //        CreatedBy = savedBy
                            //    }
                            //}
                        };

                        if (approvalStatus.Name == Constants.ComplianceApplicationApprovalStatuses.Approved)
                        {
                            application.IsActive = true;
                        }

                        complianceApp.Applications.Add(application);
                    }
                }
            }

            this.Add(complianceApp);

            return complianceApp;
        }

        public ComplianceApplication Update(ApplicationStatus cancelledStatus, List<ApplicationVersion> activeVersions, ComplianceApplicationApprovalStatus approvalStatus, ApplicationStatus status, ComplianceApplicationItem model, int cycleNumber, string savedBy)
        {
            var complianceApp = this.GetById(model.Id.GetValueOrDefault());

            if (complianceApp == null)
            {
                throw new Exception("Cannot find Compliance Application");
            }

            complianceApp.AccreditationGoal = model.AccreditationGoal;
            complianceApp.CoordinatorId = model.Coordinator.UserId.GetValueOrDefault();
            complianceApp.InspectionScope = model.InspectionScope;
            complianceApp.ComplianceApplicationApprovalStatusId = approvalStatus.Id;
            complianceApp.CoordinatorId = model.Coordinator.UserId.GetValueOrDefault();
            complianceApp.ReportReviewStatusId = string.IsNullOrEmpty(model.ReportReviewStatus) ? (Int32?)null : Convert.ToInt32(model.ReportReviewStatus);
            complianceApp.UpdatedBy = savedBy;
            complianceApp.UpdatedDate = DateTime.Now;

            if (complianceApp.ApplicationStatus.Name == Constants.ApplicationStatus.PastDue)
            {
                complianceApp.ApplicationStatusId = status.Id;
            }

            foreach (var app in complianceApp.Applications)
            {
                if (model.ComplianceApplicationSites.Any(x => x.Site.SiteId == app.SiteId.GetValueOrDefault()))
                    continue;

                app.IsActive = false;
                app.ApplicationStatusId = cancelledStatus.Id;
                app.UpdatedBy = savedBy;
                app.UpdatedDate = DateTime.Now;
            }

            foreach (var site in model.ComplianceApplicationSites)
            {
                var apps = complianceApp.Applications.Where(x => x.SiteId == site.Site.SiteId);

                foreach (var app in apps)
                {
                    if (site.Applications.All(x => x.ApplicationTypeId != app.ApplicationTypeId))
                    {
                        app.IsActive = false;
                        app.ApplicationStatusId = cancelledStatus.Id;
                        app.UpdatedBy = savedBy;
                        app.UpdatedDate = DateTime.Now;
                    }
                }

                foreach (var myApp in site.Applications)
                {
                    var notApplicables =
                        myApp.NotApplicables.Select(x => new ApplicationQuestionNotApplicable
                        {
                            Id = Guid.NewGuid(),
                            ApplicationSectionQuestionId = x.QuestionId,
                            CreatedDate = DateTime.Now,
                            CreatedBy = savedBy
                        }).ToList();

                    var questionIds = string.Join(",", myApp.NotApplicables.Select(x => x.QuestionId.ToString()));

                    if (complianceApp.Applications == null)
                    {
                        complianceApp.Applications = new List<Application>();
                    }

                    var isComplianceApp = myApp.ComplianceApplicationId.HasValue;//myApp.ApplicationTypeName.Contains("Compliance");

                    var app =
                        complianceApp.Applications?.SingleOrDefault(
                            x => x.ApplicationTypeId == myApp.ApplicationTypeId && x.SiteId == site.Site.SiteId && x.ApplicationStatusId != cancelledStatus.Id);
                    if (app == null)
                    {
                        var version =
                            activeVersions.SingleOrDefault(
                                x => x.IsActive && x.ApplicationTypeId == myApp.ApplicationTypeId);

                        var application = new Application
                        {
                            ApplicationTypeId = myApp.ApplicationTypeId,
                            OrganizationId = model.OrganizationId,
                            ApplicationStatusId = status.Id,
                            UniqueId = Guid.NewGuid(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = savedBy,
                            SiteId = site.Site.SiteId,
                            CoordinatorId = isComplianceApp == true ? model.Coordinator.UserId.GetValueOrDefault() : (Guid?)null,//model.Coordinator.UserId.GetValueOrDefault(),
                            DueDate = !string.IsNullOrEmpty(model.DueDate) ? Convert.ToDateTime(model.DueDate) : (DateTime?)null,
                            ApplicationVersionId = version?.Id,
                            ApplicationQuestionNotApplicables = notApplicables,
                            CycleNumber = cycleNumber == 0 ? 1 : cycleNumber
                            //SiteApplicationVersions = new List<SiteApplicationVersion>
                            //{
                            //    new SiteApplicationVersion
                            //    {
                            //        Id = Guid.NewGuid(),
                            //        SiteId = site.Site.SiteId,
                            //        ApplicationVersionId = version.ApplicationVersion.Id,
                            //        ApplicationStatusId = status.Id,
                            //        CreatedDate = DateTime.Now,
                            //        CreatedBy = savedBy,
                            //        NotApplicables = notApplicables
                            //    }
                            //}
                        };

                        if (approvalStatus.Name == Constants.ComplianceApplicationApprovalStatuses.Approved)
                        {
                            application.IsActive = true;
                        }

                        complianceApp.Applications.Add(application);
                    }
                    else
                    {
                        //app.ApplicationQuestionNotApplicables = notApplicables;
                        app.UpdatedBy = savedBy;

                        if (approvalStatus.Name == Constants.ComplianceApplicationApprovalStatuses.Approved && !app.IsActive.GetValueOrDefault())
                        {
                            app.IsActive = true;
                        }

                        if (app.ApplicationStatus.Name == Constants.ApplicationStatus.PastDue)
                        {
                            app.ApplicationStatusId = status.Id;
                        }

                        if (isComplianceApp == true)
                        {
                            app.CoordinatorId = model.Coordinator.UserId.GetValueOrDefault();
                        }

                        app.UpdatedDate = DateTime.Now;
                        app.DueDate = !string.IsNullOrEmpty(model.DueDate) ? Convert.ToDateTime(model.DueDate) : (DateTime?)null;

                        if (!string.IsNullOrEmpty(questionIds))
                        {
                            this.notApplicableRepository.AddForApplication(app.Id, questionIds, savedBy);
                        }
                    }
                    //else
                    //{
                    //    app.SiteApplicationVersions.Add(new SiteApplicationVersion
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        SiteId = site.Site.SiteId,
                    //        ApplicationVersionId = version.ApplicationVersion.Id,
                    //        ApplicationStatusId = status.Id,
                    //        CreatedDate = DateTime.Now,
                    //        CreatedBy = savedBy,
                    //        NotApplicables = notApplicables
                    //    });
                    //}

                }
            }

            this.Save(complianceApp);

            return complianceApp;
        }

        public void SetComplianceApprovalStatus(ApplicationStatus rfiStatus, ComplianceApplicationApprovalStatus approvalStatus, Guid complianceApplicationId, string rejectionComments, string savedBy)
        {
            var complianceApp = this.GetById(complianceApplicationId);

            if (complianceApp == null)
            {
                throw new Exception("Cannot find Compliance Application");
            }

            complianceApp.ComplianceApplicationApprovalStatusId = approvalStatus.Id;
            complianceApp.UpdatedBy = savedBy;
            complianceApp.UpdatedDate = DateTime.Now;

            if (rfiStatus != null)
            {
                complianceApp.ApplicationStatusId = rfiStatus.Id;
            }

            if (!string.IsNullOrEmpty(rejectionComments))
                complianceApp.RejectionComments = rejectionComments;

            if (approvalStatus.Name == Constants.ComplianceApplicationApprovalStatuses.Approved)
            {
                foreach (var application in complianceApp.Applications)
                {
                    application.IsActive = true;
                    application.UpdatedBy = savedBy;
                    application.UpdatedDate = DateTime.Now;

                    if (rfiStatus != null)
                    {
                        application.ApplicationStatusId = rfiStatus.Id;
                    }
                }
            }

            this.Save(complianceApp);
        }


        /// <summary>
        /// Sends email to coordinator when Compliance Application approval status is changed to Rejected
        /// </summary>
        /// <param name="url">Url of the current server</param>
        public void ComplianceApplicationRejectedEmail(string url, List<string> tos, string userEmail, string rejectionComments, string autoCcs, ComplianceApplication complianceApplication)
        {
            var ccs = new List<string> { userEmail, "portal@factwebsite.org" };

            if (!string.IsNullOrEmpty(autoCcs))
            {
                var add = autoCcs.Split(';');

                ccs.AddRange(add.ToList());
            }

            LogMessage("ComplianceApplicationRejectedEmail (ComplianceApplicationManager)");

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

            var rejectionHtml = url + "app/email.templates/adminComplianceApplicationApprovalStatus.html";
            var rejectionPageUrl = string.Format("{0}{1}", url, "#/Admin/complianceApplicationApproval?complianceApplicationId=" + complianceApplication.Id);

            var html = WebHelper.GetHtml(rejectionHtml);
            html = html.Replace("{URL}", rejectionPageUrl);
            html = html.Replace("{Comments}", rejectionComments);

            html = html.Replace("{CoordinatorName}", $"{coordinator.FirstName} {coordinator.LastName}, {creds}");
            html = html.Replace("{CoordinatorTitle}", coordinator.Title);
            html = html.Replace("{CoordinatorPhoneNumber}", coordinator.WorkPhoneNumber);
            html = html.Replace("{CoordinatorEmailAddress}", coordinator.EmailAddress);
            //html = string.Format(html, rejectionPageUrl, rejectionPageUrl);

            EmailHelper.Send(tos, ccs, $"Compliance Application Rejected - {complianceApplication.Organization.Name}", html, null, true);
        }

        public async Task SetApplicationStatusAsync(Guid complianceApplicationId, int applicationStatusId, bool updateApplications, string updatedBy)
        {
            var complianceApplication = this.GetById(complianceApplicationId);

            if (complianceApplication == null)
            {
                throw new Exception("Cannot find application");
            }

            complianceApplication.ApplicationStatusId = applicationStatusId;
            complianceApplication.UpdatedBy = updatedBy;
            complianceApplication.UpdatedDate = DateTime.Now;

            if (updateApplications)
            {
                foreach (var application in complianceApplication.Applications)
                {
                    application.ApplicationStatusId = applicationStatusId;
                    application.UpdatedBy = updatedBy;
                    application.UpdatedDate = DateTime.Now;
                }
            }

            await base.Repository.SaveAsync(complianceApplication);
        }

        public List<CbTotal> GetCbTotals(Guid complianceApplicationId)
        {
            return base.Repository.GetCbTotals(complianceApplicationId);
        }

        public List<CtTotal> GetCtTotals(Guid complianceApplicationId)
        {
            return base.Repository.GetCtTotals(complianceApplicationId);
        }

        public void CreateComplianceApplicationSections(Guid complianceApplicationId)
        {
            base.Repository.CreateComplianceApplicationSections(complianceApplicationId);
        }

        public bool AppHasRfis(Guid complianceApplicationId)
        {
            return base.Repository.AppHasRfis(complianceApplicationId);
        }
    }
}

