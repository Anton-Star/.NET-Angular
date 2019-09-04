using FactWeb.Infrastructure;
using FactWeb.Model.Reporting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace FactWeb.BusinessLayer
{
    public class ReportingManager
    {
        private const string Server = "http://appsfactwebsite.centralus.cloudapp.azure.com:83/";
        private const string Api = "api/reportserver/";
        private readonly string userName;
        private readonly string password;

        public ReportingManager()
        {
            this.userName = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.ReportingUserName];
            this.password = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.ReportingPassword];
        }

        public Login Login()
        {
            var response = WebHelper.Post<Login>($"{Server}Token", $"grant_type=password&username={this.userName}&password={this.password}", null, null, null);

            return response;
        }

        public List<Report> GetReports(string token)
        {
            var response = WebHelper.Get<List<Report>>($"{Server}{Api}reports", "Bearer", token);

            return response;
        }

        public Report GetReport(string token, string reportName)
        {
            var reports = this.GetReports(token);

            var report = reports.SingleOrDefault(x => x.Name == reportName);

            return report;
        }

        public Document GetPdfDocument(string token, string reportName, Dictionary<string, string> parameters)
        {
            var report = this.GetReport(token, reportName);

            var parms = new StringBuilder();
            var parmValues = string.Empty;

            if (parameters != null)
            {
                foreach (var parm in parameters)
                {
                    parms.Append("\"" + parm.Key + "\": \"" + parm.Value + "\", ");
                }

                parmValues = parms.ToString();
                parmValues = parmValues.Substring(0, parmValues.Length - 2);
            }

            var values = "{\"ReportId\": \"" + report.Id.Replace(" ", "") + "\", \"Format\": \"pdf\", \"ParameterValues\": {" + parmValues + "} }";

            var response = WebHelper.Post<Document>($"{Server}{Api}documents", values, "Bearer", "application/json", token);
  
            return response;
        }

        public MemoryStream DownloadPdf(string token, string reportName, Dictionary<string, string> parameters)
        {
            var document = this.GetPdfDocument(token, reportName, parameters);

            if (document == null)
            {
                throw new Exception("Cannot get document");
            }

            return WebHelper.Download($"{Server}{Api}documents/{document.DocumentId}?content-disposition=attachment");
        }
    }
}
