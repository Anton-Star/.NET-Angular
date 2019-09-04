using FactWeb.BusinessFacade;
using FactWeb.BusinessLayer;
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
    public class ApplicationResponseCommentController : BaseWebApiController<ApplicationResponseCommentController>
    {
        public ApplicationResponseCommentController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/ApplicationResponseComment")]
        public async Task<List<ApplicationResponseCommentItem>> Get(int applicationId, string questionId, int answerResponseStatusId, int commentTypeId)
        {
            try
            {
                var start = DateTime.Now;
                var applicationResponseCommentFacade = this.Container.GetInstance<ApplicationResponseCommentFacade>();

                var applicationResponseCommentList = await applicationResponseCommentFacade.GetByApplicationIdQuestionIdAsync(applicationId, new Guid(questionId), answerResponseStatusId, commentTypeId);

                base.LogMessage("GetAll", DateTime.Now - start);

                return applicationResponseCommentList.Select(ModelConversions.Convert).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        [MyAuthorize]
        [Route("api/ApplicationResponseComment")]
        public ServiceResponse<ApplicationResponseCommentItem> Save(ApplicationResponseCommentItem applicationResponseCommentItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var applicationResponseCommentFacade = this.Container.GetInstance<ApplicationResponseCommentFacade>();

                var applicationResponseComment = applicationResponseCommentFacade.Save(applicationResponseCommentItem, base.UserId.GetValueOrDefault(), base.Email);

                base.LogMessage("Save", DateTime.Now - startTime);

                var manager = this.Container.GetInstance<ApplicationResponseCommentManager>();

                var comment = manager.GetById(applicationResponseComment.Id);

                return new ServiceResponse<ApplicationResponseCommentItem>()
                {
                    Item = ModelConversions.Convert(comment)
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<ApplicationResponseCommentItem>()
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/ApplicationResponseComment/Visibility")]
        public ServiceResponse SetVisibility(ApplicationResponseCommentItem model)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var applicationResponseCommentFacade = this.Container.GetInstance<ApplicationResponseCommentFacade>();

                    applicationResponseCommentFacade.SetCommentVisibility(model.ApplicationResponseCommentId,
                        model.VisibleToApplicant, base.Email);

                base.LogMessage("Save", DateTime.Now - startTime);

                return new ServiceResponse();

            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/ApplicationResponseComment/Inclusion")]
        public ServiceResponse SetInclusion(ApplicationResponseCommentItem model)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var applicationResponseCommentFacade = this.Container.GetInstance<ApplicationResponseCommentFacade>();

                applicationResponseCommentFacade.SetCommentInclusion(model.ApplicationResponseCommentId,
                    model.IncludeInReporting, base.Email);

                base.LogMessage("Save", DateTime.Now - startTime);

                return new ServiceResponse();

            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpDelete]
        [MyAuthorize]
        [Route("api/ApplicationResponseComment/{applicationResponseCommentId}")]
        public ServiceResponse<bool> Delete(int applicationResponseCommentId)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var applicationResponseCommentFacade = this.Container.GetInstance<ApplicationResponseCommentFacade>();

                applicationResponseCommentFacade.Delete(applicationResponseCommentId, base.IsFactStaff, base.Email);

                base.LogMessage("Delete", DateTime.Now - startTime);

                return new ServiceResponse<bool>()
                {
                    Item = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>()
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }


        [HttpGet]
        [MyAuthorize]
        [Route("api/ApplicationResponseComment/CommentTypes")]
        public async Task<List<CommentTypeItem>> GetCommentTypes()
        {
            try
            {
                var start = DateTime.Now;
                var commentTypeFacade = this.Container.GetInstance<CommentTypeFacade>();

                var commentTypeList = await commentTypeFacade.GetAllAsync();

                base.LogMessage("GetCommentTypes", DateTime.Now - start);

                return commentTypeList.Select(ModelConversions.Convert).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}