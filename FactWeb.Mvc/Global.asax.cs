using FactWeb.BusinessFacade;
using FactWeb.Mvc.App_Start;
using log4net;
using log4net.Config;
using Newtonsoft.Json.Serialization;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FactWeb.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(MvcApplication));

        public class Claims
        {
            public const string OrganizationId = "Organization";
            public const string RoleId = "Role";
            public const string RoleName = "RoleName";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string EmailAddress = "EmailAddress";
            public const string UserId = "User";
            public const string CanManageUsers = "CanManageUsers";
            public const string DocumentLibraryUser = "DocumentLibraryUser";
            public const string TwoFactor = "TwoFactor";
            public const string IsImpersonation = "IsImpersonation";
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = SimpleInjectorInitialize.Initialize();

            var config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            XmlConfigurator.Configure();

            var applicationFacade = container.GetInstance<ApplicationFacade>();

            HostingEnvironment.QueueBackgroundWorkItem(x =>
            {
                applicationFacade.GetActiveApplicationVersions();

                this.Log.Debug("Loading Active Application Versions Completed.");
            });
        }
    }
}
