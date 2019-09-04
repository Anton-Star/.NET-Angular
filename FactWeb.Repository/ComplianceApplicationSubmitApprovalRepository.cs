using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace FactWeb.Repository
{
    public class ComplianceApplicationSubmitApprovalRepository : BaseRepository<ComplianceApplicationSubmitApproval>, IComplianceApplicationSubmitApprovalRepository
    {
        public ComplianceApplicationSubmitApprovalRepository(FactWebContext context) : base(context)
        {
        }

        public List<ComplianceApplicationSubmitApproval> GetByCompliance(Guid complianceApplicationId)
        {
            return base.Context.ComplianceApplicationSubmitApprovals
                .Include(x => x.User)
                .Where(x => x.ComplianceApplicationId == complianceApplicationId)
                .ToList();
        }

        public ComplianceApplicationSubmitApproval GetByUser(Guid complianceApplicationId, Guid userId)
        {
            return base.Fetch(x => x.ComplianceApplicationId == complianceApplicationId && x.UserId == userId);
        }

        public void CreateApprovals(Guid complianceApplicationId, string createdBy)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[2];
            paramList[0] = complianceApplicationId;
            paramList[1] = createdBy;

            objectContext.ExecuteStoreCommand(
                "EXEC usp_createComplianceApprovals @ComplianceApplicationId={0}, @CreatedBy={1}", paramList);
        }
    }
}
