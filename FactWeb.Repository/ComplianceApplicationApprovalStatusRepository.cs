using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ComplianceApplicationApprovalStatusRepository : BaseRepository<ComplianceApplicationApprovalStatus>, IComplianceApplicationApprovalStatusRepository
    {
        public ComplianceApplicationApprovalStatusRepository(FactWebContext context) : base(context)
        {
        }

        public ComplianceApplicationApprovalStatus GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<ComplianceApplicationApprovalStatus> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
