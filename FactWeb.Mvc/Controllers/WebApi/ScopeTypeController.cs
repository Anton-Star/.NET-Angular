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
    public class ScopeTypeController : BaseWebApiController<ScopeTypeController>
    {
        public ScopeTypeController(Container container) : base(container)
        {
        }

        /// <summary>
        /// Get all scope type records form database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<ScopeTypeItem>> GetAsync()
        {
            DateTime startTime = DateTime.Now;
            var scopeTypeFacade = this.Container.GetInstance<ScopeTypeFacade>();

            base.LogMessage("GetAsync", DateTime.Now - startTime);
            return ModelConversions.Convert(await scopeTypeFacade.GetAsync()).ToList();
        }

        /// <summary>
        /// Get all scope type records form database which are active but not archived
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<ScopeTypeItem>> GetAllActiveNonArchivedAsync()
        {
            DateTime startTime = DateTime.Now;
            var scopeTypeFacade = this.Container.GetInstance<ScopeTypeFacade>();

            base.LogMessage("GetAsync", DateTime.Now - startTime);
            return ModelConversions.Convert(await scopeTypeFacade.GetAllActiveNonArchivedAsync()).ToList();
        }

        

        /// <summary>
        /// add/update scope type record in database
        /// </summary>
        /// <param name="scopeTypeItem"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        public ServiceResponse<ScopeTypeItem> Save(ScopeTypeItem scopeTypeItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var scopeTypeFacade = this.Container.GetInstance<ScopeTypeFacade>();
                string errorMessage = scopeTypeFacade.IsDuplicateScope(scopeTypeItem.ScopeTypeId, scopeTypeItem.Name, scopeTypeItem.ImportName);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return new ServiceResponse<ScopeTypeItem>
                    {
                        HasError = true,
                        Message = errorMessage,
                        Item = null
                    };
                }
                else
                {
                    var scopeType = scopeTypeFacade.Save(scopeTypeItem, Email);

                    base.LogMessage("SaveAsync", DateTime.Now - startTime);

                    return new ServiceResponse<ScopeTypeItem>()
                    {
                        Item = ModelConversions.Convert(scopeType)
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ScopeTypeItem>()
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Soft delete scope type record
        /// </summary>
        /// <param name="scopeTypeItem"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        public ServiceResponse<ScopeTypeItem> Delete(ScopeTypeItem scopeTypeItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var scopeTypeFacade = this.Container.GetInstance<ScopeTypeFacade>();

                var scopeType = scopeTypeFacade.Delete(scopeTypeItem.ScopeTypeId, Email);

                base.LogMessage("SaveAsync", DateTime.Now - startTime);

                return new ServiceResponse<ScopeTypeItem>()
                {
                    Item = ModelConversions.Convert(scopeType)
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<ScopeTypeItem>()
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

    }
}
