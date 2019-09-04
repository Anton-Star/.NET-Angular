using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class TransplantTypeRepository : BaseRepository<TransplantType>, ITransplantTypeRepository
    {
        public TransplantTypeRepository(FactWebContext context) : base(context)
        {
        }

        public TransplantType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<TransplantType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
