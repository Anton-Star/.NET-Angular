using FactWeb.Model;
using System;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IAccreditationOutcomeRepository : IRepository<AccreditationOutcome>
    {
        List<AccreditationOutcome> GetByOrgId(int organizationId);
        List<AccreditationOutcome> GetByOrgIdAndAppId(int organizationId, int applicationId);
        List<AccreditationOutcome> GetByAppId(Guid applicationUniqueId);
    }
}

