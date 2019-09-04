using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class ApplicationSubmitApprovalManager : BaseManager<ApplicationSubmitApprovalManager, IApplicationSubmitApprovalRepository, ApplicationSubmitApproval>
    {
        public ApplicationSubmitApprovalManager(IApplicationSubmitApprovalRepository repository) : base(repository)
        {
        }

        public List<ApplicationSubmitApproval> GetAllForApplication(int applicationId)
        {
            return base.Repository.GetAllForApplication(applicationId);
        }

        public List<ApplicationSubmitApproval> GetAllForApplication(Guid applicationUniqueId)
        {
            return base.Repository.GetAllForApplication(applicationUniqueId);
        }
    }
}
