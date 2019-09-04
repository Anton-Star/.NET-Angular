using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class ApplicationVersionRepositoryTests
    {
        private TransactionScope scope;
        private IApplicationVersionRepository repository;
        private const int Id = 1;
        private const int OrganizationId = 1;
        private const int ApplicationTypeId = 3;

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new ApplicationVersionRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetActiveApplicationVersionsTest()
        {
            var items = this.repository.GetActive();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetActiveApplicationVersionsAsyncTest()
        {
            var items = this.repository.GetActiveAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetApplicationVersionTest()
        {
            var items = this.repository.GetFlatApplication(Guid.Parse("09739f9a-76ad-4cd3-b1b0-e77de3f628c2"));
            Console.WriteLine(items.Count);
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
            var newItem = new ApplicationVersion
            {
                Id = Guid.NewGuid(),
                ApplicationTypeId = 4,
                Title = "New",
                VersionNumber = "999",
                IsActive = false,
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
            var newItem = new ApplicationVersion
            {
                Id = Guid.NewGuid(),
                ApplicationTypeId = 4,
                Title = "New",
                VersionNumber = "999",
                IsActive = false,
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
