using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using log4net;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class AccreditationOutcomeFacade
    {
        private readonly Container container;

        private readonly ILog Log = LogManager.GetLogger(typeof(AccreditationOutcomeFacade));

        public AccreditationOutcomeFacade(Container container)
        {
            this.container = container;
        }

        public List<AccreditationOutcome> GetAccreditationOutcome()
        {
            var accreditationOutcomeManager = this.container.GetInstance<AccreditationOutcomeManager>();

            return accreditationOutcomeManager.GetAll().OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<AccreditationOutcome> GetAccreditationOutcomeByOrgId(int organizationId)
        {
            var accreditationOutcomeManager = this.container.GetInstance<AccreditationOutcomeManager>();

            return accreditationOutcomeManager.GetByOrgId(organizationId).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<AccreditationOutcome> GetAccreditationOutcomeByOrgIdAppId(int organizationId, int applicationId)
        {
            var accreditationOutcomeManager = this.container.GetInstance<AccreditationOutcomeManager>();

            return accreditationOutcomeManager.GetByOrgIdAndAppId(organizationId, applicationId).OrderByDescending(x => x.CreatedDate).ToList();
        }

        /// <summary>
        /// Prepares to, cc and subject for accreditation outcome email
        /// </summary>
        /// <param name="outcomeLevel"></param>
        /// <param name="organizationId"></param>
        /// <param name="appUniqueId"></param>
        /// <returns></returns>
        public EmailTemplateItem GetAccrediationEmailItems(string outcomeLevel, int organizationId, string appUniqueId, string url)
        {
            var accreditationOutcomeManager = this.container.GetInstance<AccreditationOutcomeManager>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var applicationSettingManager = this.container.GetInstance<ApplicationSettingManager>();
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var emailTemplateManager = this.container.GetInstance<EmailTemplateManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var emailTemplateItem = new EmailTemplateItem();
            var subject = string.Empty;
            var emailTemplateName = string.Empty;
            var body = string.Empty;
            var ccList = new List<string>();
            var toList = new List<string>();

            emailTemplateName = outcomeLevel == Constants.OutcomeStatus.Level1 ? Constants.EmailTemplateName.AccreditationLevel1 : Constants.EmailTemplateName.AccreditationOther;
            var outcomeEmailTemplete = emailTemplateManager.GetByName(emailTemplateName);
            var organization = organizationManager.GetById(organizationId);
            var application = applicationManager.GetByUniqueId(new Guid(appUniqueId));

            if (outcomeEmailTemplete == null)
            {
                throw new Exception("Cannot Find the Accredition Outcome Email Template");
            }

            body = PrepareOutcomeEmail(outcomeEmailTemplete.Html, organization, application, outcomeLevel, url);

            var factEmail = applicationSettingManager.GetByName(Constants.ApplicationSettings.AutoCcEmailAddress);
            var orgFacilityDirector = organization.OrganizationFacilities.Select(x => x.Facility).Select(y => y.FacilityDirector?.EmailAddress).ToList();
            var orgPrimaryContact = organization.PrimaryUser.EmailAddress;

            subject = outcomeLevel == Constants.OutcomeStatus.Level1 ? "FACT Accreditation Awarded - " + organization.Name : Constants.EmailTemplates.AccreditationOutcomeSubject + organization.Name;

            foreach (var director in organization.Users.Where(x=> x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector))
            {
                toList.Add(director.User.EmailAddress);
            }

            ccList.Add(factEmail?.Value ?? "portal@factwebsite.org");

            if (orgFacilityDirector.Count() > 0)
                ccList.AddRange(orgFacilityDirector.Distinct());

            if (!string.IsNullOrEmpty(orgPrimaryContact))
                ccList.Add(orgPrimaryContact);

            if (!string.IsNullOrEmpty(organization.CcEmailAddresses))
            {
                var add = organization.CcEmailAddresses.Split(';');

                ccList.AddRange(add.ToList());
            }

            //if (factCoordinators.Count() > 0)
            //    ccList.AddRange(factCoordinators.Distinct());

            if (application.ComplianceApplicationId.HasValue)
            {
                ccList.Add(application.ComplianceApplication.Coordinator.EmailAddress);
            }
            else if (application?.CoordinatorId != null)
            {
                ccList.Add(application.Coordinator.EmailAddress);
            }

            if (outcomeLevel == Constants.OutcomeStatus.Level1)
            {
                var inspectionScheduleDetail = inspectionScheduleDetailManager.GetAllActiveByApplication(new Guid(appUniqueId));
                var inspectionTeam = inspectionScheduleDetail.Select(x => x.User.EmailAddress).ToList();

                if (inspectionTeam.Count() > 0)
                    ccList.AddRange(inspectionTeam);
            }

            emailTemplateItem.To = String.Join(", ", toList);
            emailTemplateItem.Cc = String.Join(", ", ccList);
            emailTemplateItem.Subject = subject;
            emailTemplateItem.Html = body;

            return emailTemplateItem;
        }

        /// <summary>
        /// Prepares email for accreditation outcome
        /// </summary>
        /// <param name="html"></param>
        /// <param name="organization"></param>
        /// <param name="application"></param>
        /// <param name="outcomeLevel"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private string PrepareOutcomeEmail(string html, Organization organization, Application application, string outcomeLevel, string url)
        {
            var coord = application.CoordinatorId ?? application.ComplianceApplication?.CoordinatorId;
            User coordinator = null;

            if (coord.HasValue)
            {
                var userManager = this.container.GetInstance<UserManager>();
                coordinator = userManager.GetById(coord.GetValueOrDefault());
            }
            else
            {
                throw new Exception("Cannot Find the Coordinator against this application.");
            }

            var lastNames = "";

            foreach (var director in organization.Users.Where(x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector))
            {
                lastNames += director.User.LastName + ", ";
            }

            if (lastNames.Length > 0)
            {
                lastNames = lastNames.Substring(0, lastNames.Length - 2);
            }
            
            html = html.Replace("{Organization's Director Last Name}", lastNames);
            html = html.Replace("{URL TBD}", string.Format("{0}{1}", url, "#/Compliance?app=" + application.UniqueId + "&c=" + application.ComplianceApplicationId));

            html = html.Replace("{Coordinator Name}", string.Format("{0} {1}", coordinator.FirstName, coordinator.LastName));
            html = html.Replace("{CoordinatorTitle}", coordinator.Title);
            html = html.Replace("{Coordinator Credentials}",
                coordinator.UserCredentials.Count > 0
                    ? string.Join(", ", coordinator.UserCredentials.Select(x => x.Credential.Name))
                    : "");

            html = html.Replace("{Coordinator Phone}", coordinator.PreferredPhoneNumber);
            html = html.Replace("{Coordinator Email Address}", coordinator.EmailAddress);

            html = html.Replace("{URL to application}", string.Format("{0}{1}", url, "#/Compliance?app=" + application.UniqueId + "&c=" + application.ComplianceApplicationId));
            html = html.Replace("{application coordinator's email address}", coordinator.EmailAddress);
            html = html.Replace("{application coordinator's phone number}", coordinator.PreferredPhoneNumber);


            return html;
        }

        public List<AccreditationOutcome> GetAccreditationOutcomeByAppId(Guid applicationUniqueId)
        {
            var accreditationOutcomeManager = this.container.GetInstance<AccreditationOutcomeManager>();

            return accreditationOutcomeManager.GetByAppId(applicationUniqueId).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public void Save(AccreditationOutcomeItem accreditationOutcomeItem, string currentUser, string accessToken)
        {
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var inspectionScheduleFacade = this.container.GetInstance<InspectionScheduleFacade>();
            var accreditationOutcomeManager = this.container.GetInstance<AccreditationOutcomeManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            var accreditationStatusManager = this.container.GetInstance<AccreditationStatusManager>();

            var org = orgManager.GetById(accreditationOutcomeItem.OrganizationId);
            accreditationOutcomeItem.OrganizationName = org.Name;
            var application = applicationManager.GetById(accreditationOutcomeItem.ApplicationId);
            var completeStatus = applicationStatusManager.GetByName(Constants.ApplicationStatus.Complete);
            var cancelledStatus = applicationStatusManager.GetByName(Constants.ApplicationStatus.Cancelled);
            var appResponseStatus = applicationStatusManager.GetByName(Constants.ApplicationStatus.ApplicantResponse);

            var accreditationStatuses = accreditationStatusManager.GetAll();

            var accreditedStatus =
                        accreditationStatuses.SingleOrDefault(x => x.Name == Constants.AccreditationStatuses.Accredited);

            var suspendedStatus =
                            accreditationStatuses.SingleOrDefault(
                                x => x.Name == Constants.AccreditationStatuses.Suspended);

            var withdrawnStatus =
                accreditationStatuses.SingleOrDefault(x => x.Name == Constants.AccreditationStatuses.Withdrawn);

            if (accreditedStatus == null || suspendedStatus == null || withdrawnStatus == null)
            {
                throw new Exception("Cannot Find the Accredition Statuses");
            }

            accreditationOutcomeItem.SendEmail = !string.IsNullOrWhiteSpace(accreditationOutcomeItem.EmailContent);

            accreditationOutcomeManager.Save(accreditationOutcomeItem, currentUser);

            var apps =
                applicationManager.GetByComplianceApplicationId(application.ComplianceApplicationId.GetValueOrDefault());

            foreach (var app in apps)
            {
                if (accreditationOutcomeItem.OutcomeStatusId == (int)Constants.OutcomeStatuses.Level1) //Level1 = Complete
                {
                    app.ApplicationStatusId = completeStatus.Id;
                }
                else if (accreditationOutcomeItem.OutcomeStatusId == (int)Constants.OutcomeStatuses.Level6) //Level6 = Cancelled
                    app.ApplicationStatusId = cancelledStatus.Id;
                else
                {
                    app.ApplicationStatusId = appResponseStatus.Id;
                    app.RFIDueDate = accreditationOutcomeItem.DueDate.GetValueOrDefault();
                }

                app.UpdatedBy = currentUser;
                app.UpdatedDate = DateTime.Now;
                applicationManager.Save(app);
            }

            

            switch (accreditationOutcomeItem.OutcomeStatusId)
            {
                case (int)Constants.OutcomeStatuses.Level1:
                    if (accreditationOutcomeItem.ReportReviewStatusName != Constants.ReportReviewStatus.AddOn)
                    {
                        orgManager.ChangeOrgToAccredited(org, accreditedStatus, currentUser);
                    }

                    var reportingFacade = this.container.GetInstance<ReportingFacade>();
                    

                    var fileName =
                       $"Outcomes Rpt -{org.Name}_{application.CycleNumber}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Year} - {org.Name.Replace(" ", "")}.pdf";

                    reportingFacade.CopyReport(Constants.Reports.OutcomesData, org.DocumentLibraryVaultId,
                        application.ComplianceApplicationId.GetValueOrDefault(), org.Name, 0, true, fileName, fileName);

                    break;
                case (int)Constants.OutcomeStatuses.Level4:
                case (int)Constants.OutcomeStatuses.Level5:
                    var inspectionSchedule =
                        inspectionScheduleManager.GetInspectionScheduleByAppIdOrganizationID(
                            accreditationOutcomeItem.OrganizationId, accreditationOutcomeItem.ApplicationId, false);

                    if (inspectionSchedule != null && inspectionSchedule.Count > 0)
                    {
                        inspectionScheduleFacade.ArchiveScheduleAsync(inspectionSchedule[0].Id);
                    }
                    break;
                case (int)Constants.OutcomeStatuses.Level6:
                    org.AccreditationStatusId = org.AccreditationStatusId == accreditedStatus.Id ? suspendedStatus.Id : withdrawnStatus.Id;
                    break;
            }

            org.UseTwoYearCycle = accreditationOutcomeItem.UseTwoYearCycle;
            org.UpdatedBy = currentUser;
            org.UpdatedDate = DateTime.Now;
            orgManager.Save(org);

            this.SendAccrediationEmail(accreditationOutcomeItem, org, application, accessToken);
        }

        private void SendAccrediationEmail(AccreditationOutcomeItem accreditationOutcomeItem, Organization org, Application application, string accessToken)
        {

            var applicationSettingManager = this.container.GetInstance<ApplicationSettingManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();
            var reportingFacade = this.container.GetInstance<ReportingFacade>();

            if (!string.IsNullOrWhiteSpace(accreditationOutcomeItem.EmailContent))
            {
                Log.Info($"Starting to send email with subject: {accreditationOutcomeItem.Subject}");

                var factEmail = applicationSettingManager.GetByName(Constants.ApplicationSettings.AutoCcEmailAddress);

                Stream accReportStream = null;

                List<String> cc = new List<string>();

                if (!string.IsNullOrEmpty(accreditationOutcomeItem.Cc))
                    cc = accreditationOutcomeItem.Cc.Split(new char[] { ',', ';' }).ToList();

                var emailMessage = EmailHelper.BuildMessage(accreditationOutcomeItem.To.Split(new char[] { ',', ';' }).ToList(), cc, accreditationOutcomeItem.Subject, accreditationOutcomeItem.EmailContent);
                var streams = new List<Stream>();

                foreach (var doc in accreditationOutcomeItem.AttachedDocuments)
                {
                    var stream = trueVaultManager.DownloadFile(accessToken, org.DocumentLibraryVaultId, doc.RequestValues);

                    streams.Add(stream);
                    emailMessage.Message.Attachments.Add(new Attachment(stream, doc.Name));
                }

                if (accreditationOutcomeItem.IncludeAccreditationReport)
                {
                    var documentManager = this.container.GetInstance<DocumentManager>();

                    var cycleNumber = 1;

                    if (org.OrganizationAccreditationCycles != null &&
                        org.OrganizationAccreditationCycles.Count > 0)
                    {
                        cycleNumber = org.OrganizationAccreditationCycles.Max(c => c.Number);
                    }

                    var fileName =
                        $"Accreditation Rpt - {DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Year} - {org.Name.Replace(" ", "")}.pdf";

                    var document = reportingFacade.CopyReport(Constants.Reports.AccreditationReport, org.DocumentLibraryVaultId,
                        application.ComplianceApplicationId.GetValueOrDefault(), org.Name, cycleNumber, false, fileName, fileName);

                    accReportStream = trueVaultManager.DownloadFile(accessToken, org.DocumentLibraryVaultId, document.RequestValues);

                    emailMessage.Message.Attachments.Add(new Attachment(accReportStream, fileName));
                }
                
                emailMessage.SmtpClient.Send(emailMessage.Message);

                streams.ForEach(x => x.Close());
                accReportStream?.Dispose();

                Log.Info($"Finished sending email with subject: {accreditationOutcomeItem.Subject}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var accreditationOutcomeManager = this.container.GetInstance<AccreditationOutcomeManager>();

            await accreditationOutcomeManager.DeleteAsync(id);
        }
    }
}
