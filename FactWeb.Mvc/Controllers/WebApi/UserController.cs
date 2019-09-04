using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Infrastructure.GeoCoding;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Models;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class UserController : BaseWebApiController<UserController>
    {
        public UserController(Container container) : base(container)
        {
        }

        [HttpGet]
        [Route("api/User/Token/{token}")]
        public UserItem GetByToken(string token)
        {
            DateTime startTime = DateTime.Now;
            var facade = this.Container.GetInstance<UserFacade>();

            var user = facade.GetUserByToken(token);

            base.LogMessage("GetByToken", DateTime.Now - startTime);

            return ModelConversions.Convert(user, false);
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/User")]
        public List<UserItem> GetAllUsers(string includeAll)
        {
            DateTime startTime = DateTime.Now;
            var facade = this.Container.GetInstance<UserFacade>();

            var userList = facade.GetAll();

            base.LogMessage("GetAllUsers", DateTime.Now - startTime);
            var users = userList.OrderBy(x => x.LastName);

            return users.Select(x=>ModelConversions.Convert(x, includeAll == "Y")).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/User/AuditorObserver")]
        public List<UserItem> GetAuditorObservers()
        {
            var startTime = DateTime.Now;
            var facade = this.Container.GetInstance<UserFacade>();

            var userList = facade.GetAuditorObservers();

            var users =
                userList.Select(x => ModelConversions.Convert(x, false, false)).OrderBy(x => x.LastName).ToList();

            base.LogMessage("GetAuditorObservers", DateTime.Now - startTime);

            return users;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/User/EditPermissions")]
        public bool CheckEditPermissions()
        {
            return base.IsQualityManagerOrHigher || base.CanManageUsers;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/User/GetAllUsersWithOrganization")]
        public List<UserItem> GetAllUsersWithOrganization()
        {
            DateTime startTime = DateTime.Now;
            var facade = this.Container.GetInstance<UserFacade>();

            var userList = facade.GetUsersToManage(base.UserId.GetValueOrDefault(), base.IsQualityManagerOrHigher);

            base.LogMessage("GetAllUsersWithOrganization", DateTime.Now - startTime);

            var users = userList.Select(x => ModelConversions.Convert(x, true, true)).OrderBy(x=>x.IsActive).ThenBy(x=>x.LastName).ToList();

            return users;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/User/Impersonation")]
        public async Task<List<UserItem>> GetImpersonationUsers()
        {
            DateTime startTime = DateTime.Now;
            var facade = this.Container.GetInstance<UserFacade>();

            var userList = await facade.GetAllUsersForImpersonation();

            base.LogMessage("GetImpersonationUsers", DateTime.Now - startTime);

            return userList.OrderBy(x => x.LastName).Select(x => ModelConversions.Convert(x, false, false)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/User/{organizationId}")]
        public async Task<List<UserItem>> GetByOrganizationAsync(int organizationId)
        {
            DateTime startTime = DateTime.Now;
            var facade = this.Container.GetInstance<UserFacade>();

            var userList = await facade.GetByOrganizationAsync(organizationId);

            base.LogMessage("GetByOrganizationAsync", DateTime.Now - startTime);

            return ModelConversions.Convert(userList.OrderBy(x => x.LastName).ToList());
        }

        /// <summary>
        /// Get all users within the radius of 500 miles of facility
        /// </summary>
        /// <param name="selectedFacility"></param>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/User/Site/{selectedSite}")]
        public List<UserItem> GetUsersNearSite(string selectedSite)
        {
            DateTime startTime = DateTime.Now;
            var facade = this.Container.GetInstance<UserFacade>();

            var userList = facade.GetUsersNearSite(selectedSite);

            base.LogMessage("GetUsersNearSite", DateTime.Now - startTime);

            return userList;
        }



        [HttpGet]
        [MyAuthorize]
        [Route("api/User/AccreditationRoles")]
        public async Task<List<AccreditationRoleItem>> GetAccreditationRoles()
        {
            DateTime startTime = DateTime.Now;
            var facade = this.Container.GetInstance<UserFacade>();

            var roleList = await facade.GetAllAccreditationRolesAsync();

            base.LogMessage("GetAccreditationRoles", DateTime.Now - startTime);

            return ModelConversions.Convert(roleList);
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/User/Save")]
        public async Task<ServiceResponse<UserItem>> Save(UserModel userModel)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var userFacade = this.Container.GetInstance<UserFacade>();

                await userFacade.RegisterUserAsync(userModel.user, base.IPAddress, userModel.AddToExistingUser, base.Email);

                var user = userFacade.GetByEmail(userModel.user.EmailAddress);

                var result = ModelConversions.Convert(user);

                base.LogMessage("Save", DateTime.Now - startTime);

                return new ServiceResponse<UserItem>
                {
                    Item = result
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<UserItem>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/User/FactStaff")]
        public async Task<List<UserItem>> GetFactStaff()
        {
            if (!base.IsFactStaff)
            {
                return null;
            }

            var userFacade = this.Container.GetInstance<UserFacade>();

            var users = await userFacade.GetFactStaffAsync(base.RoleId.Value);

            return users.OrderBy(x => x.LastName).Select(x => ModelConversions.Convert(x, false)).ToList();
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/User/AuditorObserver")]
        public async Task<ServiceResponse> SaveAuditorObserver(UserAuditorObserverModel model)
        {
            DateTime startTime = DateTime.Now;

            if (!base.IsQualityManagerOrHigher)
            {
                throw new Exception("Not Authorized");
            }

            try
            {
                var userFacade = this.Container.GetInstance<UserFacade>();

                await userFacade.SetAuditorObserverAsync(model.UserId, model.IsAuditor, model.IsObserver, base.Email);

                base.LogMessage("SaveAuditorObserver", DateTime.Now - startTime);

                return new ServiceResponse
                {
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/User/Consultant")]
        public async Task<List<UserItem>> GetConsultants()
        {
            if (base.RoleId != (int)Constants.Role.FACTAdministrator && base.RoleId != (int)Constants.Role.FACTConsultantCoordinator)
            {
                return null;
            }

            var userFacade = this.Container.GetInstance<UserFacade>();

            var users = await userFacade.GetByRoleAsync(Constants.Roles.FACTConsultant);

            return users.OrderBy(x => x.LastName).Select(x => ModelConversions.Convert(x, false)).ToList();
        }

        [HttpGet]
        [Route("api/User/Distance")]
        public HttpResponseMessage GetDistance(string authKey)
        {
            try
            {
                if (authKey != "CSr8I7oysrU8HyvRTVjRcLgvZ3M9CzHCIXiCo1IUqmytYdIzfjf6dS38Fz2qQtm0GxpGh")
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.ProxyAuthenticationRequired, "Not Authorized");
                }

                var facade = this.Container.GetInstance<DistanceFacade>();

                HostingEnvironment.QueueBackgroundWorkItem(x => facade.SetDistanceForInspectors());

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                base.HandleExceptionForResponse(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            
        }

        [HttpPost]
        //[MyAuthorize]
        [Route("api/User/DocumentLibrary")]
        public HttpResponseMessage SetupDocumentLibrary()
        {
            var start = DateTime.Now;

            try
            {
                var facade = this.Container.GetInstance<UserFacade>();
                facade.SetupDocumentLibrary();
                base.LogMessage("SetupDocumentLibrary", DateTime.Now - start);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }


        }

        [HttpPost]
        //[MyAuthorize]
        [Route("api/User/DocumentLibrary")]
        public HttpResponseMessage SetupDocumentLibraryForUser(Guid userId)
        {
            var start = DateTime.Now;

            try
            {
                var facade = this.Container.GetInstance<UserFacade>();
                facade.SetupDocumentLibrary();
                base.LogMessage("SetupDocumentLibrary", DateTime.Now - start);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }


        }

        [HttpPost]
        //[MyAuthorize]
        [Route("api/User/FactStaffDocumentLibrary")]
        public HttpResponseMessage SetupFactStaffOrgs()
        {
            try
            {
                var facade = this.Container.GetInstance<UserFacade>();

                facade.AddFactStaffToTrueVaultGroups();

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
