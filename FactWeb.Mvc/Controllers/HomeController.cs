using SimpleInjector;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;

namespace FactWeb.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClaimsPrincipal identity;
        protected readonly Container Container;

        public HomeController(Container container)
        {
            this.Container = container;
            this.identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        }

        public ActionResult Index()
        {
            var claim = this.identity.Claims.FirstOrDefault(c => c.Type == MvcApplication.Claims.UserId);

            this.ViewBag.HasUser = claim != null;
            this.ViewBag.UseTwoFactor = ConfigurationManager.AppSettings["UseTwoFactor"];
            this.ViewBag.GenericKey = ConfigurationManager.AppSettings["GenericDocLibrary"];
            this.ViewBag.FactVault = ConfigurationManager.AppSettings["FactOnlyVault"];

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}