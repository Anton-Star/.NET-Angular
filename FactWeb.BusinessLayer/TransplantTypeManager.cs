using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class TransplantTypeManager : BaseManager<TransplantTypeManager, ITransplantTypeRepository, TransplantType>
    {
        public TransplantTypeManager(ITransplantTypeRepository repository) : base(repository)
        {
        }

        public TransplantType GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<TransplantType> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}
