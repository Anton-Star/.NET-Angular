using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IClinicalPopulationTypeRepository : IRepository<ClinicalPopulationType>
    {
        ClinicalPopulationType GetByName(string name);
        Task<ClinicalPopulationType> GetByNameAsync(string name);
    }
}
