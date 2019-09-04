using FactWeb.BusinessLayer;
using FactWeb.Repository;
using SimpleInjector;
using System.Linq;

namespace FactWeb.Mvc.Tests
{
    public static class BuildContainer
    {
        private const string BaseRepositoryName = "IRepository`1";
        private const string IDocumentLibrary = "IDocumentLibrary";
        private const string BaseManagerName = "BaseManager`2";
        private const string Migrations = "Migration";

        /// <summary>Initialize the container and register it as Dependency Resolver.</summary>
        public static Container Initialize()
        {
            var container = new Container();

            InitializeContainer(container);

            return container;
        }

        private static void InitializeContainer(Container container)
        {
            RegisterRepository(container);

            var items = typeof(UserManager).Assembly.GetExportedTypes()
                .Where(x => x.Name != BaseManagerName && !x.Name.Contains(IDocumentLibrary))
                .ToList();


            items.ForEach(container.Register);
        }

        private static void RegisterRepository(Container container)
        {
            container.Register<FactWebContext>();

            var repAssembly = typeof(UserRepository).Assembly;

            //Ignore the Context since we want to register that as a Single
            var registrations = repAssembly.GetExportedTypes()
                                           .Where(x => x.GetInterfaces().Any(y => !y.Name.Contains(BaseRepositoryName) && !y.Name.Contains(Migrations))
                                               && x != typeof(FactWebContext))
                                           .Select(x => new
                                           {
                                               Service = x.GetInterfaces().SingleOrDefault(y => !y.Name.Contains(BaseRepositoryName) && !y.Name.Contains(Migrations)),
                                               Implementation = x
                                           });

            foreach (var reg in registrations)
            {
                container.Register(reg.Service, reg.Implementation, Lifestyle.Transient);
            }
        }
    }
}
