using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class UserAddressRepositoryTests
    {
        private TransactionScope scope;
        private IUserAddressRepository repository;
        private static readonly Guid Id = Guid.Parse("f7310ef2-ace8-4308-a593-036e6a0c8758");


        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new UserAddressRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetUserAddressByFacilityTest()
        {
            var items = this.repository.GetByUser(Id);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetUserAddressByFacilityAsyncTest()
        {
            var items = this.repository.GetByUserAsync(Id).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetUserAddressByAddressTest()
        {
            var items = this.repository.GetByAddress(1);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetUserAddressByAddressAsyncTest()
        {
            var items = this.repository.GetByAddressAsync(1).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllUserAddressesTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllUserAddressesAsyncTest()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddUserAddressTest()
        {
            var newItem = new UserAddress
            {
                UserId = Id,
                AddressId = 2,
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var rows = this.repository.GetByUser(Id);
            Assert.AreNotEqual(0, rows.Count);

            var find = rows.SingleOrDefault(x => x.AddressId == 2);
            Assert.IsNotNull(find);
            Assert.AreEqual("test", find.CreatedBy);
        }

        [TestMethod]
        public void UpdateUserAddressTest()
        {
            var rows = this.repository.GetByUser(Id);
            Assert.AreNotEqual(0, rows.Count);
            var row = rows.FirstOrDefault();
            Assert.IsNotNull(row);
            row.UpdatedBy = "test";
            this.repository.Save(row);
            var reselect = this.repository.GetByUser(row.UserId);
            Assert.AreNotEqual(0, reselect.Count);
            var check = reselect.SingleOrDefault(x => x.AddressId == row.AddressId);
            Assert.IsNotNull(check);
            Assert.AreEqual("test", check.UpdatedBy);
        }
    }
}
