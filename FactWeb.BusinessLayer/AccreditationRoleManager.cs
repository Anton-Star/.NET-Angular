using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class AccreditationRoleManager : BaseManager<AccreditationRoleManager, IAccreditationRoleRepository, AccreditationRole>
    {
        public AccreditationRoleManager(IAccreditationRoleRepository repository) : base(repository)
        {
        }
    }
}
