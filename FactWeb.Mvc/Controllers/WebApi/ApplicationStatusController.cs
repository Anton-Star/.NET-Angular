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
    public class ApplicationStatusController : BaseWebApiController<ApplicationStatusController>
    {
        public ApplicationStatusController(Container container) : base(container)
        {
        }

        /// <summary>
        /// Get all application status records asynchronously
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/ApplicationStatus")]
        public async Task<List<ApplicationStatusItem>> GetApplicationStatusAsync()
        {
            DateTime startTime = DateTime.Now;
            var applicationStatusFacade = this.Container.GetInstance<ApplicationStatusFacade>();

            base.LogMessage("GetApplicationStatusAsync", DateTime.Now - startTime);
            return ModelConversions.Convert(await applicationStatusFacade.GetApplicationStatusAsync()).ToList();
        }

        /// <summary>
        /// Update application status
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        [Route("api/ApplicationStatus")]
        public async Task<ServiceResponse<bool>> SaveStatusAsync(ApplicationStatusItem model)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var applicationStatusFacade = this.Container.GetInstance<ApplicationStatusFacade>();

                base.LogMessage("SaveStatusAsync", DateTime.Now - startTime);

                return new ServiceResponse<bool>
                {
                    Item = await applicationStatusFacade.SaveStatus(model.Id, model.Name, model.NameForApplicant, Email),
                    Message = "Application Status updated successfully"
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
