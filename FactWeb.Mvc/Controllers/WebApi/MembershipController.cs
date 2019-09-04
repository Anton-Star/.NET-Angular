using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class MembershipController : BaseWebApiController<MembershipController>
    {
        private readonly MembershipFacade facade;

        public MembershipController(MembershipFacade membershipFacade, Container container) : base(container)
        {
            this.facade = membershipFacade;
        }

        public async Task<List<MembershipItem>> Get()
        {
            try
            {
                var items = await this.facade.GetAllAsync();

                return items.OrderBy(x=>x.Order).ThenBy(x=>x.Name).Select(ModelConversions.Convert).ToList();
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }
    }
}
