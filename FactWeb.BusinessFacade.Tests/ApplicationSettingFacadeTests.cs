using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Repository;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade.Tests
{
    [TestClass]
    public class ApplicationSettingFacadeTests
    {
        readonly Container container = new Container();
        private ApplicationSettingFacade facade;

        [TestInitialize]
        public void Initialize()
        {
            this.container.Register<FactWebContext>();
            this.container.Register<IApplicationSettingRepository, ApplicationSettingRepository>();
            this.container.Register<ApplicationSettingManager>();

            this.facade = new ApplicationSettingFacade(this.container);
        }

        [TestMethod]
        public void ApplicationSettingFacadeGetAllTest()
        {
            var items = this.facade.GetAll();
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void ApplicationSettingFacadeGetAllAsyncTest()
        {
            var items = this.facade.GetAllAsync().Result;
            Assert.AreNotEqual(0, items.Count);
        }

        [TestMethod]
        public void ApplicationSettingFacadeGetByNameTest()
        {
            var item = this.facade.GetByName(Constants.ApplicationSettings.InspectorMileageRange);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void ApplicationSettingFacadeGetByNameAsyncTest()
        {
            var item = this.facade.GetByNameAsync(Constants.ApplicationSettings.InspectorMileageRange).Result;
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void ApplicationSettingFacadeSetValueTest()
        {
            this.facade.SetValue(Constants.ApplicationSettings.InspectorMileageRange, "450", "test");
            var item = this.facade.GetByName(Constants.ApplicationSettings.InspectorMileageRange);
            Assert.IsNotNull(item);
            Assert.AreEqual("450", item.Value);
        }

        [TestMethod]
        public void ApplicationSettingFacadeSetValueNotFoundTest()
        {
            this.facade.SetValue("Test", "450", "test");
            var item = this.facade.GetByName("Test");
            Assert.IsNull(item);
        }

        [TestMethod]
        public void ApplicationSettingFacadeSetValueAsyncTest()
        {
            Task.Run(async () =>
            {
                await this.facade.SetValueAsync(Constants.ApplicationSettings.InspectorMileageRange, "425", "test");

            }).GetAwaiter().GetResult();
 
            var item = this.facade.GetByNameAsync(Constants.ApplicationSettings.InspectorMileageRange).Result;
            Assert.IsNotNull(item);
            Assert.AreEqual("425", item.Value);
        }

        [TestMethod]
        public void ApplicationSettingFacadeSetValueAsyncNotFoundTest()
        {
            Task.Run(async () =>
            {
                await this.facade.SetValueAsync("Test", "425", "test");

            }).GetAwaiter().GetResult();

            var item = this.facade.GetByNameAsync("Test").Result;
            Assert.IsNull(item);
        }

        [TestMethod]
        public void ApplicationSettingFacadeSetValuesTest()
        {
            this.facade.SetValues(new List<ApplicationSetting>
            {
                new ApplicationSetting
                {
                    Name = Constants.ApplicationSettings.InspectorMileageRange,
                    Value = "600"
                }
            }, "test");
            var item = this.facade.GetByName(Constants.ApplicationSettings.InspectorMileageRange);
            Assert.IsNotNull(item);
            Assert.AreEqual("600", item.Value);
        }

        [TestMethod]
        public void ApplicationSettingFacadeSetValuesAsyncTest()
        {
            Task.Run(async () =>
            {
                await this.facade.SetValuesAsync(new List<ApplicationSetting>
            {
                new ApplicationSetting
                {
                    Name = Constants.ApplicationSettings.InspectorMileageRange,
                    Value = "460"
                }
            }, "test");

            }).GetAwaiter().GetResult();

            
            var item = this.facade.GetByName(Constants.ApplicationSettings.InspectorMileageRange);
            Assert.IsNotNull(item);
            Assert.AreEqual("460", item.Value);
        }
    }
}
