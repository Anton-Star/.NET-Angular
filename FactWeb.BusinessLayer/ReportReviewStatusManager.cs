using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class ReportReviewStatusManager : BaseManager<ReportReviewStatusManager, IReportReviewStatusRepository, ReportReviewStatus>
    {
        public ReportReviewStatusManager(IReportReviewStatusRepository repository) : base(repository)
        {
        }
    }
}
