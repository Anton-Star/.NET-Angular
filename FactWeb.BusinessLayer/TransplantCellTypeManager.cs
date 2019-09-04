using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class TransplantCellTypeManager : BaseManager<TransplantCellTypeManager, ITransplantCellTypeRepository, TransplantCellType>
    {
        public TransplantCellTypeManager(ITransplantCellTypeRepository repository) : base(repository)
        {
        }

        public TransplantCellType GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<TransplantCellType> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}
