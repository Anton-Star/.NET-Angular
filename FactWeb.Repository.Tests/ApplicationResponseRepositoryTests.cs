using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class ApplicationResponseRepositoryTests
    {
        private TransactionScope scope;
        private IApplicationResponseRepository repository;
        private const int Id = 1;
        private const int OrganizationId = 1;
        private const int ApplicationTypeId = 3;

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new ApplicationResponseRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetApplicationResponsesByOrganizationAndApplicationTest()
        {
            var items = this.repository.GetApplicationResponses(OrganizationId, ApplicationTypeId);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetApplicationResponsesByOrganizationAndApplicationAsyncTest()
        {
            var items = this.repository.GetApplicationResponsesAsync(OrganizationId, ApplicationTypeId).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllApplicationResponsesTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllApplicationResponsesAsyncTest()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddApplicationResponseTest()
        {
            var newItem = new ApplicationResponse
            {
                ApplicationId = 10,
                ApplicationSectionQuestionId = Guid.Parse("D8EF105A-FDB7-48C6-93F7-8F1AED093EE1"),
                Text = "Add",
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
        }

        [TestMethod]
        public void UpdateApplicationResponseTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
            var row = items.First();
            Assert.IsNotNull(row);
            row.UpdatedBy = "test";
            this.repository.Save(row);
            var check = this.repository.GetById(row.Id);
            Assert.IsNotNull(check);
            Assert.AreEqual("test", check.UpdatedBy);
        }

        [TestMethod]
        public void DeleteApplicationResponseTest()
        {
            var newItem = new ApplicationResponse
            {
                ApplicationId = 10,
                ApplicationSectionQuestionId = Guid.Parse("D8EF105A-FDB7-48C6-93F7-8F1AED093EE1"),
                Text = "Delete",
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
            this.repository.Remove(row.Id);
            row = this.repository.GetById(newItem.Id);
            Assert.IsNull(row);
        }
    }
}
