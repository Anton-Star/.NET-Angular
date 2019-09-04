using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class NetcordMembershipController : BaseWebApiController<NetcordMembershipController>
    {
        public NetcordMembershipController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/NetcordMembership/Types")]
        public async Task<List<NetcordMembershipTypeItem>> GetTypes()
        {
            var start = DateTime.Now;
            var facade = this.Container.GetInstance<OrganizationFacade>();

            var items = await facade.GetNetcordMembershipTypesAsync();

            base.LogMessage("GetTypes", DateTime.Now - start);

            return items.Select(ModelConversions.Convert).ToList();
        }
    }
}
