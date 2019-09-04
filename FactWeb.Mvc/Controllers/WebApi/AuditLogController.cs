using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class AuditLogController : BaseWebApiController<AuditLogController>
    {
        private readonly Container container;
        public AuditLogController(Container container) : base(container)
        {
            this.container = container;
        }

        /// <summary>
        /// Get all Audit Log 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<AuditLogItem>> GetAuditLogAsync()
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var auditLogFacade = this.Container.GetInstance<AuditLogFacade>();
                var auditLogList = await auditLogFacade.GetAuditLogAsync();
                var auditLogItemList = ModelConversions.Convert(auditLogList);

                base.LogMessage("GetAuditLogAsync", DateTime.Now - startTime);

                return auditLogItemList;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }
    }
}
