using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class AddressRepositoryTests
    {
        private TransactionScope scope;
        private IAddressRepository repository;
        private const int Id = 1;


        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new AddressRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetAddressByIdTest()
        {
            var item = this.repository.GetById(1);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetAllAddressesTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllAddressesTestAsync()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddAddressTest()
        {
            var newItem = new Address
            { 
                AddressTypeId = 1,
                City = "Test",
                //State = "PA",
                Street1 = "Street",
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
        }

        [TestMethod]
        public void UpdateAddressTest()
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
        public void DeleteAddressTest()
        {
            var newItem = new Address
            {
                AddressTypeId = 1,
                City = "Test",
                //State = "PA",
                Street1 = "Street",
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
