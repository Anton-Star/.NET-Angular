using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class FacilityRepositoryTests
    {
        private TransactionScope scope;
        private IFacilityRepository repository;
        private const int Id = 1;
        private const string Name = "OneBlood, Inc";


        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new FacilityRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetFacilityByIdTest()
        {
            var item = this.repository.GetById(1);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetFacilityByNameTest()
        {
            var item = this.repository.GetByName(Name);
            Assert.IsNotNull(item);
            Assert.AreEqual(1, item.ServiceTypeId);
        }

        [TestMethod]
        public void GetFacilityByNameAsyncTest()
        {
            var item = this.repository.GetByNameAsync(Name).Result;
            Assert.IsNotNull(item);
            Assert.AreEqual(1, item.ServiceTypeId);
        }

        [TestMethod]
        public void GetAllFacilityTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllFacilityTestAsync()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddFacilityTest()
        {
            var newItem = new Facility
            {
                Name = "Test",
                ServiceTypeId = 1,
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
        }

        [TestMethod]
        public void UpdateFacilityTest()
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
        public void DeleteFacilityTest()
        {
            var newItem = new Facility
            {
                Name = "Delete",
                ServiceTypeId = 1,
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
