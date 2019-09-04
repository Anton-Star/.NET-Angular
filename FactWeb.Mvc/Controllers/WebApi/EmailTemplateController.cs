using FactWeb.BusinessFacade;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class EmailTemplateController : BaseWebApiController<EmailTemplateController>
    {
        public EmailTemplateController(Container container) : base(container)
        {
        }

        [HttpGet]
        //[MyAuthorize]
        [Route("api/EmailTemplate")]
        public async Task<List<EmailTemplate>> GetAll()
        {
            var facade = this.Container.GetInstance<EmailTemplateFacade>();

            return await facade.GetAllAsync();
        } 
    }
}
