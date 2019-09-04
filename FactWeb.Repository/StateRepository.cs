using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class StateRepository : BaseRepository<State>, IStateRepository
    {
        public StateRepository(FactWebContext context) : base(context)
        {
        }
    }
}
