using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class OrganizationAddressRepositoryTests
    {
        private TransactionScope scope;
        private IOrganizationAddressRepository repository;
        private const int Id = 1;


        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new OrganizationAddressRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetOrganizationAddressByOrganizationTest()
        {
            var items = this.repository.GetByOrganization(1);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetOrganizationAddressByOrganizationAsyncTest()
        {
            var items = this.repository.GetByOrganizationAsync(1).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetOrganizationAddressByAddressTest()
        {
            var items = this.repository.GetByAddress(1);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetOrganizationAddressByAddressAsyncTest()
        {
            var items = this.repository.GetByAddressAsync(1).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllOrganizationAddressesTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllOrganizationAddressesAsyncTest()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddOrganizationAddressTest()
        {
            var newItem = new OrganizationAddress
            {
                OrganizationId = 1,
                AddressId = 2,
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var rows = this.repository.GetByOrganization(1);
            Assert.AreNotEqual(0, rows.Count);

            var find = rows.SingleOrDefault(x => x.AddressId == 2);
            Assert.IsNotNull(find);
            Assert.AreEqual("test", find.CreatedBy);
        }

        [TestMethod]
        public void UpdateOrganizationAddressTest()
        {
            var rows = this.repository.GetByOrganization(1);
            Assert.AreNotEqual(0, rows.Count);
            var row = rows.FirstOrDefault();
            Assert.IsNotNull(row);
            row.UpdatedBy = "test";
            this.repository.Save(row);
            var reselect = this.repository.GetByOrganization(row.OrganizationId);
            Assert.AreNotEqual(0, reselect.Count);
            var check = reselect.SingleOrDefault(x => x.AddressId == row.AddressId);
            Assert.IsNotNull(check);
            Assert.AreEqual("test", check.UpdatedBy);
        }
    }
}
