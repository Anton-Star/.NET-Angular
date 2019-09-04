using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;

namespace FactWeb.BusinessLayer
{
    public class EmailTemplateManager : BaseManager<EmailTemplateManager, IEmailTemplateRepository, EmailTemplate>
    {
        public EmailTemplateManager(IEmailTemplateRepository repository) : base(repository)
        {
        }


        public EmailTemplate GetByName(string templateName)
        {
            LogMessage("GetByName (EmailTemplateManager)");

            return base.Repository.GetByName(templateName);
        }

        public EmailContent GetContent(int? applicationId, Guid? applicationUniqueId, string emailName,
            int? inspectionScheduleId,
            string inspectionScope, string submitter)
        {
            return base.Repository.GetContent(applicationId, applicationUniqueId, emailName, inspectionScheduleId,
                inspectionScope, submitter);
        }
    }
}
