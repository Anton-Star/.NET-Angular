using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer.Tests
{
    [TestClass()]
    public class OrganizationFacilityManagerTests
    {
        private Mock<IOrganizationFacilityRepository> repositoryMock;
        private OrganizationFacilityManager manager;

        [TestInitialize]
        public void Initialize()
        {
            this.repositoryMock = new Mock<IOrganizationFacilityRepository>();
            this.manager = new OrganizationFacilityManager(this.repositoryMock.Object);

            var items = new List<OrganizationFacility>
                        {
                            new OrganizationFacility
                            {
                                Id = 1,
                                OrganizationId = 1,
                                FacilityId = 1,
                                CreatedBy = "1234",
                                Organization = new Organization
                                {
                                    Id = 1,
                                    Name = "Org 1",
                                    Number = "12345"
                                },
                                Facility = new Facility
                                {
                                    Id = 1,
                                    Name = "Fac 1"
                                }
                            },
                            new OrganizationFacility
                            {
                                Id = 2,
                                OrganizationId = 1,
                                FacilityId = 2,
                                CreatedBy = "12345",
                                Organization = new Organization
                                {
                                    Id = 1,
                                    Name = "Org 1",
                                    Number = "12345"
                                },
                                Facility = new Facility
                                {
                                    Id = 2,
                                    Name = "Fac 2"
                                }
                            },
                            new OrganizationFacility
                            {
                                Id = 3,
                                OrganizationId = 2,
                                FacilityId = 3,
                                CreatedBy = "1234",
                                Organization = new Organization
                                {
                                    Id = 2,
                                    Name = "Org 2",
                                    Number = "12345"
                                },
                                Facility = new Facility
                                {
                                    Id = 3,
                                    Name = "Fac 3"
                                }
                            },
                        };

            this.repositoryMock.Setup(u => u.GetAll())
                .Returns(items);

            this.repositoryMock.Setup(u => u.GetAllAsync())
                .Returns(Task.FromResult(items));
        }

        [TestMethod()]
        public void AddRelationTest()
        {
            this.manager.AddRelation(1, 1, true, "123");

            this.repositoryMock.Verify(x=>x.AddRelation(1, 1, true, "123"), Times.Once);
        }

        [TestMethod()]
        public void AddRelationAsyncTest()
        {
            this.manager.SaveRelationAsync(0,1, 1, true, "123");

            this.repositoryMock.Verify(x => x.AddRelationAsync(1, 1, true, "123"), Times.Once);
        }

        [TestMethod()]
        public void DeleteRelationTest()
        {
            this.manager.DeleteRelation(1);

            this.repositoryMock.Verify(x => x.DeleteRelation(1), Times.Once);
        }

        [TestMethod()]
        public void DeleteRelationAsyncTest()
        {
            this.manager.DeleteRelationAsync(1);

            this.repositoryMock.Verify(x => x.DeleteRelationAsync(1), Times.Once);
        }
    }
}