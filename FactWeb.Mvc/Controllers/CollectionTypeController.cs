using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Controllers.WebApi;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers
{
    public class CollectionTypeController : BaseWebApiController<CollectionTypeController>
    {
        public CollectionTypeController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/CollectionType")]
        public async Task<List<CollectionTypeItem>> GetAll()
        {
            var start = DateTime.Now;

            var facade = base.Container.GetInstance<SiteFacade>();

            var items = await facade.GetAllCollectionTypesAsync();

            base.LogMessage("GetAll", DateTime.Now - start);

            return items.Select(ModelConversions.Convert).ToList();
        }
    }
}
