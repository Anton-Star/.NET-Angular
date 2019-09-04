using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class QuestionTypeController : BaseWebApiController<QuestionTypeController>
    {
        public QuestionTypeController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/QuestionType")]
        public List<QuestionTypeItem> GetAll()
        {
            try
            {
                var facade = this.Container.GetInstance<QuestionFacade>();

                var items = facade.GetAllQuestionTypes();

                return items.Select(ModelConversions.Convert).ToList();
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }
    }
}
