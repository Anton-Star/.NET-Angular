using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class JobFunctionFacade
    {
        private readonly Container container;

        public JobFunctionFacade(Container container)
        {
            this.container = container;
        }

        public List<JobFunction> GetAll()
        {
            var manager = this.container.GetInstance<JobFunctionManager>();

            return manager.GetAll();
        }

        public Task<List<JobFunction>> GetAllAsync()
        {
            var manager = this.container.GetInstance<JobFunctionManager>();

            return manager.GetAllAsync();
        }
    }
}
