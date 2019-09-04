using FactWeb.BusinessFacade;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class CacheStatusController : BaseWebApiController<CacheStatusController>
    {
        public CacheStatusController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/CacheStatus")]
        public async Task<List<CacheStatus>> GetAll()
        {
            var facade = this.Container.GetInstance<CacheStatusFacade>();

            return await facade.GetAllAsync();
        }
    }
}
