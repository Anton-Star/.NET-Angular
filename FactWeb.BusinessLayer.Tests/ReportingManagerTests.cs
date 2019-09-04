using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace FactWeb.BusinessLayer.Tests
{
    [TestClass]
    public class ReportingManagerTests
    {
        [TestMethod]
        public void ReportingLoginTest()
        {
            var manager = new ReportingManager();
            var login = manager.Login();

            Assert.IsNotNull(login);
            Assert.AreNotEqual("", login.Access_token);
            Console.WriteLine(login.Access_token);
        }

        [TestMethod]
        public void GetAllReportsTest()
        {
            var manager = new ReportingManager();
            var login = manager.Login();
            Assert.IsNotNull(login);
            Assert.AreNotEqual("", login.Access_token);

            var reports = manager.GetReports(login.Access_token);

            Assert.IsNotNull(reports);
            Assert.AreNotEqual(0, reports.Count);

            foreach (var report in reports)
            {
                Console.WriteLine($"{report.Id} - {report.Name}");
            }
        }

        [TestMethod]
        public void GetReportTest()
        {
            var manager = new ReportingManager();
            var login = manager.Login();
            Assert.IsNotNull(login);
            Assert.AreNotEqual("", login.Access_token);

            var report = manager.GetReport(login.Access_token, "Inspection Summary");

            Assert.IsNotNull(report);
            Assert.AreNotEqual("", report.Id);
            Console.WriteLine($"{report.Id} - {report.Name}");
        }

        [TestMethod]
        public void GetReportDocumentTest()
        {
            var manager = new ReportingManager();
            var login = manager.Login();
            Assert.IsNotNull(login);
            Assert.AreNotEqual("", login.Access_token);

            var parms = new Dictionary<string, string>
            {
                {"complianceApplicationId", "3c52f351-fd66-4738-8d2d-b215e0159171"},
                {
                    "orgName",
                    "First ORG"
                }
            };

            var document = manager.GetPdfDocument(login.Access_token, "Accreditation Report", parms);

            Assert.IsNotNull(document);
            Assert.AreNotEqual("", document.DocumentId);

            Console.WriteLine(document.DocumentId);
        }

        [TestMethod]
        public void DownloadReportTest()
        {
            var manager = new ReportingManager();
            var login = manager.Login();
            Assert.IsNotNull(login);
            Assert.AreNotEqual("", login.Access_token);

            var parms = new Dictionary<string, string>
            {
                {"complianceApplicationId", "3d770eb7-d7b8-48e4-a155-41f482e86cc7"},
                {
                    "orgName",
                    "cGMP+Cell+Processing+Facility+Diabetes+Research+Institute+University+of+Miami+Miller+School+of+Medicine+ORG"
                }
            };

            using (var stream = manager.DownloadPdf(login.Access_token, "Inspection Summary", parms))
            {
                Assert.IsNotNull(stream);

                var trueVaultManager = new TrueVaultManager();
                var apiKey = ConfigurationManager.AppSettings["DocumentLibraryApiKey"];

                var result = trueVaultManager.Upload(apiKey, "9c0d8021-0660-4e03-9fb0-00b47e0c76dd", "InspectionSummary_2016_12_22.pdf", stream);

                Assert.IsNotNull(result);
                Assert.AreNotEqual("", result.Result);
                Console.WriteLine(result.Blob_id);
                //stream.Position = 0;
                //File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "\\test.pdf", stream.ToArray());
            }
            
        }
    }
}
