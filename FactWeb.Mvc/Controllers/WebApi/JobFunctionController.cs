using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class JobFunctionController : BaseWebApiController<JobFunctionController>
    {

        private readonly JobFunctionFacade facade;

        public JobFunctionController(JobFunctionFacade jobFunctionFacade, Container container): base(container)
        {
            this.facade = jobFunctionFacade;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/JobFunction")]
        public async Task<List<JobFunctionItem>> Get()
        {
            try
            {
                var functions = await this.facade.GetAllAsync();

                return functions.Where(x=>x.IsActive).OrderBy(x => x.Order).ThenBy(x=>x.Name).Select(ModelConversions.Convert).ToList();
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }
    }
}
