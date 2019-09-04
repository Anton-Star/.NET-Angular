using FactWeb.Mvc.Models;
using SimpleInjector;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class AppLogController : BaseWebApiController<AppLogController>
    {
        public AppLogController(Container container) : base(container)
        {

            
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/AppLog")]
        public HttpResponseMessage AddLog(AppLogAddModel model)
        {
            try
            {
                base.LogMessage(model.Message);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                base.HandleExceptionForResponse(ex);

                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}

