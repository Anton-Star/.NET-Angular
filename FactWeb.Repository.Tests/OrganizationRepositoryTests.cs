using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class OrganizationRepositoryTests
    {
        private TransactionScope scope;
        private IOrganizationRepository repository;
        private const int Id = 1;
        private const string Name = "Organization 1";


        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new OrganizationRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetOrganizationByIdTest()
        {
            var item = this.repository.GetById(1);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetOrganizationsSearchTest()
        {
            var items = this.repository.Search(Name, string.Empty, string.Empty);
            Assert.AreEqual(1, items.Count);
        }

        [TestMethod]
        public void GetOrganizationsSearchAsyncTest()
        {
            var items = this.repository.SearchAsync(Name, string.Empty, string.Empty).Result;
            Assert.AreEqual(1, items.Count);
        }

        [TestMethod]
        public void GetOrganizationsByFacilityTest()
        {
            var items = this.repository.GetByFacility(1);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetOrganizationsByFacilityAsyncTest()
        {
            var items = this.repository.GetByFacilityAsync(1).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetOrganizationByNameTest()
        {
            var item = this.repository.GetByName(Name);
            Assert.IsNotNull(item);
            Assert.AreEqual("123", item.Number);
        }

        [TestMethod]
        public void GetOrganizationByNameAsyncTest()
        {
            var item = this.repository.GetByNameAsync(Name).Result;
            Assert.IsNotNull(item);
            Assert.AreEqual("123", item.Number);
        }

        [TestMethod]
        public void GetAllOrganizationTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllOrganizationTestAsync()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddOrganizationTest()
        {
            var newItem = new Organization
            {
                Name = "Test",
                Number = "567",
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
        }

        [TestMethod]
        public void AddOrganizationAsyncTest()
        {
            var newItem = new Organization
            {
                Name = "Test",
                Number = "567",
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            Task.Run(async () =>
            {
                await this.repository.AddAsync(newItem);

            }).GetAwaiter().GetResult();

            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
        }

        [TestMethod]
        public void AddOrganizationAsyncBatchTest()
        {
            var newItem = new Organization
            {
                Name = "Test",
                Number = "567",
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.BatchAdd(newItem);
            Task.Run(async () =>
            {
                await this.repository.SaveChangesAsync();

            }).GetAwaiter().GetResult();
            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
        }

        [TestMethod]
        public void UpdateOrganizationTest()
        {
            var row = this.repository.GetById(Id);
            Assert.IsNotNull(row);
            row.UpdatedBy = "test";
            this.repository.BatchSave(row);
            this.repository.SaveChanges();
            var check = this.repository.GetById(Id);
            Assert.IsNotNull(check);
            Assert.AreEqual("test", check.UpdatedBy);
        }

        [TestMethod]
        public void UpdateOrganizationAsyncTest()
        {
            var row = this.repository.GetById(Id);
            Assert.IsNotNull(row);
            row.UpdatedBy = "test";
            this.repository.Save(row);
            var check = this.repository.GetById(Id);
            Assert.IsNotNull(check);
            Assert.AreEqual("test", check.UpdatedBy);
        }

        [TestMethod]
        public void DeleteOrganizationTest()
        {
            var newItem = new Organization
            {
                Name = "Delete",
                Number = "111",
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
