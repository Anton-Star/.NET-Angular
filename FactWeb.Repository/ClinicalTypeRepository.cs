using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class ClinicalTypeRepository : BaseRepository<ClinicalType>, IClinicalTypeRepository
    {
        public ClinicalTypeRepository(FactWebContext context) : base(context)
        {
        }
    }
}
