using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace FactWeb.Mvc.Controllers.WebApi
{
    /// <summary>
    /// Extend AuthorizeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        private const string OTP_HEADER = "XOTP";

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (Thread.CurrentPrincipal.Identity.Name.Length == 0)
            {
                var response = HttpContext.Current.Response;
                response.SuppressFormsAuthenticationRedirect = true;
                response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                response.End();
                return base.IsAuthorized(actionContext);
            }

            if (ConfigurationManager.AppSettings["UseTwoFactor"] != "Y") return base.IsAuthorized(actionContext);
            
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            var impersonateClaim = principal?.Claims?.SingleOrDefault(x => x.Type == MvcApplication.Claims.IsImpersonation);

            if (impersonateClaim != null && impersonateClaim.Value == "Y") return base.IsAuthorized(actionContext);

            var twoFactorClaim = principal?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.Authentication);
            var twoFactorHeader = actionContext.Request.Headers.GetValues(OTP_HEADER).FirstOrDefault();

            if (twoFactorClaim?.Value != twoFactorHeader)
            {
                var response = HttpContext.Current.Response;
                response.SuppressFormsAuthenticationRedirect = true;
                response.StatusCode = (int)System.Net.HttpStatusCode.ProxyAuthenticationRequired;
                response.End();
            }
            
            return base.IsAuthorized(actionContext);
        }
    }
}