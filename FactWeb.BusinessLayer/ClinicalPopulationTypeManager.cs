using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ClinicalPopulationTypeManager : BaseManager<ClinicalPopulationTypeManager, IClinicalPopulationTypeRepository, ClinicalPopulationType>
    {
        public ClinicalPopulationTypeManager(IClinicalPopulationTypeRepository repository) : base(repository)
        {
        }

        public ClinicalPopulationType GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<ClinicalPopulationType> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}
