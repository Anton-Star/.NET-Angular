using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private TransactionScope scope;
        private IUserRepository repository;
        private static readonly Guid Id = Guid.Parse("f7310ef2-ace8-4308-a593-036e6a0c8758");


        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new UserRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            var item = this.repository.GetById(Id);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetUserByEmailTest()
        {
            var item = this.repository.GetByEmailAddress("test@test.com");
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetUserByTokenTest()
        {
            var item = this.repository.GetByToken("ABC123");
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetUserByTokenAsyncTest()
        {
            var item = this.repository.GetByTokenAsync("ABC123").Result;
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetUsersByFirstLastEmailTest()
        {
            var item = this.repository.GetByFirstNameLastNameOrEmailAddress("Org", "Admin", null);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetUsersByFirstLastEmailAsyncTest()
        {
            var item = this.repository.GetByFirstNameLastNameOrEmailAddressAsync("Org", "Admin", null).Result;
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetUsersByOrgTest()
        {
            var items = this.repository.GetByOrganization(1);
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetUsersByOrgAsyncTest()
        {
            var items = this.repository.GetByOrganizationAsync(1).Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetUserByOrgAndEmailTest()
        {
            var item = this.repository.GetByOrganizationAndEmailAddress(1, "test@test.com");
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetUsersByOrgAndEmailAsyncTest()
        {
            var item = this.repository.GetByOrganizationAndEmailAddressAsync(1, "test@test.com").Result;
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetAllUsersTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllUsersAsyncTest()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddUserTest()
        {
            var newItem = new User
            {
                Id = Guid.NewGuid(),
                //OrganizationId = 1,
                FirstName = "Add",
                LastName = "Test",
                EmailAddress = "add.test@test.com",
                Password = "blah",
                PreferredPhoneNumber = "1234567890",
                WorkPhoneNumber = "1234567890",
                RoleId = 1,
                PasswordChangeDate = DateTime.Now,
                IsActive = true,
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var find = this.repository.GetById(Id);
            Assert.IsNotNull(find);
            Assert.AreEqual("test", find.CreatedBy);
        }

        [TestMethod]
        public void UpdateUserTest()
        {
            var row = this.repository.GetById(Id);
            Assert.IsNotNull(row);
            row.UpdatedBy = "test";
            this.repository.Save(row);
            var check = this.repository.GetById(row.Id);
            Assert.IsNotNull(check);
            Assert.AreEqual("test", check.UpdatedBy);
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            var newItem = new User
            {
                Id = Guid.NewGuid(),
               // OrganizationId = 1,
                FirstName = "Delete",
                LastName = "Test",
                EmailAddress = "add.test@test.com",
                Password = "blah",
                PreferredPhoneNumber = "1234567890",
                WorkPhoneNumber = "1234567890",
                RoleId = 1,
                PasswordChangeDate = DateTime.Now,
                IsActive = true,
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
