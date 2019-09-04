using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class ApplicationSectionQuestionAnswerRepositoryTests
    {
        private TransactionScope scope;
        private IApplicationSectionQuestionAnswerRepository repository;

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new ApplicationSectionQuestionAnswerRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod, ExpectedException(typeof(NotImplementedException))]
        public void GetApplicationSectionQuestionAnswerByIdTest()
        {
            var item = this.repository.GetById(1);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetApplicationSectionQuestionAnswerByIdGuidTest()
        {
            var item = this.repository.GetById(Guid.Parse("9E955168-C8DA-4C15-AF07-009934AD3B6D"));
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetAllApplicationSectionQuestionAnswersTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllApplicationSectionQuestionAnswersAsyncTest()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddApplicationSectionQuestionAnswerTest()
        {
            var newItem = new ApplicationSectionQuestionAnswer
            {
                Id = Guid.NewGuid(),
                ApplicationSectionQuestionId = Guid.Parse("D8EF105A-FDB7-48C6-93F7-8F1AED093EE1"),
                Text = "New",
                IsActive = true,
                Order = 999,
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
        }

        [TestMethod]
        public void UpdateApplicationSectionQuestionAnswerTest()
        {
            var row = this.repository.GetById(Guid.Parse("9E955168-C8DA-4C15-AF07-009934AD3B6D"));
            Assert.IsNotNull(row);
            row.UpdatedBy = "test";
            this.repository.Save(row);
            var check = this.repository.GetById(row.Id);
            Assert.IsNotNull(check);
            Assert.AreEqual("test", check.UpdatedBy);
        }

        [TestMethod]
        public void DeleteApplicationSectionQuestionAnswerTest()
        {
            var newItem = new ApplicationSectionQuestionAnswer
            {
                Id = Guid.NewGuid(),
                ApplicationSectionQuestionId = Guid.Parse("D8EF105A-FDB7-48C6-93F7-8F1AED093EE1"),

                Text = "Delete",
                IsActive = true,
                Order = 999,
                CreatedBy = "test",
                CreatedDate = DateTime.Now
            };

            this.repository.Add(newItem);
            var row = this.repository.GetById(newItem.Id);
            Assert.IsNotNull(row);
            row.IsActive = false;
            this.repository.Save(row);
            row = this.repository.GetById(row.Id);
            Assert.IsNotNull(row);
            Assert.AreEqual(false, row.IsActive);
        }
    }
}
