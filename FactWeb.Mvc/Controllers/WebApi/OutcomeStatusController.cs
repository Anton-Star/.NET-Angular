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
    public class OutcomeStatusController : BaseWebApiController<OutcomeStatusController>
    {
        public OutcomeStatusController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/OutcomeStatus")]
        public async Task<List<OutcomeStatusItem>> GetAllAsync()
        {
            DateTime startTime = DateTime.Now;
            var outcomeStatusFacade = this.Container.GetInstance<OutcomeStatusFacade>();

            var outcomeStatuses = await outcomeStatusFacade.GetAllAsync();

            base.LogMessage("GetAllAsync", DateTime.Now - startTime);

            return ModelConversions.Convert(outcomeStatuses);
        }
    }
}
