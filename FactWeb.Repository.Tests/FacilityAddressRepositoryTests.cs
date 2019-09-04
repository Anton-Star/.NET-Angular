using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class FacilityAddressRepositoryTests
    {
        private TransactionScope scope;
        private IFacilityAddressRepository repository;
        private const int Id = 1;


        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new FacilityAddressRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetFacilityAddressByFacilityTest()
        {
            var items = this.repository.GetByFacility(1);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetFacilityAddressByFacilityAsyncTest()
        {
            var items = this.repository.GetByFacilityAsync(1).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetFacilityAddressByAddressTest()
        {
            var items = this.repository.GetByAddress(1);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetFacilityAddressByAddressAsyncTest()
        {
            var items = this.repository.GetByAddressAsync(1).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllFacilityAddressesTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllFacilityAddressesAsyncTest()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddFacilityAddressTest()
        {
            var newItem = new FacilityAddress
            {
                FacilityId = 1,
                AddressId = 2,
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var rows = this.repository.GetByFacility(1);
            Assert.AreNotEqual(0, rows.Count);

            var find = rows.SingleOrDefault(x => x.AddressId == 2);
            Assert.IsNotNull(find);
            Assert.AreEqual("test", find.CreatedBy);
        }

        [TestMethod]
        public void UpdateFacilityAddressTest()
        {
            var rows = this.repository.GetByFacility(1);
            Assert.AreNotEqual(0, rows.Count);
            var row = rows.FirstOrDefault();
            Assert.IsNotNull(row);
            row.UpdatedBy = "test";
            this.repository.Save(row);
            var reselect = this.repository.GetByFacility(row.FacilityId);
            Assert.AreNotEqual(0, reselect.Count);
            var check = reselect.SingleOrDefault(x => x.AddressId == row.AddressId);
            Assert.IsNotNull(check);
            Assert.AreEqual("test", check.UpdatedBy);
        }
    }
}
