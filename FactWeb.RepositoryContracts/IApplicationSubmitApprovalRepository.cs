using FactWeb.Model;
using System;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationSubmitApprovalRepository : IRepository<ApplicationSubmitApproval>
    {
        List<ApplicationSubmitApproval> GetAllForApplication(int applicationId);
        List<ApplicationSubmitApproval> GetAllForApplication(Guid applicationUniqueId);
    }
}
