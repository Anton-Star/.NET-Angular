using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class CacheStatusFacade
    {
        private readonly Container container;

        public CacheStatusFacade(Container container)
        {
            this.container = container;
        }

        public List<CacheStatus> GetAll()
        {
            var manager = this.container.GetInstance<CacheStatusManager>();

            return manager.GetAll();
        }

        public Task<List<CacheStatus>> GetAllAsync()
        {
            var manager = this.container.GetInstance<CacheStatusManager>();

            return manager.GetAllAsync();
        }
    }
}
