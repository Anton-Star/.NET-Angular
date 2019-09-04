using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class JobFunctionRepository : BaseRepository<JobFunction>, IJobFunctionRepository
    {
        public JobFunctionRepository(FactWebContext context) : base(context)
        {
        }

        public JobFunction GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }
    }
}
