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
    public class CredentialController : BaseWebApiController<CredentialController>
    {
        public CredentialController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Credential")]
        public async Task<List<CredentialItem>> GetAllAsync()
        {
            DateTime startTime = DateTime.Now;
            var credentialFacade = this.Container.GetInstance<CredentialFacade>();

            var credentials = await credentialFacade.GetAllAsync();

            base.LogMessage("GetAllAsync", DateTime.Now - startTime);

            return ModelConversions.Convert(credentials);
        }
    }
}
