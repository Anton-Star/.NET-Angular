using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;

namespace FactWeb.Mvc.Controllers
{
    public class DownloadController : Controller
    {
        private readonly ClaimsPrincipal identity;
        private readonly Container container;

        public DownloadController(Container container)
        {
            this.container = container;
            this.identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        }

        [Authorize]
        public FileResult Index(string fileName, string organizationName)
        {
            var documentFacade = this.container.GetInstance<DocumentFacade>();

            DocumentDownload response = null;


            if (this.RoleId == (int) Constants.Role.FACTAdministrator ||
                this.RoleId == (int) Constants.Role.QualityManager ||
                this.RoleId == (int) Constants.Role.Inspector)
            {
                response = documentFacade.GetFile(organizationName, fileName, null);
            }
            else
            {
                response = documentFacade.GetFile(organizationName, fileName, this.UserId);
            }
            

            return this.File(response.File, response.ContentType, fileName.Replace("\"", ""));
        }

        protected bool IsFactStaff => this.RoleId == (int)Constants.Role.FACTAdministrator ||
                                      this.RoleId == (int)Constants.Role.QualityManager;

        protected int? RoleId
        {
            get
            {
                var claim = this.identity.Claims.SingleOrDefault(x => x.Type == MvcApplication.Claims.RoleId);

                if (claim == null)
                {
                    return null;
                }
                else
                {
                    return Convert.ToInt32(claim.Value);
                }
            }
        }

        protected Guid? UserId
        {
            get
            {
                var claim = this.identity.Claims.SingleOrDefault(c => c.Type == MvcApplication.Claims.UserId);

                if (claim == null)
                {
                    return null;
                }
                else
                {
                    return Guid.Parse(claim.Value);
                }
            }
        }
    }
}