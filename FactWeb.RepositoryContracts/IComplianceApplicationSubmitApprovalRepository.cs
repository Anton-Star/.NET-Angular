using FactWeb.Model;
using System;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IComplianceApplicationSubmitApprovalRepository : IRepository<ComplianceApplicationSubmitApproval>
    {
        ComplianceApplicationSubmitApproval GetByUser(Guid complianceApplicationId, Guid userId);
        List<ComplianceApplicationSubmitApproval> GetByCompliance(Guid complianceApplicationId);
        void CreateApprovals(Guid complianceApplicationId, string createdBy);
    }
}
