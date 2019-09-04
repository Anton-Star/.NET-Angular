using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;

namespace FactWeb.BusinessFacade
{
    public class EmailFacade
    {
        private readonly Container container;

        public EmailFacade(Container container)
        {
            this.container = container;
        }

        public void Send(List<string> tos, List<string> ccs, string subject, string html,
            bool includeAccreditationReport, int cycleNumber, string orgName = "", Guid? complianceApplicationId = null, string apiKey = "", bool factOnly = true)
        {
            var vaultId = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.FactOnlyVault];
            apiKey = string.IsNullOrEmpty(apiKey) ? ConfigurationManager.AppSettings[Constants.ConfigurationConstants.DocumentLibraryApiKey] : apiKey;

            if (includeAccreditationReport)
            {
                var organizationManager = this.container.GetInstance<OrganizationManager>();
                var reportingFacade = this.container.GetInstance<ReportingFacade>();
                var trueVaultManager = this.container.GetInstance<TrueVaultManager>();

                var org = organizationManager.GetByName(orgName);

                if (org != null && !factOnly)
                {
                    vaultId = org.DocumentLibraryVaultId;
                }

                var fileName =
                    $"Accreditation Rpt - {DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Year} {(factOnly ? "- Inspector" : "")} - {org.Name.Replace(" ", "")}.pdf";

                var document = reportingFacade.CopyReport(Constants.Reports.AccreditationReport, vaultId,
                    complianceApplicationId.GetValueOrDefault(), org.Name, cycleNumber, factOnly, fileName, fileName);

                using (var stream = trueVaultManager.DownloadFile(apiKey, vaultId, document.RequestValues))
                {
                    var attachment = new Attachment(stream, fileName);
                    EmailHelper.Send(tos, ccs, subject, html, attachment);
                }

            }
            else
            {
                EmailHelper.Send(tos, ccs, subject, html);
            }
        }
    }
}
