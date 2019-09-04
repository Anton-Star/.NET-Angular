using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer.Tests
{
    [TestClass]
    public class ApplicationResponseManagerTests
    {
        private Mock<IApplicationResponseRepository> repositoryMock;
        private ApplicationResponseManager manager;

        [TestInitialize]
        public void Initialize()
        {
            this.repositoryMock = new Mock<IApplicationResponseRepository>();
            this.manager = new ApplicationResponseManager(this.repositoryMock.Object);

            var items = new List<ApplicationResponse>
                        {
                            new ApplicationResponse
                            {
                                Id = 1,
                                Application = new Application
                                {
                                    OrganizationId = 1,
                                    ApplicationTypeId = 3
                                }
                            },
                            new ApplicationResponse
                            {
                                Id = 2,
                                Application = new Application
                                {
                                    OrganizationId = 1,
                                    ApplicationTypeId = 3
                                }
                            },
                            new ApplicationResponse
                            {
                                Id = 3,
                                Application = new Application
                                {
                                    OrganizationId = 2,
                                    ApplicationTypeId = 3
                                }
                            },
                        };

            this.repositoryMock.Setup(u => u.GetAll())
                .Returns(items);

            this.repositoryMock.Setup(u => u.GetAllAsync())
                .Returns(Task.FromResult(items));

            this.repositoryMock.Setup(u => u.GetApplicationResponses(It.IsAny<long>(), It.IsAny<int>()))
                .Returns((long org, int typeId) => items.Where(x => x.Application.OrganizationId == org && x.Application.ApplicationTypeId == typeId).ToList());

            this.repositoryMock.Setup(u => u.GetApplicationResponsesAsync(It.IsAny<long>(), It.IsAny<int>()))
                .Returns((long org, int typeId) => Task.FromResult(items.Where(x => x.Application.OrganizationId == org && x.Application.ApplicationTypeId == typeId).ToList()));
        }

        [TestMethod]
        public void GetAllApplicationResponsesForManagerTest()
        {
            var items = this.manager.GetAll();
            Assert.AreEqual(3, items.Count);

            this.repositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        [TestMethod]
        public void GetAllApplicationResponsesAsyncForManagerTest()
        {
            var items = this.manager.GetAllAsync().Result;
            Assert.AreEqual(3, items.Count);

            this.repositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [TestMethod]
        public void ApplicationResponsesManagerGetByOrgAndTypeTest()
        {
            var items = this.manager.GetApplicationResponses(1, 3);
            Assert.AreEqual(2, items.Count);

            this.repositoryMock.Verify(x => x.GetApplicationResponses(It.IsAny<long>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void ApplicationResponsesManagerGetByOrgAndTypeAsyncTest()
        {
            var items = this.manager.GetApplicationResponsesAsync(1, 3).Result;
            Assert.AreEqual(2, items.Count);

            this.repositoryMock.Verify(x => x.GetApplicationResponsesAsync(It.IsAny<long>(), It.IsAny<int>()), Times.Once);
        }
    }
}
