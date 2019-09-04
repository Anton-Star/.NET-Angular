using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ProcessingTypeRepository : BaseRepository<ProcessingType>, IProcessingTypeRepository
    {
        public ProcessingTypeRepository(FactWebContext context) : base(context)
        {
        }

        public ProcessingType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<ProcessingType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
