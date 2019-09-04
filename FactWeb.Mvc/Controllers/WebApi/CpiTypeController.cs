using FactWeb.BusinessFacade;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class CpiTypeController : BaseWebApiController<CpiTypeController>
    {
        public CpiTypeController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/CpiType")]
        public List<CpiTypeItem> GetCpiTypes()
        {
            var start = DateTime.Now;
            var facade = this.Container.GetInstance<CpiTypeFacade>();

            var items = facade.GetAll();

            base.LogMessage("GetCpiTypes", DateTime.Now - start);

            return items.Select(x => new CpiTypeItem
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

    }
}
