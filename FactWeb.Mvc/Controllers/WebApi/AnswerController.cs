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
    public class AnswerController : BaseWebApiController<AnswerController>
    {
        public AnswerController(Container container) : base(container)
        {
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Answer")]
        public async Task<ServiceResponse<Guid>> Save(Answer model)
        {
            if (base.RoleId != (int)Constants.Role.FACTAdministrator && base.RoleId != (int)Constants.Role.QualityManager)
            {
                return new ServiceResponse<Guid>
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<AnswerFacade>();

                var id = await facade.SaveAsync(model, base.Email);

                return new ServiceResponse<Guid>
                {
                    Item = id
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<Guid>(ex);
            }
        }

        [HttpDelete]
        [MyAuthorize]
        [Route("api/Answer/{id}")]
        public async Task<ServiceResponse> Delete(Guid id)
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
                var facade = this.Container.GetInstance<AnswerFacade>();

                await facade.DeleteAsync(id, base.Email);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpDelete]
        [MyAuthorize]
        [Route("api/Answer/RemoveHides/{id}")]
        public async Task<ServiceResponse> RemoveHides(Guid id)
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
                var facade = this.Container.GetInstance<AnswerFacade>();

                await facade.RemoveHidesAsync(id, base.Email);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Answer/AddHides")]
        public async Task<ServiceResponse<List<QuestionAnswerDisplay>>> AddHides(AnswerAddHidesViewModel model)
        {
            if (base.RoleId != (int)Constants.Role.FACTAdministrator && base.RoleId != (int)Constants.Role.QualityManager)
            {
                return new ServiceResponse<List<QuestionAnswerDisplay>>
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<AnswerFacade>();

                var items = await facade.AddHidesAsync(model.AnswerId, model.Questions, base.Email);
                var displays = items.Select(ModelConversions.Convert).ToList();

                foreach (var item in displays)
                {
                    var question = model.Questions.SingleOrDefault(x => x.Id == item.QuestionId);

                    if (question == null) continue;

                    item.QuestionText = question.Text;
                }

                return new ServiceResponse<List<QuestionAnswerDisplay>>
                {
                    Item = displays
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<List<QuestionAnswerDisplay>>(ex);
            }
        }
    }
}
