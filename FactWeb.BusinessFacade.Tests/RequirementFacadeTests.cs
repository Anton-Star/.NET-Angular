using FactWeb.BusinessLayer;
using FactWeb.Repository;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System.IO;
using System.Linq;

namespace FactWeb.BusinessFacade.Tests
{
    [TestClass]
    public class RequirementFacadeTests
    {
        readonly Container container = new Container();
        private RequirementFacade facade;



        [TestInitialize]
        public void Initialize()
        {
            this.container.Register<FactWebContext>();
            this.container.Register<IApplicationTypeRepository, ApplicationTypeRepository>();
            this.container.Register<IApplicationSectionRepository, ApplicationSectionRepository>();
            this.container.Register<IScopeTypeRepository, ScopeTypeRepository>();
            this.container.Register<IApplicationVersionRepository, ApplicationVersionRepository>();
            this.container.Register<ApplicationTypeManager>();
            this.container.Register<ApplicationSectionManager>();
            this.container.Register<ScopeTypeManager>();
            this.container.Register<ApplicationVersionManager>();

            this.facade = new RequirementFacade(this.container);
        }

        [TestMethod]
        public void ImportTest()
        {
            var lines = File.ReadAllLines(@"C:\FactWeb\FACTWeb Replacement\FactWeb\FactWeb.BusinessFacade.Tests\CBB 6.0 - Training - expected Answer.csv");
            
            var contents = lines.ToList();

            var headersRow = contents[0];
            var headers = headersRow.Split(',').ToList();
            var rows = contents.GetRange(1, contents.Count - 1);

            this.facade.Import("Eligibility Application", "Test Version 2", "1", headers, rows, "test@test.com");
        }
    }
}
