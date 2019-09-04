using FactWeb.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Transactions;

namespace FactWeb.Repository.Tests
{
    [TestClass()]
    public class OrganizationFacilityRepositoryTests
    {
        public FactWebContext context = new FactWebContext();
        private TransactionScope scope;

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.scope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var context = new FactWebContext();
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            this.scope.Dispose();
        }

        [TestMethod()]
        public void AddRelationTest()
        {
            var organizationFacilityRepository = new OrganizationFacilityRepository(context);

            var organizationFacility = new OrganizationFacility();

            var organizationId = new OrganizationRepository(context).GetAll().OrderByDescending(x => x.Id).FirstOrDefault().Id;
            var facilityId = new FacilityRepository(context).GetAll().OrderByDescending(x => x.Id).FirstOrDefault().Id;
            var relation = true;
            var createdBy = "1234";

            var result = organizationFacilityRepository.AddRelationAsync(organizationId, facilityId, relation, createdBy).Result;

            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public void DeleteRelationTest()
        {
            var organizationFacilityRepository = new OrganizationFacilityRepository(context);
            var organizationFacilityId = new OrganizationFacilityRepository(context).GetAll().OrderByDescending(x => x.Id).FirstOrDefault().Id;

            var result = organizationFacilityRepository.DeleteRelationAsync(organizationFacilityId).Result;

            Assert.AreEqual(true, result);
        }

       
    }
}