using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.Repository
{
    public class OutcomeStatusRepository : BaseRepository<OutcomeStatus>, IOutcomeStatusRepository
    {
        public OutcomeStatusRepository(FactWebContext context) : base(context)
        {
        }
    }
}
