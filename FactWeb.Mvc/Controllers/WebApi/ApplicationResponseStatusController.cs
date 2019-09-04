using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Models;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class ApplicationResponseStatusController : BaseWebApiController<ApplicationResponseStatusController>
    {
        public ApplicationResponseStatusController(Container container) : base(container)
        {
        }

        /// <summary>
        /// Get all application response status records asynchronously
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/ApplicationResponseStatus")]
        public async Task<List<ApplicationResponseStatusItem>> GetApplicationResponseStatusAsync()
        {
            DateTime startTime = DateTime.Now;
            var appRespStatusFacade = this.Container.GetInstance<ApplicationResponseStatusFacade>();

            base.LogMessage("GetApplicationResponseStatusAsync", DateTime.Now - startTime);
            return ModelConversions.Convert(await appRespStatusFacade.GetApplicationResponseStatusAsync()).ToList();
        }

        /// <summary>
        /// Update application status
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        [Route("api/ApplicationResponseStatus")]
        public async Task<ServiceResponse<bool>> SaveStatusAsync(ApplicationResponseStatusItem applicationResponseStatusItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var appRespStatusFacade = this.Container.GetInstance<ApplicationResponseStatusFacade>();

                base.LogMessage("SaveStatusAsync", DateTime.Now - startTime);

                return new ServiceResponse<bool>
                {
                    Item = await appRespStatusFacade.SaveStatus(applicationResponseStatusItem.Id, applicationResponseStatusItem.Name, applicationResponseStatusItem.NameForApplicant, Email),
                    Message = "Application Response Status updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }

        }
    }
}
