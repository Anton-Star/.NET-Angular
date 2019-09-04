using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace FactWeb.BusinessLayer.Tests
{
    [TestClass]
    public class OrganizationManagerTests
    {
        private Mock<IOrganizationRepository> repositoryMock;
        private OrganizationManager manager;

        [TestInitialize]
        public void Initialize()
        {
            this.repositoryMock = new Mock<IOrganizationRepository>();
            this.manager = new OrganizationManager(this.repositoryMock.Object);

            var items = new List<Organization>
                        {
                            new Organization
                            {
                                Id = 1,
                                Name = "Org 1",
                                Number = "12345",
                                OrganizationAddresses = new List<OrganizationAddress>
                                {
                                    new OrganizationAddress
                                    {
                                        AddressId = 1,
                                        Address = new Address
                                        {
                                            City = "Test2",
                                            //State = "PA"
                                        }
                                    }
                                },
                                OrganizationFacilities = new List<OrganizationFacility>
                                {
                                    new OrganizationFacility
                                    {
                                        Id = 1,
                                        FacilityId = 1,
                                        Facility = new Facility
                                        {
                                            Name = "Fac 1"
                                        }
                                    }
                                }
                            },
                            new Organization
                            {
                                Id = 2,
                                Name = "Org 2",
                                OrganizationAddresses = new List<OrganizationAddress>
                                {
                                    new OrganizationAddress
                                    {
                                        AddressId = 2,
                                        Address = new Address
                                        {
                                            City = "Test",
                                            //State = "PA"
                                        }
                                    }
                                },
                                OrganizationFacilities = new List<OrganizationFacility>
                                {
                                    new OrganizationFacility
                                    {
                                        Id = 2,
                                        FacilityId = 2,
                                        Facility = new Facility
                                        {
                                            Name = "Fac 2"
                                        }
                                    }
                                }
                            },
                            new Organization
                            {
                                Id = 3,
                                Name = "Org 3",
                                OrganizationAddresses = new List<OrganizationAddress>
                                {
                                    new OrganizationAddress
                                    {
                                        AddressId = 3,
                                        Address = new Address
                                        {
                                            City = "Test",
                                            //State = "PA"
                                        }
                                    }
                                },
                                OrganizationFacilities = new List<OrganizationFacility>
                                {
                                    new OrganizationFacility
                                    {
                                        Id = 3,
                                        FacilityId = 1,
                                        Facility = new Facility
                                        {
                                            Name = "Fac 1"
                                        }
                                    }
                                }
                            }
                        };

            this.repositoryMock.Setup(u => u.GetAll())
                .Returns(items);

            this.repositoryMock.Setup(u => u.GetAllAsync())
                .Returns(System.Threading.Tasks.Task.FromResult(items));

            this.repositoryMock.Setup(u => u.Search(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string e, string c, string s) => items.Where(x => x.Name.Equals(e) || x.OrganizationAddresses.Any(y=>y.Address.City == c || y.Address.State.Id.ToString() == s)).ToList());

            this.repositoryMock.Setup(u => u.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string e, string c, string s) => System.Threading.Tasks.Task.FromResult(items.Where(x => x.Name.Equals(e) || x.OrganizationAddresses.Any(y => y.Address.City == c || y.Address.State.Id.ToString() == s)).ToList()));

            this.repositoryMock.Setup(u => u.GetByFacility(It.IsAny<int>()))
                .Returns((int f) => items.Where(x=>x.OrganizationFacilities.Any(y=>y.FacilityId == f)).ToList());

            this.repositoryMock.Setup(u => u.GetByFacilityAsync(It.IsAny<int>()))
                .Returns((int f) => System.Threading.Tasks.Task.FromResult(items.Where(x => x.OrganizationFacilities.Any(y => y.FacilityId == f)).ToList()));

            this.repositoryMock.Setup(u => u.GetById(It.IsAny<int>()))
                .Returns((int f) => items.SingleOrDefault(x=>x.Id == f));

            this.repositoryMock.Setup(u => u.GetByName(It.IsAny<string>()))
                .Returns((string name) => items.SingleOrDefault(x=>x.Name == name));

            this.repositoryMock.Setup(u => u.GetByNameAsync(It.IsAny<string>()))
                .Returns((string name) => System.Threading.Tasks.Task.FromResult(items.SingleOrDefault(x=>x.Name == name)));
        }

        [TestMethod]
        public void GetAllOrganizationsForManagerTest()
        {
            var items = this.manager.GetAll();
            Assert.AreEqual(3, items.Count);

            this.repositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        [TestMethod]
        public void GetAllOrganizationsAsyncForManagerTest()
        {
            var items = this.manager.GetAllAsync().Result;
            Assert.AreEqual(3, items.Count);

            this.repositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [TestMethod]
        public void OrganizationsManagerSearchOrgNameTest()
        {
            var items = this.manager.Search("Org 1", string.Empty, string.Empty);
            Assert.AreEqual(1, items.Count);

            this.repositoryMock.Verify(x=>x.Search(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void OrganizationsManagerSearchOrgNameAsyncTest()
        {
            var items = this.manager.SearchAsync("Org 1", string.Empty, string.Empty).Result;
            Assert.AreEqual(1, items.Count);

            this.repositoryMock.Verify(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void OrganizationsManagerSearchCityTest()
        {
            var items = this.manager.Search(string.Empty, "Test", string.Empty);
            Assert.AreEqual(2, items.Count);

            this.repositoryMock.Verify(x => x.Search(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void OrganizationsManagerSearchCityAsyncTest()
        {
            var items = this.manager.SearchAsync(string.Empty, "Test", string.Empty).Result;
            Assert.AreEqual(2, items.Count);

            this.repositoryMock.Verify(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void OrganizationsManagerSearchStateTest()
        {
            var items = this.manager.Search(string.Empty, string.Empty, "PA");
            Assert.AreEqual(3, items.Count);

            this.repositoryMock.Verify(x => x.Search(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void OrganizationsManagerSearchStateAsyncTest()
        {
            var items = this.manager.SearchAsync(string.Empty, string.Empty, "PA").Result;
            Assert.AreEqual(3, items.Count);

            this.repositoryMock.Verify(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void OrganizationsManagerSearchOrgIdTest()
        {
            var items = this.manager.Search(1, null);
            Assert.AreEqual(1, items.Count);

            this.repositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void OrganizationsManagerSearchOrgIdAsyncTest()
        {
            var items = this.manager.SearchAsync(1, null).Result;
            Assert.AreEqual(1, items.Count);

            this.repositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void OrganizationsManagerSearchFacilityIdTest()
        {
            var items = this.manager.Search(null, 1);
            Assert.AreEqual(2, items.Count);
            var item = items.First();
            Assert.AreEqual("Fac 1", item.OrganizationFacilities.First().Facility.Name);

            this.repositoryMock.Verify(x => x.GetByFacility(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void OrganizationsManagerSearchFacilityIdAsyncTest()
        {
            var items = this.manager.SearchAsync(null, 2).Result;
            Assert.AreEqual(1, items.Count);

            this.repositoryMock.Verify(x => x.GetByFacilityAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void OrganizationManagerGetByNameTest()
        {
            var item = this.manager.GetByName("Org 1");
            Assert.IsNotNull(item);
            Assert.AreEqual("12345", item.Number);

            this.repositoryMock.Verify(x=>x.GetByName(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void OrganizationManagerGetByNameAsyncTest()
        {
            var item = this.manager.GetByNameAsync("Org 1").Result;
            Assert.IsNotNull(item);
            Assert.AreEqual("12345", item.Number);

            this.repositoryMock.Verify(x => x.GetByNameAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
