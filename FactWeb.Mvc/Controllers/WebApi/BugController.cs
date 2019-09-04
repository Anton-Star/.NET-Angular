using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model;
using SimpleInjector;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class BugController : BaseWebApiController<BugController>
    {
        public BugController(Container container) : base(container)
        {
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Bug")]
        public HttpResponseMessage AddBug(Bug model)
        {
            try
            {
                var facade = this.Container.GetInstance<BugFacade>();

                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                var version = fvi.FileVersion;
                var dte = File.GetCreationTime(assembly.Location);

                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];

                facade.AddBug(model.BugText, version, dte, model.BugUrl, base.Email, url, model.ApplicationUniqueId);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                base.HandleExceptionForResponse(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
