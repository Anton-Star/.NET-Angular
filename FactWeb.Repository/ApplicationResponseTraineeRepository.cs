using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace FactWeb.Repository
{
    public class ApplicationResponseTraineeRepository : BaseRepository<ApplicationResponseTrainee>, IApplicationResponseTraineeRepository
    {
        public ApplicationResponseTraineeRepository(FactWebContext context) : base(context)
        {
        }

        public List<ApplicationResponseTrainee> GetApplicationResponsesTrainee(long organizationId, int applicationTypeId)
        {
            return base.Context.ApplicationResponseTrainee.Where(x =>
                x.Application.OrganizationId == organizationId &&
                x.Application.ApplicationTypeId == applicationTypeId)
                .Include(x => x.ApplicationResponseStatus)
                .ToList();

        }

        public List<ApplicationResponseTrainee> GetApplicationResponses(Guid applicationUniqueId)
        {
            return base.Context.ApplicationResponseTrainee.Where(x => x.Application.UniqueId == applicationUniqueId)
                .Include(x => x.ApplicationResponseStatus)
                .ToList();
        }

        public void BulkUpdate(int applicationId, Guid applicationSectionId, int fromStatusId, int toStatusId, string updatedBy)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[5];

            paramList[0] = applicationId;
            paramList[1] = applicationSectionId;
            paramList[2] = fromStatusId;
            paramList[3] = toStatusId;
            paramList[4] = updatedBy;

            objectContext.ExecuteStoreCommand(
                "EXEC usp_BulkUpdateTrainee @ApplicationId={0}, @ApplicationSectionId={1}, @FromStatusId={2}, @ToStatusId={3}, @UpdatedBy={4}", paramList);
        }
    }
}
