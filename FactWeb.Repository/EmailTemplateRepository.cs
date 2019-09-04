using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace FactWeb.Repository
{
    public class EmailTemplateRepository : BaseRepository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(FactWebContext context) : base(context)
        {
        }

        public EmailTemplate GetByName(string templateName)
        {
            return base.Fetch(x => x.Name.ToLower() == templateName.ToLower());
        }

        public EmailContent GetContent(int? applicationId, Guid? applicationUniqueId, string emailName, int? inspectionScheduleId,
            string inspectionScope, string submitter)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[6];

            paramList[0] = applicationId;
            paramList[1] = applicationUniqueId;
            paramList[2] = emailName;
            paramList[3] = inspectionScheduleId;
            paramList[4] = inspectionScope;
            paramList[5] = submitter;

            var data = objectContext.ExecuteStoreQuery<EmailContent>(
                "EXEC usp_getEmailContent @pApplicationId={0}, @pApplicationUniqueId={1}, @pEmailType={2}, @pInspectionScheduleDetailId={3}, @pInspectionScope={4}, @pSubmitter={5}",
                paramList).FirstOrDefault();

            return data;
        }
    }
}
