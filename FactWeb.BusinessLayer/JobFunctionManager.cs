using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class JobFunctionManager : BaseManager<JobFunctionManager, IJobFunctionRepository, JobFunction>
    {
        public JobFunctionManager(IJobFunctionRepository repository) : base(repository)
        {
        }

        public JobFunction GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }
    }
}
