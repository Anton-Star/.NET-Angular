using FactWeb.BusinessFacade;
using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Constants = FactWeb.Infrastructure.Constants;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class AccountController : BaseWebApiController<AccountController>
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return this.Request.GetOwinContext().Authentication;
            }
        }

        public AccountController(Container container) : base(container)
        {
        }

        [HttpGet]
        [Route("api/Account/CurrentUser")]
        public UserItem GetCurrentUser()
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var userFacade = this.Container.GetInstance<UserFacade>();

                if (!base.UserId.HasValue)
                {
                    return null;
                }

                var user = userFacade.GetById(base.UserId.Value);

                base.LogMessage("GetCurrentUser", DateTime.Now - startTime);

                var userItem = ModelConversions.Convert(user, true , true);
                userItem.DocumentLibraryAccessToken = base.AccessToken;

                return userItem;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);

                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ServiceResponse<UserItem> Login(LoginViewModel model)
        {
            DateTime startTime = DateTime.Now;
            var userFacade = this.Container.GetInstance<UserFacade>();
            var auditLogFacade = this.Container.GetInstance<AuditLogFacade>();
            var trueVaultManager = this.Container.GetInstance<TrueVaultManager>();
            var userManager = this.Container.GetInstance<UserManager>();

            try
            {
                var user = userFacade.Login(model.UserName, model.Password);

                if (user == null || user.Item1 == null)
                {
                    auditLogFacade.AddAuditLog(base.Email, base.IPAddress, "Login Attempt - Failed");

                    return new ServiceResponse<UserItem>
                    {
                    };
                } else if (user.Item2)
                {
                    return new ServiceResponse<UserItem>
                    {
                        HasError = true,
                        Message = "Password Expired"
                    };
                }

                var token = trueVaultManager.GetAccessToken(user.Item1.DocumentLibraryUserId);

                if (token.Result != TrueVaultManager.Success)
                {
                    throw new Exception("Cannot log into True Vault");
                }

                var userItem = ModelConversions.Convert(user.Item1, true,true);
                userItem.Code = EncryptionHelper.Encrypt(user.Item1.Id.ToString(), "code", true);
                userItem.DocumentLibraryAccessToken = EncodeHelper.EncodeToBase64(token.User.Access_token + ":");
               
                var identity = this.GenerateUserIdentity(user.Item1, userItem.Code, userItem.DocumentLibraryAccessToken);

                this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                this.AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

                auditLogFacade.AddAuditLog(model.UserName, base.IPAddress, "Login Attempt - Successful");

                base.LogMessage("Login", DateTime.Now - startTime);
                
                return new ServiceResponse<UserItem>
                {
                    Item = userItem
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<UserItem>(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        public ServiceResponse Logout()
        {
            DateTime startTime = DateTime.Now;
            var auditLogFacade = this.Container.GetInstance<AuditLogFacade>();

            try
            {
                this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                auditLogFacade.AddAuditLog(base.Email, base.IPAddress, "Log Out - Successful");

                base.LogMessage("Logout", DateTime.Now - startTime);
                
                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpPost]
        public async Task<ServiceResponse> Reminder(ReminderModel model)
        {
            DateTime startTime = DateTime.Now;
            var userFacade = this.Container.GetInstance<UserFacade>();
            try
            {
                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];

                await userFacade.SendReminderAsync(model.FirstName, model.LastName, model.EmailAddress, url);

                base.LogMessage("Reminder", DateTime.Now - startTime);
                
                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpPost]
        public async Task<ServiceResponse> UpdatePassword(UpdatePasswordModel model)
        {
            DateTime startTime = DateTime.Now;
            var facade = this.Container.GetInstance<UserFacade>();
            var auditLogFacade = this.Container.GetInstance<AuditLogFacade>();

            try
            {
                var user = await facade.UpdatePasswordAsync(model.Token, model.Password, model.PasswordConfirm,
                    base.Email ?? string.Empty);

                auditLogFacade.AddAuditLog(base.Email, base.IPAddress, $"Update Password for {user.EmailAddress} - Successful");

                base.LogMessage("UpdatePassword", DateTime.Now - startTime);
                
                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpPost]
        public async Task<ServiceResponse> Update(UpdateModel model)
        {
            DateTime startTime = DateTime.Now;
            var facade = this.Container.GetInstance<UserFacade>();

            try
            {
                await facade.UpdateUserAsync(base.UserId.GetValueOrDefault(), model.User, base.IPAddress, base.Email ?? string.Empty);

                base.LogMessage("Update", DateTime.Now - startTime);
                
                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Account/TwoFactor")]
        public HttpResponseMessage GetTwoFactorCode()
        {
            try
            {
                var userFacade = this.Container.GetInstance<UserFacade>();
                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];

                userFacade.GetTwoFactorCode(base.UserId.GetValueOrDefault(), url);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                base.HandleExceptionForResponse(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Account/TwoFactor")]
        public HttpResponseMessage ValidateTwoFactor(TwoFactorModel model)
        {
            try
            {
                var userFacade = this.Container.GetInstance<UserFacade>();
                
                if (!userFacade.IsTwoFactorCodeCorrect(base.UserId.GetValueOrDefault(), model.Code))
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    var encCode = EncryptionHelper.Encrypt(base.UserId.ToString(), "code", true);

                    return this.Request.CreateResponse(HttpStatusCode.OK, encCode);
                }
            }
            catch (Exception ex)
            {
                base.HandleExceptionForResponse(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            
        }
        
        private ClaimsIdentity GenerateUserIdentity(User user, string code, string token, string email = null, bool isImpersonation = false)
        {
            var startTime = DateTime.Now;
            var claims = new List<Claim>
                         {
                             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String),
                             new Claim(ClaimsIdentity.DefaultNameClaimType, user.EmailAddress, ClaimValueTypes.String),
                             new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", DefaultAuthenticationTypes.ApplicationCookie),
                             new Claim(MvcApplication.Claims.UserId, user.Id.ToString()),
                             new Claim(MvcApplication.Claims.FirstName, user.FirstName),
                             new Claim(MvcApplication.Claims.RoleId, user.RoleId.ToString()),
                             new Claim(MvcApplication.Claims.RoleName, user.Role.Name),
                             new Claim(MvcApplication.Claims.LastName, user.LastName),
                             new Claim(MvcApplication.Claims.EmailAddress, email ?? user.EmailAddress),
                             new Claim(MvcApplication.Claims.CanManageUsers, user.CanManageUsers.GetValueOrDefault().ToString()),
                             new Claim(MvcApplication.Claims.DocumentLibraryUser, token),
                             new Claim(MvcApplication.Claims.IsImpersonation, isImpersonation ? "Y" : "N"),
                             new Claim(ClaimTypes.Authentication, code)
                         };

            base.LogMessage("GenerateUserIdentity", DateTime.Now - startTime);
            
            return new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Account/FailedAttempts")]
        public ServiceResponse<int> FailedAttempts(string emailAddress)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                var userFacade = this.Container.GetInstance<UserFacade>();

                var user = userFacade.GetFailedAttempts(emailAddress);

                base.LogMessage("FailedAttempts", DateTime.Now - startTime);

                var attempts = user?.FailedLoginAttempts.GetValueOrDefault() ?? 0;

                if (attempts > 0 &&
                    user?.UpdatedDate.GetValueOrDefault().AddHours(3) <= DateTime.Now)
                {
                    attempts = 0;
                }

                return new ServiceResponse<int>
                {
                    HasError = false,
                    Item = attempts
                };
            }
            catch(Exception ex)
            {
                return new ServiceResponse<int>
                {
                    HasError = true,
                    Item = 0,
                    Message = ex.Message
                };
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Account/FailedAttempts")]
        public ServiceResponse SetFailedAttempts(FailedAttemptModel failedAttemptModel)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                var userFacade = this.Container.GetInstance<UserFacade>();

                userFacade.SetFailedAttempts(failedAttemptModel.EmailAddress, failedAttemptModel.FailedAttempts);

                base.LogMessage("SetFailedAttempts", DateTime.Now - startTime);

                return new ServiceResponse
                {
                    HasError = false
                };
            }
            catch(Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpDelete]
        [MyAuthorize]
        [Route("api/Account/Impersonate")]
        public ServiceResponse<UserItem> StopImpersonation()
        {

            var currentUser = base.Email;

            this.Logout();

            DateTime startTime = DateTime.Now;
            var userFacade = this.Container.GetInstance<UserFacade>();
            var auditLogFacade = this.Container.GetInstance<AuditLogFacade>();
            var trueVaultManager = this.Container.GetInstance<TrueVaultManager>();

            try
            {
                var user = userFacade.GetByEmail(currentUser);

                if (user == null)
                {
                    return new ServiceResponse<UserItem>
                    {
                    };
                }
                
                var token = trueVaultManager.GetAccessToken(user.DocumentLibraryUserId);

                if (token.Result != TrueVaultManager.Success)
                {
                    throw new Exception("Cannot log into True Vault");
                }

                var code = EncryptionHelper.Encrypt(user.Id.ToString(), "code", true);

                var identity = this.GenerateUserIdentity(user, code, EncodeHelper.EncodeToBase64(token.User.Access_token + ":"), currentUser);

                this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                this.AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

                auditLogFacade.AddAuditLog(currentUser, base.IPAddress, $"User Exited Impersonation as {user.EmailAddress}");

                base.LogMessage("StopImpersonation", DateTime.Now - startTime);

                return new ServiceResponse<UserItem>
                {
                    Item = ModelConversions.Convert(user)
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<UserItem>(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Account/Impersonate")]
        public ServiceResponse<UserItem> Impersonate(Guid u)
        {
            if (!base.IsFactStaff)
            {
                throw new Exception("Not Authorized");
            }

            var currentUser = base.Email;

            this.Logout();

            DateTime startTime = DateTime.Now;
            var userFacade = this.Container.GetInstance<UserFacade>();
            var auditLogFacade = this.Container.GetInstance<AuditLogFacade>();
            var trueVaultManager = this.Container.GetInstance<TrueVaultManager>();

            try
            {
                var user = userFacade.GetById(u);

                if (user == null)
                {
                    return new ServiceResponse<UserItem>
                    {
                    };
                }

                var token = trueVaultManager.GetAccessToken(user.DocumentLibraryUserId);

                if (token.Result != TrueVaultManager.Success)
                {
                    throw new Exception("Cannot log into True Vault");
                }

                var userItem = ModelConversions.Convert(user, true, true);
                userItem.DocumentLibraryAccessToken = EncodeHelper.EncodeToBase64(token.User.Access_token + ":");

                var identity = this.GenerateUserIdentity(user, "", userItem.DocumentLibraryAccessToken, currentUser, true);

                this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                this.AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

                auditLogFacade.AddAuditLog(currentUser, base.IPAddress, $"User Impersonation as {user.EmailAddress}");

                base.LogMessage("Impersonate", DateTime.Now - startTime);

                return new ServiceResponse<UserItem>
                {
                    Item = userItem
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<UserItem>(ex);
            }
        }

        [HttpPost]
        [Route("api/Account/RequestAccess")]
        public ServiceResponse RequestAccess(AccessRequestModel model)
        {

            DateTime startTime = DateTime.Now;
            var userFacade = this.Container.GetInstance<UserFacade>();

            try
            {
                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];

                userFacade.RequestAccess(url, model.Record, base.Email);

                base.LogMessage("RequestAccess", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }
    }
}