using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class CBUnitTypeManager : BaseManager<CBUnitTypeManager, ICBUnitTypeRepository, CBUnitType>
    {
        public CBUnitTypeManager(ICBUnitTypeRepository repository) : base(repository)
        {
        }

        public CBUnitType GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<CBUnitType> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}
