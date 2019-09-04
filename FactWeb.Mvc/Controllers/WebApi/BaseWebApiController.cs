using FactWeb.Infrastructure;
using FactWeb.Mvc.Models;
using log4net;
using SimpleInjector;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class BaseWebApiController<T> : ApiController
    {
        private readonly ClaimsPrincipal identity;
        protected readonly Container Container;

        private readonly ILog Log = LogManager.GetLogger(typeof(T));

        protected string Email
        {
            get
            {
                var email = this.identity.Claims.SingleOrDefault(c => c.Type == MvcApplication.Claims.EmailAddress);

                return email == null ? null : email.Value;
            }
        }

        protected bool IsFactStaff => this.RoleId == (int)Constants.Role.FACTAdministrator ||
                                      this.RoleId == (int)Constants.Role.QualityManager || this.RoleId == (int) Constants.Role.FACTCoordinator;
        //this.RoleId == (int) Constants.Role.FACTStaffCoordinator;

        protected bool IsReviewer => this.RoleId == (int) Constants.Role.FACTAdministrator ||
                                    this.RoleId == (int) Constants.Role.Inspector;

        protected bool IsConsultantCoordinator => this.RoleId == (int) Constants.Role.FACTConsultantCoordinator;

        protected bool IsConsultant => this.RoleName == Constants.Roles.FACTConsultant;

        protected bool IsQualityManagerOrHigher => this.RoleId == (int) Constants.Role.FACTAdministrator ||
                                                   this.RoleId == (int) Constants.Role.QualityManager || this.RoleId == (int) Constants.Role.FACTCoordinator;

        protected bool IsUser
            =>
                this.RoleId == (int) Constants.Role.User || this.RoleId == (int) Constants.Role.OrganizationalDirector ||
                this.RoleId == (int) Constants.Role.FACTConsultant;

        protected bool IsImpersonating
        {
            get
            {
                var impersonateClaim = this.identity.Claims.SingleOrDefault(x => x.Type == MvcApplication.Claims.IsImpersonation);

                return impersonateClaim != null && impersonateClaim.Value == "Y";
            }
        }

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

        protected string AccessToken
        {
            get
            {
                var claim = this.identity.Claims.SingleOrDefault(x => x.Type == MvcApplication.Claims.DocumentLibraryUser);

                return claim?.Value;
            }
        }

        protected string RoleName
        {
            get
            {
                var claim = this.identity.Claims.SingleOrDefault(x => x.Type == MvcApplication.Claims.RoleName);

                return claim?.Value;
            }
        }

        protected bool CanManageUsers
        {
            get
            {
                var claim = this.identity.Claims.SingleOrDefault(x => x.Type == MvcApplication.Claims.CanManageUsers);

                return claim != null && Convert.ToBoolean(claim.Value);
            }
        }

        //protected long OrganizationId
        //{
        //    get
        //    {
        //        return Convert.ToInt64(this.identity.Claims.Where(c => c.Type == MvcApplication.Claims.OrganizationId)
        //           .Select(c => c.Value).Single());
        //    }
        //}
        protected string ClaimTypeEmail
        {
            get
            {
                var claim = this.identity.Claims.SingleOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType);

                return claim?.Value;
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

        protected string TwoFactorCode
        {
            get
            {
                var claim = this.identity.Claims.SingleOrDefault(c => c.Type == MvcApplication.Claims.TwoFactor);

                return claim?.Value;
            }
        }

        protected string IPAddress
        {
            get
            {
                var ipAddress = GetIP4Address();

                return ipAddress == null ? null : ipAddress;
            }
        }

        private static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }

        protected BaseWebApiController(Container container)
        {
            this.Container = container;
            this.identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            HttpContext.Current.Server.ScriptTimeout = 7200;
        }

        protected ServiceResponse HandleException(Exception ex)
        {
            this.Log.Fatal(ex.Message);
            this.Log.Fatal(ex.StackTrace);

            return new ServiceResponse
            {
                HasError = true,
                Message = ex.Message
            };
        }

        protected ServiceResponse<T> HandleException<T>(Exception ex)
        {
            this.Log.Fatal(ex.Message);
            this.Log.Fatal(ex.StackTrace);

            return new ServiceResponse<T>
            {
                HasError = true,
                Message = ex.Message
            };
        }

        protected void HandleExceptionForResponse(Exception ex)
        {
            this.Log.Fatal(ex.Message);
            this.Log.Fatal(ex.StackTrace);
        }

        protected void LogMessage(string methodName, TimeSpan timeTaken)
        {
            this.Log.Debug("Method: " + methodName + " completed. Time Taken: " + timeTaken.ToString());
        }

        protected void LogMessage(string message)
        {
            this.Log.Info(message);
        }
    }
}