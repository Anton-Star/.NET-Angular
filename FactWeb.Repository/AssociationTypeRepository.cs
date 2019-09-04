using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class AssociationTypeRepository : BaseRepository<AssociationType>, IAssociationTypeRepository
    {
        public AssociationTypeRepository(FactWebContext context) : base(context)
        {
        }

        public AssociationType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<AssociationType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
