using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace FactWeb.BusinessFacade
{
    public class ReportingFacade
    {
        private readonly Container container;

        public ReportingFacade(Container container)
        {
            this.container = container;
        }

        public Document CopyReport(string reportName, string vaultId, Guid appId, string organizationName, int cycleNumber = 0, bool factOnly = true, string fileName = "", string fullFileName = "")
        {
            var manager = new ReportingManager();
            var documentFacade = this.container.GetInstance<DocumentFacade>();
            var login = manager.Login();
            
            var parms = new Dictionary<string, string>();

            switch (reportName)
            {
                case Constants.Reports.AccreditationReport:
                    parms = new Dictionary<string, string>
                    {
                        {"complianceApplicationId", appId.ToString()},
                        {
                            "orgName", organizationName.Replace(" ", "+")
                        }
                    };

                    if (string.IsNullOrEmpty(fullFileName))
                    {
                        fullFileName =
                        $"Accreditation Rpt - {organizationName.Replace(" ", "")}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Year}.pdf";
                    }

                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = $"Accreditation Rpt - {DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Year} - .pdf";
                    }

                    break;
                case Constants.Reports.InspectionSummary:
                    parms = new Dictionary<string, string>
                    {
                        {"complianceApplicationId", appId.ToString()},
                        {
                            "orgName", organizationName.Replace(" ", "+")
                        }
                    };
                    fullFileName =
                        $"InspectionSummary_{organizationName.Replace(" ", "")}_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.pdf";

                    fileName = $"InspectionSummary_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.pdf";
                    break;
                case Constants.Reports.SingleApplication:
                    parms = new Dictionary<string, string>
                    {
                        {"applicationUniqueId", appId.ToString()}
                    };
                    fullFileName =
                        $"AnnualApplication_{organizationName.Replace(" ", "")}_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.pdf";

                    fileName = $"AnnualApplication_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.pdf";
                    break;
                case Constants.Reports.OutcomesData:
                    parms = new Dictionary<string, string>
                    {
                        {"complianceApplicationId", appId.ToString()},
                        {
                            "orgName", organizationName.Replace(" ", "+")
                        }
                    };
                    if (string.IsNullOrWhiteSpace(fullFileName))
                    {
                        fullFileName =
                            $"Outcomes/Data Rpt - {organizationName.Replace(" ", "")}_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.pdf";
                    }

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        fileName = $"Outcomes/Data Rpt - {organizationName.Replace(" ", "")}_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.pdf";
                    }

                    
                    break;
            }

            using (var stream = manager.DownloadPdf(login.Access_token, reportName, parms))
            {
                var trueVaultManager = new TrueVaultManager();
                var apiKey = ConfigurationManager.AppSettings["DocumentLibraryApiKey"];

                var result = trueVaultManager.Upload(apiKey, vaultId, fullFileName, stream);

                if (result.Result != TrueVaultManager.Success)
                {
                    throw new Exception("Error saving to True Vault");
                }

                return documentFacade.AddToLibrary(appId, organizationName, fileName, null, factOnly, "System",
                    result.Blob_id, true, false);
            }

        }
    }
}
