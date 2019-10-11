﻿using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private TransactionScope scope;
        private IAddressTypeRepository repository;
        private const int Id = 1;


        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new AddressTypeRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetAddressTypeByIdTest()
        {
            var item = this.repository.GetById(1);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetAllAddressTypesTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllAddressTypesAsyncTest()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddAddressTypeTest()
        {
            var newItem = new AddressType
            {
                Name = "New",
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
        }

        [TestMethod]
        public void UpdateAddressTypeTest()
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
        public void DeleteAddressTypeTest()
        {
            var newItem = new AddressType
            {
                Name = "Delete",
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