using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;

namespace FactWeb.RepositoryContracts
{
    public interface IEmailTemplateRepository : IRepository<EmailTemplate>
    {
        /// <summary>
        /// Get email template by name
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        EmailTemplate GetByName(string templateName);

        EmailContent GetContent(int? applicationId, Guid? applicationUniqueId, string emailName,
            int? inspectionScheduleId, string inspectionScope, string submitter);
    }
}
