using FactWeb.BusinessLayer;
using FactWeb.Repository;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace FactWeb.BusinessFacade.Tests
{
    [TestClass]
    public class ApplicationFacadeTests
    {
        readonly Container container = new Container();
        private ApplicationFacade facade;

        [TestInitialize]
        public void Initialize()
        {
            this.container.Register<FactWebContext>();
            this.container.Register<IApplicationVersionCacheRepository, ApplicationVersionCacheRepository>();
            this.container.Register<ApplicationVersionCacheManager>();
            this.container.Register<IApplicationVersionRepository, ApplicationVersionRepository>();
            this.container.Register<ApplicationVersionManager>();
            this.container.Register<IApplicationSectionRepository, ApplicationSectionRepository>();
            this.container.Register<ApplicationSectionManager>();

            this.facade = new ApplicationFacade(this.container);
        }
        
    }
}
