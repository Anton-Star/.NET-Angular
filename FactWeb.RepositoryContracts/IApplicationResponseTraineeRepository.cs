using FactWeb.Model;
using System;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationResponseTraineeRepository : IRepository<ApplicationResponseTrainee>
    {
        List<ApplicationResponseTrainee> GetApplicationResponsesTrainee(long organizationId, int applicationTypeId);
        List<ApplicationResponseTrainee> GetApplicationResponses(Guid applicationUniqueId);

        void BulkUpdate(int applicationId, Guid applicationSectionId, int fromStatusId, int toStatusId, string updatedBy);
    }
}
