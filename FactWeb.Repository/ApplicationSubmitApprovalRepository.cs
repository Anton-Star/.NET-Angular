using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FactWeb.Repository
{
    public class ApplicationSubmitApprovalRepository : BaseRepository<ApplicationSubmitApproval>, IApplicationSubmitApprovalRepository
    {
        public ApplicationSubmitApprovalRepository(FactWebContext context) : base(context)
        {
        }

        public List<ApplicationSubmitApproval> GetAllForApplication(int applicationId)
        {
            return base.Context.ApplicationSubmitApprovals.Include(x => x.User)
                .Where(x => x.ApplicationId == applicationId)
                .ToList();
        }

        public List<ApplicationSubmitApproval> GetAllForApplication(Guid applicationUniqueId)
        {
            return base.Context.ApplicationSubmitApprovals.Include(x => x.User)
                .Where(x => x.Application.UniqueId == applicationUniqueId)
                .ToList();
        }
    }
}
