using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class InspectionFacade
    {
        private readonly Container container;

        public InspectionFacade(Container container)
        {
            this.container = container;
        }

        private void Save(InspectionItem inspectionItem, int applicationId, Guid userId, bool isTrainee, bool isReinspection, string currentUser)
        {
            var siteManager = this.container.GetInstance<SiteManager>();
            var inspectionManager = this.container.GetInstance<InspectionManager>();

            inspectionManager.Save(inspectionItem, applicationId, userId, isTrainee, isReinspection, isTrainee ? inspectionItem.SiteDescription : null, currentUser);

            if (isTrainee) return;

            var site = siteManager.GetById(inspectionItem.SiteId);

            if (site == null || site.Description == inspectionItem.SiteDescription) return;

            site.Description = inspectionItem.SiteDescription;
            site.UpdatedBy = currentUser;
            site.UpdatedDate = DateTime.Now;

            siteManager.Save(site);
        }

        private bool IsReinspection(Application app)
        {
            return app.InspectionSchedules.Count > 1;
        }

        private bool IsTrainee(Application app, Guid userId, int siteId)
        {
            foreach (var schedule in app.InspectionSchedules)
            {
                if (schedule?.InspectionScheduleDetails == null) continue;

                if (
                    schedule.InspectionScheduleDetails.Any(
                        x =>
                            x.UserId == userId &&
                            x.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees)) return true;

            }

            return false;
   ;     }

        public void SaveCoordinator(InspectionDetail detail, string currentUser)
        {
            var siteManager = this.container.GetInstance<SiteManager>();
            var inspectionManager = this.container.GetInstance<InspectionManager>();

            var site = siteManager.GetById(detail.SiteId);

            if (site != null)
            {
                if (site.Description != detail.SiteDescription)
                {
                    site.OverridenDescription = detail.SiteDescription;
                    site.UpdatedBy = currentUser;
                    site.UpdatedDate = DateTime.Now;

                    siteManager.Save(site);
                }
            }

            var inspection = inspectionManager.GetById(detail.InspectionId);

            if (inspection != null)
            {
                var hasChanges = false;
                if (inspection.CommendablePractices != detail.CommendablePractices)
                {
                    inspection.OverridenPractices = detail.CommendablePractices;
                    hasChanges = true;
                }

                if (inspection.OverallImpressions != detail.OverallImpressions)
                {
                    inspection.OverridenImpressions = detail.OverallImpressions;
                    hasChanges = true;
                }

                if (hasChanges)
                {
                    inspection.UpdatedBy = currentUser;
                    inspection.UpdatedDate = DateTime.Now;

                    inspectionManager.Save(inspection);
                }
                
            }
        }

        public async Task<bool> SaveAsync(InspectionItem inspectionItem, Guid userId, string currentUser)
        {
            var userManager = this.container.GetInstance<UserManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var app = await applicationManager.GetByUniqueIdAsync(inspectionItem.ApplicationUniqueId);

            if (app == null || !app.ComplianceApplicationId.HasValue)
            {
                throw new Exception("Cannot find application");
            }

            var user = userManager.GetById(userId);

            if (app.SiteId != inspectionItem.SiteId)
            {
                var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();
                var complianceApplication = complianceApplicationManager.GetById(app.ComplianceApplicationId.Value);

                var applications = complianceApplication.Applications.Where(x => x.SiteId == inspectionItem.SiteId);

                foreach (var application in applications)
                {
                    var isTrainee = this.IsTrainee(app, user.Id, application.SiteId.GetValueOrDefault());

                    inspectionItem.ApplicationUniqueId = application.UniqueId;
                    inspectionItem.ApplicationId = application.Id;

                    this.Save(inspectionItem, application.Id, user.Id, isTrainee, this.IsReinspection(application), currentUser);
                }
            }
            else
            {
                var isTrainee = this.IsTrainee(app, user.Id, inspectionItem.SiteId);

                this.Save(inspectionItem, app.Id, user.Id, isTrainee, this.IsReinspection(app), currentUser);
            }          

            return true;
        }

        public void SetReviewOutcome(Guid compAppId, Guid userId, string updatedBy)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();

            var inspectors = inspectionScheduleDetailManager.GetAllActiveByComplianceApplication(compAppId);

            var details = inspectors.Where(x => x.UserId == userId).ToList();

            foreach (var detail in details)
            {
                detail.ReviewedOutcomesData = true;
                detail.UpdatedBy = updatedBy;
                detail.UpdatedDate = DateTime.Now;

                inspectionScheduleDetailManager.BatchSave(detail);
            }

            inspectionScheduleDetailManager.SaveChanges();
        }

        public async Task<List<Inspection>> GetInspectionAsync(Guid applicationUniqueId, Guid userId)
        {
            var inspectionManager = this.container.GetInstance<InspectionManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var app = applicationManager.GetByUniqueId(applicationUniqueId);
            //var user = userManager.GetByEmailAddress(currentUserEmail);

            var isTrainee = this.IsTrainee(app, userId, app.SiteId.GetValueOrDefault());

            return new List<Inspection>
            {
                await inspectionManager.GetInspectionAsync(applicationUniqueId, isTrainee, false),
                await inspectionManager.GetInspectionAsync(applicationUniqueId, isTrainee, true)
            };
        }

        public List<Inspection> GetInspection(Guid applicationUniqueId, string siteName, string currentUserEmail)
        {
            var inspectionManager = this.container.GetInstance<InspectionManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var app = applicationManager.GetByUniqueId(applicationUniqueId);
            var user = userManager.GetByEmailAddress(currentUserEmail);

            var isTrainee = this.IsTrainee(app, user.Id, app.SiteId.GetValueOrDefault());

            return new List<Inspection>
            {
                inspectionManager.GetInspection(applicationUniqueId, isTrainee, false),
                inspectionManager.GetInspection(applicationUniqueId, isTrainee, true)
            };
        }

        public async Task<List<Inspection>> GetInspectionBySiteAsync(Guid complianceAppId, string siteName, Guid userId)
        {
            var inspectionManager = this.container.GetInstance<InspectionManager>();
            var applicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var app = applicationManager.GetById(complianceAppId);
            //var user = userManager.GetByEmailAddress(currentUserEmail);

            var isTrainee = false;
            foreach (var application in app.Applications)
            {
                isTrainee = this.IsTrainee(application, userId, application.SiteId.GetValueOrDefault());
                
                if (isTrainee) break;
            }

            return new List<Inspection>
            {
                await inspectionManager.GetInspectionBySiteAsync(complianceAppId, siteName, isTrainee, false),
                await inspectionManager.GetInspectionBySiteAsync(complianceAppId, siteName, isTrainee, true)
            };
        }

        public List<Inspection> GetInspectionBySite(Guid complianceAppId, string siteName, string currentUserEmail)
        {
            var inspectionManager = this.container.GetInstance<InspectionManager>();
            var applicationManager = this.container.GetInstance<ComplianceApplicationManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var app = applicationManager.GetById(complianceAppId);
            var user = userManager.GetByEmailAddress(currentUserEmail);

            var isTrainee = false;
            foreach (var application in app.Applications)
            {
                isTrainee = this.IsTrainee(application, user.Id, application.SiteId.GetValueOrDefault());

                if (isTrainee) break;
            }

            return new List<Inspection>
            {
                inspectionManager.GetInspectionBySite(complianceAppId, siteName, isTrainee, false),
                inspectionManager.GetInspectionBySite(complianceAppId, siteName, isTrainee, true)
            };
        }

        public Task<Inspection> GetInspectionByAppIdAsync(Guid applicationId)
        {
            var inspectionManager = this.container.GetInstance<InspectionManager>();

            return inspectionManager.GetInspectionByAppIdAsync(applicationId);
        }

        public InspectionOverallDetail GetInspectionDetails(Guid complianceAppId)
        {
            var inspectionManager = this.container.GetInstance<InspectionManager>();

            return inspectionManager.GetInspectionOverallDetails(complianceAppId);
        }

        public void SendMentorCompleteEmail(string url, Guid compAppId, Guid currentUserId)
        {
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var compAppManager = this.container.GetInstance<ComplianceApplicationManager>();
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var applicationSettingManager = this.container.GetInstance<ApplicationSettingManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();


            var scheds = inspectionScheduleManager.GetAllForCompliance(compAppId);

            var compApp = compAppManager.GetById(compAppId);

            var app = applicationManager.GetByComplianceApplicationId(compAppId)?.FirstOrDefault();

            if (compApp == null || app == null)
            {
                throw new Exception("Cannot find application");
            }

            var coordinator = userManager.GetById(compApp.CoordinatorId);

            var creds = coordinator.UserCredentials.Aggregate(string.Empty,
                (current, cred) => current + (cred.Credential.Name + ", "));

            if (creds.Length > 2)
            {
                creds = creds.Substring(0, creds.Length - 2);
            }

            var factEmail = applicationSettingManager.GetByName(Constants.ApplicationSettings.AutoCcEmailAddress);

            foreach (var sched in scheds)
            {
                var ccs = new List<string>
                {
                    factEmail?.Value ?? "portal@factwebsite.org",
                    coordinator.EmailAddress
                };

                var traineeDetail =
                    sched.InspectionScheduleDetails.FirstOrDefault(
                        x => x.AccreditationRoleId == (int) Constants.AccreditationRoles.InspectorTrainee);

                var mentorDetail = sched.InspectionScheduleDetails.FirstOrDefault(x => x.IsMentor && x.UserId == currentUserId);

                if (traineeDetail == null || mentorDetail == null) continue;

                var trainee = userManager.GetById(traineeDetail.UserId);
                var mentor = userManager.GetById(mentorDetail.UserId);

                var org = orgManager.GetById(sched.OrganizationId);

                var reminderHtml = url + "app/email.templates/mentorComplete.html";
                var appUrl = $"{url}#/Compliance?app={app.Id}&c={compAppId}";
                var inspectionReportUrl = $"{url}#/Reporting?app={app.Id}&c={compAppId}&org={org.Name}&r=Trainee%20Inspection%20Summary";

                var html = WebHelper.GetHtml(reminderHtml);
                html = html.Replace("{ApplicationUrl}", appUrl);
                html = html.Replace("{InspectionReportUrl}", inspectionReportUrl);
                html = html.Replace("{OrgName}", org.Name);
                html = html.Replace("{MentorName}", $"{mentor.FirstName} {mentor.LastName}");
                html = html.Replace("{TraineeName}", $"{trainee.FirstName} {trainee.LastName}");
                html = html.Replace("{CoordinatorName}", $"{coordinator.FirstName} {coordinator.LastName}, {creds}");
                html = html.Replace("{CoordinatorTitle}", coordinator.Title);
                html = html.Replace("{CoordinatorPhoneNumber}", coordinator.WorkPhoneNumber);
                html = html.Replace("{CoordinatorEmailAddress}", coordinator.EmailAddress);

                var subject = $"FACT Mentor Review Complete – {org.Name}";

                ccs.Add(mentor.EmailAddress);

                EmailHelper.Send(new List<string> { trainee.EmailAddress }, ccs, subject, html, null, true);
            }

            
        }

        public void UpdateDetails(InspectionOverallDetail detail, string updatedBy)
        {
            var manager = this.container.GetInstance<InspectionScheduleManager>();

            manager.UpdateDetails(detail, updatedBy);
        }
    }
}

