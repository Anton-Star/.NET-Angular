using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace FactWeb.BusinessLayer.Tests
{
    [TestClass]
    public class ApplicationSettingManagerTests
    {
        private Mock<IApplicationSettingRepository> repositoryMock;
        private ApplicationSettingManager manager;

        [TestInitialize]
        public void Initialize()
        {
            this.repositoryMock = new Mock<IApplicationSettingRepository>();
            this.manager = new ApplicationSettingManager(this.repositoryMock.Object);

            var items = new List<ApplicationSetting>
                        {
                            new ApplicationSetting
                            {
                                Id = 1,
                                Name = "Setting 1",
                                Value = "Value 1"
                            },
                            new ApplicationSetting
                            {
                                Id = 2,
                                Name = "Setting 2",
                                Value = "Value 2"
                            },
                            new ApplicationSetting
                            {
                                Id = 3,
                                Name = "Setting 3",
                                Value = "Value 3"
                            },
                        };

            this.repositoryMock.Setup(u => u.GetAll())
                .Returns(items);

            this.repositoryMock.Setup(u => u.GetAllAsync())
                .Returns(System.Threading.Tasks.Task.FromResult(items));

            this.repositoryMock.Setup(u => u.GetByName(It.IsAny<string>()))
                .Returns((string name) => items.SingleOrDefault(x => x.Name == name));

            this.repositoryMock.Setup(u => u.GetByNameAsync(It.IsAny<string>()))
                .Returns((string name) => System.Threading.Tasks.Task.FromResult(items.SingleOrDefault(x => x.Name == name)));
        }

        [TestMethod]
        public void GetAllApplicationSettingsForManagerTest()
        {
            var items = this.manager.GetAll();
            Assert.AreEqual(3, items.Count);

            this.repositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        [TestMethod]
        public void GetAllApplicationSettingsAsyncForManagerTest()
        {
            var items = this.manager.GetAllAsync().Result;
            Assert.AreEqual(3, items.Count);

            this.repositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [TestMethod]
        public void ApplicationSettingsManagerGetByNameTest()
        {
            var item = this.manager.GetByName("Setting 1");
            Assert.IsNotNull(item);
            Assert.AreEqual("Value 1", item.Value);

            this.repositoryMock.Verify(x => x.GetByName(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ApplicationSettingsManagerGetByNameAsyncTest()
        {
            var item = this.manager.GetByNameAsync("Setting 1").Result;
            Assert.IsNotNull(item);
            Assert.AreEqual("Value 1", item.Value);

            this.repositoryMock.Verify(x => x.GetByNameAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ApplicationSettingsManagerSetValueTest()
        {
            this.manager.SetValue("Setting 1", "Value 1 1", string.Empty);

            this.repositoryMock.Verify(x => x.GetByName(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ApplicationSettingsManagerSetValueAsyncTest()
        {
            var item = this.manager.GetByNameAsync("Setting 1").Result;
            Assert.IsNotNull(item);
            Assert.AreEqual("Value 1", item.Value);

            this.repositoryMock.Verify(x => x.GetByNameAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
