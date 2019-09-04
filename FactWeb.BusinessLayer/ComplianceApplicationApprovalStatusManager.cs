using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ComplianceApplicationApprovalStatusManager : BaseManager<ComplianceApplicationApprovalStatusManager, IComplianceApplicationApprovalStatusRepository, ComplianceApplicationApprovalStatus>
    {
        public ComplianceApplicationApprovalStatusManager(IComplianceApplicationApprovalStatusRepository repository) : base(repository)
        {
        }

        public ComplianceApplicationApprovalStatus GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<ComplianceApplicationApprovalStatus> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}
