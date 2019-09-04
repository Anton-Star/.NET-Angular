using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Repository;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System;

namespace FactWeb.BusinessFacade.Tests
{
    [TestClass]
    public class ReportingFacadeTests
    {
        readonly Container container = new Container();

        [TestInitialize]
        public void Initialize()
        {
            this.container.Register<FactWebContext>();
            this.container.Register<IDocumentRepository, DocumentRepository>();
            this.container.Register<IOrganizationRepository, OrganizationRepository>();
            this.container.Register<DocumentManager>();
            this.container.Register<OrganizationManager>();
            this.container.Register<TrueVaultManager>();
            this.container.Register<ReportingManager>();
        }

        [TestMethod]
        public void SaveReportTest()
        {
            var reportingFacade = new ReportingFacade(this.container);
            reportingFacade.CopyReport(Constants.Reports.InspectionSummary, "063f366e-34bc-4dbf-b3a6-9c7e11d0c763", Guid.Parse("3d770eb7-d7b8-48e4-a155-41f482e86cc7"), "cGMP Cell Processing Facility Diabetes Research Institute University of Miami Miller School of Medicine ORG", 1);
        }

        [TestMethod]
        public void SaveAnnualReportTest()
        {
            var reportingFacade = new ReportingFacade(this.container);
            reportingFacade.CopyReport(Constants.Reports.SingleApplication, "063f366e-34bc-4dbf-b3a6-9c7e11d0c763", Guid.Parse("2ac83abc-161d-42d0-bdb0-1399e7e1607a"), "Ann & Robert H. Lurie Children's Hospital of Chicago Stem Cell Transplant Program", 1);
        }
    }
}
