using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class TransplantCellTypeRepository : BaseRepository<TransplantCellType>, ITransplantCellTypeRepository
    {
        public TransplantCellTypeRepository(FactWebContext context) : base(context)
        {
        }

        public TransplantCellType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<TransplantCellType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
