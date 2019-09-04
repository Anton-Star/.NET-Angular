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
    public class TransplantCellTypeController : BaseWebApiController<TransplantCellTypeController>
    {
        public TransplantCellTypeController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/TransplantCellType")]
        public async Task<List<TransplantCellTypeItem>> GetAll()
        {
            var start = DateTime.Now;

            var facade = base.Container.GetInstance<SiteFacade>();

            var items = facade.GetAllTransplantCellTypes();

            base.LogMessage("GetApplications", DateTime.Now - start);

            return items.Select(ModelConversions.Convert).ToList();
        }
    }
}
