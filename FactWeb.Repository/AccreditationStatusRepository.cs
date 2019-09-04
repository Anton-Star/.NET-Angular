using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class AccreditationStatusRepository : BaseRepository<AccreditationStatus>, IAccreditationStatusRepository
    {
        public AccreditationStatusRepository(FactWebContext context) : base(context)
        {
        }

        public AccreditationStatus GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<AccreditationStatus> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}

