using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class ReportReviewStatusFacade
    {
        readonly Container container;

        public ReportReviewStatusFacade(Container container)
        {
            this.container = container;
        }

        public List<ReportReviewStatus> GetAll()
        {
            var manager = this.container.GetInstance<ReportReviewStatusManager>();

            return manager.GetAll();
        }

        public Task<List<ReportReviewStatus>> GetAllAsync()
        {
            var manager = this.container.GetInstance<ReportReviewStatusManager>();

            return manager.GetAllAsync();
        }
    }
}
