using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class AssociationTypeManager : BaseManager<AssociationTypeManager, IAssociationTypeRepository, AssociationType>
    {
        public AssociationTypeManager(IAssociationTypeRepository repository) : base(repository)
        {
        }

        public AssociationType GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<AssociationType> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}
