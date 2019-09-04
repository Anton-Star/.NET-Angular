using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ApplicationTypeRepository : BaseRepository<ApplicationType>, IApplicationTypeRepository
    {
        public ApplicationTypeRepository(FactWebContext context) : base(context)
        {
        }

        public ApplicationType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<ApplicationType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
