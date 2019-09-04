using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class ApplicationSettingRepositoryTests
    {
        private TransactionScope scope;
        private IApplicationSettingRepository repository;
        private const int Id = 1;


        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new ApplicationSettingRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetApplicationSettingByNameTest()
        {
            var item = this.repository.GetByName(Constants.ApplicationSettings.InspectorMileageRange);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetApplicationSettingByNameAsyncTest()
        {
            var item = this.repository.GetByNameAsync(Constants.ApplicationSettings.InspectorMileageRange).Result;
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetAllApplicationSettingsTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllApplicationSettingsAsyncTest()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddApplicationSettingTest()
        {
            var newItem = new ApplicationSetting
            {
                Name = "New",
                Value = "Test",
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
        }

        [TestMethod]
        public void UpdateApplicationSettingTest()
        {
            var row = this.repository.GetByName(Constants.ApplicationSettings.InspectorMileageRange);
            Assert.IsNotNull(row);
            row.UpdatedBy = "test";
            this.repository.Save(row);
            var check = this.repository.GetById(row.Id);
            Assert.IsNotNull(check);
            Assert.AreEqual("test", check.UpdatedBy);
        }

        [TestMethod]
        public void DeleteApplicationSettingTest()
        {
            var newItem = new ApplicationSetting
            {
                Name = "Delete",
                Value = "Test",
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
