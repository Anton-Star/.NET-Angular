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
    public class QuestionController : BaseWebApiController<QuestionController>
    {
        public QuestionController(Container container) : base(container)
        {
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Question")]
        public async Task<ServiceResponse<Question>> Save(Question model)
        {
            if (base.RoleId != (int)Constants.Role.FACTAdministrator && base.RoleId != (int)Constants.Role.QualityManager)
            {
                return new ServiceResponse<Question>
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<QuestionFacade>();

                var applicationSectionQuestion = await facade.SaveAsync(model, base.Email);
                var question = ModelConversions.Convert(applicationSectionQuestion, true);
                return new ServiceResponse<Question>()
                {
                    Item = question
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Question>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpDelete]
        [MyAuthorize]
        [Route("api/Question/{id}")]
        public ServiceResponse Delete(Guid id)
        {
            if (base.RoleId != (int)Constants.Role.FACTAdministrator && base.RoleId != (int)Constants.Role.QualityManager)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<QuestionFacade>();

                facade.Delete(id, base.Email);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpGet]
        [Route("api/Question/Displays")]
        public List<QuestionAnswerDisplay> GetAllDisplaysForQuestion(Guid qid)
        {
            var facade = this.Container.GetInstance<QuestionFacade>();

            var items = facade.GetAllDisplaysForQuestion(qid);

            return items.Select(ModelConversions.Convert).ToList();
        }

        [HttpGet]
        [Route("api/Question/Section")]
        [MyAuthorize]
        public List<Question> GetSectionQuestions(Guid uniqueId, Guid sectionId)
        {
            var facade = this.Container.GetInstance<ApplicationFacade>();

            var isTrainee = facade.IsAccreditationRoleTrainee(base.UserId.GetValueOrDefault(), uniqueId);

            var items = facade.GetSectionQuestions(uniqueId, sectionId, base.UserId.GetValueOrDefault(), (base.IsFactStaff || base.IsReviewer), isTrainee, base.IsUser);

            return items;
        }
    }
}
