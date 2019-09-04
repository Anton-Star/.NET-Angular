using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using static FactWeb.Infrastructure.Constants;
using FactWeb.Infrastructure;
using System.Linq;

namespace FactWeb.BusinessFacade
{
    public class AuditLogFacade
    {
        private readonly Container container;

        public AuditLogFacade(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets all of the records for the entity object
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<AuditLog> GetAuditLog()
        {
            var auditLogManager = this.container.GetInstance<AuditLogManager>();

            return auditLogManager.GetAll().OrderByDescending(x => x.Date).ToList();
        }

        /// <summary>
        /// Gets all of the records for the entity object asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<List<AuditLog>> GetAuditLogAsync()
        {
            var auditLogManager = this.container.GetInstance<AuditLogManager>();

            var auditLogList = auditLogManager.GetAllAsync().Result.OrderByDescending(x => x.Date).ToList();

            return Task.FromResult(auditLogList);
        }

        /// <summary>
        /// Gets all of the records for the entity object
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public void AddAuditLog(string userName, string ipAddress, string description)
        {
            var auditLogManager = this.container.GetInstance<AuditLogManager>();
            var auditLogItem = this.container.GetInstance<AuditLog>();

            auditLogItem.UserName = userName;
            auditLogItem.IPAddress = ipAddress;
            auditLogItem.Description = description;
            auditLogItem.Date = DateTimeOffset.Now;
            auditLogItem.CreatedDate = DateTime.Now;
            auditLogItem.CreatedBy = "Audit Log";

            auditLogManager.Add(auditLogItem);
            auditLogManager.SaveChanges();
        }
    }
}
