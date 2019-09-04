using FactWeb.BusinessLayer;
using FactWeb.Repository;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace FactWeb.Mvc.App_Start
{
    public class SimpleInjectorInitialize
    {
        private const string BaseRepositoryName = "IRepository`1";
        private const string BaseManagerName = "BaseManager`3";
        private const string IDocumentLibraryName = "IDocumentLibrary";
        private const string AzureBlobDocumentStorageName = "AzureBlobDocumentStorage";
        private const string Migrations = "Migration";

        /// <summary>Initialize the container and register it as Dependency Resolver.</summary>
        public static Container Initialize()
        {
            var container = new Container();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.RegisterMvcIntegratedFilterProvider();

            //container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            return container;
        }

        private static void InitializeContainer(Container container)
        {
            RegisterRepository(container);

            var items = typeof(UserManager).Assembly.GetExportedTypes()
                .Where(x => x.Name != BaseManagerName && !x.Name.Contains(IDocumentLibraryName) && !x.Name.Contains(AzureBlobDocumentStorageName))
                .ToList();


            items.ForEach(container.Register);
            container.Register<IDocumentLibrary, AzureBlobDocumentStorage>();
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