using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class ComplianceApplicationRepositoryTests
    {
        private TransactionScope scope;
        private IComplianceApplicationRepository repository;

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new ComplianceApplicationRepository(context);
        }

        [TestMethod]
        public void HasAccessTest()
        {
            var items = this.repository.DoesInspectorHaveAccess(null, Guid.Parse("1dead9c3-d759-4ba6-8450-1de160e0aa25"), Guid.Parse("c03b99da-f2e8-4f4b-88d7-050bd2702728"));
            Assert.IsNotNull(items);

            Assert.AreEqual(true, items.Found);
        }

        [TestMethod]
        public void UpdateStatus()
        {
            this.repository.UpdateComplianceApplicationStatus(Guid.Parse("52bef1f2-8680-4112-ac8f-002b9383e8a3"), "RFI In Progress", "Nick");

            var row = this.repository.GetById(Guid.Parse("52bef1f2-8680-4112-ac8f-002b9383e8a3"));

            Assert.IsNotNull(row);

            Assert.AreEqual("RFI In Progress", row.ApplicationStatus.Name);
        }
    }
}
