using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ProcessingTypeManager : BaseManager<ProcessingTypeManager, IProcessingTypeRepository, ProcessingType>
    {
        public ProcessingTypeManager(IProcessingTypeRepository repository) : base(repository)
        {
        }

        public ProcessingType GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<ProcessingType> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}
