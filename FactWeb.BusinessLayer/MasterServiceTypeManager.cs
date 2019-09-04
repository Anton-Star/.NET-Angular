using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class MasterServiceTypeManager : BaseManager<MasterServiceTypeManager, IMasterServiceTypeRepository, MasterServiceType>
    {
        public MasterServiceTypeManager(IMasterServiceTypeRepository repository) : base(repository)
        {
        }

        public MasterServiceType GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<MasterServiceType> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}
