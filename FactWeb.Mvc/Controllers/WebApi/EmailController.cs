using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Mvc.Models;
using SimpleInjector;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class EmailController : BaseWebApiController<EmailController>
    {
        public EmailController(Container container) : base(container)
        {
        }

        [MyAuthorize]
        [HttpPost]
        [Route("api/Email")]
        public HttpResponseMessage Send(SendEmailModel model)
        {
            if (!base.IsQualityManagerOrHigher && base.RoleId != (int)Constants.Role.Inspector)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Not Authorized");
            }

            try
            {
                var facade = this.Container.GetInstance<EmailFacade>();

                facade.Send(model.To.Split(';').ToList(), model.Cc.Split(';').ToList(), model.Subject, model.Html, model.IncludeAccReport, model.CycleNumber, model.OrgName, model.AppId);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);

                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }
    }
}
