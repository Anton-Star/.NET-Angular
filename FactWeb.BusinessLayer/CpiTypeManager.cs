using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class CpiTypeManager : BaseManager<CpiTypeManager, ICpiTypeRepository, CpiType>
    {
        public CpiTypeManager(ICpiTypeRepository repository) : base(repository)
        {
        }
    }
}
