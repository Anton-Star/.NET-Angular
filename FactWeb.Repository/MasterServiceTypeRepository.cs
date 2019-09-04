using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class MasterServiceTypeRepository : BaseRepository<MasterServiceType>, IMasterServiceTypeRepository
    {
        public MasterServiceTypeRepository(FactWebContext context) : base(context)
        {
        }

        public MasterServiceType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<MasterServiceType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
