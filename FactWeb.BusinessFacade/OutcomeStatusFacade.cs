using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class OutcomeStatusFacade
    {
        readonly Container container;

        public OutcomeStatusFacade(Container container)
        {
            this.container = container;
        }

        public List<OutcomeStatus> GetAll()
        {
            var manager = this.container.GetInstance<OutcomeStatusManager>();

            return manager.GetAll();
        }

        public Task<List<OutcomeStatus>> GetAllAsync()
        {
            var manager = this.container.GetInstance<OutcomeStatusManager>();

            return manager.GetAllAsync();
        }
    }
}
