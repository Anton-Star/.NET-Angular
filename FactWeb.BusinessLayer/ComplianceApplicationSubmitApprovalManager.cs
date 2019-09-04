using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class ComplianceApplicationSubmitApprovalManager : BaseManager<ComplianceApplicationSubmitApprovalManager, IComplianceApplicationSubmitApprovalRepository, ComplianceApplicationSubmitApproval>
    {
        public ComplianceApplicationSubmitApprovalManager(IComplianceApplicationSubmitApprovalRepository repository) : base(repository)
        {
        }

        public List<ComplianceApplicationSubmitApproval> GetByCompliance(Guid complianceApplicationId)
        {
            return base.Repository.GetByCompliance(complianceApplicationId);
        }

        public void MarkUserAsApproved(Guid complianceApplicationId, Guid userId, bool isApproved, string approvedBy)
        {
            var record = base.Repository.GetByUser(complianceApplicationId, userId);

            if (record == null) return;

            record.IsApproved = isApproved;
            record.UpdatedBy = approvedBy;
            record.UpdatedDate = DateTime.Now;

            base.Repository.Save(record);
        }

        public void CreateApprovals(Guid complianceApplicationId, string createdBy)
        {
            base.Repository.CreateApprovals(complianceApplicationId, createdBy);
        }
    }
}
