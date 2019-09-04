using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class LanguageController : BaseWebApiController<LanguageController>
    {
        private readonly LanguageFacade languageFacade;

        public LanguageController(LanguageFacade languageFacade, Container container) : base(container)
        {
            this.languageFacade = languageFacade;
        }

        [HttpGet]
        [MyAuthorize]
        public async Task<List<LanguageItem>> Get()
        {
            try
            {
                var languages = await this.languageFacade.GetAllAsync();

                return languages.OrderBy(x=>x.Order).ThenBy(x=>x.Name).Select(ModelConversions.Convert).ToList();
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }
    }
}
