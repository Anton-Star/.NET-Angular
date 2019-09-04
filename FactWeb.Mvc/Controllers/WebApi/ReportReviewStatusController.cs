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
    public class ReportReviewStatusController : BaseWebApiController<ReportReviewStatusController>
    {
        public ReportReviewStatusController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/ReportReviewStatus")]
        public async Task<List<ReportReviewStatusItem>> GetAllAsync()
        {
            DateTime startTime = DateTime.Now;
            var reportReviewStatusFacade = this.Container.GetInstance<ReportReviewStatusFacade>();

            var reportReviewStatuses = await reportReviewStatusFacade.GetAllAsync();

            base.LogMessage("GetAllAsync", DateTime.Now - startTime);

            return ModelConversions.Convert(reportReviewStatuses);
        }
    }
}
