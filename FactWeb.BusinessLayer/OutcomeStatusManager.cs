using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class OutcomeStatusManager : BaseManager<OutcomeStatusManager, IOutcomeStatusRepository, OutcomeStatus>
    {
        public OutcomeStatusManager(IOutcomeStatusRepository repository) : base(repository)
        {
        }
    }
}
