using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class ReportReviewStatusRepository : BaseRepository<ReportReviewStatus>, IReportReviewStatusRepository
    {
        public ReportReviewStatusRepository(FactWebContext context) : base(context)
        {
        }
    }
}
