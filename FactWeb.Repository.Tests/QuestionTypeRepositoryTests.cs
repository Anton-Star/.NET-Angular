using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass]
    public class QuestionTypeRepositoryTests
    {
        private TransactionScope scope;
        private IQuestionTypeRepository repository;
        private const int Id = 1;
        private const string Name = "Fact Staff";

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
            this.repository = new QuestionTypeRepository(context);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod]
        public void GetQuestionTypeByIdTest()
        {
            var item = this.repository.GetById(1);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetAllQuestionTypesTest()
        {
            var items = this.repository.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void GetAllQuestionTypesAsyncTest()
        {
            var items = this.repository.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void AddQuestionTypeTest()
        {
            var newItem = new QuestionType
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
        public void UpdateQuestionTypeTest()
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
        public void DeleteQuestionTypeTest()
        {
            var newItem = new QuestionType
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
