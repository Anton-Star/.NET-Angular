using FactWeb.BusinessLayer;
using FactWeb.Repository;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System.Linq;

namespace FactWeb.BusinessFacade.Tests
{
    [TestClass()]
    public class OrganizationFacilityFacadeTests
    {
        readonly Container container = new Container();
        private OrganizationFacilityFacade facade;

        [TestInitialize]
        public void Initialize()
        {
            this.container.Register<FactWebContext>();
            this.container.Register<IOrganizationFacilityRepository, OrganizationFacilityRepository>();
            this.container.Register<IFacilityRepository, FacilityRepository>();
            this.container.Register<OrganizationFacilityManager>();
            this.container.Register<FacilityManager>();

            this.facade = new OrganizationFacilityFacade(this.container);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var listOrganizationFacility = this.facade.GetAll();
            Assert.AreNotEqual(0, listOrganizationFacility.Count);
        }

        //[TestMethod()]
        //public void GetAllAsyncTest()
        //{
        //    var listOrganizationFacility = this.facade.GetAllAsync().Result;
        //    Assert.AreNotEqual(0, listOrganizationFacility.Count);
        //}

        [TestMethod()]
        public void AddRelationAsyncTest()
        {
            var result = this.facade.SaveRelationAsync(203,1, 1, true, "kamranu@5thmethod.com").Result;

            Assert.AreNotEqual(result, false);
        }
        
        [TestMethod()]
        public void AddRelationTest()
        {
            var result = this.facade.AddRelation(1, 1, true, "kamranu@5thmethod.com");

            Assert.AreNotEqual(result, false);
        }

        [TestMethod()]
        public void DeleteRelationAsyncTest()
        {
            var result = this.facade.DeleteRelationAsync(1).Result;
            Assert.AreEqual(true, result);

            var deletedRecord = this.facade.GetAll().FirstOrDefault(x => x.Id == 1);

            Assert.AreEqual(deletedRecord, null);
        }

        [TestMethod()]
        public void DeleteRelationTest()
        {
            var result = this.facade.DeleteRelation(1);
            Assert.AreEqual(true, result);

            var deletedRecord = this.facade.GetAll().FirstOrDefault(x => x.Id == 1);

            Assert.AreEqual(deletedRecord, null);
        }

        [TestMethod()]
        public void IsDuplicateRelationTest()
        {
            var result = this.facade.IsDuplicateRelation(0,1, 2);

            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        public void IsDuplicateRelationAsyncTest()
        {
            var result = this.facade.IsDuplicateRelationAsync(0,1, 2).Result;

            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        public void CheckBusinessRulesTest()
        {
            var result = this.facade.CheckBusinessRules(0,7, 5, "kamranu@5thmethod.com");

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod()]
        public void CheckBusinessRulesAsyncTest()
        {
            var result = this.facade.CheckBusinessRulesAsync(0,7, 5, "kamranu@5thmethod.com").Result;

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod()]
        public void SearchAsyncTest()
        {
            var result = this.facade.SearchAsync(1, 1).Result;

            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod()]
        public void SearchTest()
        {
            var result = this.facade.Search(1, 1);

            Assert.AreNotEqual(0, result.Count);
        }
    }
}