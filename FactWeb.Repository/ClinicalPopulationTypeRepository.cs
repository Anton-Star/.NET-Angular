using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ClinicalPopulationTypeRepository : BaseRepository<ClinicalPopulationType>, IClinicalPopulationTypeRepository
    {
        public ClinicalPopulationTypeRepository(FactWebContext context) : base(context)
        {
        }

        public ClinicalPopulationType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<ClinicalPopulationType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
