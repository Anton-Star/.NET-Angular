using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using FactWeb.Model.TrueVault;
using FactWeb.Mvc.Models;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class DocumentController : BaseWebApiController<DocumentController>
    {
        public DocumentController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Document")]
        public async Task<List<DocumentItem>> Get(string org)
        {
            var facade = this.Container.GetInstance<DocumentFacade>();

            var canSeeFactOnly = facade.CanSeeFactOnlyDocuments(org, base.RoleId.GetValueOrDefault(),
                base.UserId.GetValueOrDefault());

            var documents = await facade.GetByOrgAsync(org, canSeeFactOnly);

            var result = documents.Select(x=>ModelConversions.Convert(x, null, true)).ToList();

            return result;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Document")]
        public List<DocumentItem> GetByApplication(Guid appId)
        {
            var facade = this.Container.GetInstance<DocumentFacade>();

            var documents = facade.GetByDocumentLibrary(appId, base.RoleId.GetValueOrDefault(),
                base.UserId.GetValueOrDefault());

            var result = documents.Select(x => ModelConversions.Convert(x, null, true)).ToList();

            return result;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Document/Post")]
        public List<DocumentItem> GetPostInspection(string org)
        {
            var facade = this.Container.GetInstance<DocumentFacade>();

            var documents = facade.GetPostInspection(org, base.RoleId.GetValueOrDefault());

            var result = documents.Select(x=>ModelConversions.Convert(x, null, true)).ToList();

            return result;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Document/BAA")]
        public async Task<List<DocumentItem>> GetBAADocuments(string org)
        {
            var facade = this.Container.GetInstance<DocumentFacade>();

            var documents = await facade.GetBAAByOrgAsync(org);

            return documents.Select(x=>ModelConversions.Convert(x, null, true)).ToList();
        }

        [HttpPost]
        [Route("api/Document")]
        public ServiceResponse<DocumentItem> Add(DocumentItem model)
        {
            return this.AddInternal(model);
        }

        [HttpPost]
        [Route("api/Document/AddBAA")]
        public ServiceResponse<DocumentItem> AddBAA(DocumentItem model)
        {
            model.IsBaaDocument = true;

            return this.AddInternal(model);            
        }
        
        [HttpPost]
        [Route("api/Document/Exclude")]
        public ServiceResponse AddDocumentExcludingDb(DocumentItem model)
        {
            DateTime startTime = DateTime.Now;
            var documentFacade = this.Container.GetInstance<DocumentFacade>();

            try
            {
                documentFacade.AddToLibrary(model.AppUniqueId, model.OrganizationName, model.Name, model.OriginalName, model.StaffOnly, base.Email, model.RequestValues, false);

                base.LogMessage("Add", DateTime.Now - startTime);

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

        [MyAuthorize]
        [HttpPost]
        [Route("api/Document")]
        public async Task<ServiceResponse> Delete(string orgName, Guid id)
        {
            var startTime = DateTime.Now;
            var documentFacade = this.Container.GetInstance<DocumentFacade>();

            try
            {
                await documentFacade.RemoveDocumentAsync(orgName, id, base.Email);

                base.LogMessage("Add", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [MyAuthorize]
        [HttpPost]
        [Route("api/Document/BAA")]
        public async Task<ServiceResponse> DeleteBAA(string orgName, Guid id)
        {
            var startTime = DateTime.Now;
            var documentFacade = this.Container.GetInstance<DocumentFacade>();

            try
            {
                await documentFacade.RemoveBAADocumentAsync(orgName, id, base.Email);

                base.LogMessage("DeleteBAA", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [MyAuthorize]
        [HttpPost]
        [Route("api/Document/IncludeInReporting")]
        public ServiceResponse SaveIncludeInReporting(IncludeInReportingModel model)
        {
            var startTime = DateTime.Now;
            var documentFacade = this.Container.GetInstance<DocumentFacade>();

            try
            {
                documentFacade.SaveIncludeInReporting(model.OrgName, model.Documents, base.Email);

                base.LogMessage("IncludeInReporting", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [MyAuthorize]
        [HttpGet]
        [Route("api/Document/Access")]
        public AccessTokenDetail GetAccessToken(string name)
        {
            var startTime = DateTime.Now;
            var documentFacade = this.Container.GetInstance<DocumentFacade>();

            var result = documentFacade.GetAccessTokenDetail(name, base.UserId.GetValueOrDefault(), base.IsFactStaff || base.IsConsultantCoordinator);

            base.LogMessage("GetAccessToken", DateTime.Now - startTime);

            return result;
        }

        [MyAuthorize]
        [HttpGet]
        [Route("api/Document/Access")]
        public AccessTokenDetail GetAccessToken(Guid appId)
        {
            var startTime = DateTime.Now;
            var documentFacade = this.Container.GetInstance<DocumentFacade>();

            var result = documentFacade.GetAcessTokenDetail(appId);

            base.LogMessage("GetAccessToken", DateTime.Now - startTime);

            return result;
        }

        private ServiceResponse<DocumentItem> AddInternal(DocumentItem model)
        {
            DateTime startTime = DateTime.Now;
            var documentFacade = this.Container.GetInstance<DocumentFacade>();

            try
            {
                if (model.ReplacementOfId.HasValue)
                {
                    documentFacade.ChangeLatest(model.ReplacementOfId.Value, base.Email);
                }

                var document = documentFacade.AddToLibrary(model.AppUniqueId, model.OrganizationName, model.Name, model.OriginalName, model.StaffOnly, base.Email, model.RequestValues, true, model.IsBaaDocument.GetValueOrDefault());

                base.LogMessage("Add", DateTime.Now - startTime);

                return new ServiceResponse<DocumentItem>
                {
                    Item = ModelConversions.Convert(document, false, true)
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<DocumentItem>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpGet]
        [Route("api/Document/Migration")]
        public HttpResponseMessage MigrateDocumentLibrary(int organizationId, string authKey)
        {
            if (authKey != "CSr8I7oysrU8HyvRTVjRcLgvZ3M9CzHCIXiCo1IUqmytYdIzfjf6dS38Fz2qQtm0GxpGh")
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not Authorized");
            }

            try
            {
                DateTime startTime = DateTime.Now;
                var documentFacade = this.Container.GetInstance<DocumentFacade>();

                documentFacade.MigrateDocumentLibrary(organizationId, "K2");

                base.LogMessage("Add", DateTime.Now - startTime);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }
    }
}
