using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class ApplicationSectionQuestionRepositoryTests
    {
        private TransactionScope scope;
        private IApplicationSectionQuestionRepository repository;

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new ApplicationSectionQuestionRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod, ExpectedException(typeof(NotImplementedException))]
        public void GetApplicationSectionQuestionByIdTest()
        {
            var item = this.repository.GetById(1);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetApplicationSectionQuestionByIdGuidTest()
        {
            var item = this.repository.GetById(Guid.Parse("AB4C012A-6745-4AD5-A16E-00A7CFF39182"));
            Assert.IsNotNull(item);
        }
        
        [TestMethod]
        public void GetAllApplicationSectionQuestionsTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllApplicationSectionQuestionsAsyncTest()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddApplicationSectionQuestionTest()
        {
            var newItem = new ApplicationSectionQuestion
            {
                ApplicationSectionId = Guid.Parse("5285E76D-AD44-4CD8-B63D-00E112361DC7"),
                QuestionTypeId = 1,
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
        public void UpdateApplicationSectionQuestionTest()
        {
            var row = this.repository.GetById(Guid.Parse("AB4C012A-6745-4AD5-A16E-00A7CFF39182"));
            Assert.IsNotNull(row);
            row.UpdatedBy = "test";
            this.repository.Save(row);
            var check = this.repository.GetById(row.Id);
            Assert.IsNotNull(check);
            Assert.AreEqual("test", check.UpdatedBy);
        }

        [TestMethod]
        public void DeleteApplicationSectionQuestionTest()
        {
            var newItem = new ApplicationSectionQuestion
            {
                ApplicationSectionId = Guid.Parse("5285E76D-AD44-4CD8-B63D-00E112361DC7"),
                QuestionTypeId = 1,
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
