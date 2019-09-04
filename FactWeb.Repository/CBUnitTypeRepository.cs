using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class CBUnitTypeRepository : BaseRepository<CBUnitType>, ICBUnitTypeRepository
    {
        public CBUnitTypeRepository(FactWebContext context) : base(context)
        {
        }

        public CBUnitType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<CBUnitType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
