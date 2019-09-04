using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IComplianceApplicationApprovalStatusRepository : IRepository<ComplianceApplicationApprovalStatus>
    {
        ComplianceApplicationApprovalStatus GetByName(string name);
        Task<ComplianceApplicationApprovalStatus> GetByNameAsync(string name);
    }
}
