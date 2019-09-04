using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Mvc.Hubs;
using FactWeb.Mvc.Models;
using Microsoft.AspNet.SignalR;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class ApplicationSettingController : BaseWebApiController<ApplicationSettingController>
    {
        public ApplicationSettingController(Container container) : base(container)
        {
        }

        [HttpGet]
        //[MyAuthorize]
        [Route("api/ApplicationSetting")]
        public async Task<List<ApplicationSetting>> GetAll()
        {
            var facade = this.Container.GetInstance<ApplicationSettingFacade>();

            return await facade.GetAllAsync();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/ApplicationSetting/{name}")]
        public async Task<ApplicationSetting> GetByName(string name)
        {
            var facade = this.Container.GetInstance<ApplicationSettingFacade>();

            return await facade.GetByNameAsync(name);
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/ApplicationSetting")]
        public async Task<ServiceResponse> Set(ApplicationSettingUpdateModel model)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var facade = this.Container.GetInstance<ApplicationSettingFacade>();

                await facade.SetValueAsync(model.ApplicationSettingName, model.ApplicationSettingValue, base.Email);

                base.LogMessage("Set", DateTime.Now - startTime);

                this.SendInvalidation();

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        private void SendInvalidation()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CacheHub>();
            hubContext.Clients.All.Invalidated(Constants.CacheStatuses.AppSettings);
        }

        [HttpPut]
        [MyAuthorize]
        [Route("api/ApplicationSetting")]
        public async Task<ServiceResponse> Save(ApplicationSettingSaveModel model)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var facade = this.Container.GetInstance<ApplicationSettingFacade>();

                await facade.SetValuesAsync(model.Settings, base.Email);
                base.LogMessage("Save", DateTime.Now - startTime);

                this.SendInvalidation();

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }
    }
}
