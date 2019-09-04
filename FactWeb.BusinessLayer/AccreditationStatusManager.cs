using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class AccreditationStatusManager : BaseManager<AccreditationStatusManager, IAccreditationStatusRepository, AccreditationStatus>
    {
        public AccreditationStatusManager(IAccreditationStatusRepository repository) : base(repository)
        {
        }

        public AccreditationStatus GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<AccreditationStatus> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}
