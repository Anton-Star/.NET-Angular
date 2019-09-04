using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class ClinicalTypeManager : BaseManager<ClinicalTypeManager, IClinicalTypeRepository, ClinicalType>
    {
        public ClinicalTypeManager(IClinicalTypeRepository repository) : base(repository)
        {
        }
    }
}
