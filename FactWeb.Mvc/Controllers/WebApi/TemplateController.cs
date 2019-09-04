using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Mvc.Models;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class TemplateController : BaseWebApiController<TemplateController>
    {
        public TemplateController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Template")]
        public async Task<List<Template>> GetTemplates()
        {
            if (!base.IsFactStaff)
            {
                return null;
            }

            var dateTime = new DateTime();

            var facade = this.Container.GetInstance<TemplateFacade>();

            var items =  await facade.GetAllAsync();

            base.LogMessage("GetTemplates", DateTime.Now - dateTime);

            return items;
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Template")]
        public async Task<ServiceResponse<Template>> AddTemplate(Template template)
        {
            if (base.RoleId != (int)Constants.Role.FACTAdministrator)
            {
                return null;
            }

            var dateTime = new DateTime();

            try
            {
                var facade = this.Container.GetInstance<TemplateFacade>();

                var item = await facade.AddAsync(template.Name, template.Text, base.Email);

                base.LogMessage("AddTemplate", DateTime.Now - dateTime);

                return new ServiceResponse<Template>
                {
                    Item = item
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Template>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPut]
        [MyAuthorize]
        [Route("api/Template")]
        public async Task<ServiceResponse> UpdateTemplate(Template template)
        {
            if (base.RoleId != (int)Constants.Role.FACTAdministrator)
            {
                return null;
            }

            var dateTime = new DateTime();

            try
            {
                var facade = this.Container.GetInstance<TemplateFacade>();

                await facade.UpdateAsync(template.Id, template.Name, template.Text, base.Email);

                base.LogMessage("UpdateTemplate", DateTime.Now - dateTime);

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

        [HttpDelete]
        [MyAuthorize]
        [Route("api/Template/{id}")]
        public async Task<ServiceResponse> DeleteTemplate(Guid id)
        {
            if (base.RoleId != (int)Constants.Role.FACTAdministrator)
            {
                return null;
            }

            var dateTime = new DateTime();

            try
            {
                var facade = this.Container.GetInstance<TemplateFacade>();

                await facade.RemoveAsync(id);

                base.LogMessage("DeleteTemplate", DateTime.Now - dateTime);

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
