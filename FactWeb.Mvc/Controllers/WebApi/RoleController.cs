using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class RoleController : BaseWebApiController<RoleController>
    {
        public RoleController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Role")]
        public async Task<List<RoleItem>> GetAllRoles()
        {
            DateTime startTime = DateTime.Now;
            var roleFacade = this.Container.GetInstance<RoleFacade>();

            var roles = await roleFacade.GetAllAsync();

            base.LogMessage("GetAllRoles", DateTime.Now - startTime);

            return ModelConversions.Convert(roles);
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Role")]
        public async Task<List<RoleItem>> GetRolesByRole(int roleId)
        {
            DateTime startTime = DateTime.Now;
            var roleFacade = this.Container.GetInstance<RoleFacade>();

            var roles = await roleFacade.GetRolesByRoleAsync(roleId);

            base.LogMessage("GetRolesByRole", DateTime.Now - startTime);

            return ModelConversions.Convert(roles);
        }
    }
}
