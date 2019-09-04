using System.Threading.Tasks;
using FactWeb.Model;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IJobFunctionRepository : IRepository<JobFunction>
    {
        JobFunction GetByName(string name);
    }
}
