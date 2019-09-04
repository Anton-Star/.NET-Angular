using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class ApplicationSectionRepositoryTests
    {
        private TransactionScope scope;
        private IApplicationSectionRepository repository;
        private const int Id = 1;
        private const string Name = "Fact Staff";

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new ApplicationSectionRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod, ExpectedException(typeof(NotImplementedException))]
        public void GetApplicationSectionByIdIntTest()
        {
            var item = this.repository.GetById(1);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetApplicationSectionByIdGuidTest()
        {
            var item = this.repository.GetById(Guid.Parse("51b3de50-b330-46ee-bac8-6211544b95eb"));
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetApplicationSectionsByApplicationTypeTest()
        {
            var items = this.repository.GetAllForApplicationType(3);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetApplicationSectionsByApplicationTypeAsyncTest()
        {
            var items = this.repository.GetAllForApplicationTypeAsync(3).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetApplicationSectionsByApplicationTypeNameTest()
        {
            var items = this.repository.GetAllForApplicationType(Constants.ApplicationTypes.Eligibility);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetApplicationSectionsByApplicationTypeNameAsyncTest()
        {
            var items = this.repository.GetAllForApplicationTypeAsync(Constants.ApplicationTypes.Eligibility).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllApplicationSectionsTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllApplicationSectionsAsyncTest()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetRootApplicationSectionsTest()
        {
            var items = this.repository.GetRootItems(5);
            Assert.AreEqual(319, items.Count);
        }

        [TestMethod]
        public void GetAllActiveForApplicationTypeTest()
        {
            var items = this.repository.GetAllActiveForApplicationType(3);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllActiveForApplicationTypeAsyncTest()
        {
            var items = this.repository.GetAllActiveForApplicationTypeAsync(3).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetRootApplicationSectionsAsyncTest()
        {
            var items = this.repository.GetRootItemsAsync(3).Result;
            Assert.AreEqual(7, items.Count);
        }

        [TestMethod]
        public void AddApplicationSectionTest()
        {
            var newItem = new ApplicationSection
            {
                ApplicationTypeId = 3,
                ApplicationVersionId = Guid.Parse("09739F9A-76AD-4CD3-B1B0-E77DE3F628C2"),
                PartNumber = 999,
                Name = "New",
                Order = "999",
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
        }

        [TestMethod]
        public void UpdateApplicationSectionTest()
        {
            var row = this.repository.GetById(Guid.Parse("51b3de50-b330-46ee-bac8-6211544b95eb"));
            Assert.IsNotNull(row);
            row.UpdatedBy = "test";
            this.repository.Save(row);
            var check = this.repository.GetById(row.Id);
            Assert.IsNotNull(check);
            Assert.AreEqual("test", check.UpdatedBy);
        }
    }
}
